using System;
using System.Windows;
using System.Windows.Mvvm;

using ProgressBarExample.ViewModels;

namespace ProgressBarExample
{
	public partial class MainApplication : Application
	{
		private readonly INavigationService _navigationService;

		public MainApplication(INavigationService navigationService)
		{
			InitializeComponent();

			_navigationService = navigationService;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			_navigationService.NavigateToViewModel<MainViewModel>();
		}
	}
}