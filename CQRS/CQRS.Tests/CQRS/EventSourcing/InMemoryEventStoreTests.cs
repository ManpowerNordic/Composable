using Composable.CQRS.EventSourcing;
using NUnit.Framework;

namespace CQRS.Tests.CQRS.EventSourcing
{
    [TestFixture]
    class InMemoryEventStoreTests : EventStoreTests
    {
        protected override IEventStore CreateStore()
        {
            return new InMemoryEventStore();
        }
    }
}