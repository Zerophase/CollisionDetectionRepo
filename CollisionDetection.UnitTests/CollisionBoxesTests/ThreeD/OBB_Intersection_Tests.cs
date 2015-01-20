using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.IntersectionTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	// TODO write seperate Rotation tests for testing b's rotation against a
	[TestFixture]
	public class OBB_Intersection_Tests
	{
		private static Vector3[] orientationMatrix = new[]
		{
			new Vector3(1.0f, 0.0f, 0.0f),
			new Vector3(0.0f, 1.0f, 0.0f),
			new Vector3(0.0f, 0.0f, 1.0f),
		};

		private OBBIntersection obbIntersection = new OBBIntersection();

		private OBB3D a = new OBB3D(new Vector3(0.0f, 0.0f, 0.0f), orientationMatrix, new Vector3(1.0f, 1.0f, 1.0f));

		[Test]
		public void Intersect_OBBaxAxis_ReturnFalse()
		{
			
			OBB3D b = new OBB3D(new Vector3(3.0f, 0.0f, 0.0f), orientationMatrix, new Vector3(1.0f, 1.0f, 1.0f));

			bool actual = obbIntersection.TestOBBOBB(a, b);

			Assert.IsFalse(actual);
		}

		[Test]
		public void Intersect_OBBayAxis_ReturnFalse()
		{
			OBB3D b = new OBB3D(new Vector3(0.0f, 3.0f, 0.0f), orientationMatrix, new Vector3(1.0f, 1.0f, 1.0f));

			bool actual = obbIntersection.TestOBBOBB(a, b);

			Assert.IsFalse(actual);
		}

		[Test]
		public void Intersect_OBBazAxis_ReturnsFalse()
		{
			OBB3D b = new OBB3D(new Vector3(0.0f, 0.0f, 3.0f), orientationMatrix, new Vector3(1.0f, 1.0f, 1.0f));

			bool actual = obbIntersection.TestOBBOBB(a, b);

			Assert.IsFalse(actual);
		}

		[Test]
		public void Intersect_OBBsOverlap_ReturnsTrue()
		{
			OBB3D b = new OBB3D(new Vector3(0.5f, 0.5f, 0.5f), orientationMatrix, new Vector3(1.0f, 1.0f, 1.0f));

			bool actual = obbIntersection.TestOBBOBB(a, b);

			Assert.IsTrue(actual);
		}
	}
}