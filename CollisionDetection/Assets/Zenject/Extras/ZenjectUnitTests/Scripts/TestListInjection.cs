using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestListInjection : TestWithContainer
    {
        class Test1
        {
            public Test1(List<int> values)
            {
            }
        }

        class Test2
        {
            public Test2( 
                [InjectOptional] List<int> values)
            {
            }
        }

        class Test3
        {
            [Inject]
            public List<int> values = null;
        }

        class Test4
        {
            [InjectOptional]
            public List<int> values = null;
        }

        [Test]
        [ExpectedException]
        public void TestCase1()
        {
            Container.Bind<Test1>().ToSingle();

            Container.ResolveMany<Test1>();
        }

        [Test]
        public void TestCase2()
        {
            Container.Bind<Test2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            var result = Container.ResolveMany<Test2>();

            TestAssert.That(result != null);
        }

        [Test]
        [ExpectedException(typeof(ZenjectResolveException))]
        public void TestCase3()
        {
            Container.Bind<Test3>().ToSingle();

            Container.ResolveMany<Test3>();
        }

        [Test]
        public void TestCase4()
        {
            Container.Bind<Test4>().ToSingle();

            var result = Container.ResolveMany<Test4>();

            TestAssert.That(result != null);
        }
    }
}



