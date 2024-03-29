﻿using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Windows;
using ClearCut.Main;
using ClearCut.Support.Abstractions;
using ClearCut.Support.Settings.Services;
using ClearCut.Support.Witness.Services;
using Foundation;

namespace ClearCut
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      var services = new ServiceCollection();
            ILogger log = new LoggerConfiguration()
              .Enrich.FromLogContext()
              .WriteTo.File(
                  path: Constants.Logging.Path,
                  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                  rollingInterval: RollingInterval.Day,
                  rollOnFileSizeLimit: true)
              .CreateLogger();
              //.ForContext<SerilogExtensions>();

      var ioc = new WindsorContainer();

      ioc.Register(Castle.MicroKernel.Registration.Component.For<ILogger>().Instance(log));
      ioc.Register(Castle.MicroKernel.Registration.Component.For<ISettingsManager>().ImplementedBy<SettingsManager>());
      ioc.Register(Castle.MicroKernel.Registration.Component.For<IWitnessManager>().ImplementedBy<WitnessManager>());
      ioc.Register(Castle.MicroKernel.Registration.Component.For<MainWindow>().ImplementedBy<MainWindow>());

      var window = ioc.Resolve<MainWindow>();
      window.ShowDialog();
    }
  }
}