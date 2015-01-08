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
    public class TestConditionsIdentifier : TestWithContainer
    {
        class Test0
        {
        }

        class Test1
        {
            public Test1(
                [Inject("foo")]
                Test0 name1)
            {
            }
        }

        class Test2
        {
            [Inject("foo")]
            public Test0 name2 = null;
        }

        public override void Setup()
        {
            base.Setup();

            Container.Bind<Test1>().ToTransient();
            Container.Bind<Test2>().ToTransient();
            Container.Bind<Test3>().ToTransient();
            Container.Bind<Test4>().ToTransient();
        }

        [Test]
        public void TestUnspecifiedNameConstructorInjection()
        {
            Container.Bind<Test0>().ToTransient();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());
        }

        [Test]
        public void TestUnspecifiedNameFieldInjection()
        {
            Container.Bind<Test0>().ToTransient();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test2>(); });

            TestAssert.That(Container.ValidateResolve<Test2>().Any());
        }

        [Test]
        public void TestTooManySpecified()
        {
            Container.Bind<Test0>().ToTransient();
            Container.Bind<Test0>().ToTransient();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            TestAssert.That(Container.ValidateResolve<Test2>().Any());
        }

        [Test]
        public void TestSuccessConstructorInjectionString()
        {
            Container.Bind<Test0>().To(new Test0());
            Container.Bind<Test0>().To(new Test0()).As("foo");

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            TestAssert.IsNotNull(Container.Resolve<Test1>());
        }

        [Test]
        public void TestSuccessFieldInjectionString()
        {
            Container.Bind<Test0>().To(new Test0());
            Container.Bind<Test0>().To(new Test0()).As("foo");

            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            TestAssert.IsNotNull(Container.Resolve<Test2>());
        }

        enum TestEnum
        {
            TestValue1,
            TestValue2,
            TestValue3,
        }

        class Test3
        {
            public Test3(
                [Inject(TestEnum.TestValue2)]
                Test0 test0)
            {
            }
        }

        class Test4
        {

        }

        [Test]
        public void TestFailConstructorInjectionEnum()
        {
            Container.Bind<Test0>().To(new Test0());
            Container.Bind<Test0>().To(new Test0()).As(TestEnum.TestValue1);

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test3>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());
        }

        [Test]
        public void TestSuccessConstructorInjectionEnum()
        {
            Container.Bind<Test0>().To(new Test0());
            Container.Bind<Test0>().To(new Test0()).As(TestEnum.TestValue2);

            TestAssert.That(Container.ValidateResolve<Test3>().IsEmpty());
            TestAssert.IsNotNull(Container.Resolve<Test3>());
        }

        [Test]
        public void TestFailFieldInjectionEnum()
        {
            Container.Bind<Test0>().To(new Test0());
            Container.Bind<Test0>().To(new Test0()).As(TestEnum.TestValue1);

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test3>(); });

            TestAssert.That(Container.ValidateResolve<Test3>().Any());
        }

        [Test]
        public void TestSuccessFieldInjectionEnum()
        {
            Container.Bind<Test0>().To(new Test0());
            Container.Bind<Test0>().To(new Test0()).As(TestEnum.TestValue3);

            TestAssert.That(Container.ValidateResolve<Test4>().IsEmpty());
            TestAssert.IsNotNull(Container.Resolve<Test4>());
        }
    }
}
