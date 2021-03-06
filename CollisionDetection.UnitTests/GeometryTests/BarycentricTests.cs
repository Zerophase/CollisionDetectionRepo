﻿using System.Linq.Expressions;
using Assets.Scripts.CoordinateTests;
using Assets.Scripts.EqualityComparison;
using Assets.Scripts.EqualityComparison.Float;
using Assets.Scripts.Policies;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.GeometryTests
{
	// TODO Make tests pass for Vectors on (1f, 1f), and other pairs with same numbers
	[TestFixture]
	public class BarycentricTests
	{
		// TODO Move into Factory Method triangle around origin
		private Vector2 vector1 = new Vector2(-1f, 1f);
		private Vector2 vector2 = new Vector2(1f, 1f);
		private Vector2 vector3 = new Vector2(0f, -1f);

		private Vector3 vector3One = new Vector3(-1f, -1f, -1f);
		private Vector3 vector3Two = new Vector3(1f, 1f, 1f);
		private Vector3 vector3Three = new Vector3(0f, 1f, -1f);
		
		private  FloatComparerTolerance floatTolerance = 
			new FloatComparerTolerance(0.0001f);

		private static object[] outsideCasesVector2 =
		{
			new object[] {1.5f, -3.5f, 3.0f, new Vector2(-5f, -5f)},
			new object[] {1.5f, 1.5f, -2.0f, new Vector2(0f, 5f)},
			new object[] {-3.5f, 1.5f, 3.0f, new Vector2(5f, -5f)}
		};

		[Test, TestCaseSource("outsideCasesVector2")]
		public void BarycentricVector2_OutsideOfTriangle_OutWeightings(
			float expectedU, float expectedV, float expectedW, Vector2 testVector)
		{
			float u, v, w;
			GeometryChecks.Barycentric(vector1, vector2, vector3,
				testVector, out u, out v, out w);
			
			Assert.That(u, Is.EqualTo(expectedU).Using(floatTolerance));
			Assert.That(v, Is.EqualTo(expectedV).Using(floatTolerance));
			Assert.That(w, Is.EqualTo(expectedW).Using(floatTolerance));
		}

		[Test]
		public void BarycentricVector2_InOriginTriangle_OutWeightings()
		{
			Vector2 centerVector = new Vector2(0f, 0.333f);

			float u, v, w;
			float expectedU = 0.33325f;
			float expectedV = 0.33325f;
			float expectedW = 0.33350f;
			GeometryChecks.Barycentric(vector1, vector2, vector3,
				centerVector, out u, out v, out w);

			Assert.That(u, Is.EqualTo(expectedU).Using(floatTolerance));
			Assert.That(v, Is.EqualTo(expectedV).Using(floatTolerance));
			Assert.That(w, Is.EqualTo(expectedW).Using(floatTolerance));
		}

		private static object[] onLineCasesVector2 =
		{
			new object[] {0.5f, 0.0f, 0.5f, new Vector2(-0.5f, 0.0f)},
			new object[] {0.0f, 0.5f, 0.5f, new Vector2(0.5f, 0.0f)},
			new object[] {0.5f, 0.5f, 0.0f, new Vector2(0.0f, 1.0f)}
		};

		[Test, TestCaseSource("onLineCasesVector2")]
		public void BarycentricVector2_OnCenterOfLineSegments_OutWeightings(
			float expectedU, float expectedV, float expectedW, Vector2 centerOfLine)
		{
			float u, v, w;
			GeometryChecks.Barycentric(vector1, vector2, vector3,
				centerOfLine, out u, out v, out w);

			Assert.That(u, Is.EqualTo(expectedU).Using(floatTolerance));
			Assert.That(v, Is.EqualTo(expectedV).Using(floatTolerance));
			Assert.That(w, Is.EqualTo(expectedW).Using(floatTolerance));
		}

		// Not sure if Vector3 coordinates are correct for testing
		// outside of Triangle
		// 0001f
		private static object[] outsideCasesVector3 =
		{
			new object[] {3.0f, -2.0f, 0.0f, new Vector3(-5f, -5f, -5f)},
			new object[] {-2.0f, 3.0f, 0.0f, new Vector3(0f, 5f, 5f)},
			new object[] {3.0f, -2.0f, 0.0f, new Vector3(5f, -5f, -5f)}
		};

		[Test, TestCaseSource("outsideCasesVector3")]
		public void BarycentricVector3_OutsideOfTriangle_OutWeightings(
			float expectedU, float expectedV, float expectedW, Vector3 testVector)
		{
			float u, v, w;
			GeometryChecks.Barycentric(vector3One, vector3Two, vector3Three,
				testVector, out u, out v, out w);

			Assert.That(u, Is.EqualTo(expectedU).Using(floatTolerance));
			Assert.That(v, Is.EqualTo(expectedV).Using(floatTolerance));
			Assert.That(w, Is.EqualTo(expectedW).Using(floatTolerance));
		}

		[Test]
		public void BarycentricVector3_InOriginTriangle_OutWeightings()
		{
			Vector3 centerVector = new Vector3( 0.0f, 0.333f, -0.333f);

			float u, v, w;
			float expectedU = 0.3335f;
			float expectedV = 0.3335f;
			float expectedW = 0.33299f;
			GeometryChecks.Barycentric(vector3One, vector3Two, vector3Three,
				centerVector, out u, out v, out w);

			Assert.That(u, Is.EqualTo(expectedU).Using(floatTolerance));
			Assert.That(v, Is.EqualTo(expectedV).Using(floatTolerance));
			Assert.That(w, Is.EqualTo(expectedW).Using(floatTolerance));
		}

		private static object[] onLineCasesVector3 =
		{
			new object[] {0.5f, 0.5f, 0.0f, new Vector3(0.0f, 0.0f, 0.0f)},
			new object[] {0.0f, 0.5f, 0.5f, new Vector3(0.5f, 1.0f, 0.0f)},
			new object[] {0.5f, 0.0f, 0.5f, new Vector3(-0.5f, 0.0f, -1.0f)}
		};

		[Test, TestCaseSource("onLineCasesVector3")]
		public void BarycentricVector3_OnCenterOfLineSegments_OutWeightings(
			float expectedU, float expectedV, float expectedW, Vector3 centerOfLine)
		{
			float u, v, w;
			GeometryChecks.Barycentric(vector3One, vector3Two, vector3Three,
				centerOfLine, out u, out v, out w);

			Assert.That(u, Is.EqualTo(expectedU).Using(floatTolerance));
			Assert.That(v, Is.EqualTo(expectedV).Using(floatTolerance));
			Assert.That(w, Is.EqualTo(expectedW).Using(floatTolerance));
		}

		[Test]
		public void TestPointInTriangleVector3_PointInTriangle_ReturnTrue()
		{
			Vector3 vectorInTriangle = new Vector3( 0.0f, 0.333f, -0.333f);

			var expected = true;
			var result = GeometryChecks.TestPointInTriangle(vector3One, vector3Two, vector3Three,
				vectorInTriangle);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void TriangleArea2D_TriangleHasExpectedArea_ReturnsFloat()
		{
			Vector3 pointInTriangle1 = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 pontInTriangle2 = new Vector3(1.0f, 1.0f, 1.0f);
			Vector3 pointInTriangle3 = new Vector3(0.0f, 1.0f, 0.0f);

			float expected = 1.0f;
			float actual = GeometryChecks.TriangleArea2D(pointInTriangle1.x, pointInTriangle1.y,
				pontInTriangle2.x, pontInTriangle2.y, pointInTriangle3.x, pointInTriangle3.y);

			Assert.AreEqual(expected, actual);
		}
	}
}
