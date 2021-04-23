using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Windows;

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
                  path: "logs/ClearCut..log",
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true)
        .CreateLogger();

      var ioc = new WindsorContainer();

      ioc.Register(Castle.MicroKernel.Registration.Component.For<ILogger>().Instance(log));

      ioc.Register(Castle.MicroKernel.Registration.Component.For<MainWindow>().ImplementedBy<MainWindow>());

      var window = ioc.Resolve<MainWindow>();
      window.ShowDialog();
    }
  }
}