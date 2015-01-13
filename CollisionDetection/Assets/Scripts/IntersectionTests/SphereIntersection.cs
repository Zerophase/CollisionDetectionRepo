using Assets.Scripts.CollisionBoxes.ThreeD;
using UnityEngine;

namespace Assets.Scripts.IntersectionTests
{
	public class SphereIntersection
	{
		public bool TestSphereSphere(Sphere3D a, Sphere3D b)
		{
			Vector2 distance = a.Center - b.Center;
			float distSquared = Vector2.Dot(distance, distance);
			float radiusSum = a.Radius + b.Radius;
			return distSquared <= radiusSum*radiusSum;
		}
	}
}