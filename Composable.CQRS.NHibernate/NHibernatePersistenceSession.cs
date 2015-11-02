#region usings

using System.Linq;
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

        public T Get<T>(object id)
        {
            T value;
            if (!TryGet(id, out value))
            {
                throw new NoSuchDocumentException(id, typeof(T));
            }
            return value;
        }

        public bool TryGet<TEntity>(object key, out TEntity document)
        {
            document = Session.Get<TEntity>(key);
            return document != null;
        }

        public void Save(object instance)
        {
            Session.Save(instance);
        }

        public void Delete(object instance)
        {
            Session.Delete(instance);
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