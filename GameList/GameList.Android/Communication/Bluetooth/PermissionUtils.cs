using System.Linq;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using AndroidResource = Android.Resource;

namespace GameList.Droid.Communication.Bluetooth
{
	public static class PermissionUtils
	{
		public const int RC_LOCATION_PERMISSIONS = 1000;

		public static readonly string[] LOCATION_PERMISSIONS = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };

		public static void RequestPermissionsForApp(this Android.App.Fragment frag)
		{
			var showRequestRationale = ActivityCompat.ShouldShowRequestPermissionRationale(frag.Activity, Manifest.Permission.AccessFineLocation) ||
									   ActivityCompat.ShouldShowRequestPermissionRationale(frag.Activity, Manifest.Permission.AccessCoarseLocation);

			if (showRequestRationale)
			{
				var rootView = frag.Activity.FindViewById(AndroidResource.Id.Content);
				Snackbar.Make(rootView, "request_location_permissions", Snackbar.LengthIndefinite)
						.SetAction("ok", v =>
						{
							frag.RequestPermissions(LOCATION_PERMISSIONS, RC_LOCATION_PERMISSIONS);
						})
						.Show();
			}
			else
			{
				frag.RequestPermissions(LOCATION_PERMISSIONS, RC_LOCATION_PERMISSIONS);
			}
		}

		public static bool AllPermissionsGranted(this Android.Content.PM.Permission[] grantResults)
		{
			if (grantResults.Length < 1)
			{
				return false;
			}

			return grantResults.All(result => result != Permission.Denied);
		}

		public static bool HasLocationPermissions(this Context context)
		{
			foreach (var perm in LOCATION_PERMISSIONS)
			{
				if (ContextCompat.CheckSelfPermission(context, perm) != Android.Content.PM.Permission.Granted)
				{
					return false;
				}
			}
			return true;
		}
	}
}