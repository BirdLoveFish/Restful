using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Restful.Core;
using Serilog;
using Serilog.Events;

namespace Restful.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .CreateLogger();
            //Log.Information("Ah, there you are!");

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var loggerFactory = service.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = service.GetRequiredService<MyContext>();
                    MyDbContextSeed.Seed(context, loggerFactory);
                }
                catch (Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message,"db error");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            try
            {
                return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context,config)=>
                {
                    config.MinimumLevel.Debug();
                    config.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
                    config.Enrich.FromLogContext();
                    config.WriteTo.Console();
                    config.WriteTo.File("./restful.txt");
                });
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host builder error");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
            
    }
}
