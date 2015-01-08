using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestCircularDependencies : TestWithContainer
    {
        class Test1
        {
            [Inject]
            public Test2 test = null;
        }

        class Test2
        {
            [Inject]
            public Test2 test = null;
        }

        [Test]
        public void Test()
        {
            Container.Bind<Test1>().ToSingle();
            Container.Bind<Test2>().ToSingle();

            TestAssert.Throws<ZenjectResolveException>(
                delegate { Container.Resolve<Test2>(); });
        }
    }
}


