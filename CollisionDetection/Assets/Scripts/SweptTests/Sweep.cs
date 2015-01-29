using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.IntersectionTests;

namespace Assets.Scripts.SweptTests
{
	public class Sweep
	{
		SphereIntersection intersection = new SphereIntersection();
		AABBIntersection aabbIntersection = new AABBIntersection();

		//TODO An AABB would serve as a good Substitute for us
		public bool TestMovingSphereSphere(Sphere3D s0, Vector3 d, float time0, float time1,
			Sphere3D s1, ref float time)
		{
			Sphere3D b;
			float mid = (time0 + time1) * 0.5f;
			b = new Sphere3D(s0.Center + d * mid, (mid - time0) * d.magnitude + s0.Radius);
			if(!intersection.TestSphereSphere(b, s1))
				return false;

			if (time1 - time0 < Time.deltaTime)
			{
				time = time0;
				return true;
			}

			if(TestMovingSphereSphere(s0, d, time0, mid, s1, ref time))
				return true;

			return TestMovingSphereSphere(s0, d, mid, time1, s1, ref time);
		}

		public bool IntervalCollision(AABB3D a, AABB3D b, float startTime, float endTime, ref float hitTime)
		{
			float maxMoveA = MaximumObjectMovementOverTime(a, startTime, endTime);
			float maxMoveB = MaximumObjectMovementOverTime(b, startTime, endTime);
			float maxMoveDistSum = maxMoveA + maxMoveB;

			float minDistStart = minimumObjectDistanctAtTime(a, b, startTime);
			if (minDistStart > maxMoveDistSum)
				return false;

			float minDistEnd = minimumObjectDistanctAtTime(a, b, endTime);
			if(endTime - startTime < Time.deltaTime)
			{
				hitTime = startTime;
				return true;
			}

			float midTime = (startTime + endTime) * 0.5f;
			if (IntervalCollision(a, b, startTime, midTime, ref hitTime))
				return true;

			return IntervalCollision(a, b, midTime, endTime, ref hitTime);
		}

		// TODO Rewrite with correct calcuations
		public float minimumObjectDistanctAtTime(AABB3D a, AABB3D b, float time)
		{
			float distanceTraveled = a.Center.x * time;
			return distanceTraveled;
		}

		// TODO Rewrite with correct calcuations
		public float MaximumObjectMovementOverTime(AABB3D movingObject, float startTime, float endTime)
		{
			float distanceTraveled = movingObject.Center.x * (startTime - endTime);
			return distanceTraveled;
		}
	}
}
