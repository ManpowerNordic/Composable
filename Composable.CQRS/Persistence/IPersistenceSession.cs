#region usings

using System;
using System.Diagnostics.Contracts;
using Composable.CQRS;

#endregion

namespace Composable.Persistence
{
    public interface IPersistenceSession : IEntityFetcher, IEntityPersister, IDisposable
    {
    }
}