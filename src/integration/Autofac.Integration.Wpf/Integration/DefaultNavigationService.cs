using System;
using System.Windows.Mvvm;
using System.Windows.Navigation;

namespace Autofac.Integration.Wpf
{
	internal class DefaultNavigationService : INavigationService
	{
		private readonly ILifetimeScope _lifetimeScope;
		private readonly NavigationService _navigationService;

		internal DefaultNavigationService(ILifetimeScope lifetimeScope, NavigationService navigationService)
		{
			_lifetimeScope = lifetimeScope;
			_navigationService = navigationService;
		}

		public bool NavigateToViewModel<TViewModel>() where TViewModel : IViewModel
		{
			var viewModel = _lifetimeScope.Resolve<TViewModel>();

			return _navigationService.Navigate(viewModel.AttachedView);
		}
	}
}