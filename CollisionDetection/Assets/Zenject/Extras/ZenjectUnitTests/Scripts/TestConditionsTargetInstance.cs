using System;
using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using TestAssert = NUnit.Framework.Assert;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestConditionsTargetInstance : TestWithContainer
    {
        class Test0
        {
        }

        class Test1
        {
            [Inject]
            public Test0 test0 = null;
        }

        Test1 _test1;

        public override void Setup()
        {
            base.Setup();

            _test1 = new Test1();
            Container.Bind<Test0>().ToSingle().When(r => r.EnclosingInstance == _test1);
            Container.Bind<Test1>().To(_test1);
        }

        [Test]
        public void TestTargetConditionError()
        {
            FieldsInjecter.Inject(Container, _test1);

            TestAssert.That(_test1.test0 != null);
        }
    }
}
