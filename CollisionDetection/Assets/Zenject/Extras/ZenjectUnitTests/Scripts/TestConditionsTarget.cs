using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestConditionsTarget : TestWithContainer
    {
        private class Test0
        {
        }

        private class Test1
        {
            public Test1(Test0 test)
            {
            }
        }

        private class Test2
        {
            public Test2(Test0 test)
            {
            }
        }

        public override void Setup()
        {
            base.Setup();
            Container.Bind<Test0>().ToSingle().When(r => r.EnclosingType == typeof(Test2));
        }

        [Test]
        public void TestTargetConditionError()
        {
            Container.Bind<Test1>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());
        }

        [Test]
        public void TestTargetConditionSuccess()
        {
            Container.Bind<Test2>().ToSingle();
            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            var test2 = Container.Resolve<Test2>();

            TestAssert.That(test2 != null);
        }
    }
}


