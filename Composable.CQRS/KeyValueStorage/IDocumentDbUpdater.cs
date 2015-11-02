using System;
using Composable.DDD;

namespace Composable.KeyValueStorage
{
    public interface IDocumentDbUpdater : IDocumentUpdatedNotifier
    {
        /// <summary>Like Get but, if supported by implementing class, eagerly locks the instance in the database.</summary>
        TValue GetForUpdate<TValue>(object key);

        /// <summary>Like TryGet but , if supported by implementing class, eagerly locks the instance in the database.</summary>
        bool TryGetForUpdate<TValue>(object key, out TValue value);

        [Obsolete("Please do not use this horrible hack. Will be removed in the future.")]
        void Save<TValue>(object id, TValue value);
        void Delete<TEntity>(object id);
        void Save<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>;
        void Delete<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>;
        void SaveChanges();
    }
}