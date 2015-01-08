using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestMultiBindAgain : TestWithContainer
    {
        private class Test0
        {
        }

        private class Test3 : Test0
        {
        }

        private class Test4 : Test0
        {
        }

        private class Test2
        {
            public Test0 test;

            public Test2(Test0 test)
            {
                this.test = test;
            }
        }

        private class Test1
        {
            public List<Test0> test;

            public Test1(List<Test0> test)
            {
                this.test = test;
            }
        }

        [Test]
        [ExpectedException]
        public void TestMultiBind2()
        {
            // Multi-binds should not map to single-binds
            Container.Bind<Test0>().ToSingle<Test3>();
            Container.Bind<Test0>().ToSingle<Test4>();
            Container.Bind<Test2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            Container.Resolve<Test2>();
        }
    }
}


