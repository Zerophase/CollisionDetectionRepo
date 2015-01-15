using Assets.Scripts.EqualityComparison.Float;
using Assets.Scripts.Maths;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.Math
{
	[TestFixture]
	public class MathConversionTests
	{
		private FloatComparerTolerance floatComparerTolerance =
			new FloatComparerTolerance(0.01f);

		[Test]
		public void QuaternionTo3x3_NinetyDegreeRight_Return3x3Representation()
		{
			Quaternion quaternion = new Quaternion(0.70f, 0.0f, 0.0f, 0.70f);

			float[][] expected =
			{
				new[] {1.0f, 0.0f, 0.0f},
				new []{0.0f, 0.02f, -0.97f},
				new []{0.0f, 0.97f, 0.02f}
			};
			float[][] actual = quaternion.QuaternionTo3x3();

			Assert.That(expected, Is.EqualTo(actual).Using(floatComparerTolerance));
		}

		[Test]
		public void QuaternionTo3x3_NinetyDegreeBack_Returnds3x3Representation()
		{
			Quaternion quaternion = new Quaternion(0.70f, 0.0f, 0.70f, 0.0f);

			float[][] expected =
			{
				new[] {0.02f, 0.0f, 0.97f},
				new[] {0.0f, -0.95f, 0.0f},
				new []{0.97f, 0.0f, 0.02f}
			};
			float[][] actual = quaternion.QuaternionTo3x3();

			Assert.That(expected, Is.EqualTo(actual).Using(floatComparerTolerance));
		}

		[Test]
		public void QuaternionTo3x3_NinetyDegreeFlip_Returnds3x3Representation()
		{
			Quaternion quaternion = new Quaternion(0.70f, 0.70f, 0.0f, 0.0f);

			float[][] expected =
			{
				new[] {0.02f, 0.97f, 0.0f},
				new[] {0.97f, 0.02f, 0.0f},
				new []{0.0f, 0.0f, -0.95f}
			};
			float[][] actual = quaternion.QuaternionTo3x3();

			Assert.That(expected, Is.EqualTo(actual).Using(floatComparerTolerance));
		}
	}
}