#region usings

using System;
using System.Linq;
using Composable.DDD;
using Composable.KeyValueStorage;
using NHibernate;
using NHibernate.Linq;
using Composable.Persistence;

#endregion

namespace Composable.CQRS.NHibernate
{
    public class NHibernatePersistenceSession : IQueryablePersistenceSession
    {
        public NHibernatePersistenceSession(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }

        public T Get<T>(Guid id) { return InternalGet<T>(id, null); }

        private T InternalGet<T>(Guid id, LockMode lockMode)
        {
            T value;
            if(!InternalTryGet(id, out value, lockMode))
            {
                throw new NoSuchDocumentException(id, typeof(T));
            }
            return value;
        }

        public bool TryGet<TEntity>(Guid key, out TEntity document)
        {
            return InternalTryGet(key, out document, null);
        }

        private bool InternalTryGet<TEntity>(Guid key, out TEntity document, LockMode lockMode)
        {
            document = Session.Get<TEntity>(key, lockMode);
            return document != null;
        }

        public TValue GetForUpdate<TValue>(Guid key) { return InternalGet<TValue>(key, LockMode.Write); }
        public bool TryGetForUpdate<TValue>(Guid key, out TValue value) => InternalTryGet(key, out value, LockMode.Write);

        public void Save<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>
        {
            Session.Save(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : IHasPersistentIdentity<Guid>
        {
            Session.Delete(entity);
        }

        public void Clear()
        {
            Session.Clear();
        }


        #region Implementation of IDisposable


        public void Dispose()
        {
            Session.Dispose();
        }

        #endregion
    }
}