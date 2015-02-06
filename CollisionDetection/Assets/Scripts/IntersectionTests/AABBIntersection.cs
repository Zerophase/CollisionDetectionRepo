using System;
using Assets.Scripts.CollisionBoxes.ThreeD;
using UnityEngine;

namespace Assets.Scripts.IntersectionTests
{
	public class AABBIntersection
	{
		public bool Intersect(AABB3D a, AABB3D b)
		{
			if (Math.Abs(a.Center.x - b.Center.x) > (a.HalfWidth + b.HalfWidth))
				return false;
			if (Math.Abs(a.Center.y - b.Center.y) > (a.HalfHeight + b.HalfHeight))
				return false;
			if (Math.Abs(a.Center.z - b.Center.z) > (a.HalfDepth + b.HalfDepth))
				return false;

			return true;
		}

		public Vector3 FindCrossProducts(AABB3D a, AABB3D b)
		{
			Vector3 collisionVector = Vector3.zero; 

			float max_x_a = a.Center.x + a.HalfWidth;
			float min_x_a = a.Center.x - a.HalfWidth;
			float max_x_b = b.Center.x + b.HalfWidth;
			float min_x_b = b.Center.x - b.HalfWidth;
			
			float max_y_a = a.Center.y + a.HalfHeight;
			float min_y_a = a.Center.y - a.HalfHeight;
			float max_y_b = b.Center.y + b.HalfHeight;
			float min_y_b = b.Center.y - b.HalfHeight;

			float max_z_a = a.Center.z + a.HalfDepth;
			float min_z_a = a.Center.z - a.HalfDepth;
			float max_z_b = b.Center.z + b.HalfDepth;
			float min_z_b = b.Center.z - b.HalfDepth;

			Vector3 vP2 = new Vector3(min_x_a, min_y_a, min_z_a);
			Vector3 vP1 = new Vector3(max_x_a, min_y_a, max_z_a);
			Vector3 vP3 = new Vector3(min_x_a, min_y_a, max_z_a);

			Vector3 vN1 = (vP2 - vP1);
			Vector3 vN2 = (vP3 - vP2);
			Vector3 bTop = new Vector3(max_x_b, max_x_b, max_z_b);
			Vector3 cross = Vector3.Cross(vN1, vN2);
			cross.Normalize();

			float dot = -Vector3.Dot(vP1, cross);

			float p = (Vector3.Dot(cross, a.Center) + dot);
			float pStartLoc;

			return Vector3.zero;
		}
	}
}