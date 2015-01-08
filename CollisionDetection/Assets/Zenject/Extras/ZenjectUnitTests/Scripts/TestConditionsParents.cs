using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using System.Linq;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestConditionsParents : TestWithContainer
    {
        class Test0
        {
        }

        interface ITest1
        {
        }

        class Test1 : ITest1
        {
            public Test0 test0;

            public Test1(Test0 test0)
            {
                this.test0 = test0;
            }
        }

        class Test2 : ITest1
        {
            public Test0 test0;

            public Test2(Test0 test0)
            {
                this.test0 = test0;
            }
        }

        class Test3 : ITest1
        {
            public Test1 test1;

            public Test3(Test1 test1)
            {
                this.test1 = test1;
            }
        }

        class Test4 : ITest1
        {
            public Test1 test1;

            public Test4(Test1 test1)
            {
                this.test1 = test1;
            }
        }

        [Test]
        public void TestCase1()
        {
            Container.Bind<Test1>().ToSingle();
            Container.Bind<Test0>().ToSingle().When(c => c.ParentTypes.Contains(typeof(Test2)));

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test1>(); });

            TestAssert.That(Container.ValidateResolve<Test0>().Any());
        }

        [Test]
        public void TestCase2()
        {
            Container.Bind<Test1>().ToSingle();
            Container.Bind<Test0>().ToSingle().When(c => c.ParentTypes.Contains(typeof(Test1)));

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            var test1 = Container.Resolve<Test1>();
            TestAssert.That(test1 != null);
        }

        // Test using parents to look deeper up the heirarchy..
        [Test]
        public void TestCase3()
        {
            var t0a = new Test0();
            var t0b = new Test0();

            Container.Bind<Test3>().ToSingle();
            Container.Bind<Test4>().ToSingle();
            Container.Bind<Test1>().ToTransient();

            Container.Bind<Test0>().To(t0a).When(c => c.ParentTypes.Contains(typeof(Test3)));
            Container.Bind<Test0>().To(t0b).When(c => c.ParentTypes.Contains(typeof(Test4)));

            TestAssert.That(Container.ValidateResolve<Test3>().IsEmpty());
            var test3 = Container.Resolve<Test3>();

            TestAssert.That(Container.ValidateResolve<Test4>().IsEmpty());
            var test4 = Container.Resolve<Test4>();

            TestAssert.That(ReferenceEquals(test3.test1.test0, t0a));
            TestAssert.That(ReferenceEquals(test4.test1.test0, t0b));
        }

        [Test]
        public void TestCase4()
        {
            Container.Bind<ITest1>().ToSingle<Test2>();
            Container.Bind<Test0>().ToSingle().When(c => c.ParentTypes.Contains(typeof(ITest1)));

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<ITest1>(); });

            TestAssert.That(Container.ValidateResolve<Test1>().Any());
        }

        [Test]
        public void TestCase5()
        {
            Container.Bind<ITest1>().ToSingle<Test2>();
            Container.Bind<Test0>().ToSingle().When(c => c.ParentTypes.Contains(typeof(Test2)));

            TestAssert.That(Container.ValidateResolve<ITest1>().IsEmpty());
            var test1 = Container.Resolve<ITest1>();
            TestAssert.That(test1 != null);
        }

        [Test]
        public void TestCase6()
        {
            Container.Bind<ITest1>().ToSingle<Test2>();
            Container.Bind<Test0>().ToSingle().When(c => c.ParentTypes.Where(x => typeof(ITest1).IsAssignableFrom(x)).Any());

            TestAssert.That(Container.ValidateResolve<ITest1>().IsEmpty());
            var test1 = Container.Resolve<ITest1>();
            TestAssert.That(test1 != null);
        }
    }
}

