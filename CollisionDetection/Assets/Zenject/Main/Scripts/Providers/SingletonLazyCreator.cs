using System;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    internal class SingletonLazyCreator
    {
        int _referenceCount;
        object _instance;
        SingletonProviderMap _owner;
        DiContainer _container;
        Instantiator _instantiator;
        GameObjectInstantiator _gameObjInstantiator;
        bool _hasInstance;
        Func<DiContainer, object> _createMethod;
        SingletonId _id;

        public SingletonLazyCreator(
            DiContainer container, SingletonProviderMap owner,
            SingletonId id, Func<DiContainer, object> createMethod = null)
        {
            _container = container;
            _owner = owner;
            _id = id;
            _createMethod = createMethod;
        }

        public bool HasCustomCreateMethod
        {
            get
            {
                return _createMethod != null;
            }
        }

        public void IncRefCount()
        {
            _referenceCount += 1;
        }

        public void DecRefCount()
        {
            _referenceCount -= 1;

            if (_referenceCount <= 0)
            {
                _owner.RemoveCreator(_id);
            }
        }

        public void SetInstance(object instance)
        {
            Assert.IsNull(_instance);
            Assert.That(instance != null || _container.AllowNullBindings);

            _instance = instance;
            // We need this flag for validation
            _hasInstance = true;
        }

        public bool HasInstance()
        {
            if (_hasInstance)
            {
                Assert.That(_container.AllowNullBindings || _instance != null);
            }

            return _hasInstance;
        }

        public Type GetInstanceType()
        {
            return _id.Type;
        }

        public object GetInstance(Type contractType)
        {
            if (!_hasInstance)
            {
                _instance = Instantiate(contractType);

                if (_instance == null)
                {
                    throw new ZenjectException(
                        "Unable to instantiate type '{0}' in SingletonLazyCreator".With(contractType));
                }

                _hasInstance = true;
            }

            return _instance;
        }

        object Instantiate(Type contractType)
        {
            if (_createMethod != null)
            {
                return _createMethod(_container);
            }

            var concreteType = GetTypeToInstantiate(contractType);

            if (_id.Prefab != null)
            {
                Assert.That(concreteType.DerivesFrom<Component>(), "Expected '{0}' to derive from 'Component'", concreteType.Name);

                if (_gameObjInstantiator == null)
                {
                    _gameObjInstantiator = _container.Resolve<GameObjectInstantiator>();
                }

                return _gameObjInstantiator.Instantiate(_id.Type, _id.Prefab);
            }

            if (_instantiator == null)
            {
                _instantiator = _container.Resolve<Instantiator>();
            }

            return _instantiator.Instantiate(concreteType);
        }

        Type GetTypeToInstantiate(Type contractType)
        {
            if (_id.Type.IsOpenGenericType())
            {
                Assert.That(!contractType.IsAbstract);
                Assert.That(contractType.GetGenericTypeDefinition() == _id.Type);
                return contractType;
            }

            Assert.That(_id.Type.DerivesFromOrEqual(contractType));
            return _id.Type;
        }
    }
}
