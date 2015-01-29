using System;
using Microsoft.Win32.SafeHandles;
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
				                            "requires a Vector3[3]");
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

		// Compute the center point 'c' and axis orientation oM[0] and oM[1], of
		// the minimum area rectanble to the xy plane containing the points pt[]
		float MinAreaRect(Vector2[] point, int numPoints, ref Vector2 c, ref Vector2[] oM)
		{
			float minArea = float.MaxValue;

			// Loop through all edges; j trails i by 1, modulo numpts
			for (int i = 0, j = numPoints - 1; i < numPoints; j = i, i++)
			{
				// Get current edge e0 (e0x, e0y), normalized
				Vector2 e0 = point[i] - point[j];
				e0.Normalize();

				// Get an axis e1 orthogonal to edge e0
				Vector2 e1 = new Vector2(-e0.y, e0.x); // = Perp2D(e0)

				// Loop through all point to get maximum extents
				float min0 = 0.0f, min1 = 0.0f, max0 = 0.0f, max1 = 0.0f;
				for (int k = 0; k < numPoints; k++)
				{
					// Project points onto axes e0 and e1 and keep track
					// of minimum and maximum values along both axes
					Vector2 distance = point[k] - point[j];
					float dot = Vector2.Dot(distance, e0);
					if (dot < min0)
						min0 = dot;
					if (dot > max0)
						max0 = dot;
					dot = Vector2.Dot(distance, e1);
					if (dot < min1)
						min1 = dot;
					if (dot > max1)
						max1 = dot;
				}
				float area = (max0 - min0)*(max1 - min1);

				// if best so far, remember area, center, and axes
				if (area < minArea)
				{
					minArea = area;
					c = point[j] + 0.5f*((min0 + max0)*e0 + (min1 + max1)*e1);
					oM[0] = e0;
					oM[1] = e1;
				}
			}
			return minArea;
		}


		public void UpdateRotation(Vector3[] oM)
		{
			this.orientationMatrix = oM;
		}

		public void DrawBoundingBox()
		{
			Vector3 tempCenter = center;
			Vector3 tempHalfWidths = halfWidths;
			tempCenter.Normalize();
			tempHalfWidths.Normalize();
			var xtopLeftBack = orientationMatrix[0].x * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y - tempHalfWidths.y) 
				+ orientationMatrix[2].x * (tempCenter.z - tempHalfWidths.z);
			var xtopRightBack = orientationMatrix[0].x * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y - tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z - tempHalfWidths.z);
			var xbottomLeftBack = orientationMatrix[0].x * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y + tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z - tempHalfWidths.z);
			var xBottomRightBack = orientationMatrix[0].x * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y + tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z - tempHalfWidths.z);

			var xtopRightFront = orientationMatrix[0].x * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y - tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z + tempHalfWidths.z);
			var xtopLeftFront = orientationMatrix[0].x * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y - tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z + tempHalfWidths.z);
			var xBottomRightFront = orientationMatrix[0].x * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y + tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z + tempHalfWidths.z);
			var xBottomLeftFront = orientationMatrix[0].x * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].x * (tempCenter.y + tempHalfWidths.y)
				+ orientationMatrix[2].x * (tempCenter.z + tempHalfWidths.z);

			var yTopLeftBack = orientationMatrix[0].y * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z - tempHalfWidths.z);
			var yTopRightBack = orientationMatrix[0].y * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y +  tempHalfWidths.y) + 
				orientationMatrix[2].y * (tempCenter.z - tempHalfWidths.z);
			var yBottomLeftBack = orientationMatrix[0].y * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z - tempHalfWidths.z);
			var yBottomRightBack = orientationMatrix[0].y * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z - tempHalfWidths.z);
			
			var yTopLeftFront = orientationMatrix[0].y * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z + tempHalfWidths.z);
			var yTopRightFront = orientationMatrix[0].y * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z + tempHalfWidths.z);
			var yBottomLeftFront = orientationMatrix[0].y * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z + tempHalfWidths.z);
			var yBottomRightFront = orientationMatrix[0].y * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].y * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].y * (tempCenter.z + tempHalfWidths.z);

			var zTopRightBack = orientationMatrix[0].z * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y - tempHalfWidths.y) + 
				orientationMatrix[2].z * (tempCenter.z + tempHalfWidths.z);
			var zTopLeftBack = orientationMatrix[0].z * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z + tempHalfWidths.z);
			var zBottomLeftBack = orientationMatrix[0].z * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z + tempHalfWidths.z);
			var zBottomRightBack = orientationMatrix[0].z * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z + tempHalfWidths.z);

			var zTopLeftFront = orientationMatrix[0].z * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z - tempHalfWidths.z);
			var zTopRightFront = orientationMatrix[0].z * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y - tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z - tempHalfWidths.z);
			var zBottomLeftFront = orientationMatrix[0].z * (tempCenter.x - tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z - tempHalfWidths.z);
			var zBottomRightFront = orientationMatrix[0].z * (tempCenter.x + tempHalfWidths.x) + orientationMatrix[1].z * (tempCenter.y + tempHalfWidths.y) +
				orientationMatrix[2].z * (tempCenter.z - tempHalfWidths.z);

			Vector3 topLeftBack = new Vector3(xtopLeftBack, yTopLeftBack, zTopLeftBack) + Center;
			Vector3 topRightBack = new Vector3(xtopRightBack, yTopRightBack, zTopRightBack) + Center;
			Vector3 bottomLeftBack = new Vector3(xbottomLeftBack, yBottomLeftBack, zBottomLeftBack) + Center;
			Vector3 bottomRightBack = new Vector3(xBottomRightBack, yBottomRightBack, zBottomRightBack) + Center;
			Vector3 topLeftFront = new Vector3(xtopLeftFront, yTopLeftFront, zTopLeftFront) + Center;
			Vector3 topRightFront = new Vector3(xtopRightFront, yTopRightFront, zTopRightFront) + Center;
			Vector3 bottomLeftFront = new Vector3(xBottomLeftFront, yBottomLeftFront, zBottomLeftFront) + Center;
			Vector3 bottomRightFront = new Vector3(xBottomRightFront, yBottomRightFront, zBottomRightFront) + Center;

			// back face
			Debug.DrawLine(topLeftBack, topRightBack, Color.magenta);
			Debug.DrawLine(bottomLeftBack, bottomRightBack, Color.magenta);
			Debug.DrawLine(topLeftBack, bottomLeftBack, Color.magenta);
			Debug.DrawLine(topRightBack, bottomRightBack, Color.magenta);

			//// front face
			Debug.DrawLine(topLeftFront, topRightFront, Color.magenta);
			Debug.DrawLine(bottomLeftFront, bottomRightFront, Color.magenta);
			Debug.DrawLine(topLeftFront, bottomLeftFront, Color.magenta);
			Debug.DrawLine(topRightFront, bottomRightFront, Color.magenta);
			//// left side face
			Debug.DrawLine(topLeftFront, topLeftBack, Color.magenta);
			Debug.DrawLine(bottomLeftFront, bottomLeftBack, Color.magenta);

			//// right side face
			Debug.DrawLine(topRightFront, topRightBack, Color.magenta);
			Debug.DrawLine(bottomRightFront, bottomRightBack, Color.magenta);
		}
	}
}