using Assets.Scripts.CollisionBoxes.ThreeD;
using UnityEngine;

namespace Assets.Scripts.IntersectionTests
{
	public class SphereIntersection
	{
		public bool TestSphereSphere(Sphere3D a, Sphere3D b)
		{
			float distSquared = sqrDistanceBetweenPoints(a.Center, b.Center);
			float radiusSum = a.Radius + b.Radius;
			return distSquared <= radiusSum*radiusSum;
		}

		public bool InsideSphere(Sphere3D sphere, Vector3 point)
		{
			float sqrDistance = sqrDistanceBetweenPoints(sphere.Center, point);

			if (sqrDistance <= sphere.Radius*sphere.Radius)
				return true;
			return false;
		}

		private float sqrDistanceBetweenPoints(Vector3 a, Vector3 b)
		{
			Vector3 distance = a - b;
			float distSquared = Vector3.Dot(distance, distance);
			return distSquared;
		}

		public float DistanceToSphereSquared(Sphere3D sphere, Vector3 point)
		{
			return DistanceToPointSquared(sphere.Center, point) - sphere.Radius*sphere.Radius;
		}

		public float DistanceToPointSquared(Vector3 p, Vector3 q)
		{
			float test = (p - q).sqrMagnitude;
			return test;
		}
	}
}