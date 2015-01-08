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
    public class TestRebind : TestWithContainer
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

        [Test]
        public void Run()
        {
            Container.Bind<Test1>().ToSingle<Test2>();

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            TestAssert.That(Container.Resolve<Test1>() is Test2);

            Container.Rebind<Test1>().ToSingle<Test3>();

            TestAssert.That(Container.ValidateResolve<Test1>().IsEmpty());
            TestAssert.That(Container.Resolve<Test1>() is Test3);
        }
    }
}

