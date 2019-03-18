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
using GameList.Models;
using GameList.Models.Enum;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GameList.Droid.Communication.Bluetooth
{
	class ChatHandler : Handler
	{
		
		public ChatHandler()
		{
		}
		public override void HandleMessage(Message msg)
		{
			switch (msg.What)
			{
				case Constants.MESSAGE_STATE_CHANGE:
					switch (msg.What)
					{
						case BluetoothChatService.STATE_CONNECTED:
							break;
						case BluetoothChatService.STATE_CONNECTING:
							break;
						case BluetoothChatService.STATE_LISTEN:
							break;
						case BluetoothChatService.STATE_NONE:
							break;
					}
					break;
				case Constants.MESSAGE_WRITE:
					var writeBuffer = (byte[])msg.Obj;
					var writeMessage = Encoding.ASCII.GetString(writeBuffer);
					break;
				case Constants.MESSAGE_READ:
					var readBuffer = (byte[])msg.Obj;
					var readMessage = Encoding.ASCII.GetString(readBuffer);
					var data = JsonConvert.DeserializeObject<GameData>(readMessage);
					MessagingCenter.Send(new object(), nameof(Messages.NewGame), data);
					break;
				case Constants.MESSAGE_DEVICE_NAME:
					var device = msg.Data.GetString(Constants.DEVICE_NAME);
					MessagingCenter.Send(new object(), nameof(Messages.ConnectSuccess),device);
				    break;
				case Constants.MESSAGE_TOAST:
					break;
			}
		}
	}
}