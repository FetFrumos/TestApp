using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GameList.Interfaces;
using Plugin.FilePicker;
using Xamarin.Forms;

namespace GameList.Core
{
	public class AppFilePicker : IAppFilePicker
	{
		readonly string[] _fileTypes = null;
		public AppFilePicker()
		{
			if (Device.RuntimePlatform == Device.Android)
			{
				_fileTypes = new string[] { "image/png", "image/jpeg" };
			}

			if (Device.RuntimePlatform == Device.iOS)
			{
				_fileTypes = new string[] { "public.image" }; // same as iOS constant UTType.Image
			}
		}

		public async Task<Tuple<string, Stream>> GetFile()
		{
			var pickedFile = await CrossFilePicker.Current.PickFile(_fileTypes);
			if (pickedFile != null)
			{
				return new Tuple<string, Stream>(pickedFile.FilePath, pickedFile.GetStream());
			}
			else
			{
				return new Tuple<string, Stream>(string.Empty, Stream.Null);
			}
		}
	}
}
