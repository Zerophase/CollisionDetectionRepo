using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using ModestTree;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestBaseClassPropertyInjection : TestWithContainer
    {
        class Test0
        {
        }

        class Test3
        {
        }

        class Test1 : Test3
        {
            [Inject] protected Test0 val = null;

            public Test0 GetVal()
            {
                return val;
            }
        }

        class Test2 : Test1
        {
        }

        [Test]
        public void TestCaseBaseClassPropertyInjection()
        {
            Container.Bind<Test0>().ToSingle();
            Container.Bind<Test2>().ToSingle();

            TestAssert.That(Container.ValidateResolve<Test2>().IsEmpty());
            var test1 = Container.Resolve<Test2>();

            TestAssert.That(test1.GetVal() != null);
        }
    }
}


