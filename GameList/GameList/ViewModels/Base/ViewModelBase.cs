using GameList.Db.Interfaces;
using GameList.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace GameList.ViewModels.Base
{
	public class ViewModelBase : BindableBase, INavigationAware, IDestructible
	{
		public ViewModelBase(INavigationService nav, IPageDialogService dialog)
		{
			NavigationService = nav;
			Dialog = dialog;
		}
		protected IPageDialogService Dialog { get; }
		protected INavigationService NavigationService { get; }
		private string _title;
		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		
		public virtual void OnNavigatedFrom(INavigationParameters parameters)
		{

		}

		public virtual void OnNavigatedTo(INavigationParameters parameters)
		{

		}

		public virtual void OnNavigatingTo(INavigationParameters parameters)
		{

		}

		public virtual void Destroy()
		{

		}
	}
}
