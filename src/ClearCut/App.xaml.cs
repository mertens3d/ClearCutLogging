using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

      //CreateHostBuilder().Build().Run();

      var services = new ServiceCollection();

      //var host = new HostBuilder()
      //.ConfigureServices((hostConect, services) =>
      //{
      //  services.AddSingleton<MainWindow>();
      //})
      //.ConfigureLogging(logBuilder =>
      //{
      //  logBuilder.SetMinimumLevel(LogLevel.Trace);
      //  logBuilder.AddLog4Net("log4net.config");
      //})
      //.Build();

      //services.AddScoped(factory => LogManager.GetLogger(GetType()));

      //using (var serviceScope = host.Services.CreateScope())
      //{
      //  var services = serviceScope.ServiceProvider;
      //  var logger = services.GetRequiredService<ILogger>();
      //  var masterWindow = services.GetRequiredService<MainWindow>();
      //  //masterWindow.Show();

      //}


      ILogger log = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.File(
                  path: "logs/ClearCut..log", 
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
            rollingInterval: RollingInterval.Day, 
            rollOnFileSizeLimit: true )
        .CreateLogger();
      

      var ioc = new WindsorContainer();

      ioc.Register(Castle.MicroKernel.Registration.Component.For<ILogger>().Instance(log));

      ioc.Register(Castle.MicroKernel.Registration.Component.For<MainWindow>().ImplementedBy<MainWindow>());


      var window = ioc.Resolve<MainWindow>();
      window.ShowDialog();

    }

    //  private IHostBuilder CreateHostBuilder() =>
    //    Host.CreateDefaultBuilder()
    //       .ConfigureLogging(logging =>
    //       {
    //         logging.ClearProviders();
    //         logging.AddConsole();
    //       });

  }
}