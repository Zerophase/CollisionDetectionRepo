using Assets.Scripts.CoordinateTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.GeometryTests
{
	[TestFixture]
	public class PlaneTests
	{
		[Test]
		public void ComputePlane_PositionPlaneOnPoints_ReturnPlane()
		{
			Vector3 pointOne = new Vector3(0f, 0f, 0f);
			Vector3 pointTwo = new Vector3(-1f, 1f, 0f);
			Vector3 pointThree = new Vector3(0f, 1f, 0f);

			Plane expected = new Plane(pointOne, pointTwo,
				pointThree);
			Plane result = GeometryChecks.ComputePlane(pointOne,
				pointTwo, pointThree);

			Assert.AreEqual(result, expected);
		}
	}
}