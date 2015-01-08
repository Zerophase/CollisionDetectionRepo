using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestTransientInjection : TestWithContainer
    {
        private class Test1
        {
        }

        [Test]
        public void TestTransientType()
        {
            Container.Bind<Test1>().ToTransient();

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());

            var test1 = Container.Resolve<Test1>();
            var test2 = Container.Resolve<Test1>();

            TestAssert.That(test1 != null && test2 != null);
            TestAssert.That(!ReferenceEquals(test1, test2));
        }

        [Test]
        public void TestTransientTypeUntyped()
        {
            Container.Bind(typeof(Test1)).ToTransient();

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());

            var test1 = Container.Resolve<Test1>();
            var test2 = Container.Resolve<Test1>();

            TestAssert.That(test1 != null && test2 != null);
            TestAssert.That(!ReferenceEquals(test1, test2));
        }
    }
}


