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
    public class TestBindScope : TestWithContainer
    {
        class Test0
        {
        }

        [Test]
        public void TestIsRemoved()
        {
            using (var scope = Container.CreateScope())
            {
                var test1 = new Test0();

                scope.Bind<Test0>().To(test1);

                TestAssert.That(Container.ValidateResolve<Test0>().IsEmpty());
                TestAssert.That(ReferenceEquals(test1, Container.Resolve<Test0>()));
            }

            TestAssert.That(Container.ValidateResolve<Test0>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>(); });
        }

        class Test1
        {
            [Inject]
            public Test0 Test = null;
        }

        [Test]
        public void TestCase2()
        {
            Test0 test0;
            Test1 test1;

            using (var scope = Container.CreateScope())
            {
                var test0Local = new Test0();

                scope.Bind<Test0>().To(test0Local);
                scope.Bind<Test1>().ToSingle();

                TestAssert.That(Container.ValidateResolve<Test0>().IsEmpty());
                test0 = Container.Resolve<Test0>();
                TestAssert.AreEqual(test0Local, test0);

                TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
                test1 = Container.Resolve<Test1>();
            }

            TestAssert.That(Container.ValidateResolve<Test0>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test0>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            Container.Bind<Test0>().ToSingle();
            Container.Bind<Test1>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test0>().IsEmpty());
            TestAssert.That(Container.Resolve<Test0>() != test0);

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            TestAssert.That(Container.Resolve<Test1>() != test1);
        }

        interface IFoo
        {
        }

        interface IFoo2
        {
        }

        class Foo : IFoo, IFoo2
        {
        }

        [Test]
        public void TestMultipleSingletonDifferentScope()
        {
            IFoo foo1;

            using (var scope = Container.CreateScope())
            {
                scope.Bind<IFoo>().ToSingle<Foo>();
                foo1 = Container.Resolve<IFoo>();
            }

            TestAssert.That(!Container.ValidateResolve<IFoo>().IsEmpty());

            using (var scope = Container.CreateScope())
            {
                scope.Bind<IFoo>().ToSingle<Foo>();
                var foo2 = Container.Resolve<IFoo>();

                TestAssert.That(foo2 != foo1);
            }
        }
    }
}

