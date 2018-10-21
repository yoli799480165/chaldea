using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Chaldea.Node
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);
            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
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