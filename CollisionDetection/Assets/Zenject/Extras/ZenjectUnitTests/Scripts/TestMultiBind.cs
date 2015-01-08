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
    public class TestMultiBind : TestWithContainer
    {
        class Test1
        {
        }

        class Test2 : Test1
        {
        }

        class Test3 : Test1
        {
        }

        class TestImpl1
        {
            public List<Test1> tests;

            public TestImpl1(List<Test1> tests)
            {
                this.tests = tests;
            }
        }

        class TestImpl2
        {
            [Inject]
            public List<Test1> tests = null;
        }

        [Test]
        public void TestMultiBind1()
        {
            Container.Bind<Test1>().ToSingle<Test2>();
            Container.Bind<Test1>().ToSingle<Test3>();
            Container.Bind<TestImpl1>().ToSingle();

            TestAssert.That(Container.ValidateResolve<TestImpl1>().IsEmpty());
            var test1 = Container.Resolve<TestImpl1>();

            TestAssert.That(test1.tests.Count == 2);
        }

        [Test]
        public void TestMultiBind2()
        {
            Container.Bind<TestImpl1>().ToSingle();

            // optional list dependencies should be declared as optional
            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<TestImpl1>(); });

            TestAssert.That(Container.ValidateResolve<TestImpl1>().Any());
        }

        [Test]
        public void TestMultiBind2Validate()
        {
            Container.Bind<TestImpl1>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<TestImpl1>(); });

            TestAssert.That(Container.ValidateResolve<Test2>().Any());
        }

        [Test]
        public void TestMultiBindListInjection()
        {
            Container.Bind<Test1>().ToSingle<Test2>();
            Container.Bind<Test1>().ToSingle<Test3>();
            Container.Bind<TestImpl2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<TestImpl2>().IsEmpty());
            var test = Container.Resolve<TestImpl2>();
            TestAssert.That(test.tests.Count == 2);
        }
    }
}


