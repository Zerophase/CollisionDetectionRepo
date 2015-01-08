using System;
using TestAssert=NUnit.Framework.Assert;
using NUnit.Framework;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestUnbind : TestWithContainer
    {
        interface IFoo
        {
        }

        class Foo : IFoo
        {
        }

        [Test]
        public void TestCase1()
        {
            Container.Bind<IFoo>().ToSingle<Foo>();

            TestAssert.That(Container.ValidateResolve<IFoo>().IsEmpty());

            Container.Unbind<IFoo>();

            TestAssert.That(!Container.ValidateResolve<IFoo>().IsEmpty());
        }
    }
}
