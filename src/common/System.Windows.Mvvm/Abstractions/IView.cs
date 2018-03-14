using System;
using System.Windows;

namespace System.Windows.Mvvm
{
	public interface IView
	{
		event RoutedEventHandler Loaded;

		event RoutedEventHandler Unloaded;

		object DataContext { get; set; }
	}
}