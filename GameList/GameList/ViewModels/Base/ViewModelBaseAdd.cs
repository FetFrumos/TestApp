using System;
using System.Collections.Generic;
using System.Text;
using GameList.Db.Interfaces;
using GameList.Interfaces;
using Prism.Navigation;
using Prism.Services;

namespace GameList.ViewModels.Base
{
    public class ViewModelBaseAdd : ViewModelBase
	{
		
		protected IDataService Ds { get; }
		protected IAppFile AppFile { get; }
		protected IAppSettings AppSet { get; }

		public ViewModelBaseAdd(INavigationService navigationService, IDataService ds, IPageDialogService dialog, IAppFile appFile, IAppSettings appSet) : base(navigationService, dialog)
		{
			Ds = ds;
			AppFile = appFile;
			AppSet = appSet;
		}
	}
}
