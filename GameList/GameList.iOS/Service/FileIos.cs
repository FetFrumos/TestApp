using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using GameList.Interfaces;
using UIKit;

namespace GameList.iOS.Service
{
	public class FileIos:IAppFile
	{
		public string GetPath()
		{
			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			return documentsPath;
		}

		public string ImageToStr(string file)
		{
			return String.Empty;
		}
	}
}