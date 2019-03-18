using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GameList.Interfaces;
using Plugin.CurrentActivity;

namespace GameList.Droid.Services
{
	public class PicturePicker : IPicturePicker
	{
		public Task<Tuple<Stream, string>> GetImageStreamAsync()
		{
			Intent intent = new Intent();
			intent.SetType("image/*");
			intent.SetAction(Intent.ActionGetContent);
			// Start the picture-picker activity (resumes in MainActivity.cs)
			CrossCurrentActivity.Current.Activity.StartActivityForResult(
				Intent.CreateChooser(intent, "Выберите рисунок"),
				MainActivity.PickImageId);

			// Save the TaskCompletionSource object as a MainActivity property
			MainActivity.PickImageTaskCompletionSource = new TaskCompletionSource<Tuple<Stream,string>>();

			// Return Task object
			return MainActivity.PickImageTaskCompletionSource.Task;
		}
	}
}