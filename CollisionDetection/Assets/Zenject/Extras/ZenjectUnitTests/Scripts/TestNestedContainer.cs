using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestNestedContainer : TestWithContainer
    {
        public interface IFoo
        {
            int GetBar();
        }

        public class Foo : IFoo
        {
            public int GetBar()
            {
                return 0;
            }
        }

        public class Foo2 : IFoo
        {
            public int GetBar()
            {
                return 1;
            }
        }

        [Test]
        public void TestCase1()
        {
            var nestedContainer = new DiContainer();

            TestAssert.Throws<ZenjectResolveException>(() => nestedContainer.Resolve<IFoo>());
            TestAssert.Throws<ZenjectResolveException>(() => Container.Resolve<IFoo>());

            Container.Bind<IFoo>().ToSingle<Foo>();

            TestAssert.Throws<ZenjectResolveException>(() => nestedContainer.Resolve<IFoo>());
            TestAssert.AreEqual(Container.Resolve<IFoo>().GetBar(), 0);

            nestedContainer.FallbackProvider = new DiContainerProvider(Container);

            TestAssert.AreEqual(nestedContainer.Resolve<IFoo>().GetBar(), 0);
            TestAssert.AreEqual(Container.Resolve<IFoo>().GetBar(), 0);

            nestedContainer.Bind<IFoo>().ToSingle<Foo2>();

            TestAssert.AreEqual(nestedContainer.Resolve<IFoo>().GetBar(), 1);
            TestAssert.AreEqual(Container.Resolve<IFoo>().GetBar(), 0);
        }
    }
}
