using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameList.Core;
using GameList.Db.Domain;
using GameList.Db.Interfaces;
using GameList.Db.Models;
using GameList.Interfaces;
using GameList.Models;
using GameList.Models.Enum;
using GameList.Resx;
using GameList.ViewModels.Base;
using GameList.Views;
using Newtonsoft.Json;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Xamarin.Forms;

namespace GameList.ViewModels
{
	public class MainPageViewModel : ViewModelBaseAdd
	{
		
		private ActionTimer _timer;
		
		public MainPageViewModel(INavigationService navigationService, IDataService ds, IPageDialogService dialog, IAppFile appFile, IAppSettings appSet)
			: base(navigationService, ds, dialog, appFile, appSet)
		{
			Title = AppRes.AppTitle;
			AddCommand = new DelegateCommand(AddCommandExec);
			DeleteCommand = new DelegateCommand(DeleteCommandExec);
			SearchCommand =new DelegateCommand<string>(SearchCommandExec);
			DevicesCommand = new DelegateCommand(DevicesCommandExec);
			SendCommand = new DelegateCommand(SendCommandExec);
			InitSettings();
		}

		

		private void InitSettings()
		{
			if (string.IsNullOrEmpty(AppSet.AppId))
				AppSet.AppId = Guid.NewGuid().ToString();
		}


		public override async void OnNavigatedTo(INavigationParameters param)
		{
			if (Ds.NeedInit)
			{
				var path = AppFile.GetPath();
				path = Path.Combine(path, AppUtils.DbName);
				await Ds.Init(path);
			}

			var runTimer5 = param?[nameof(NavParamEnum.RunTimer5)] as bool?;
			if (runTimer5 != null && (bool) runTimer5)
			{
				var objGameName = param?[nameof(NavParamEnum.GameName)];
				if (objGameName !=null)
				{
					_gameName = objGameName.ToString();
					if (_timer == null)
					{
						_timer = new ActionTimer();
						_timer.InitAndStart(ShowMess);
					}
					else
					{
						_timer.Restart();
					}
				}

			}

			await InitGames();
		}

		private async Task InitGames()
		{
			_fullItems = await Ds.GetTitles();
			GameTitles = new ObservableCollection<GameTitle>(_fullItems);
		}

		private List<GameTitle> _fullItems;
		private string _gameName =string.Empty;
		private ObservableCollection<GameTitle> _gameTitles;
		private string _textSearch;
		private GameTitle _selectedItem;

		private void ShowMess()
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				var mess = $"{AppRes.NewGameCreated} - {_gameName}";
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, mess, AppRes.Ok);
			});
		}

		public ObservableCollection<GameTitle> GameTitles
		{
			get => _gameTitles;
			set => SetProperty(ref _gameTitles, value);
		}

		public string TextSearch
		{
			get => _textSearch;
			set
			{
				_textSearch = value;
				if (string.IsNullOrEmpty(_textSearch))
				{
					if (GameTitles.Count<_fullItems.Count)
					{
						SearchCommand.Execute(string.Empty);
					}
				}
			}
		}

		public GameTitle SelectedItem
		{
			get => _selectedItem;
			set => SetProperty(ref _selectedItem, value);
		}

		#region command

		public DelegateCommand DevicesCommand { get; set; }

		private async void DevicesCommandExec()
		{
			var status = await AppUtils.GetPermission(Permission.Location, async () =>
				{
					await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.NeedLocate, AppRes.Ok);
				}
			);
			if(status == PermissionStatus.Granted)
			await NavigationService.NavigateAsync(nameof(ConnectPage));
		}

		public DelegateCommand AddCommand { get; set; }
		
		private async void AddCommandExec()
		{
			await NavigationService.NavigateAsync(nameof(GameObjPage));
		}

		public DelegateCommand<string> SearchCommand { get; set; }
		private void SearchCommandExec(string search)
		{
			if(_fullItems.Count==0)
				return;
			if (string.IsNullOrEmpty(search))
			{
				GameTitles = new ObservableCollection<GameTitle>(_fullItems);
			}
			else
			{
				var items = _fullItems.Where(
					i => i.Name.ToLower().Contains(search) || i.Autor.ToLower().Contains(search));
				GameTitles = new ObservableCollection<GameTitle>(items);
			}
		}

		public DelegateCommand DeleteCommand { get; set; }
		private async void DeleteCommandExec()
		{
			if (SelectedItem == null)
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.SelectItem, AppRes.Ok);
				return;
			}

			var mess = $"{AppRes.WantDelete} - {SelectedItem.Name}?";
			var res = await Dialog.DisplayAlertAsync(AppRes.AppTitle, mess, AppRes.Ok, AppRes.Cancel);
			if (res)
			{
				await Ds.DeleteGame(SelectedItem.Id);
			}
			await InitGames();
		}

		public DelegateCommand SendCommand { get; set; }

		private async void SendCommandExec()
		{
			if (SelectedItem == null)
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.SelectItem, AppRes.Ok);
				return;
			}

			Tuple<Game, GameDescription> data = await  Ds.GetGameData(SelectedItem.Id);
			var gameData = new GameData
			{
				Game = data.Item1,
				Image = data.Item2.Description
			};
			var mess = JsonConvert.SerializeObject(gameData);
			var image = AppFile.ImageToStr(data.Item2.Image);
			gameData.Image = image;
			MessagingCenter.Send(this, nameof(Messages.SendMessage), mess);
		}

		#endregion

	}
}
