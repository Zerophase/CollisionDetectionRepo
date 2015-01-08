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
    public class TestSingleton : TestWithContainer
    {
        private interface IFoo
        {
            int ReturnValue();
        }

        private class Foo : IFoo
        {
            public int ReturnValue()
            {
                return 5;
            }
        }

        [Test]
        public void TestClassRegistration()
        {
            Container.Bind<Foo>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Foo>().IsEmpty());
            TestAssert.That(Container.Resolve<Foo>().ReturnValue() == 5);
        }

        [Test]
        public void TestSingletonOneInstance()
        {
            Container.Bind<Foo>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Foo>().IsEmpty());
            var test1 = Container.Resolve<Foo>();
            TestAssert.That(Container.ValidateResolve<Foo>().IsEmpty());
            var test2 = Container.Resolve<Foo>();

            TestAssert.That(test1 != null && test2 != null);
            TestAssert.That(ReferenceEquals(test1, test2));
        }

        [Test]
        public void TestSingletonOneInstanceUntyped()
        {
            Container.Bind(typeof(Foo)).ToSingle();

            TestAssert.That(Container.ValidateResolve<Foo>().IsEmpty());
            var test1 = Container.Resolve<Foo>();
            TestAssert.That(Container.ValidateResolve<Foo>().IsEmpty());
            var test2 = Container.Resolve<Foo>();

            TestAssert.That(test1 != null && test2 != null);
            TestAssert.That(ReferenceEquals(test1, test2));
        }

        [Test]
        public void TestInterfaceBoundToImplementationRegistration()
        {
            Container.Bind<IFoo>().ToSingle<Foo>();

            TestAssert.That(Container.ValidateResolve<IFoo>().IsEmpty());
            TestAssert.That(Container.Resolve<IFoo>().ReturnValue() == 5);
        }

        [Test]
        public void TestInterfaceBoundToImplementationRegistrationUntyped()
        {
            Container.Bind(typeof(IFoo)).ToSingle(typeof(Foo));

            TestAssert.That(Container.ValidateResolve<IFoo>().IsEmpty());
            TestAssert.That(Container.Resolve<IFoo>().ReturnValue() == 5);
        }

        [Test]
        public void TestInterfaceBoundToInstanceRegistration()
        {
            IFoo instance = new Foo();

            Container.Bind<IFoo>().To(instance);

            TestAssert.That(Container.ValidateResolve<IFoo>().IsEmpty());
            var builtInstance = Container.Resolve<IFoo>();

            TestAssert.That(ReferenceEquals(builtInstance, instance));
            TestAssert.That(builtInstance.ReturnValue() == 5);
        }

        [Test]
        public void TestInterfaceBoundToInstanceRegistrationUntyped()
        {
            IFoo instance = new Foo();

            Container.Bind(typeof(IFoo)).To(instance);

            TestAssert.That(Container.ValidateResolve<IFoo>().IsEmpty());
            var builtInstance = Container.Resolve<IFoo>();

            TestAssert.That(ReferenceEquals(builtInstance, instance));
            TestAssert.That(builtInstance.ReturnValue() == 5);
        }

        [Test]
        public void TestDuplicateBindings()
        {
            // Note: does not error out until a request for Foo is made
            Container.Bind<Foo>().ToSingle();
            Container.Bind<Foo>().ToSingle();
        }

        [Test]
        public void TestDuplicateBindingsFail()
        {
            Container.Bind<Foo>().ToSingle();
            Container.Bind<Foo>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Foo>(); });

            TestAssert.That(Container.ValidateResolve<Foo>().Any());
        }

        [Test]
        public void TestDuplicateBindingsFailUntyped()
        {
            Container.Bind(typeof(Foo)).ToSingle();
            Container.Bind(typeof(Foo)).ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Foo>(); });

            TestAssert.That(Container.ValidateResolve<Foo>().Any());
        }

        [Test]
        public void TestDuplicateInstanceBindingsFail()
        {
            var instance = new Foo();

            Container.Bind<Foo>().To(instance);
            Container.Bind<Foo>().To(instance);

            TestAssert.That(Container.ValidateResolve<Foo>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Foo>(); });
        }

        [Test]
        public void TestDuplicateInstanceBindingsFailUntyped()
        {
            var instance = new Foo();

            Container.Bind(typeof(Foo)).To(instance);
            Container.Bind(typeof(Foo)).To(instance);

            TestAssert.That(Container.ValidateResolve<Foo>().Any());

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Foo>(); });
        }

        [Test]
        [ExpectedException(typeof(ZenjectBindException))]
        public void TestToSingleWithInstance()
        {
            Container.Bind<Foo>().ToSingle(new Foo());
            Container.Bind<Foo>().ToSingle(new Foo());
        }

        [Test]
        [ExpectedException(typeof(ZenjectBindException))]
        public void TestToSingleWithInstanceUntyped()
        {
            Container.Bind(typeof(Foo)).ToSingle(new Foo());
            Container.Bind(typeof(Foo)).ToSingle(new Foo());
        }

        [Test]
        public void TestToSingleWithInstanceIsUnique()
        {
            var foo = new Foo();

            Container.Bind<Foo>().ToSingle(foo);
            Container.Bind<IFoo>().ToSingle<Foo>();

            TestAssert.That(
                ReferenceEquals(Container.Resolve<IFoo>(), Container.Resolve<Foo>()));
        }

        [Test]
        public void TestToSingleWithInstanceIsUniqueUntyped()
        {
            var foo = new Foo();

            Container.Bind(typeof(Foo)).ToSingle(foo);
            Container.Bind(typeof(IFoo)).ToSingle<Foo>();

            TestAssert.That(
                ReferenceEquals(Container.Resolve<IFoo>(), Container.Resolve<Foo>()));
        }

        [Test]
        public void TestToSingleWithInstance2()
        {
            var foo = new Foo();

            Container.Bind<Foo>().To(foo);
            Container.Bind<IFoo>().ToSingle<Foo>();

            TestAssert.That(
                !ReferenceEquals(Container.Resolve<IFoo>(), Container.Resolve<Foo>()));
        }

        [Test]
        public void TestToSingleWithInstance2Untyped()
        {
            var foo = new Foo();

            Container.Bind(typeof(Foo)).To(foo);
            Container.Bind(typeof(IFoo)).ToSingle<Foo>();

            TestAssert.That(
                !ReferenceEquals(Container.Resolve<IFoo>(), Container.Resolve<Foo>()));
        }

        [Test]
        public void TestToSingleMethod()
        {
            var foo = new Foo();

            Container.Bind(typeof(Foo)).ToSingleMethod((container) => foo);
            Container.Bind(typeof(IFoo)).ToSingle<Foo>();

            TestAssert.That(ReferenceEquals(Container.Resolve<Foo>(), foo));
            TestAssert.That(ReferenceEquals(Container.Resolve<Foo>(), Container.Resolve<IFoo>()));
        }

        [Test]
        [ExpectedException(typeof(ZenjectBindException))]
        public void TestToSingleMethod2()
        {
            var foo = new Foo();
            var foo2 = new Foo();

            Container.Bind(typeof(Foo)).ToSingleMethod((container) => foo);
            Container.Bind(typeof(IFoo)).ToSingleMethod((container) => foo2);
        }

        [Test]
        [ExpectedException(typeof(ZenjectBindException))]
        public void TestToSingleMethod3()
        {
            Container.Bind<Foo>().ToSingle();
            Container.Bind(typeof(IFoo)).ToSingleMethod((container) => new Foo());
        }
    }
}


