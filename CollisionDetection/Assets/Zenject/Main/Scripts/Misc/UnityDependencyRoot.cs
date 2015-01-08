using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    public sealed class UnityDependencyRoot : MonoBehaviour, IDependencyRoot
    {
        [Inject]
        public TickableManager _tickableManager;

        [Inject]
        public InitializableManager _initializableManager;

        [Inject]
        public DisposableManager _disposablesManager;

        [PostInject]
        public void Initialize()
        {
            _initializableManager.Initialize();
        }

        public void OnDestroy()
        {
            _disposablesManager.Dispose();
        }

        public void Update()
        {
            _tickableManager.Update();
        }

        public void FixedUpdate()
        {
            _tickableManager.FixedUpdate();
        }

        public void LateUpdate()
        {
            _tickableManager.LateUpdate();
        }
    }
}
