using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.EqualityComparison.Float
{
	// TODO write Unit Tests for this in case of Tunneling
	public static class FloatComparer
	{
		public static bool Compare(float a, float b, float epsilon)
		{
			float absA = Math.Abs(a);
			float absB = Math.Abs(b);
			float diff = Math.Abs(a - b);

			if (a == b)
				return true;
			else if (a * b == 0)
				return diff < (epsilon * epsilon);
			else if (absA + absB == diff)
				return diff < epsilon;
			else
				return diff / (absA + absB) < epsilon;
		}

		public static bool Compare(float a, float b)
		{
			float absA = Math.Abs(a);
			float absB = Math.Abs(b);
			float diff = Math.Abs(a - b);

			if (a == b)
				return true;
			else if (a * b == 0)
				return diff < (float.Epsilon * float.Epsilon);
			else if (absA + absB == diff)
				return diff < float.Epsilon;
			else
				return diff / (absA + absB) < float.Epsilon;
		}
	}
}