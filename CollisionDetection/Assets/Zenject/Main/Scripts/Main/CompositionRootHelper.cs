using System;
using System.Collections.Generic;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    public static class CompositionRootHelper
    {
        public static void InstallStandardInstaller(DiContainer container, GameObject rootObj)
        {
            container.Bind<GameObject>().To(rootObj).WhenInjectedInto<StandardUnityInstaller>();
            container.Bind<IInstaller>().ToSingle<StandardUnityInstaller>();
            container.InstallInstallers();
            Assert.That(!container.HasBinding<IInstaller>());
        }

        public static void InstallSceneInstallers(
            DiContainer container, IEnumerable<IInstaller> installers)
        {
            foreach (var installer in installers)
            {
                if (installer == null)
                {
                    Log.Warn("Found null installer in composition root");
                    continue;
                }

                if (installer.IsEnabled)
                {
                    // The installers that are part of the scene are monobehaviours
                    // and therefore were not created via Zenject and therefore do
                    // not have their members injected
                    // At the very least they will need the container injected but
                    // they might also have some configuration passed from another
                    // scene as well
                    FieldsInjecter.Inject(container, installer);
                    container.Bind<IInstaller>().To(installer);

                    // Install this installer and also any other installers that it installs
                    container.InstallInstallers();

                    Assert.That(!container.HasBinding<IInstaller>());
                }
            }
        }
    }
}
