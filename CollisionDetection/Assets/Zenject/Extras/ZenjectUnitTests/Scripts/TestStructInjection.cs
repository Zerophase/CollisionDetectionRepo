using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestStructInjection : TestWithContainer
    {
        private struct Test1
        {
        }

        private class Test2
        {
            public Test2(Test1 t1)
            {
            }
        }

        [Test]
        public void TestCase1()
        {
            Container.BindValue<Test1>().To(new Test1());
            Container.Bind<Test2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            var t2 = Container.Resolve<Test2>();

            TestAssert.That(t2 != null);
        }
    }
}

