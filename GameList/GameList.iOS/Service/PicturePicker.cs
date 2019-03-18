using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using GameList.Interfaces;
using UIKit;
using Xamarin.Forms;
namespace GameList.iOS.Service
{
	public class PicturePicker : IPicturePicker
	{
		TaskCompletionSource<Tuple<Stream, string>> taskCompletionSource;
		UIImagePickerController imagePicker;

		public Task<Tuple<Stream, string>> GetImageStreamAsync()
		{
			// Create and define UIImagePickerController
			imagePicker = new UIImagePickerController
			{
				SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
				MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
			};

			// Set event handlers
			imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
			imagePicker.Canceled += OnImagePickerCancelled;

			// Present UIImagePickerController;
			UIWindow window = UIApplication.SharedApplication.KeyWindow;
			var viewController = window.RootViewController;
			viewController.PresentModalViewController(imagePicker, true);

			// Return Task object
			taskCompletionSource = new TaskCompletionSource<Tuple<Stream, string>>();
			return taskCompletionSource.Task;
		}
		void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
		{
			UIImage image = args.EditedImage ?? args.OriginalImage;

			if (image != null)
			{
				var url = (NSUrl)args.Info.ValueForKey(new NSString("UIImagePickerControllerImageURL"));
				// Convert UIImage to .NET Stream object
				NSData data = image.AsJPEG(1);
				Stream stream = data.AsStream();

				UnregisterEventHandlers();

				// Set the Stream as the completion of the Task
				var res = new Tuple<Stream, string>(stream, url.Path);
				taskCompletionSource.SetResult(res);
			}
			else
			{
				UnregisterEventHandlers();
				taskCompletionSource.SetResult(null);
			}
			imagePicker.DismissModalViewController(true);
		}

		void OnImagePickerCancelled(object sender, EventArgs args)
		{
			UnregisterEventHandlers();
			taskCompletionSource.SetResult(null);
			imagePicker.DismissModalViewController(true);
		}

		void UnregisterEventHandlers()
		{
			imagePicker.FinishedPickingMedia -= OnImagePickerFinishedPickingMedia;
			imagePicker.Canceled -= OnImagePickerCancelled;
		}
	}
}