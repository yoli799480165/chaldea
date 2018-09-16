using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Chaldea.Repositories
{
    public class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>, new()
    {
        private readonly IMongoCollection<TEntity> _db;

        public Repository(ChaldeaDbContext db)
        {
            _db = db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _db.InsertOneAsync(entity);
        }

        public async Task AddManyAsync(List<TEntity> entities)
        {
            await _db.InsertManyAsync(entities);
        }

        public async Task UpdateAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            await _db.UpdateOneAsync(filter, update);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await _db.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new UpdateOptions {IsUpsert = true});
        }

        public async Task DeleteAsync(TKey id)
        {
            await _db.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _db.CountDocumentsAsync(predicate, cancellationToken: cancellationToken);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _db.Find(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await _db.Find(_ => true).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(ProjectionDefinition<TEntity> filter)
        {
            return await _db.Find(_ => true).Project<TEntity>(filter).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(FilterDefinition<TEntity> filter)
        {
            return await _db.Find(filter).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _db.Find(predicate).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetAsync(TKey id)
        {
            return await _db.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public IFindFluent<TEntity, TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _db.Find(predicate ?? (_ => true));
        }
    }
}