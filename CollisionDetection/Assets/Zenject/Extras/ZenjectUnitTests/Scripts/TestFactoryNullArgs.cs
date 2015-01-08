using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestFactoryUsingIFactory : TestWithContainer
    {
        public interface ITest
        {
        }

        class Test2 : ITest
        {
            public class Factory : Factory<Test2>
            {
            }
        }

        [Test]
        public void Test()
        {
            Container.Bind<Test2.Factory>().ToSingle();

            Container.BindFactoryToCustomFactory<ITest, Test2, Test2.Factory>();

            var result = Container.Resolve<IFactory<ITest>>().Create();

            TestAssert.That(result.GetType() == typeof(Test2));
        }
    }

    [TestFixture]
    public class TestFactoryNullArgs : TestWithContainer
    {
        public interface ITest
        {
        }

        public class Foo
        {
        }

        public class Bar
        {
        }

        class Test2 : ITest
        {
            [Inject]
            public Foo Foo = null;

            [Inject]
            public Bar Bar = null;

            public class Factory : Factory<Bar, Test2>
            {
            }
        }

        [Test]
        public void Run1()
        {
            Container.Bind<Foo>().ToSingle();
            Container.Bind<Test2.Factory>().ToSingle();

            var factory = Container.Resolve<Test2.Factory>();
            var test2 = factory.Create(new Bar());

            Assert.IsNotNull(test2);
        }

        [Test]
        public void Run2()
        {
            Container.Bind<Foo>().ToSingle();
            Container.Bind<Test2.Factory>().ToSingle();

            var factory = Container.Resolve<Test2.Factory>();
            var test2 = factory.Create(null);

            Assert.IsNotNull(test2);
            Assert.IsNull(test2.Bar);
        }

        class Test3 : ITest
        {
            [Inject]
            public Foo Foo1 = null;

            [Inject]
            public Foo Foo2 = null;

            [Inject]
            public Foo Foo3 = null;

            public class Factory : Factory<Foo, Foo, Foo, Test3>
            {
            }
        }

        [Test]
        public void Run3()
        {
            Container.Bind<Test3.Factory>().ToSingle();

            var factory = Container.Resolve<Test3.Factory>();
            var test = factory.Create(null, null, new Foo());

            Assert.IsNull(test.Foo1);
            Assert.IsNull(test.Foo2);
            Assert.IsNotNull(test.Foo3);

            test = factory.Create(null, new Foo(), null);

            Assert.IsNull(test.Foo1);
            Assert.IsNotNull(test.Foo2);
            Assert.IsNull(test.Foo3);
        }
    }
}
