using System;
using System.Collections.Generic;
using System.Text;
using GameList.Interfaces;
using Xamarin.Essentials;

namespace GameList.Core
{
	public class AppSettings : IAppSettings
	{
		public string AppId
		{
			get => Preferences.Get(nameof(AppId), string.Empty);
			set => Preferences.Set(nameof(AppId), value);
		}
	}
}
