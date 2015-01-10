using System;
using Assets.Scripts.CollisionBoxes.TwoD;

namespace Assets.Scripts.IntersectionTests
{
	public class AABBIntersection
	{
		public bool Intersect(AABB2D a, AABB2D b)
		{
			if (Math.Abs(a.Center.x - b.Center.x) > (a.HalfWidth + b.HalfWidth))
				return false;
			if (Math.Abs(a.Center.y - b.Center.y) > (a.HalfHeight + b.HalfHeight))
				return false;

			return true;
		}
	}
}