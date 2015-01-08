using System;
using Assets.Scripts.Policies.Interfaces;
using ModestTree;
using UnityEngine;

namespace Assets.Scripts.CoordinateTests
{
	public static class  GeometryChecks
	{
		public static void Barycentric(Vector2 a, Vector2 b, Vector2 c,
			Vector2 p, out float u, out float v, out float w)
		{
			//// Unnormalized triangle normal
			//Vector2 m = Vector3.Cross(b - a, c - a);
			//// nominators and one-over-denominator for u and v ratios
			//float nu, nv, ood;
			//// Absolute components for determining projection plane
			//var x = Math.Abs(m.x);
			//var y = Math.Abs(m.y);

			//if(x >= y)
			//	nu = 
			// TODO learn how to calculate triangle area with ratios
			Vector2 v0 = b - a, v1 = c - a, v2 = p - a;
			float d00 = Vector2.Dot(v0, v0);
			float d01 = Vector2.Dot(v0, v1);
			float d11 = Vector2.Dot(v1, v1);
			float d20 = Vector2.Dot(v2, v0);
			float d21 = Vector2.Dot(v2, v1);
			float denom = d00 * d11 - d01 * d01;
			v = (d11 * d20 - d01 * d21) / denom;
			w = (d00 * d21 - d01 * d20) / denom;
			u = 1.0f - v - w;
		}

		public static void Barycentric(Vector3 a, Vector3 b, Vector3 c,
			Vector3 p, out float u, out float v, out float w)
		{
			// Unnormalized triangle normal
			var m = Vector3.Cross(b - a, c - a);
			// Nominators and one-over denominator for u and v ratios
			float nu, nv, ood;
			// Absolute components for determining projection plane
			var x = Math.Abs(m.x);
			var y = Math.Abs(m.y);
			var z = Math.Abs(m.z);

			if (x >= y && x >= z)
			{
				nu = TriangleArea2D(p.y, p.z, b.y, b.z, c.y, c.z);
				nv = TriangleArea2D(p.y, p.z, c.y, c.z, a.y, a.z);
				ood = 1.0f/m.x;
			}
			else if (y >= x && y >= z)
			{
				nu = TriangleArea2D(p.x, p.z, b.x, b.z, c.x, c.z);
				nv = TriangleArea2D(p.x, p.z, c.x, c.z, a.x, a.z);
				ood = 1.0f/-m.y;
			}
			else
			{
				nu = TriangleArea2D(p.x, p.y, b.x, b.y, c.x, c.y);
				nv = TriangleArea2D(p.x, p.y, c.x, c.y, a.x, a.y);
				ood = 1.0f/m.z;
			}

			u = nu*ood;
			v = nv*ood;
			w = 1.0f - u - v;
		}

		public static float TriangleArea2D(float x1, float y1, float x2,
			float y2, float x3, float y3)
		{
			return (x1 - x2) * (y2 - y3) - (x2 - x3) * (y1 - y2);
		}

		public static bool TestPointInTriangle(Vector3 a, Vector3 b, Vector3 c,
			Vector3 p)
		{
			float u, v, w;
			Barycentric(a, b, c, p, out u, out v, out w);
			return v >= 0.0f && w >= 0.0f && (v + w) <= 1.0f;
		}
	}
}
