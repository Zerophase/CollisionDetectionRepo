using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestParameters : TestWithContainer
    {
        private class Test1
        {
            public int f1;
            public int f2;

            public Test1(int f1, int f2)
            {
                this.f1 = f1;
                this.f2 = f2;
            }
        }

        [Test]
        public void TestExtraParametersSameType()
        {
            var factory1 = new FactoryUntyped<Test1>(Container);
            var test1 = factory1.Create(5, 10);

            TestAssert.That(test1 != null);
            TestAssert.That(test1.f1 == 5 && test1.f2 == 10);

            var factory2 = new FactoryUntyped<Test1>(Container);
            var test2 = factory2.Create(10, 5);

            TestAssert.That(test2 != null);
            TestAssert.That(test2.f1 == 10 && test2.f2 == 5);
        }

        [Test]
        public void TestMissingParameterThrows()
        {
            Container.Bind<Test1>().ToTransient();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());
        }
    }
}


