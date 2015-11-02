using System;
using System.Linq;

namespace Composable.Persistence
{
    public interface IEntityFetcher
    {
        TEntity Get<TEntity>(Guid entityId);
        bool TryGet<TEntity>(Guid key, out TEntity document);
    }
}