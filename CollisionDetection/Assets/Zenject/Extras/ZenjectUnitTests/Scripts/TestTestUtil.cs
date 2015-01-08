using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using Zenject.Tests;
using TestAssert=NUnit.Framework.Assert;
using System.Linq;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestTestUtil
    {
        [Test]
        public void TestTrue()
        {
            TestAssert.IsTrue(TestListComparer.ContainSameElements(
                new List<int> {1},
                new List<int> {1}));

            TestAssert.IsTrue(TestListComparer.ContainSameElements(
                new List<int> {1, 2},
                new List<int> {2, 1}));

            TestAssert.IsTrue(TestListComparer.ContainSameElements(
                new List<int> {1, 2, 3},
                new List<int> {3, 2, 1}));

            TestAssert.IsTrue(TestListComparer.ContainSameElements(
                new List<int> {},
                new List<int> {}));
        }

        [Test]
        public void TestFalse()
        {
            TestAssert.IsFalse(TestListComparer.ContainSameElements(
                new List<int> {1, 2, 3},
                new List<int> {3, 2, 3}));

            TestAssert.IsFalse(TestListComparer.ContainSameElements(
                new List<int> {1, 2},
                new List<int> {1, 2, 3}));
        }
    }
}



