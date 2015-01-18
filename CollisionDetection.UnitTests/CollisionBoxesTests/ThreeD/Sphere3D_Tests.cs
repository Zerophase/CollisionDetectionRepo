using Assets.Scripts.CollisionBoxes.ThreeD;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	[TestFixture]
	public class Sphere3D_Tests
	{
		[Test]
		public void Constructor_SameSpheres_CreatesSphere()
		{
			Vector3 center = Vector3.zero;
			float radius = 1.0f;

			Sphere3D expected = new Sphere3D(center, radius);
			Sphere3D actual = new Sphere3D(center, radius);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Equals_OperatesCorreclty_ReturnsTrue()
		{
			Vector3 centerOne = Vector3.zero;
			float radiusOne = 1.0f;
			Vector3 centerTwo = Vector3.zero;
			float radiusTwo = 1.0f;
			Sphere3D comparer = new Sphere3D(centerOne, radiusOne);
			Sphere3D comparee = new Sphere3D(centerTwo, radiusTwo);

			bool expected = true;
			bool result = comparer.Equals(comparee);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Equals_OperatesCorrectly_ReturnsFalse()
		{
			Vector3 centerOne = Vector3.zero;
			float radiusOne = 1.0f;
			Vector3 centerTwo = new Vector3(0.0f, 0.0f, 0.0f);
			float radiusTwo = 2.0f;
			Sphere3D comparer = new Sphere3D(centerOne, radiusOne);
			Sphere3D comparee = new Sphere3D(centerTwo, radiusTwo);

			bool expected = false;
			bool result = comparer.Equals(comparee);

			Assert.AreEqual(expected, result);
		}
	}
}