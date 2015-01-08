using System;
using System.Collections.Generic;
using Assets.Scripts.EqualityComparison.Float;
using UnityEngine;

namespace Assets.Scripts.EqualityComparison
{
	public class Vector3EqualityComparerWithTolerance : IEqualityComparer<Vector3>
	{
		private float epsilon;

		public Vector3EqualityComparerWithTolerance(float epsilon = float.Epsilon)
		{
			this.epsilon = epsilon;
		}

		public bool Equals(Vector3 v1, Vector3 v2)
		{
			return equalsWithTolerance(v1, v2);
		}

		private bool equalsWithTolerance(Vector3 v1, Vector3 v2)
		{
			if (FloatComparer.Compare(v1.x, v2.x) && FloatComparer.Compare(v1.y, v2.y)
				&& FloatComparer.Compare(v1.z, v2.z))
				return true;
			
			return false;
		}

		public int GetHashCode(Vector3 obj)
		{
			throw new NotImplementedException();
		}
	}
}
