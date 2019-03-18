using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GameList.Core;

namespace GameList.Droid.Core
{
	public static class DroidUtils
	{
		public static string DbFullPath()
		{
			var sqliteFilename = AppUtils.DbName;
			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var path = Path.Combine(documentsPath, sqliteFilename);
			return path;
		}
	}
}