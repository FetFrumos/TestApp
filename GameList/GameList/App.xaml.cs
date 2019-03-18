using System;
using System.Threading.Tasks;
using GameList.Core;
using GameList.Db;
using GameList.Db.Interfaces;
using GameList.Interfaces;
using Prism;
using Prism.Ioc;
using GameList.ViewModels;
using GameList.Views;
using Prism.Unity;
using Unity.Lifetime;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GameList
{
	public partial class App
	{
		/* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
		public App() : this(null) { }

		public App(IPlatformInitializer initializer) : base(initializer) { }

		protected override async void OnInitialized()
		{
			InitializeComponent();
			var startUrl = $"{nameof(NavigationPage)}/{nameof(MainPage)}";
			await NavigationService.NavigateAsync(startUrl);
		}

		protected override  void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
			containerRegistry.RegisterForNavigation<GameObjPage, GameObjPageViewModel>();

			containerRegistry.RegisterSingleton<IDataService, DataService>();
			containerRegistry.RegisterSingleton<IAppSettings, AppSettings>();
			containerRegistry.RegisterForNavigation<ConnectPage, ConnectPageViewModel>();
		}
	}
}
