using Assets.Scripts.CoordinateTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.GeometryTests
{
	[TestFixture]
	public class PolygonIntersectionTests
	{
		[Test]
		public void IsConvexQuad_Convex_ReturnTrue()
		{
			Vector3 pointOne = new Vector3(1.0f, 1.0f, 1.0f);
			Vector3 pointTwo = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 pointThree = new Vector3(1.0f, 1.0f, 0.0f);
			Vector3 pointFour = new Vector3(0.0f, 1.0f, 1.0f);

			bool expected = true;
			bool result = GeometryChecks.IsConvexQuad(pointOne, pointTwo, pointThree,
				pointFour);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void IsConvexQuad_Concave_ReturnsFalse()
		{
			Vector3 pointOne = new Vector3(0.0f, 1.0f, 1.0f);
			Vector3 pointTwo = new Vector3(0.0f, 0.0f, 3.0f);
			Vector3 pointThree = new Vector3(0.25f, 0.25f, 4.0f);
			Vector3 pointFour = new Vector3(1.0f, 1.0f, 2.0f);

			bool expected = false;
			bool result = GeometryChecks.IsConvexQuad(pointOne, pointTwo, pointThree,
				pointFour);

			Assert.AreEqual(expected, result);
		}
	}
}