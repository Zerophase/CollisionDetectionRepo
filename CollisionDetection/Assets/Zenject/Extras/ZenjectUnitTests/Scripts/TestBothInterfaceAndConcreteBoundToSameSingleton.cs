using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestBothInterfaceAndConcreteBoundToSameSingleton : TestWithContainer
    {
        private abstract class Test0
        {
        }

        private class Test1 : Test0
        {
        }

        [Test]
        public void TestCaseBothInterfaceAndConcreteBoundToSameSingleton()
        {
            Container.Bind<Test0>().ToSingle<Test1>();
            Container.Bind<Test1>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test0>().IsEmpty());
            var test1 = Container.Resolve<Test0>();

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            var test2 = Container.Resolve<Test1>();

            TestAssert.That(ReferenceEquals(test1, test2));
        }
    }
}


