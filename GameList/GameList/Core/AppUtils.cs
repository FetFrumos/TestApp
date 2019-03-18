using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace GameList.Core
{
	public static class AppUtils
	{
		public const string DbName = "game.db";

		public static async Task<PermissionStatus> GetPermission(Permission permission, Func<Task> func)
		{
			var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
			if (status != PermissionStatus.Granted)
			{
				if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
				{
					await func();
				}

				var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { permission });
				status = results[permission];
			}
			return status;
		}
	}
}
