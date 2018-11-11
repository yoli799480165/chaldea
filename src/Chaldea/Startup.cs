using System;
using System.IO;
using System.Threading.Tasks;
using Chaldea.Core;
using Chaldea.Infrastructure.Sms.Aliyun;
using Chaldea.Services;
using Chaldea.Services.Nodes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Chaldea
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
            services.AddSignalR();
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.EnableCaching = true;
                    options.CacheDuration = TimeSpan.FromMinutes(15);
                    options.Authority = Configuration.GetSection("Authentication:IdentityServerUrl").Value;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                    options.JwtBearerEvents.OnAuthenticationFailed += context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.StatusCode = 401;
                        }
                        return Task.CompletedTask;
                    };
                });
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "My API", Version = "v1"}); });
            services.AddDataProvider(Configuration.GetSection("MongoDB"));
            services.AddSms(cfg => { cfg.Config = Configuration.GetSection("SmsService"); });
            services.AddSingleton<EventManager>();
            services.AddSingleton<NodeManager>();
            services.AddSingleton<NodeProxy>();
            Mappings.Initialize();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Open API V1"); });
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "statics")),
                RequestPath = "/statics"
            });
            app.UseSignalR(route => { route.MapHub<NodeHub>("/node"); });
            app.UseMvc();
        }
    }
}