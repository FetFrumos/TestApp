using System.Text;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Widget;
using GameList.Droid.Communication.Bluetooth;
using GameList.Interfaces;
using Plugin.CurrentActivity;

namespace GameList.Droid.Services
{
	public class BlManager 
	{
		BluetoothAdapter bluetoothAdapter = null;
		BluetoothChatService chatService = null;
		DiscoverableModeReceiver receiver;
		ChatHandler handler;

		const int REQUEST_ENABLE_BT = 3;
		const int REQUEST_CONNECT_DEVICE_SECURE = 1;
		const int REQUEST_CONNECT_DEVICE_INSECURE = 2;

		public BlManager()
		{
			bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
			receiver = new DiscoverableModeReceiver();
			receiver.BluetoothDiscoveryModeChanged += (sender, e) =>
			{
				
			};
			handler = new ChatHandler();
		}
		
		public void OnStart()
		{
			if (!bluetoothAdapter.IsEnabled)
			{
				var enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
				//! StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
				CrossCurrentActivity.Current.Activity.StartActivityForResult(enableIntent, REQUEST_ENABLE_BT);
			}
			else 
			if (chatService == null)
			{
				chatService = new BluetoothChatService(handler);
			}

			// Register for when the scan mode changes
			var filter = new IntentFilter(BluetoothAdapter.ActionScanModeChanged);
			CrossCurrentActivity.Current.Activity.RegisterReceiver(receiver, filter);
		}
		//bool requestingPermissionsSecure, requestingPermissionsInsecure;
		
		public void OnResume()
		{
			if (chatService != null)
			{
				if (chatService.GetState() == BluetoothChatService.STATE_NONE)
				{
					chatService.Start();
				}
			}
		}

		public void OnDestroy()
		{
			CrossCurrentActivity.Current.Activity.UnregisterReceiver(receiver);
			if (chatService != null)
			{
				chatService.Stop();
			}
		}

		public void ConnectDevice(string address)
		{
			var device = bluetoothAdapter.GetRemoteDevice(address);
			chatService.Connect(device, false);
		}

	 	public void SendMessage(string message)
		{
			if (chatService.GetState() != BluetoothChatService.STATE_CONNECTED)
			{
				Toast.MakeText(CrossCurrentActivity.Current.Activity, "соединенние отсутсувует", ToastLength.Long).Show();
				return;
			}

			if (message.Length > 0)
			{
				var bytes = Encoding.ASCII.GetBytes(message);
				chatService.Write(bytes);
			}
		}
	}
}