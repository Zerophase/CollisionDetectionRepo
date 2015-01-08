using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestSameConstructorArgumentType : TestWithContainer
    {
        class Test1
        {
            public Test2 t1;
            public float f;
            public Test2 t2;

            public Test1(Test2 t1, float f, Test2 t2)
            {
                this.t1 = t1;
                this.f = f;
                this.t2 = t2;
            }
        }

        class Test2
        {
        }

        [Test]
        public void Test()
        {
            var t1 = new Test2();
            var t2 = new Test2();

            Container.Bind<FactoryUntyped<Test1>>().ToSingle();

            TestAssert.That(Container.ValidateResolve<FactoryUntyped<Test1>>().IsEmpty());
            var factory = Container.Resolve<FactoryUntyped<Test1>>();

            var test = factory.Create(t1, 5.0f, t2);

            TestAssert.That(ReferenceEquals(test.t1, t1));
            TestAssert.That(ReferenceEquals(test.t2, t2));
        }
    }
}


