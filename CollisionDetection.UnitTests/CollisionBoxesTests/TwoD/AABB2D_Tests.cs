using Assets.Scripts.CollisionBoxes.TwoD;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.TwoD
{
	[TestFixture]
	public class AABB2D_Tests
	{
		[Test]
		public void Equals_EqualsOperatesCorrectly_ReturnsTrue()
		{
			AABB2D comparer = new AABB2D(
				new Vector2(1.0f, 1.0f), 1f, 1f);
			AABB2D comparee = new AABB2D(new Vector2(1.0f, 1.0f),
				1.0f, 1.0f);

			bool expected = true;
			bool result = comparer.Equals(comparee);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Equals_EqualsOperatesCorrectly_ReturnsFalse()
		{
			AABB2D comparer = new AABB2D(
				new Vector2(1.0f, 1.0f), 1f, 1f);
			AABB2D comparee = new AABB2D(new Vector2(0.0f, 0.0f),
				1.0f, 1.0f);

			bool expected = false;
			bool result = comparer.Equals(comparee);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void Constructor_BothInstancesTheSame_NewAABB2D()
		{
			AABB2D result = new AABB2D(
				new Vector2(1.0f, 1.0f), 1f, 1f);
			AABB2D expected = new AABB2D(new Vector2(1.0f, 1.0f), 
				1.0f, 1.0f);

			Assert.AreEqual(expected, result);
		}
	}
}