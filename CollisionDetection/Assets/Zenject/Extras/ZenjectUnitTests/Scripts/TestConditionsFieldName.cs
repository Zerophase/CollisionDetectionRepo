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
    public class TestConditionsFieldName : TestWithContainer
    {
        class Test0
        {

        }

        class Test1
        {
            public Test1(Test0 name1)
            {
            }
        }

        class Test2
        {
            public Test2(Test0 name2)
            {
            }
        }

        public override void Setup()
        {
            base.Setup();
            Container.Bind<Test0>().ToSingle().When(r => r.SourceName == "name1");
        }

        [Test]
        public void TestNameConditionError()
        {
            Container.Bind<Test2>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test2>(); });

            TestAssert.That(Container.ValidateResolve<Test2>().Any());
        }

        [Test]
        public void TestNameConditionSuccess()
        {
            Container.Bind<Test1>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            var test1 = Container.Resolve<Test1>();

            TestAssert.That(test1 != null);
        }
    }
}


