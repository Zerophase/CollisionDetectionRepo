using System;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class OBB3D
	{
		private Vector3 center;

		public Vector3 Center
		{
			get { return center; } 
			set { center = value; }
		}

		// TODO Make more effiecnt by only computing 2 Orientation Vectors
		// and calculating the third at time of collision test Page 101
		private Vector3[] orientationMatrix = new Vector3[3];
		public Vector3[] OrientationMatrix
		{
			get { return orientationMatrix; } 
			set { orientationMatrix = value; } 
		}
		private Vector3 halfWidths;

		public Vector3 HalfWidths
		{
			get { return halfWidths; }
			set { halfWidths = value; }
		}

		public OBB3D(Vector3 center, Vector3[] orientationMatrix, Vector3 widths)
		{
			this.center = center;
			
			if(orientationMatrix.Length != 3)
				throw new ArgumentException("OBB3D member variable orientationMatrix " +
				                            "should be Vector3[3]");
			this.orientationMatrix = orientationMatrix;
			this.halfWidths = widths/2.0f;
		}

		public override bool Equals(object obj)
		{
			OBB3D test = (OBB3D) obj;
			if (test.Center == center &&
			    test.OrientationMatrix == orientationMatrix &&
			    test.HalfWidths == halfWidths)
				return true;

			return false;
		}
	}
}