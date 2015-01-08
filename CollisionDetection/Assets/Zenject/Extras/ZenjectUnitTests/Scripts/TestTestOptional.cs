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
    public class TestTestOptional : TestWithContainer
    {
        class Test1
        {
        }

        class Test2
        {
            [Inject]
            public Test1 val1 = null;
        }

        class Test3
        {
            [InjectOptional]
            public Test1 val1 = null;
        }

        class Test0
        {
            [InjectOptional]
            public int Val1 = 5;
        }

        [Test]
        public void TestFieldRequired()
        {
            Container.Bind<Test2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test2>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test2>(); });
        }

        [Test]
        public void TestFieldOptional()
        {
            Container.Bind<Test3>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test3>().IsEmpty());
            var test = Container.Resolve<Test3>();
            TestAssert.That(test.val1 == null);
        }

        [Test]
        public void TestFieldOptional2()
        {
            Container.Bind<Test3>().ToSingle();

            var test1 = new Test1();
            Container.Bind<Test1>().To(test1);

            TestAssert.That(Container.ValidateResolve<Test3>().IsEmpty());
            TestAssert.AreEqual(Container.Resolve<Test3>().val1, test1);
        }

        [Test]
        public void TestFieldOptional3()
        {
            Container.Bind<Test0>().ToTransient();

            // Should not redefine the hard coded value in this case
            TestAssert.AreEqual(Container.Resolve<Test0>().Val1, 5);

            Container.BindValue<int>().To(3);

            TestAssert.AreEqual(Container.Resolve<Test0>().Val1, 3);
        }

        class Test4
        {
            public Test4(Test1 val1)
            {
            }
        }

        class Test5
        {
            public Test1 Val1;

            public Test5(
                [InjectOptional]
                Test1 val1)
            {
                Val1 = val1;
            }
        }

        [Test]
        public void TestParameterRequired()
        {
            Container.Bind<Test4>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test4>(); });

            TestAssert.That(Container.ValidateResolve<Test2>().Any());
        }

        [Test]
        public void TestParameterOptional()
        {
            Container.Bind<Test5>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test5>().IsEmpty());
            var test = Container.Resolve<Test5>();
            TestAssert.That(test.Val1 == null);
        }

        class Test6
        {
            public Test6(Test2 test2)
            {
            }
        }

        [Test]
        public void TestChildDependencyOptional()
        {
            Container.Bind<Test6>().ToSingle();
            Container.Bind<Test2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test6>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test6>(); });
        }
    }
}



