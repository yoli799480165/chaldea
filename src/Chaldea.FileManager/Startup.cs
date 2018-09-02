using Chaldea.FileManager.Services;
using Chaldea.FileManager.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Chaldea.FileManager
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.Configure<Appsettings>(Configuration.GetSection("Appsettings"));
            services.AddMvcCore().AddJsonFormatters().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddApiExplorer();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"}); });
            services.AddSingleton<DirectoryManager>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Open API V1"); });
            app.UseCors(DefaultCorsPolicyName);
            app.UseHttpsRedirection();

//            app.UseStaticFiles(new StaticFileOptions
//            {
//                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "statics")),
//                RequestPath = "/statics"
//            });

            app.UseMvc();
        }
    }
}