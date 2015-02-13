using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Plane = Assets.Scripts.CollisionBoxes.ThreeD.Plane;

namespace Assets.Scripts.IntersectionTests
{
	public class PlaneIntersection
	{
		public bool IntersectPlanes(Plane p1, Plane p2, ref Vector3 p, ref Vector3 direction)
		{
			// If direction = 0 return p1.Normal?
			direction = Vector3.Cross(p1.Normal, p2.Normal);

			float denom = Vector3.Dot(direction, direction);
			if (denom < float.Epsilon)
				return false;

			p = Vector3.Cross(p1.D*p2.Normal - p2.D*p1.Normal, direction);
			return true;
		}

		public float ClosestVector3ToPlane(Vector3 q, Plane p)
		{
			//float t = Vector3.Dot(p.Normal, q) - p.D;
			return  (Vector3.Dot(p.Normal, q) - p.D) / Vector3.Dot(p.Normal, p.Normal); //q - t * p.Normal;
		}
	}
}
