using Assets.Scripts.CollisionBoxes.ThreeD;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	[TestFixture]
	public class AABB3D_Tests
	{
		[Test]
		public void Equals_EqualsOperatesCorrectly_ReturnsTrue()
		{
			AABB3D comparer = new AABB3D(
				new Vector3(1.0f, 1.0f, 1.0f), 1f, 1f, 1.0f);
			AABB3D comparee = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f),
				1.0f, 1.0f, 1.0f);

			bool expected = true;
			bool result = comparer.Equals(comparee);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Equals_EqualsOperatesCorrectly_ReturnsFalse()
		{
			AABB3D comparer = new AABB3D(
				new Vector3(1.0f, 1.0f, 1.0f), 1f, 1f, 1.0f);
			AABB3D comparee = new AABB3D(new Vector3(0.0f, 0.0f, 0.0f),
				1.0f, 1.0f, 1.0f);

			bool expected = false;
			bool result = comparer.Equals(comparee);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Constructor_BothInstancesTheSame_NewAABB2D()
		{
			AABB3D result = new AABB3D(
				new Vector3(1.0f, 1.0f, 1.0f), 1f, 1f, 1.0f);
			AABB3D expected = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f), 
				1.0f, 1.0f, 1.0f);

			Assert.AreEqual(expected, result);
		}
	}
}