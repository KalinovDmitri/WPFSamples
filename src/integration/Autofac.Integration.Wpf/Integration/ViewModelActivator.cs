using System;
using System.Collections.Generic;
using System.Windows.Mvvm;

using Autofac.Core;

namespace Autofac.Integration.Wpf
{
	internal interface IViewModelActivator { }

	internal class ViewModelActivator<TViewModel, TView> : IViewModelActivator where TViewModel : IViewModel where TView : IView
	{
		private static List<IViewModelActivator> _createdActivators;

		static ViewModelActivator()
		{
			_createdActivators = new List<IViewModelActivator>(4);
		}

		public ViewModelActivator()
		{
			_createdActivators.Add(this);
		}

		public void Activate(IActivatingEventArgs<TViewModel> args)
		{
			IView boundedView = args.Context.Resolve<TView>();

			args.Instance.AttachView(boundedView);
		}
	}
}