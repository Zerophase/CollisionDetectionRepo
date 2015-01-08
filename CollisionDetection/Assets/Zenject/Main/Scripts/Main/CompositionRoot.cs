#pragma warning disable 414
using ModestTree;

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Zenject
{
    // Define this class as a component of a top-level game object of your scene heirarchy
    // Then any children will get injected during resolve stage
    public sealed class CompositionRoot : MonoBehaviour
    {
        public static Action<DiContainer> BeforeInstallHooks;
        public static Action<DiContainer> AfterInstallHooks;

        public bool OnlyInjectWhenActive = true;

        DiContainer _container;
        IDependencyRoot _dependencyRoot = null;

        [SerializeField]
        public MonoInstaller[] Installers;

        public DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        public void Awake()
        {
            Log.Debug("Zenject Started");

            _container = CreateContainer(false, GlobalCompositionRoot.Instance.Container);

            InjectionHelper.InjectChildGameObjects(_container, gameObject, !OnlyInjectWhenActive);
            _dependencyRoot = _container.Resolve<IDependencyRoot>();
        }

        public DiContainer CreateContainer(bool allowNullBindings, DiContainer parentContainer)
        {
            var container = new DiContainer();
            container.AllowNullBindings = allowNullBindings;
            container.FallbackProvider = new DiContainerProvider(parentContainer);

            if (BeforeInstallHooks != null)
            {
                BeforeInstallHooks(container);
                // Reset extra bindings for next time we change scenes
                BeforeInstallHooks = null;
            }

            CompositionRootHelper.InstallStandardInstaller(container, this.gameObject);

            if (Installers.Where(x => x != null).IsEmpty())
            {
                Log.Warn("No installers found while initializing CompositionRoot");
            }
            else
            {
                CompositionRootHelper.InstallSceneInstallers(container, Installers);
            }

            if (AfterInstallHooks != null)
            {
                AfterInstallHooks(container);
                // Reset extra bindings for next time we change scenes
                AfterInstallHooks = null;
            }

            return container;
        }
    }
}
