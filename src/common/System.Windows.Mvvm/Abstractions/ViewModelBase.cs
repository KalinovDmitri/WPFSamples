using System;

namespace System.Windows.Mvvm
{
	public abstract class ViewModelBase : ViewAware, IViewModel
	{
		protected internal ViewModelBase() : base() { }
	}
}