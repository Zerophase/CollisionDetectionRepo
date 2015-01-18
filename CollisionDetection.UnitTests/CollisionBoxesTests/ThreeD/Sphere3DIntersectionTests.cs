using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.IntersectionTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	[TestFixture]
	public class Sphere3DIntersectionTests
	{
		[Test]
		public void TestSphereSphere_Intersection_ReturnsTrue()
		{
			SphereIntersection sphereIntersection = new SphereIntersection();
			Sphere3D a = new Sphere3D(new Vector3(0.0f, 0.0f, 0.0f), 1.0f );
			Sphere3D b = new Sphere3D(new Vector3(1.0f, 1.0f, 1.0f), 1.0f);

			bool expected = true;
			bool actual = sphereIntersection.TestSphereSphere(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void TestSphereSphere_NoIntersection_ReturnsFalse()
		{
			SphereIntersection sphereIntersection = new SphereIntersection();
			Sphere3D a = new Sphere3D(new Vector3(0.0f, 0.0f, 0.0f), 1.0f);
			Sphere3D b = new Sphere3D(new Vector3(2.0f, 2.0f, 2.0f), 1.0f);

			bool expected = false;
			bool actual = sphereIntersection.TestSphereSphere(a, b);

			Assert.AreEqual(expected, actual);
		}
	}
}