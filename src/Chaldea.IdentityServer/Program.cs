using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Conventions;

namespace Chaldea.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    var loggingConfig = hostingContext.Configuration.GetSection("Logging");
                    logging.AddFile(loggingConfig.GetSection("Serilog"));
                })
                .UseStartup<Startup>();
        }
    }
}