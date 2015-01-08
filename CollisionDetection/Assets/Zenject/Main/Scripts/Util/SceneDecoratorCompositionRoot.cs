using System;
using UnityEngine;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public sealed class SceneDecoratorCompositionRoot : MonoBehaviour
    {
        public string SceneName;

        [SerializeField]
        public DecoratorInstaller[] Installers;

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);

            ZenUtil.LoadScene(
                SceneName, AddPreBindings, AddPostBindings);
        }

        public void AddPreBindings(DiContainer container)
        {
            ProcessInstallers(container, true);
        }

        public void AddPostBindings(DiContainer container)
        {
            ProcessInstallers(container, false);
        }

        void ProcessInstallers(DiContainer container, bool isBefore)
        {
            if (Installers == null)
            {
                return;
            }

            foreach (var installer in Installers)
            {
                if (installer == null)
                {
                    Log.Warn("Found null installer in composition root");
                    continue;
                }

                if (installer.enabled)
                {
                    installer.Container = container;

                    if (isBefore)
                    {
                        installer.PreInstallBindings();
                    }
                    else
                    {
                        installer.PostInstallBindings();
                    }

                    // Install this installer and also any other installers that it installs
                    container.InstallInstallers();

                    Assert.That(!container.HasBinding<IInstaller>());
                }
            }
        }
    }
}
