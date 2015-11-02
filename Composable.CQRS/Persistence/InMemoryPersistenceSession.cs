using System;
using System.Collections.Generic;
using System.Linq;
using Composable.DDD;
using Composable.KeyValueStorage;

namespace Composable.Persistence
{
    public class InMemoryPersistenceSession : IPersistenceSession
    {
        private HashSet<object> _db = new HashSet<object>();
        public void Dispose()
        {
            _db = null;
        }

        public IQueryable<T> Query<T>()
        {
            return _db.OfType<T>().AsQueryable();
        }

        public T Get<T>(object id)
        {
            T value;
            if(!TryGet(id, out value))
            {
                throw new NoSuchDocumentException(id, typeof(T));
            }
            return value;
        }

        private static bool ValuesEqual(dynamic instance, dynamic id)
        {
            return instance.Id == id;
        }

        public bool TryGet<T>(object id, out T value)
        {
            value = _db.OfType<IPersistentEntity<Guid>>().Where(entity => entity.Id == (Guid)id).Cast<T>().SingleOrDefault();
            return value != null;
        }

        public void Save(object instance)
        {
            _db.Add(instance);
        }

        public void Delete(object instance)
        {
            _db.Remove(instance);
        }
    }
}