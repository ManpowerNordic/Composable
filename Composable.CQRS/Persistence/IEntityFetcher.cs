using System.Linq;

namespace Composable.Persistence
{
    public interface IEntityFetcher
    {
        TEntity Get<TEntity>(object entityId);
        bool TryGet<TEntity>(object key, out TEntity document);
    }
}