using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zenject
{
    public class SingletonId : IEquatable<SingletonId>
    {
        public Type Type;
        public GameObject Prefab;

        public SingletonId(Type type)
        {
            Type = type;
        }

        public SingletonId(Type type, GameObject prefab)
            : this(type)
        {
            Prefab = prefab;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + this.Type.GetHashCode();
                hash = hash * 29 + (this.Prefab == null ? 0 : this.Prefab.GetHashCode());
                return hash;
            }
        }

        public override bool Equals(object other)
        {
            if (other is SingletonId)
            {
                SingletonId otherKey = (SingletonId)other;
                return otherKey == this;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(SingletonId that)
        {
            return this == that;
        }

        public static bool operator ==(SingletonId left, SingletonId right)
        {
            return left.Type == right.Type && left.Prefab == right.Prefab;
        }

        public static bool operator !=(SingletonId left, SingletonId right)
        {
            return !left.Equals(right);
        }
    }
}

