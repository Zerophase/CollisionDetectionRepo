using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestNullableValues : TestWithContainer
    {
        private class Test2
        {
            public int? val;

            public Test2(int? val)
            {
                this.val = val;
            }
        }

        [Test]
        public void RunTest1()
        {
            Container.Bind<Test2>().ToSingle();
            Container.BindValue<int>().To(1);

            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            var test1 = Container.Resolve<Test2>();
            TestAssert.AreEqual(test1.val, 1);
        }
    }
}
