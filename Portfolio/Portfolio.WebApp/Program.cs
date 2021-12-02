using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Portfolio.WebApp.Services;
using System;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Portfolio.WebApp
{
  public class Program
  {
    public static void Main(string[] args)
    {

      var host = BuildWebHost(args);

      using (var scope = host.Services.CreateScope())
      {

        try
        {
          var context = scope.ServiceProvider.GetService<PortfolioContext>();
          context.Database.Migrate();
          context.EnsureSeedDataForContext();
        }
        catch (Exception ex)
        {

          var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
          logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        }
      }

      host.Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args)

.ConfigureKestrel(options =>
{
  options.Limits.MaxConcurrentConnections = 100;
  options.Limits.MaxConcurrentUpgradedConnections = 100;
  options.Limits.MaxRequestBodySize = 10 * 1024;
  options.Limits.MinRequestBodyDataRate =
          new MinDataRate(bytesPerSecond: 100,
          gracePeriod: TimeSpan.FromSeconds(10));
  options.Limits.MinResponseDataRate =
          new MinDataRate(bytesPerSecond: 100,
          gracePeriod: TimeSpan.FromSeconds(10));
  options.Listen(IPAddress.Any, 80);
  options.Listen(IPAddress.Any, 443
               , listenOptions => listenOptions.UseHttps("/https/webapi.pfx", "password") //dev
              );
  options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
  options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
})
.UseStartup<Startup>()

// .UseUrls("http://localhost:45004", "https://localhost:55004")
.Build();
    }
  }
}
