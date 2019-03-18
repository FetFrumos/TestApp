using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GameList.Models.Communication;
using GameList.Models.Enum;
using Xamarin.Forms;

namespace GameList.Droid.Communication.Bluetooth
{
	public class DeviceDiscoveredReceiver : BroadcastReceiver
	{
		private List<BlDevice> _devices;
		public DeviceDiscoveredReceiver(List<BlDevice> devices)
		{
			if (devices == null)
			{
				_devices = new List<BlDevice>();
			}

			{
				_devices = devices;
			}
		}

		public override void OnReceive(Context context, Intent intent)
		{
			string action = intent.Action;
			if (action == BluetoothDevice.ActionFound)
			{
				BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
				if (device.BondState != Bond.Bonded)
				{
					_devices.Add(new BlDevice
					{
						Name = device.Name,
						Address = device.Address
					});
				}
			}
			else 
			if (action == BluetoothAdapter.ActionDiscoveryFinished)
			{
				MessagingCenter.Send(new object(), nameof(Messages.StopDeviceses), _devices);
			}
		}
	}
}