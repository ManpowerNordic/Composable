using System.Collections.Generic;
using NHibernate.ByteCode.LinFu;
using NUnit.Framework;
using Void.Data.ORM.NHibernate;
using Void.Data.ORM.NHibernateRepositories.Tests.Domain;

namespace Void.Data.ORM.NHibernateRepositories.Tests.Given.ADetachedOrderWithDeletedOrderlines
{
    [TestFixture]
    public class WhenCallingSaveOrUpdate : NhibernateRepositoryTest
    {
        [Test]
        public void TheDeletedOrderLinesShouldBeRemovedFromTheDatabase()
        {
            Order order = new Order
                          {
                              Lines = new List<OrderLine>
                                      {
                                          new OrderLine(),
                                          new OrderLine(),
                                          new OrderLine(),
                                          new OrderLine()
                                      }
                          };

            using (var session = new InMemoryNHibernatePersistanceSession<ProxyFactoryFactory>())
            {
                var repo = new TransactionalRepository<Order, int>(session);
                repo.SaveOrUpdate(order);

                session.Clear();

                order.Lines.RemoveAt(2);

                repo.SaveOrUpdate(order);

                session.Clear();

                var loadedOrder = repo.Get(order.PersistentId);
                Assert.That(order.Lines, Is.EquivalentTo(loadedOrder.Lines));
            }
        }
    }
}