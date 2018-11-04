using System.Linq;
using Chaldea.Core;
using Chaldea.Core.Repositories;
using Chaldea.IdentityServer.Configuration;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chaldea.IdentityServer
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
            var dbConfig = Configuration.GetSection("MongoDB");
            services.AddDataProvider(dbConfig);
            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseSuccessEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseErrorEvents = true;
                })
                .AddConfigurationStore(dbConfig)
                .AddOperationalStore(dbConfig)
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();

            var loggerFactory = new LoggerFactory();
            var cors = new DefaultCorsPolicyService(loggerFactory.CreateLogger<DefaultCorsPolicyService>())
            {
                AllowAll = true
            };
            services.AddSingleton<ICorsPolicyService>(cors);
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