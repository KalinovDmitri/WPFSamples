using System;
using System.Windows;
using System.Windows.Mvvm;
using System.Windows.Navigation;

using Autofac.Builder;
using Autofac.Core;

namespace Autofac.Integration.Wpf
{
	public static class RegistrationExtensions
	{
		public static void RegisterNavigationService(this ContainerBuilder builder, NavigationService navigationService)
		{
			builder.RegisterInstance(navigationService);
			builder.Register(CreateNavigationService)
				.As<INavigationService>()
				.SingleInstance();
		}

		public static void Bind<TViewModel, TView>(this ContainerBuilder builder,
			InstanceLifetime viewModelLifetime = InstanceLifetime.Single,
			InstanceLifetime viewLifetime = InstanceLifetime.Single) where TViewModel : IViewModel where TView : IView
		{
			var viewBuilder = builder.RegisterType<TView>()
				.As<IView>()
				.AsSelf();
			ApplyInstanceLifetime(viewBuilder, viewLifetime);

			var activator = new ViewModelActivator<TViewModel, TView>();

			var viewModelBuilder = builder.RegisterType<TViewModel>()
				.AsImplementedInterfaces()
				.AsSelf()
				.OnActivating(activator.Activate);
			ApplyInstanceLifetime(viewModelBuilder, viewModelLifetime);
		}
		
		private static INavigationService CreateNavigationService(IComponentContext context)
		{
			var lifetimeScope = context.Resolve<ILifetimeScope>();
			var navigationService = context.Resolve<NavigationService>();

			var service = new DefaultNavigationService(lifetimeScope, navigationService);
			return service;
		}

		private static void ApplyInstanceLifetime<TLimit, TActivatorData, TRegistrationStyle>(
			IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
			InstanceLifetime instanceLifetime)
		{
			if (builder == null)
			{
				throw new ArgumentNullException(nameof(builder), "Registration builder cannot be null.");
			}

			switch (instanceLifetime)
			{
				case InstanceLifetime.Single:
					builder.SingleInstance();
					break;
				case InstanceLifetime.Scoped:
					builder.InstancePerLifetimeScope();
					break;
				case InstanceLifetime.Transient:
					builder.InstancePerDependency();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(instanceLifetime), "Unknown lifetime type.");
			}
		}
	}
}