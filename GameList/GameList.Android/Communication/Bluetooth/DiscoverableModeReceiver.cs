using System;
using Android.Bluetooth;
using Android.Content;

namespace GameList.Droid.Communication.Bluetooth
{
	public class DiscoverableModeReceiver : BroadcastReceiver
	{
		public event EventHandler<BluetoothDiscoveryModeArgs> BluetoothDiscoveryModeChanged;


		public override void OnReceive(Context context, Intent intent)
		{
			var currentScanMode = intent.GetIntExtra(BluetoothAdapter.ExtraScanMode, -1);
			var previousScanMode = intent.GetIntExtra(BluetoothAdapter.ExtraPreviousScanMode, -1);


			bool inDiscovery = currentScanMode == (int)ScanMode.ConnectableDiscoverable;

			BluetoothDiscoveryModeChanged?.Invoke(this, new BluetoothDiscoveryModeArgs(inDiscovery));

		}
	}
}