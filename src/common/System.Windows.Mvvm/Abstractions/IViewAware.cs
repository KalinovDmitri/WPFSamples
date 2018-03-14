using System;

namespace System.Windows.Mvvm
{
	public interface IViewAware
	{
		IView AttachedView { get; }

		void AttachView(IView view);
	}
}