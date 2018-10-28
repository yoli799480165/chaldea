using Chaldea.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chaldea.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddDataProvider(
            this IServiceCollection services,
            IConfigurationSection configuration)
        {
            services.Configure<DataProviderSettings>(configuration);
            services.AddTransient<ChaldeaDbContext>();
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            return services;
        }
    }
}