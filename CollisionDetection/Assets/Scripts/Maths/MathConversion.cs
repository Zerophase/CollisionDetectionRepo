using System;
using Assets.Scripts.EqualityComparison.Float;
using UnityEngine;

namespace Assets.Scripts.Maths
{
	public static class MathConversion
	{
		private static float[][] identityMatrix =
		{
			new [] {1.0f, 0.0f, 0.0f},
			new []{0.0f, 1.0f, 0.0f},
			new []{0.0f, 0.0f, 1.0f}
		};

		public static float[][] QuaternionTo3x3(this Quaternion value)
		{
			float[][] matrix3x3 =
			{
				new float[3],
				new float[3],
				new float[3],
			};

			float[][] symetricalMatrix =
			{
				new float[3] {(-(value.y * value.y) - (value.z * value.z)), value.x * value.y, value.x * value.z},
				new float[3] {value.x * value.y, (-(value.x * value.x) - (value.z * value.z)), value.y * value.z},
 				new float[3] {value.x * value.z, value.y * value.z, (-(value.x * value.x) - (value.y * value.y))} 
			};

			float[][] antiSymetricalMatrix =
			{
				new[] {0.0f, -value.z, value.y},
				new []{value.z, 0.0f, -value.x},
				new []{-value.y, value.x, 0.0f}
			};

			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					matrix3x3[i][j] = identityMatrix[i][j] +
					                  (2.0f*symetricalMatrix[i][j]) + 
									  (2.0f*value.w * antiSymetricalMatrix[i][j]);
				}
			}

			return matrix3x3;
		}
	}
}