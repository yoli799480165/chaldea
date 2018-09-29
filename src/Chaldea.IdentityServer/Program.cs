using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Chaldea.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
#if DEBUG
                .UseUrls("http://*:9002")
#endif
                .UseStartup<Startup>();
        }
    }
}