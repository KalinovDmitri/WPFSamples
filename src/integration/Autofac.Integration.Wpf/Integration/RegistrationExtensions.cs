using Autofac.Builder;
using Autofac.Core;
using System;
using System.Windows.Mvvm;
using System.Windows.Navigation;

namespace Autofac.Integration.Wpf
{
	public static class RegistrationExtensions
	{
		public static void RegisterNavigationService(this ContainerBuilder builder, NavigationService navigationService)
		{
			builder.RegisterInstance(navigationService);

			builder
				.Register(CreateCustomNavigationService)
				.As<INavigationService>()
				.SingleInstance();
		}

		public static void Bind<TViewModel, TView>(this ContainerBuilder builder,
			InstanceLifetime viewModelLifetime = InstanceLifetime.Single,
			InstanceLifetime viewLifetime = InstanceLifetime.Single) where TViewModel : IViewModel where TView : IView
		{
			builder
				.RegisterType<TView>()
				.As<IView>()
				.AsSelf()
				.ApplyInstanceLifetime(viewLifetime);

			builder
				.RegisterType<TViewModel>()
				.AsImplementedInterfaces()
				.AsSelf()
				.ApplyInstanceLifetime(viewModelLifetime)
				.OnActivating(HandleViewModelActivating);

			void HandleViewModelActivating(IActivatingEventArgs<TViewModel> args)
			{
				IView boundedView = args.Context.Resolve<TView>();

				args.Instance.AttachView(boundedView);
			}
		}

		private static INavigationService CreateCustomNavigationService(IComponentContext context)
		{
			var lifetimeScope = context.Resolve<ILifetimeScope>();
			var navigationService = context.Resolve<NavigationService>();

			var service = new DefaultNavigationService(lifetimeScope, navigationService);
			return service;
		}

		private static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ApplyInstanceLifetime<TLimit, TActivatorData, TRegistrationStyle>(
			this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
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
					throw new ArgumentOutOfRangeException(
						paramName: nameof(instanceLifetime),
						actualValue: instanceLifetime,
						message: "Unknown lifetime type.");
			}

			return builder;
		}
	}
}