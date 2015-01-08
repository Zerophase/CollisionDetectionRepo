using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ModestTree;

namespace Zenject
{
    public class SingletonProviderMap
    {
        Dictionary<SingletonId, SingletonLazyCreator> _creators = new Dictionary<SingletonId, SingletonLazyCreator>();
        DiContainer _container;

        public SingletonProviderMap(DiContainer container)
        {
            _container = container;
        }

        internal void RemoveCreator(SingletonId id)
        {
            bool success = _creators.Remove(id);
            Assert.That(success);
        }

        SingletonLazyCreator AddCreator<TConcrete>(Func<DiContainer, TConcrete> method)
        {
            SingletonLazyCreator creator;

            var id = new SingletonId(typeof(TConcrete));

            if (_creators.ContainsKey(id))
            {
                throw new ZenjectBindException(
                    "Found multiple singleton instances bound to type '{0}'".With(typeof(TConcrete)));
            }

            creator = new SingletonLazyCreator(
                _container, this, id, (container) => method(container));

            _creators.Add(id, creator);

            creator.IncRefCount();
            return creator;
        }

        SingletonLazyCreator AddCreator(Type type)
        {
            return AddCreator(new SingletonId(type));
        }

        SingletonLazyCreator AddCreator(SingletonId id)
        {
            SingletonLazyCreator creator;

            if (!_creators.TryGetValue(id, out creator))
            {
                creator = new SingletonLazyCreator(_container, this, id);
                _creators.Add(id, creator);
            }

            creator.IncRefCount();
            return creator;
        }

        public ProviderBase CreateProvider<TConcrete>()
        {
            return CreateProvider(typeof(TConcrete));
        }

        public ProviderBase CreateProvider(Type concreteType)
        {
            return new SingletonProvider(_container, AddCreator(concreteType));
        }

        public ProviderBase CreateProviderFromPrefab<TConcrete>(GameObject prefab)
            where TConcrete : Component
        {
            return CreateProviderFromPrefab(typeof(TConcrete), prefab);
        }

        public ProviderBase CreateProviderFromPrefab(Type concreteType, GameObject prefab)
        {
            return new SingletonProvider(_container, AddCreator(new SingletonId(concreteType, prefab)));
        }

        public ProviderBase CreateProvider<TConcrete>(Func<DiContainer, TConcrete> method)
        {
            return new SingletonProvider(_container, AddCreator(method));
        }

        public ProviderBase CreateProvider<TConcrete>(TConcrete instance)
        {
            return CreateProvider(typeof(TConcrete), instance);
        }

        public ProviderBase CreateProvider(Type concreteType, object instance)
        {
            Assert.That(instance != null || _container.AllowNullBindings);

            if (instance != null)
            {
                Assert.That(instance.GetType() == concreteType);
            }

            var creator = AddCreator(concreteType);

            if (creator.HasInstance())
            {
                throw new ZenjectBindException("Found multiple singleton instances bound to the type '{0}'".With(concreteType.Name()));
            }

            creator.SetInstance(instance);

            return new SingletonProvider(_container, creator);
        }
    }
}
