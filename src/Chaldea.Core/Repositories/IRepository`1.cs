using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Chaldea.Core.Repositories
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public interface IRepository<in TKey, TEntity>
    {
        Task AddAsync(TEntity entity);

        Task AddManyAsync(List<TEntity> entities);

        Task UpdateAsync(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TKey id);

        Task<long> DeleteManyAsync(FilterDefinition<TEntity> filter);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));

        IFindFluent<TEntity, TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);

        IFindFluent<TEntity, TEntity> GetAll(FilterDefinition<TEntity> filter);

        Task<List<TEntity>> GetAllListAsync();

        Task<List<TEntity>> GetAllListAsync(ProjectionDefinition<TEntity> filter);

        Task<List<TEntity>> GetAllListAsync(FilterDefinition<TEntity> filter);

        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetAsync(TKey id);

        IAsyncCursor<TResult> Aggregate<TResult>(PipelineDefinition<TEntity, TResult> pipeline);
    }

    public interface IRepository<TEntity> : IRepository<int, TEntity>
    {
    }
}