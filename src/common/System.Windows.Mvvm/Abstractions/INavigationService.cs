using System;
using System.Windows;

namespace System.Windows.Mvvm
{
	public interface INavigationService
	{
		bool NavigateToViewModel<TViewModel>() where TViewModel : IViewModel;
	}
}