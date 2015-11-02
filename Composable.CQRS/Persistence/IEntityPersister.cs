using System;
using Composable.DDD;

namespace Composable.Persistence
{
    public interface IEntityPersister
    {
        void Save<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>;
        void Delete<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>;

        /// <summary>Like Get but, if supported by implementing class, eagerly locks the instance in the database.</summary>
        TValue GetForUpdate<TValue>(Guid id);

        /// <summary>Like TryGet but, if supported by implementing class, eagerly locks the instance in the database.</summary>
        bool TryGetForUpdate<TValue>(Guid id, out TValue value);
    }
}