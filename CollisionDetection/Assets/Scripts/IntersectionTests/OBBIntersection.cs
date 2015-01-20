using System;
using Assets.Scripts.CollisionBoxes.ThreeD;
using UnityEngine;

namespace Assets.Scripts.IntersectionTests
{
	public class OBBIntersection
	{
		public bool TestOBBOBB(OBB3D a, OBB3D b)
		{
			float rotationA, rotationB;

			// TODO Write roationMatrix3x3 class
			float[][] rotation =
			{
				new float[3], 
 				new float[3],
 				new float[3], 
			};
			float[][] absRotation =
			{
				new float[3],
 				new float[3],
 				new float[3], 
			};

			// TODO Calculate rotation and absRotation
			// as the first six tests are being run to speed this test up.
			// Compute rotation matrix expressing b in a's coordinate frame
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					rotation[i][j] = Vector3.Dot(a.OrientationMatrix[i], b.OrientationMatrix[j]);
				}
			}

			// Compute translation vector translation
			Vector3 translation = b.Center - a.Center;
			// bring translation into a's coordinate frame
			translation = new Vector3(Vector3.Dot(translation, a.OrientationMatrix[0]),
									Vector3.Dot(translation, a.OrientationMatrix[1]),
									Vector3.Dot(translation, a.OrientationMatrix[2]));
			
			// Compute common subexpressions. Add in an epsilon term to counteract
			// arithmetic errors when two edges are parallel and
			// their cross product is near null.
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					absRotation[i][j] = Math.Abs(rotation[i][j]) + float.Epsilon;
				}
			}

			// Test axes L = A0, L= A1, L = A2
			for (int i = 0; i < 3; i++)
			{
				rotationA = a.HalfWidths[i];
				rotationB = b.HalfWidths[0]*absRotation[i][0] +
				            b.HalfWidths[1]*absRotation[i][1] +
				            b.HalfWidths[2]*absRotation[i][2];
				if (Math.Abs(translation[i]) > rotationA + rotationB)
					return false;
			}

			// Test axes L = B0, L = B1, L = A2
			for (int i = 0; i < 3; i++)
			{
				rotationA = a.HalfWidths[0]*absRotation[0][i] +
				            a.HalfWidths[1]*absRotation[1][i] +
				            b.HalfWidths[2]*absRotation[2][i];
				rotationB = b.HalfWidths[i];
				if (
						Math.Abs
						(
							translation[0]*rotation[0][i] +
							translation[1]*rotation[1][i] +
							translation[2]*rotation[2][i]
						) > rotationA + rotationB
					)
						return false;
			}

			// Test axis L = A0 x B0
			rotationA = a.HalfWidths[1]*absRotation[2][0] +
			            a.HalfWidths[2]*absRotation[1][0];
			rotationB = b.HalfWidths[1]*absRotation[0][2] +
			            b.HalfWidths[2]*absRotation[0][1];
			if (Math.Abs(translation[2]*rotation[1][0] -
			             translation[1]*rotation[2][0]) > rotationA + rotationB)
				return false;

			// Test axis L = A0 * B1
			rotationA = a.HalfWidths[1]*absRotation[2][1] +
			            a.HalfWidths[2]*absRotation[1][1];
			rotationB = b.HalfWidths[0]*absRotation[0][2] +
			            b.HalfWidths[2]*absRotation[0][0];
			if (Math.Abs(translation[2]*rotation[1][1] -
			             translation[1]*rotation[2][1]) > rotationA + rotationB)
				return false;

			// Test axis L = A0 x B2
			rotationA = a.HalfWidths[1]*absRotation[2][2] +
			            a.HalfWidths[2]*absRotation[1][2];
			rotationB = b.HalfWidths[0]*absRotation[0][1] +
			            b.HalfWidths[1]*absRotation[0][0];
			if (Math.Abs(translation[2]*rotation[1][2] -
			             translation[1]*rotation[2][2]) > rotationA + rotationB)
				return false;

			// Test axis L = A1 x B0
			rotationA = a.HalfWidths[0]*absRotation[2][0] +
			            a.HalfWidths[2]*absRotation[0][0];
			rotationB = b.HalfWidths[1]*absRotation[1][2] +
			            b.HalfWidths[2]*absRotation[1][1];
			if (Math.Abs(translation[0]*rotation[2][0] -
			             translation[2]*rotation[0][0]) > rotationA + rotationB)
				return false;

			// Test axis L = A1 x B1
			rotationA = b.HalfWidths[0]*absRotation[2][1] +
			            b.HalfWidths[2]*absRotation[0][1];
			rotationB = b.HalfWidths[0]*absRotation[1][2] +
			            b.HalfWidths[2]*absRotation[1][0];
			if (Math.Abs(translation[0]*rotation[2][1] -
			             translation[2]*rotation[0][1]) > rotationA + rotationB)
				return false;

			// Test axis L = A1 x B2
			rotationA = a.HalfWidths[0]*absRotation[2][2] +
			            a.HalfWidths[2]*absRotation[0][2];
			rotationB = b.HalfWidths[0]*absRotation[1][1] +
			            b.HalfWidths[1]*absRotation[1][0];
			if (Math.Abs(translation[0]*rotation[2][2] -
			             translation[2]*rotation[0][2]) > rotationA + rotationB)
				return false;

			// Test axis L = A2 x B0
			rotationA = a.HalfWidths[0]*absRotation[1][0] +
			            a.HalfWidths[1]*absRotation[0][0];
			rotationB = b.HalfWidths[1]*absRotation[2][2] +
			            b.HalfWidths[2]*absRotation[2][1];
			if (Math.Abs(translation[1]*rotation[0][0] -
			             translation[0]*rotation[1][0]) > rotationA + rotationB)
				return false;

			// Test axis L = A2 x B1
			rotationA = a.HalfWidths[0]*absRotation[1][1] +
			            a.HalfWidths[1]*absRotation[0][1];
			rotationB = b.HalfWidths[0]*absRotation[2][2] +
			            b.HalfWidths[2]*absRotation[2][0];
			if (Math.Abs(translation[1]*rotation[0][1] -
			             translation[0]*rotation[1][1]) > rotationA + rotationB)
				return false;

			// Test axis L = A2 x B2
			rotationA = a.HalfWidths[0]*absRotation[1][2] +
			            a.HalfWidths[1]*absRotation[0][2];
			rotationB = b.HalfWidths[0]*absRotation[2][1] +
			            b.HalfWidths[1]*absRotation[2][0];
			if (Math.Abs(translation[1]*rotation[0][2] -
			             translation[0]*rotation[1][2]) > rotationA + rotationB)
				return false;

			// Since no separating axis is found, the OBBs must be intersecting
			return true;
		}
	}
}