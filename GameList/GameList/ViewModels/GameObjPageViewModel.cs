using GameList.Core;
using GameList.Db.Domain;
using GameList.Db.Interfaces;
using GameList.Interfaces;
using GameList.Models;
using GameList.Resx;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.IO;
using GameList.ViewModels.Base;
using Xamarin.Forms;

namespace GameList.ViewModels
{
	public class GameObjPageViewModel : ViewModelBaseAdd
	{
		private readonly IPicturePicker _picker;
		public GameObjPageViewModel(INavigationService navigationService, IDataService ds, IPageDialogService dialog, IAppFile appFile, IAppSettings appSet, IPicturePicker picker)
			: base(navigationService, ds, dialog, appFile, appSet)
		{
			Title = AppRes.NewGame;
			SaveCommand = new DelegateCommand(SaveCommandExecute);
			SelectImgCommand = new DelegateCommand(SelectImgCommandExecute);
			_picker = picker;
		}

		
		#region prop

		private string _name;
		private string _autor;
		private string _desc;
		private string _imagePath;
		private ImageSource _image;

		public string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		public string Autor
		{
			get => _autor;
			set => SetProperty(ref _autor, value);
		}

		public string Desc
		{
			get => _desc;
			set => SetProperty(ref _desc, value);
		}



		public ImageSource Image
		{
			get => _image;
			set => SetProperty(ref _image, value);
		}

		#endregion

		#region command

		public DelegateCommand SelectImgCommand { get; }
		private async void SelectImgCommandExecute()
		{
			var status = await AppUtils.GetPermission(Permission.Storage, async () =>
				{
					await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.NeedStore, AppRes.Ok);
				}
			);

			if (status != PermissionStatus.Granted)
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.NeedStore, AppRes.Ok);
				return;
			}
			var res = await _picker.GetImageStreamAsync();
			if(res==null)
				return;
			if (!string.IsNullOrEmpty(res.Item2))
			{
				_imagePath = res.Item2;
				Image = ImageSource.FromStream(() => res.Item1);
			}
		}

		public DelegateCommand SaveCommand { get; }

		private async void SaveCommandExecute()
		{
			if (string.IsNullOrEmpty(Name))
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.InputName, AppRes.Ok);
				return;
			}

			if (string.IsNullOrEmpty(Autor))
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.Autor, AppRes.Ok);
				return;
			}

			if (string.IsNullOrEmpty(_imagePath))
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.SelectImg, AppRes.Ok);
				return;
			}

			var status = await AppUtils.GetPermission(Permission.Storage, async () =>
				{
					await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.NeedStore, AppRes.Ok);
				}
			);

			if (status != PermissionStatus.Granted)
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.NeedStore, AppRes.Ok);
				return;
			}

			var game = new Game
			{
				Name = Name,
				Author = Autor,
			};
			var id = await Ds.SaveGame(game);


			var targetPath = Path.GetFileName(_imagePath);
			targetPath = $"{AppFile.GetPath()}/{id}-{targetPath}";
			File.Copy(_imagePath, targetPath);

			var desc = new GameDescription
			{
				Image = targetPath,
				Description = Desc
			};
			await Ds.SaveDesc(desc, id);

			var navParams = new NavigationParameters
			{
				{nameof(NavParamEnum.RunTimer5), true},
				{nameof(NavParamEnum.GameName), Name}
			};
			await NavigationService.GoBackAsync(navParams);

		}

		#endregion
	}
}
