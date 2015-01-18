using System;
using Assets.Scripts.Maths.PointSearch;
using Assets.Scripts.Maths.PointSearch.Interfaces;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD.BoundingGeneration
{
	public class SphereGeneration
	{
		public IGeometrySearch GeometrySearch = new GeometrySearch();
		public Sphere3D Sphere3D;

		System.Random random = new System.Random();

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
					points[i] = points[random.Next(i + 1, points.Length + 1)];
					SphereOfSphereAndPoint(points[i]);
				}

				if (s2.Radius < Sphere3D.Radius)
					Sphere3D = s2;
			}
		}
	}
}