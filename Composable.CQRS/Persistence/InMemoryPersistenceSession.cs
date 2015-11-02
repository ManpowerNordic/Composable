using System;
using System.Collections.Generic;
using System.Linq;
using Composable.DDD;
using Composable.KeyValueStorage;

namespace Composable.Persistence
{
    public class InMemoryPersistenceSession : IQueryablePersistenceSession
    {
        private HashSet<IHasPersistentIdentity<Guid>> _db = new HashSet<IHasPersistentIdentity<Guid>>();
        public void Dispose()
        {
            _db = null;
        }

        public IQueryable<T> Query<T>()
        {
            return _db.OfType<T>().AsQueryable();
        }

        public T Get<T>(Guid id)
        {
            T value;
            if(!TryGet(id, out value))
            {
                throw new NoSuchDocumentException(id, typeof(T));
            }
            return value;
        }

        public bool TryGet<T>(Guid id, out T value)
        {
            value = _db.Where(entity => entity.Id == id).Cast<T>().SingleOrDefault();
            return value != null;
        }        

        public TValue GetForUpdate<TValue>(Guid key) => Get<TValue>(key);
        public bool TryGetForUpdate<TValue>(Guid key, out TValue value) => TryGet(key, out value);

        public void Save<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>
        {
            _db.Add(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>
        {
            _db.Remove(entity);             
        }
    }
}