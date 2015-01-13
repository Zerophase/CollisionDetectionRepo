using System;
using Assets.Scripts.CollisionBoxes.ThreeD;

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
	}
}