using System;
using System.Collections.Generic;

namespace Zenject
{
    // These classes can be derived from to add post-creation logic to your factory
    // Just call CreateInternal from your custom factory then do whatever you want before
    // or after that before returning the new object

    // Zero parameters
    public abstract class FactoryCustom<T> : IValidatableFactory
    {
        [Inject]
        DiContainer _container = null;

        public Type ConstructedType
        {
            get { return typeof(T); }
        }

        public Type[] ProvidedTypes
        {
            get { return new Type[0]; }
        }

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected T CreateInternal()
        {
            return _container.Instantiate<T>();
        }
    }

    // One parameter
    public abstract class FactoryCustom<TParam1, TValue> : IValidatableFactory
    {
        [Inject]
        DiContainer _container = null;

        public Type ConstructedType
        {
            get { return typeof(TValue); }
        }

        public Type[] ProvidedTypes
        {
            get { return new Type[] { typeof(TParam1) }; }
        }

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected TValue CreateInternal(TParam1 param)
        {
            return _container.InstantiateExplicit<TValue>(
                new List<TypeValuePair>()
                {
                    InstantiateUtil.CreateTypePair(param),
                });
        }
    }

    // Two parameters
    public abstract class FactoryCustom<TParam1, TParam2, TValue> : IValidatableFactory
    {
        [Inject]
        DiContainer _container = null;

        public Type ConstructedType
        {
            get { return typeof(TValue); }
        }

        public Type[] ProvidedTypes
        {
            get { return new Type[] { typeof(TParam1), typeof(TParam2) }; }
        }

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected TValue CreateInternal(TParam1 param1, TParam2 param2)
        {
            return _container.InstantiateExplicit<TValue>(
                new List<TypeValuePair>()
                {
                    InstantiateUtil.CreateTypePair(param1),
                    InstantiateUtil.CreateTypePair(param2),
                });
        }
    }

    // Three parameters
    public abstract class FactoryCustom<TParam1, TParam2, TParam3, TValue> : IValidatableFactory
    {
        [Inject]
        DiContainer _container = null;

        public Type ConstructedType
        {
            get { return typeof(TValue); }
        }

        public Type[] ProvidedTypes
        {
            get { return new Type[] { typeof(TParam1), typeof(TParam2), typeof(TParam3) }; }
        }

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected TValue CreateInternal(TParam1 param1, TParam2 param2, TParam3 param3)
        {
            return _container.InstantiateExplicit<TValue>(
                new List<TypeValuePair>()
                {
                    InstantiateUtil.CreateTypePair(param1),
                    InstantiateUtil.CreateTypePair(param2),
                    InstantiateUtil.CreateTypePair(param3),
                });
        }
    }

    // Four parameters
    public abstract class FactoryCustom<TParam1, TParam2, TParam3, TParam4, TValue> : IValidatableFactory
    {
        [Inject]
        DiContainer _container = null;

        public Type ConstructedType
        {
            get { return typeof(TValue); }
        }

        public Type[] ProvidedTypes
        {
            get { return new Type[] { typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4) }; }
        }

        protected DiContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected TValue CreateInternal(
            TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4)
        {
            return _container.InstantiateExplicit<TValue>(
                new List<TypeValuePair>()
                {
                    InstantiateUtil.CreateTypePair(param1),
                    InstantiateUtil.CreateTypePair(param2),
                    InstantiateUtil.CreateTypePair(param3),
                    InstantiateUtil.CreateTypePair(param4),
                });
        }
    }
}
