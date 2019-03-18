using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using GameList.Droid.Communication.Bluetooth;
using GameList.Droid.Services;
using GameList.Interfaces;
using GameList.Models.Communication;
using GameList.Models.Enum;
using GameList.ViewModels;
using GameList.Views;
using Plugin.Permissions;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace GameList.Droid
{
    [Activity(Label = "GameList", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
	    DeviceDiscoveredReceiver _receiver;
	    BluetoothAdapter _btAdapter;
	    public static readonly int PickImageId = 1000;
	    private BlManager _blManager;
		public static TaskCompletionSource<Tuple<Stream,string>> PickImageTaskCompletionSource { set; get; }
		protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
	        Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
			AndroidEnvironment.UnhandledExceptionRaiser += MyApp_UnhandledExceptionHandler;
			global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
	        _btAdapter = BluetoothAdapter.DefaultAdapter;
	        if (_btAdapter !=null)
	        {
				_blManager = new BlManager();
			}
			
	        /*var pairedDevices = btAdapter.BondedDevices;
	        if (pairedDevices.Count == 0)
	        {
		        _receiver = new DeviceDiscoveredReceiver(null);
	        }
	        else
	        {
		        var devices = pairedDevices.Select(i => new BlDevice
		        {
			        Name = i.Name,
			        Address = i.Address
		        }).ToList();
				_receiver = new DeviceDiscoveredReceiver(devices);
		        MessagingCenter.Send(new object(), nameof(Messages.StopDeviceses), devices);
			}
	        var  filter = new IntentFilter(BluetoothDevice.ActionFound);
	        RegisterReceiver(_receiver, filter);

			filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
			RegisterReceiver(_receiver, filter);*/
	       
			
			MessagingCenter.Subscribe<ConnectPageViewModel>(this, nameof(Messages.ScanDeviceses), (sender) =>
	        {
		        /*if (btAdapter.IsDiscovering)
		        {
			        btAdapter.CancelDiscovery();
		        }
				
		        var x = btAdapter.StartDiscovery();*/

		        var pairedDevices = _btAdapter.BondedDevices;
		        if (pairedDevices.Count == 0)
		        {
			        _receiver = new DeviceDiscoveredReceiver(null);
		        }
		        else
		        {
			        var devices = pairedDevices.Select(i => new BlDevice
			        {
				        Name = i.Name,
				        Address = i.Address
			        }).ToList();
			        _receiver = new DeviceDiscoveredReceiver(devices);
			        MessagingCenter.Send(new object(), nameof(Messages.StopDeviceses), devices);
		        }
			});

	        MessagingCenter.Subscribe<ConnectPageViewModel,string>(this, nameof(Messages.Connect),
		       (sender, address) =>
		        {
			        _blManager.ConnectDevice(address);
				}
		     );
			
	        MessagingCenter.Subscribe<MainPageViewModel, string>(this, nameof(Messages.SendMessage),
		        (sender, value) =>
		        {
			        _blManager.SendMessage(value);
				});
        }

		protected override void OnResume()
	    {
			base.OnResume();
			_blManager?.OnResume();
	    }


	    protected override void OnStart()
	    {
		    base.OnResume();
		    _blManager?.OnStart();
	    }

		protected override void OnDestroy()
	    {
		    base.OnDestroy();
		    _blManager?.OnDestroy();
			UnregisterReceiver(_receiver);
		}


		private void MyApp_UnhandledExceptionHandler(object sender, RaiseThrowableEventArgs e)
	    {
		    
	    }

	    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
	    {
		    PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions,
			    grantResults);
	    }

	    protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
	    {
		    base.OnActivityResult(requestCode, resultCode, intent);
			if (requestCode == PickImageId)
		    {
			    if ((resultCode == Result.Ok) && (intent != null))
			    {
				    Android.Net.Uri uri = intent.Data;
					Stream stream = ContentResolver.OpenInputStream(uri);
				    var path = GetRealPathFromUri(uri);
					PickImageTaskCompletionSource.SetResult(new Tuple<Stream, string>(stream, path));
			    }
			    else
			    {
				    PickImageTaskCompletionSource.SetResult(null);
			    }
		    }
	    }

	    private string GetRealPathFromUri(Android.Net.Uri contentUri)
	    {
		    ICursor cursor = ContentResolver.Query(contentUri, null, null, null, null);
		    cursor.MoveToFirst();
		    string documentId = cursor.GetString(0);
		    documentId = documentId.Split(':')[1];
		    cursor.Close();

		    cursor = ContentResolver.Query(
			    Android.Provider.MediaStore.Images.Media.ExternalContentUri,
			    null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
		    cursor.MoveToFirst();
		    string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
		    cursor.Close();

		    return path;
	    }
	}

	public class AndroidInitializer : IPlatformInitializer
    {
	   
		public void RegisterTypes(IContainerRegistry containerRegistry)
        {
			// Register any platform specific implementations
			containerRegistry.RegisterSingleton<IAppFile,FileDroid>();
	        containerRegistry.RegisterSingleton<IPicturePicker, PicturePicker>();
		}
    }
}

