using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Chaldea.Core.Repositories
{
    public class ChaldeaDbContext
    {
        private readonly IMongoDatabase _database;

        public ChaldeaDbContext(IOptions<DataProviderSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<TEntity> Set<TEntity>()
        {
            return _database.GetCollection<TEntity>($"{typeof(TEntity).Name.ToLower()}s");
        }
    }
}