using System;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class Sphere3D
	{
		private Vector3 center;
		public Vector3 Center { get { return center; } set { center = value; } }
		private float radius;
		public float Radius { get { return radius; } }

		public Sphere3D()
		{
		}

		public void MostSeperatedPointsOnAABB(out int min, out int max, Vector3[] points)
		{
			int minx = 0, maxx = 0, miny = 0, maxy = 0, minz = 0, maxz = 0;
			for (int i = 0; i < points.Length; i++)
			{
				if (points[i].x < points[minx].x)
					minx = i;
				if (points[i].x > points[maxx].x)
					maxx = i;
				if (points[i].y < points[miny].y)
					miny = i;
				if (points[i].y > points[maxy].y)
					maxy = i;
				if (points[i].z < points[minz].z)
					minz = i;
				if (points[i].z > points[maxz].z)
					maxz = i;
			}

			float distanceSqrX = Vector3.Dot(points[maxx] - points[minx],
				points[maxx] - points[minx]);
			float distanceSqrY = Vector3.Dot(points[maxy] - points[miny],
				points[maxy] - points[miny]);
			float distanceSqrZ = Vector3.Dot(points[maxz] - points[minz],
				points[maxz] - points[minz]);

			min = minx;
			max = maxx;

			if (distanceSqrY > distanceSqrX && distanceSqrY > distanceSqrZ)
			{
				max = maxy;
				min = miny;
			}
			if (distanceSqrZ > distanceSqrX && distanceSqrZ > distanceSqrY)
			{
				max = maxz;
				min = minz;
			}
		}

		public void SphereFromDistantPoints(Vector3[] pt)
		{
			int min, max;
			MostSeperatedPointsOnAABB(out min, out max, pt);

			center = (pt[min] + pt[max])*0.5f;
			radius = Vector3.Dot(pt[max] - center, pt[max] - center);
			radius = (float) Math.Sqrt(radius);
		}

		public void SphereOfSphereAndPoint(Vector3 point)
		{
			Vector3 distance = point - center;
			float distanceSqr = Vector2.Dot(distance, distance);
			if (distanceSqr > radius*radius)
			{
				float dist = (float)Math.Sqrt(distanceSqr);
				float newRadius = (radius + dist)*0.5f;
				float k = (newRadius - radius)/dist;
				radius = newRadius;
				Center += distance*k;
			}
		}

		public void RitterSphere(Vector3[] point)
		{
			SphereFromDistantPoints(point);

			for (int i = 0; i < point.Length; i++)
				SphereOfSphereAndPoint(point[i]);
		}

		public void DrawCenterLines()
		{
			Debug.DrawLine(center, new Vector3(center.x + radius/ 2.0f, center.y, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x - radius / 2.0f, center.y, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y + radius / 2.0f, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y - radius / 2.0f, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y, center.z + radius / 2.0f), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y, center.z - radius / 2.0f), Color.yellow);
		}

		// More accurate means of computing below
		public float Variance(float[] x)
		{
			float u = 0.0f;
			for (int i = 0; i < x.Length; i++)
				u += x[i];
			u /= x.Length;
			float sqr = 0.0f;
			for (int i = 0; i < x.Length; i++)
				sqr += (x[i] - u)*(x[i] - u);
			return sqr/x.Length;
		}

	}
}