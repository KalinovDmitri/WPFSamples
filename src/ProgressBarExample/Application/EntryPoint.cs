﻿using System;
using System.Windows;
using System.Windows.Mvvm;
using System.Windows.Navigation;

using Autofac;
using Autofac.Integration.Wpf;

using MahApps.Metro;
using MahApps.Metro.Controls;

using ProgressBarExample.Models;
using ProgressBarExample.ViewModels;
using ProgressBarExample.Views;

namespace ProgressBarExample
{
	internal class EntryPoint
	{
		[STAThread]
		internal static int Main(string[] args)
		{
			var rootWindow = new MainWindow();

			using (IContainer container = BuildContainer(rootWindow.NavigationService))
			{
				container.Resolve<MainApplication>().Run(rootWindow);
			}

			return 0;
		}

		private static IContainer BuildContainer(NavigationService navigationService)
		{
			var builder = new ContainerBuilder();

			builder.RegisterNavigationService(navigationService);

			builder.RegisterType<MainApplication>();

			builder.RegisterType<ApplicationModel>()
				.SingleInstance();

			builder.Bind<MainViewModel, MainView>();

			return builder.Build();
		}
	}
}