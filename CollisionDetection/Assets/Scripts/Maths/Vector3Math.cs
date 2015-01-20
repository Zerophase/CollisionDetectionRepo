using UnityEngine;

namespace Assets.Scripts.Maths
{
	public static class Vector3Math
	{
		// TODO Write own Vector3 Struct that implicitly converts to Unity
		// Vector3 struct
		public static float Multiply(this Vector3 v1, Vector3 v2)
		{
			return v1.x*v2.x + v1.y*v2.y + v1.z*v2.z;
		}
	}
}