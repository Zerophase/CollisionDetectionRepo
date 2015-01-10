using Assets.Scripts.CollisionBoxes.TwoD;
using UnityEngine;

namespace Assets.Scripts.IntersectionTests
{
	public class SphereIntersection
	{
		public bool TestSphereSphere(Sphere2D a, Sphere2D b)
		{
			Vector2 distance = a.Center - b.Center;
			float distSquared = Vector2.Dot(distance, distance);
			float radiusSum = a.Radius + b.Radius;
			return distSquared <= radiusSum*radiusSum;
		}
	}
}