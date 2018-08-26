using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Conventions;

namespace Chaldea
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var conventionPack = new ConventionPack {new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var host = CreateWebHostBuilder(args);
            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var loggingConfig = hostingContext.Configuration.GetSection("Logging");
                    logging.AddFile(loggingConfig.GetSection("Serilog"));
                })
                .UseStartup<Startup>()
                .Build();
        }
    }
}