using Chaldea.Node.Configuration;
using Chaldea.Node.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chaldea.Node
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Appsettings>(Configuration.GetSection("Appsettings"));
            services.Configure<BaiduDiskSettings>(Configuration.GetSection("BaiduDisk"));
            services.AddMvcCore()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddApiExplorer();

            services.AddSingleton<NodeAgent>();
            services.AddTransient<DirectoryService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            var nodeAhent = app.ApplicationServices.GetService<NodeAgent>();
            nodeAhent.Connect();
            app.UseMvc();
        }
    }
}