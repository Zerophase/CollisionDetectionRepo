using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.IntersectionTests;
using Assets.Scripts.Maths;
using Assets.Scripts.Maths.PointSearch;
using Assets.Scripts.Maths.PointSearch.Interfaces;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD.BoundingGeneration
{
	public class SphereGeneration
	{
		public IGeometrySearch GeometrySearch = new GeometrySearch();
		public Sphere3D Sphere3D;

		private SphereIntersection sphereIntersection = new SphereIntersection();

		public System.Random Random { get { return random; } set { random = value; } }
		private System.Random random = new System.Random();

		public void RitterSphere(Vector3[] point)
		{
			SphereFromDistantPoints(point);

			for (int i = 0; i < point.Length; i++)
				SphereOfSphereAndPoint(point[i]);
		}

		public void SphereOfSphereAndPoint(Vector3 point)
		{
			Vector3 distance = point - Sphere3D.Center;
			float distanceSqr = Vector2.Dot(distance, distance);
			if (distanceSqr > Sphere3D.Radius * Sphere3D.Radius)
			{
				float dist = (float)Math.Sqrt(distanceSqr);
				float newRadius = (Sphere3D.Radius + dist) * 0.5f;
				float k = (newRadius - Sphere3D.Radius) / dist;
				Sphere3D.Radius = newRadius;
				Sphere3D.Center += distance * k;
			}
		}

		public void SphereFromDistantPoints(Vector3[] pt)
		{
			int min, max;
			GeometrySearch.MostSeperatedPointsOnAABB(out min, out max, pt);

			Sphere3D.Center = (pt[min] + pt[max]) * 0.5f;
			Sphere3D.Radius = Vector3.Dot(pt[max] - Sphere3D.Center, pt[max] - Sphere3D.Center);
			Sphere3D.Radius = (float)Math.Sqrt(Sphere3D.Radius);
		}

		public void EigenSphere(Vector3[] points)
		{
			float[][] m =
			{
				new float[3],
				new float[3],
 				new float[3] 
			};

			float[][] v = 
			{
				new float[3],
				new float[3],
 				new float[3] 
			};
			GeometrySearch.CovarianceMatrix(ref m, points);
			// Decompose it into egenvectors (in v) and eigenvalues (in m)
			GeometrySearch.Jacobi(ref m, ref v);

			// find the component with the largest magnitude eigenvalue (largest spread)
			int maxc = 0;
			float maxf = Math.Abs(m[0][0]), maxe = Math.Abs(m[0][0]);
			if ((maxf = Math.Abs(m[1][1])) > maxe)
			{
				maxc = 1;
				maxe = maxf;
			}
			if ((maxf = Math.Abs(m[2][2])) > maxe)
			{
				maxc = 2;
				maxe = maxf;
			}

			Vector3 e = new Vector3(v[0][maxc], v[1][maxc], v[2][maxc]);
			// find the most extreme points along direction 'e'
			int imin, imax;
			GeometrySearch.ExtremePointsAlongDirection(e, points, out imin, out imax);
			Vector3 minPoint = points[imin];
			Vector3 maxPoint = points[imax];

			float dist = (float)Math.Sqrt(Vector3.Dot(maxPoint - minPoint, maxPoint - minPoint));
			Sphere3D.Radius = dist * 0.5f;
			Sphere3D.Center = (minPoint + maxPoint) * 0.5f;
		}

		public void RitterEigenSphere(Vector3[] points)
		{
			// Start with spehre from maximum spread
			EigenSphere(points);
			for (int i = 0; i < points.Length; i++)
				SphereOfSphereAndPoint(points[i]);
		}

		public void RitterIterative(Vector3[] points)
		{
			const int numIterations = 8;
			RitterSphere(points);

			Sphere3D s2 = Sphere3D;
			for (int i = 0; i < numIterations; i++)
			{
				// Shrink sphere somewhat to make it an underestimeate (not bound)
				s2.Radius = s2.Radius*0.95f;

				// make sphre bound data again
				for (int j = 0; j < points.Length; j++)
				{
					// swap points[i] with points[k], where k randomly from interval [i+1, numpts -1]
					if(i < numIterations - 1)
						points[i] = points[random.Next(i + 1, points.Length - 1)];
					SphereOfSphereAndPoint(points[i]);
				}

				if (s2.Radius < Sphere3D.Radius)
					Sphere3D = s2;
			}
		}
		
		/// <summary>
		/// Likely to cause stack overflow for inputs larger than a couple thousand.
		/// don't know how to fix
		/// </summary>
		/// <param name="points"></param>
		/// <param name="numPoints"></param>
		/// <param name="sos"></param>
		/// <param name="numSos"></param>
		/// <returns></returns>
		public Sphere3D WelzlSphere(Vector3[] points,  int numPoints, Vector3[] sos,  int numSos = 0)
		{
			if (numPoints == 0)
			{
				switch (numSos)
				{
					case 0:
						return ZeroPoints();
					case 1:
						return OnePoints(sos[0]);
					case 2:
						return TwoPoints(sos[0], sos[1]);
					case 3:
						return ThreePoints(sos[0], sos[1], sos[2]);
					case 4:
						return FourPoints(sos[0], sos[1], sos[2], sos[3]);
				}
			}

			int index = numPoints - 1;
			// this cause the overflow
			Sphere3D smallestSphere = WelzlSphere(points, numPoints - 1, sos, numSos);

			if (sphereIntersection.InsideSphere(smallestSphere, points[index]))
				return smallestSphere;

			sos[numSos] = points[index];
			return WelzlSphere(points, numPoints - 1, sos, numSos + 1);

			#region More Efficent but currently broken. Works in c++ with pointers

			//Sphere3D smallestSphere = new Sphere3D();
			//switch (numSos)
			//{
			//	case 0:
			//		smallestSphere = new Sphere3D();
			//		break;
			//	case 1:
			//		smallestSphere = new Sphere3D(sos[0]);
			//		break;
			//	case 2:
			//		smallestSphere = new Sphere3D(sos[0], sos[1]);
			//		break;
			//	case 3:
			//		smallestSphere = new Sphere3D(sos[0], sos[1], sos[2]);
			//		break;
			//	case 4:
			//		smallestSphere = new Sphere3D(sos[0], sos[1], sos[2], sos[3]);
			//		return smallestSphere;
			//}

			//for (uint i = 0; i < numPoints; i++)
			//{
			//	float test = sphereIntersection.DistanceToSphereSquared(smallestSphere, points[i]);
			//	if (test > 0)
			//	{

			//		for (uint j = i; j > 0; j--)
			//		{
			//			// pick point at random. Here just last point
			//			Vector3 t = points[j];
			//			// swaps point position
			//			points[j] = points[j - 1];
			//			points[j - 1] = t;
			//		}

			//		List<Vector3> newPoints = new List<Vector3>();
			//		for (int j = 0; j < points.Length; j++)
			//		{
			//			if(j < points.Length - 1)
			//				newPoints.Add(points[j + 1]);
			//		}
			//		sos[numSos] = points[i];
			//		smallestSphere = WelzlSphere(points, i, sos, numSos + 1);
			//	}
			//}

			//// recursively compute the smallest sphere of remaining points with new sos
			//return smallestSphere;

			#endregion
		}

		private float radiusEpsilon = 1e-4f;

		public Sphere3D ZeroPoints()
		{
			float radius = -1.0f;
			Vector3 center = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			return new Sphere3D(center, radius);
		}

		public Sphere3D OnePoints(Vector3 center)
		{
			float radius = 0.0f + radiusEpsilon;
			return new Sphere3D(center, radius);
		}

		public Sphere3D TwoPoints(Vector3 vectorOne, Vector3 vector2)
		{
			Vector3 a = vector2 - vectorOne;
			Vector3 tempRad = 0.5f * a;

			float radius = (float)Math.Sqrt(tempRad.Multiply(tempRad)) + radiusEpsilon;
			Vector3 center = vectorOne + tempRad;

			return new Sphere3D(center, radius);
		}

		public Sphere3D ThreePoints(Vector3 vectorOne, Vector3 vector2, Vector3 vector3)
		{
			Vector3 a = vector2 - vectorOne;
			Vector3 b = vector3 - vectorOne;

			float denominator = 2.0f * (Vector3.Cross(a, b).Multiply(Vector3.Cross(a, b)));

			Vector3 tempRadius = (b.sqrMagnitude * (Vector3.Cross(Vector3.Cross(a, b), a)) +
								  (a.sqrMagnitude) * (Vector3.Cross(b, Vector3.Cross(a, b)))) / denominator;

			float radius = tempRadius.magnitude + radiusEpsilon;
			Vector3 center = vectorOne + tempRadius;

			return new Sphere3D(center, radius);
		}

		public Sphere3D FourPoints(Vector3 vectorOne, Vector3 vector2, Vector3 vector3, Vector3 vector4)
		{
			Vector3 a = vector2 - vectorOne;
			Vector3 b = vector3 - vectorOne;
			Vector3 c = vector4 - vectorOne;

			float denominator = 2.0f * determinate(a.x, a.y, a.z,
				b.x, b.y, b.z,
				c.x, c.y, c.z);

			Vector3 tempRad = ((c.sqrMagnitude) * Vector3.Cross(a, b) +
								(b.sqrMagnitude) * Vector3.Cross(c, a) +
								(a.sqrMagnitude) * Vector3.Cross(b, c)) / denominator;
			float radius = tempRad.magnitude + radiusEpsilon;
			Vector3 center = vectorOne + tempRad;

			return new Sphere3D(center, radius);
		}

		private float determinate(float m11, float m12, float m13,
			float m21, float m22, float m23,
			float m31, float m32, float m33)
		{
			return m11 * (m22 * m33 - m32 * m23) -
				   m21 * (m12 * m33 - m32 * m13) +
				   m31 * (m12 * m23 - m22 * m13);
		}
	}
}