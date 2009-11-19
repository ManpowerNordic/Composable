using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Void.Hierarchies;
using Void.Linq;

namespace Core.Tests.Linq
{
    [TestFixture]
    public class HierarchyTests
    {
        private class Hierarchical
        {
            public Hierarchical[] Children = new Hierarchical[0];
        }

        [Test]
        public void ShouldReturnAllInstancesInGraphWithoutDuplicates()
        {
            var root1 = new Hierarchical
                        {
                            Children = new[]
                                       {
                                           new Hierarchical
                                           {
                                               Children = new[]
                                                          {
                                                              new Hierarchical(),
                                                              new Hierarchical()
                                                          }
                                           },
                                           new Hierarchical()
                                       }
                        };
            var root2 = new Hierarchical
                        {
                            Children = new[]
                                       {
                                           new Hierarchical
                                           {
                                               Children = new[]
                                                          {
                                                              new Hierarchical(),
                                                              new Hierarchical()
                                                          }
                                           },
                                           new Hierarchical()
                                       }
                        };

            var flattened = Seq.Create(root1, root2).FlattenHierarchy(root => root.Children);
            Assert.That(flattened.Count(), Is.EqualTo(10)); //Ensures no duplicates
            Assert.That(flattened.Distinct().Count(), Is.EqualTo(10)); //Ensures all objects are there.
        }


        private class Person : IHierarchy<Person>
        {
            public Person()
            {
                Children = new List<Person>();
            }
            IEnumerable<IHierarchy<Person>> IHierarchy<Person>.Children { get { return Children.Cast<IHierarchy<Person>>(); } }
            Person IHierarchy<Person>.Value { get { return this; } }

            public IList<Person> Children { get; set; }
        }


        [Test]
        public void FlatteningAHierarchicalTypeShouldWork()
        {
            var family = new Person
                         {
                             Children = new List<Person>
                                        {
                                            new Person
                                            {
                                                Children = new List<Person>
                                                           {
                                                               new Person(),
                                                               new Person()
                                                           }
                                            },
                                            new Person()
                                        }
                         };
            var familyRegister = family.Flatten();
            Assert.That(familyRegister.Count(), Is.EqualTo(5), "Should have 5 persons in the list");
            Assert.That(familyRegister.Count(), Is.EqualTo(5), "Should have 5 unique persons in the list");
        }
    }
}