using System;
using System.Linq;
using Chaldea.IdentityServer.Configuration;
using Chaldea.IdentityServer.Core;
using Chaldea.IdentityServer.Repositories;
using Chaldea.IdentityServer.Seettings;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chaldea.IdentityServer
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
            services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDB"));
            services.Configure<SmsServiceSettings>(Configuration.GetSection("SmsService"));
            services.AddTransient<ChaldeaDbContext>();
            services.AddTransient<SmsService>();
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                })
                .AddConfigurationStore(Configuration.GetSection("MongoDB"))
                .AddOperationalStore(Configuration.GetSection("MongoDB"))
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
            });
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
            var loggerFactory = new LoggerFactory();
            var cors = new DefaultCorsPolicyService(loggerFactory.CreateLogger<DefaultCorsPolicyService>())
            {
                AllowAll = true
            };
            services.AddSingleton<ICorsPolicyService>(cors);
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                EnsureSeedData(serviceScope.ServiceProvider.GetService<IConfigurationDbContext>());
            }

            EnsureDefaultUsers(app.ApplicationServices.GetRequiredService<IRepository<string, User>>());
            app.UseIdentityServer();
            app.UseIdentityServerMongoDBTokenCleanup(applicationLifetime);
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
        }

        private static void EnsureSeedData(IConfigurationDbContext context)
        {
            if (!context.Clients.Any())
                foreach (var client in Config.GetClients())
                    context.AddClient(client.ToEntity());

            if (!context.IdentityResources.Any())
                foreach (var resource in Config.GetIdentityResourceResources())
                    context.AddIdentityResource(resource.ToEntity());

            if (!context.ApiResources.Any())
                foreach (var resource in Config.GetApiResources())
                    context.AddApiResource(resource.ToEntity());
        }

        private static void EnsureDefaultUsers(IRepository<string, User> userRepository)
        {
            var count = userRepository.CountAsync(x => x.Name != "").Result;
            if (count <= 0) userRepository.AddManyAsync(Config.GetTestUsers().ToList());
        }
    }
}