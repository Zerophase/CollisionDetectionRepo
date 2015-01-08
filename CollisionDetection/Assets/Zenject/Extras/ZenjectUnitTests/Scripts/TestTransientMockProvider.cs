using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestTransientMockProvider : TestWithContainer
    {
        public interface IFoo
        {
            int GetBar();
        }

        [Test]
        public void TestCase1()
        {
            // Commented out because this requires that zenject be installed with mocking support which isn't always the case

            //Container.FallbackProvider = new TransientMockProvider(Container);

            //var foo = Container.Resolve<IFoo>();

            //TestAssert.AreEqual(foo.GetBar(), 0);
        }
    }
}
