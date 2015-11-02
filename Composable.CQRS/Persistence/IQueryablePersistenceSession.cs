using System.Linq;

namespace Composable.Persistence
{
    public interface IQueryablePersistenceSession : IPersistenceSession
    {
        IQueryable<T> Query<T>();
    }
}