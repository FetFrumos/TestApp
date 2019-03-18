using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameList.Db.Models;
using GameList.Models.Communication;
using GameList.Models.Enum;
using GameList.Resx;
using GameList.ViewModels.Base;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace GameList.ViewModels
{
	public class ConnectPageViewModel : ViewModelBase
	{
		#region prop

		private bool _isLoad;
		private ObservableCollection<BlDevice> _devices;
		private BlDevice _selectedItem;

		public bool IsLoad
		{
			get => _isLoad;
			set => SetProperty(ref _isLoad, value);
		}


		public ObservableCollection<BlDevice> Devices
		{
			get => _devices;
			set => SetProperty(ref _devices, value);
		}

		public BlDevice SelectedItem
		{
			get => _selectedItem;
			set => SetProperty(ref _selectedItem, value);
		}

		#endregion

		public ConnectPageViewModel(INavigationService nav, IPageDialogService dialog) : base(nav, dialog)
        {
	        Title = AppRes.Devices;
			MessagingCenter.Subscribe<object, List<BlDevice>>(this, nameof(Messages.StopDeviceses), (sender, list) =>
				{
					Devices = new ObservableCollection<BlDevice>(list);
					IsLoad = false;
				});
	        MessagingCenter.Subscribe<object, string>(this, nameof(Messages.ConnectSuccess), async(sender, device) =>
	        {
		            IsLoad = false;
			        await Dialog.DisplayAlertAsync(AppRes.AppTitle, $"{device}-{AppRes.Ok}", AppRes.Ok);
			        await NavigationService.GoBackAsync();
		        });
			ConnectCommand = new DelegateCommand(ConnectCommandExecute);
		}

		private async void ConnectCommandExecute()
		{
			if (SelectedItem == null)
			{
				await Dialog.DisplayAlertAsync(AppRes.AppTitle, AppRes.NeedSelected, AppRes.Ok);
				return;
			}
			MessagingCenter.Send<ConnectPageViewModel, string>(this, nameof(Messages.Connect),SelectedItem.Address);
			IsLoad = true;
		}

		public override  void OnNavigatedTo(INavigationParameters param)
		{
			MessagingCenter.Send(this, nameof(Messages.ScanDeviceses));
			//IsLoad = true;
		}

		public DelegateCommand ConnectCommand { get; set; }
	}
}
