using System;
using Zenject;
using NUnit.Framework;
using Zenject.Tests;
using TestAssert=NUnit.Framework.Assert;

namespace Zenject.Tests
{
    [TestFixture]
    public class TestMultipleInstallers : TestWithContainer
    {
        class Test0 : Installer
        {
            public static int Count;
            public override void InstallBindings()
            {
                Count++;
            }
        }

        class Test1 : Installer
        {
            public static int Count;
            public override void InstallBindings()
            {
                Container.Bind<IInstaller>().ToSingle<Test0>();
                Count++;
            }
        }

        class Test2 : Installer
        {
            public static int Count;
            public override void InstallBindings()
            {
                Container.Bind<IInstaller>().ToSingle<Test0>();
                Count++;
            }
        }

        public override void Setup()
        {
            base.Setup();

            // Reset counters since static state is not being reset by 'Unity Test Tools'.
            Test0.Count = 0;
            Test1.Count = 0;
            Test2.Count = 0;
        }

        [Test]
        public void Test()
        {
            Container.Bind<IInstaller>().ToSingle<Test1>();
            Container.Bind<IInstaller>().ToSingle<Test2>();

            Container.InstallInstallers();

            TestAssert.That(Test1.Count == 1);
            TestAssert.That(Test2.Count == 1);
            TestAssert.That(Test0.Count == 1);
        }
    }
}
