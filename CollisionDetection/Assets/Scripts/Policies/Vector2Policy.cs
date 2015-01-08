using Assets.Scripts.Policies.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Policies
{
	public class Vector2Policy : IVectorPolicy
	{

		public Vector2 Add(Vector2 a, Vector2 b)
		{
			return a + b;
		}

		public Vector2 Subtract(Vector2 a, Vector2 b)
		{
			return a - b;
		}

		public float Dot(Vector2 lhs, Vector2 rhs)
		{
			return Vector2.Dot(lhs, rhs);
		}
	}
}