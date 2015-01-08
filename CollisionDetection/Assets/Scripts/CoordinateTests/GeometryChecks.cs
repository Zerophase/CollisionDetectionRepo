using Assets.Scripts.Policies.Interfaces;
using UnityEngine;

namespace Assets.Scripts.CoordinateTests
{
	public static class  GeometryChecks
	{
		public static void Barycentric(Vector2 a, Vector2 b, Vector2 c,
			Vector2 p, out float u, out float v, out float w)
		{
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
			Vector3 v0 = b - a, v1 = c - a, v2 = p - a;
			float d00 = Vector3.Dot(v0, v0);
			float d01 = Vector3.Dot(v0, v1);
			float d11 = Vector3.Dot(v1, v1);
			float d20 = Vector3.Dot(v2, v0);
			float d21 = Vector3.Dot(v2, v1);
			float denom = d00 * d11 - d01 * d01;
			v = (d11 * d20 - d01 * d21) / denom;
			w = (d00 * d21 - d01 * d20) / denom;
			u = 1.0f - v - w;
		}
	}
}
