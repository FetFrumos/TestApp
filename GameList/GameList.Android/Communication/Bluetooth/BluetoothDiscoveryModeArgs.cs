using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GameList.Droid.Communication.Bluetooth
{
	public class BluetoothDiscoveryModeArgs : EventArgs
	{
		public BluetoothDiscoveryModeArgs(bool inDiscoveryMode)
		{
			InDiscoveryMode = inDiscoveryMode;
		}
		public bool InDiscoveryMode { get; private set; }
	}
}