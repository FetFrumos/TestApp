using System;
using System.IO;
using GameList.Interfaces;

namespace GameList.Droid.Services
{
	public class FileDroid :IAppFile
	{
		public string GetPath()
		{
			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			return documentsPath;
		}

		public string ImageToStr(string file)
		{
			using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
			{
				var imageData = new byte[fs.Length];
				fs.Read(imageData, 0, (int)fs.Length);
				var base64String = Convert.ToBase64String(imageData);
				return base64String;
			}
		}
	}
}