using System;
using System.Windows;

namespace System.Windows.Mvvm
{
	public abstract class ViewAware : PropertyChangedBase, IViewAware
	{
		private IView _attachedView;

		public IView AttachedView => _attachedView;

		protected internal ViewAware() : base() { }

		public void AttachView(IView view)
		{
			_attachedView = view ?? throw new ArgumentNullException(nameof(view));

			OnActivating();

			view.DataContext = this;
			view.Loaded += HandleViewLoaded;
		}

		private void HandleViewLoaded(object sender, RoutedEventArgs args)
		{
			_attachedView.Loaded -= HandleViewLoaded;

			OnViewLoaded(_attachedView);
		}

		private void HandleViewUnloaded(object sender, RoutedEventArgs args)
		{
			_attachedView.Unloaded -= HandleViewUnloaded;

			OnViewUnloaded();
		}

		protected virtual void OnActivating() { }

		protected virtual void OnViewLoaded(IView view) { }

		protected virtual void OnViewUnloaded() { }
	}
}