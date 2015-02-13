using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Plane = Assets.Scripts.CollisionBoxes.ThreeD.Plane;
using Assets.Scripts.EqualityComparison.Float;
using Assets.Scripts.IntersectionTests;

namespace Assets.Scripts.SweptTests
{
	public class Sweep
	{
		SphereIntersection intersection = new SphereIntersection();
		AABBIntersection aabbIntersection = new AABBIntersection();
		PlaneIntersection planeIntersection = new PlaneIntersection();

		private List<AABB3D> intervalHalvedRects = new List<AABB3D>(); 
		public void ResetRectangles()
		{
			intervalHalvedRects.Clear();
		}

		public bool TestMovingAABB(AABB3D b0, Vector3 d, float time0, float time1,
			AABB3D b1, ref float time)
		{
				
			AABB3D b;
			float mid = (time0 + time1)*0.5f;
			float midTest = mid - time0;
			Vector3 adjustedD = d*mid;
			//Debug.Log("Time tested: " + centerTest);
			b = new AABB3D(b0.Center + adjustedD, midTest * d.magnitude
				+ (b0.HalfWidth * 2), midTest * d.magnitude
				+ (b0.HalfHeight * 2), midTest * d.magnitude
				+ (b0.HalfDepth * 2));
			//Debug.Log(b);

			intervalHalvedRects.Add(b);
			//b0.DrawBoundingBox(Color.green);
			//foreach (var rects in intervalHalvedRects)
			//{
			//	rects.DrawBoundingBox(Color.red);
			//}
			if (!aabbIntersection.Intersect(b, b1))
				return false;


			if (time1 - time0 < 0.00001f)
			{
				Vector3 bottomVector = b.Bottom.Normal + b.DistanceToBottom;
				float top = planeIntersection.ClosestVector3ToPlane(bottomVector, b1.Top);

				Vector3 topVector = b.Top.Normal + b.DistanceToTop;
				float bottom = planeIntersection.ClosestVector3ToPlane(topVector, b1.Bottom);

				normalCollision(top, bottom, ref b, b1);
				time = time0;
				return true;
			}

			if (TestMovingAABB(b0, d, time0, mid, b1, ref time))
				return true;

			return TestMovingAABB(b0, d, mid, time1, b1, ref time);
		}

		private void normalCollision(float top, float bottom, ref AABB3D movingBox, AABB3D b1)
		{
			if(Math.Abs(top) > Math.Abs(bottom))
			{
				Debug.Log("Top: " + top);
				movingBox.NormalCollision[0] = movingBox.Top.Normal;
			}
			else if(Math.Abs(bottom) > Math.Abs(top))
			{
				Debug.Log("Bottom: " + bottom);
				movingBox.NormalCollision[0] = movingBox.Bottom.Normal;
			}
		}

		#region Sphere Test
		private List<Sphere3D> spheres = new List<Sphere3D>();

		public void ResetSpheres()
		{
			spheres.Clear();
		}
		public bool TestMovingSphereSphere(Sphere3D s0, Vector3 d, float time0, float time1,
			Sphere3D s1, ref float time)
		{
			if (time0 > 1.0f)
			{
				time0 = time0 % (float)((int)time0);
			}
			//if (FloatComparer.Compare(d.y, 0.0f))
			//{
			//	time = 1.0f;
			//	return true;
			//}
			Sphere3D b;
			float mid = (time0 + time1) * 0.5f;
			b = new Sphere3D(s0.Center + d * mid, (mid - time0) * d.magnitude + s0.Radius);
			
			//spheres.Add(b);
			//foreach (var sphere in spheres)
			//{
			//	sphere.DrawCenterLines();
			//}
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
			if (minDistEnd > maxMoveDistSum)
				return false;

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
			AABB3D aabb3D = new AABB3D((a.Center) + (b.Center), 
				((a.HalfWidth)  + b.HalfWidth) * 2, 
				((a.HalfHeight) + b.HalfHeight) * 2,
				((a.HalfDepth) + b.HalfDepth) * 2);
			aabb3D.DrawBoundingBox();

			Vector3 possibleCollisionPositions = aabb3D.Center +
			                                     new Vector3(aabb3D.HalfWidth, 
													 aabb3D.HalfHeight,
				                                     aabb3D.HalfDepth);
			Vector3 timed = possibleCollisionPositions*time;
			return timed.magnitude;
		}

		// TODO Rewrite with correct calcuations
		public float MaximumObjectMovementOverTime(AABB3D movingObject, float startTime, float endTime)
		{
			AABB3D aabb3D = new AABB3D(movingObject.Center,
				movingObject.HalfWidth * 2, 
				movingObject.HalfHeight * 2,
				movingObject.HalfDepth * 2);
			Vector3 possibleCollisionPositions = aabb3D.Center +
												 new Vector3(aabb3D.HalfWidth,
													 aabb3D.HalfHeight,
													 aabb3D.HalfDepth);
			float time = endTime - startTime;
			Vector3 timed = possibleCollisionPositions*time;
			return timed.magnitude;
		}
		#endregion
	}
}
