using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Zenject
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        [Inject]
        DiContainer _container = null;

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        public virtual bool IsEnabled
        {
            get
            {
                return this.enabled;
            }
        }

        public virtual void Start()
        {
            // Define this method so we expose the enabled check box
        }

        public abstract void InstallBindings();
    }
}
