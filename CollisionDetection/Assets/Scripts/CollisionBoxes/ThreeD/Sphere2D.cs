using System;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class Sphere2D
	{
		private Vector2 center;
		public Vector2 Center { get { return center; } set { center = value; } }
		private float radius;
		public float Radius { get { return radius; } }

		public Sphere2D()
		{
		}

		public void MostSeperatedPointsOnAABB(out int min, out int max, Vector2[] points)
		{
			int minx = 0, maxx = 0, miny = 0, maxy = 0;
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
			}

			float distanceSqrX = Vector2.Dot(points[maxx] - points[minx],
				points[maxx] - points[minx]);
			float distanceSqrY = Vector2.Dot(points[maxy] - points[miny],
				points[maxy] - points[miny]);

			min = minx;
			max = maxx;

			if (distanceSqrY > distanceSqrX)
			{
				max = maxy;
				min = miny;
			}
		}

		public void SphereFromDistantPoints(Vector2[] pt)
		{
			int min, max;
			MostSeperatedPointsOnAABB(out min, out max, pt);

			center = (pt[min] + pt[max])*0.5f;
			radius = Vector2.Dot(pt[max] - center, pt[max] - center);
			radius = (float) Math.Sqrt(radius);
		}

		public void SphereOfSphereAndPoint(Vector2 point)
		{
			Vector2 distance = point - center;
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

		public void RitterSphere(Vector2[] point)
		{
			SphereFromDistantPoints(point);

			for (int i = 0; i < point.Length; i++)
				SphereOfSphereAndPoint(point[i]);
		}

		public void DrawCenterLines()
		{
			Debug.DrawLine(center, new Vector2(center.x + radius/ 2, center.y), Color.yellow);
			Debug.DrawLine(center, new Vector2(center.x - radius / 2, center.y), Color.yellow);
			Debug.DrawLine(center, new Vector2(center.x, center.y + radius / 2), Color.yellow);
			Debug.DrawLine(center, new Vector2(center.x, center.y - radius / 2), Color.yellow);
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