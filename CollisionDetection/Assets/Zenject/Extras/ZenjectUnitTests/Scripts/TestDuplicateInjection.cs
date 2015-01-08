using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestDuplicateInjection : TestWithContainer
    {
        private class Test0
        {
        }

        private class Test1
        {
            public Test1(Test0 test1)
            {
            }
        }

        [Test]
        public void TestCaseDuplicateInjection()
        {
            Container.Bind<Test0>().ToSingle();
            Container.Bind<Test0>().ToSingle();

            Container.Bind<Test1>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());
        }
    }
}


