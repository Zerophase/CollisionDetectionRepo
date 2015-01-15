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

		[Test]
		public void UpdateAABB_MovesAABBDiagonalRightCorrectly_ChangeVector3Pos()
		{
			AABB3D testAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f),
				1.0f, 1.0f, 1.0f);
			AABB3D rotationBox = new AABB3D(Vector3.zero, 1.0f, 1.0f, 1.0f);
			float[][] rotationMatrix =
			{
				new[] {1.0f, 0.0f, 0.0f},
				new []{0.0f, 1.0f, 0.0f},
				new []{0.0f, 0.0f, 1.0f}
			};

			AABB3D expectedAABB = new AABB3D(new Vector3(2.0f, 2.0f, 2.0f), 1.0f, 1.0f, 1.0f);
			testAABB.UpdateAABB(rotationBox, rotationMatrix, new Vector3(2.0f, 2.0f, 2.0f), ref testAABB);

			Assert.AreEqual(expectedAABB, testAABB);
		}

		[Test]
		public void UpdateAABB_RotateAABBBack_ChangeBoundingBoxExtents()
		{
			AABB3D testAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f),
				1.0f, 0.5f, 1.0f);
			AABB3D rotationBox = new AABB3D(Vector3.zero, 1.0f, 0.5f, 1.0f);
			float[][] rotationMatrix =
			{
				new[] {0.0f, 0.0f, 1.0f},
				new []{0.0f, 1.0f, 0.0f},
				new []{-1.0f, 0.0f, 0.0f}
			};

			AABB3D expectedAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 0.5f, 1.0f);
			testAABB.UpdateAABB(rotationBox, rotationMatrix, Vector3.one, ref testAABB);
			
			Assert.AreEqual(expectedAABB, testAABB);
		}

		[Test]
		public void UpdateAABB_RotateAABBRight_ChangeBoundingBoxExtents()
		{
			AABB3D testAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f),
				1.0f, 1.0f, 0.5f);
			AABB3D rotationBox = new AABB3D(Vector3.zero, 1.0f, 1.0f, 0.5f);
			float[][] rotationMatrix =
			{
				new[] {0.0f, -1.0f, 0.0f},
				new []{1.0f, 0.0f, 0.0f},
				new []{0.0f, 0.0f, 1.0f}
			};

			AABB3D expectedAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 1.0f, 0.5f);
			testAABB.UpdateAABB(rotationBox, rotationMatrix, Vector3.one, ref testAABB);

			Assert.AreEqual(expectedAABB, testAABB);
		}

		[Test]
		public void UpdateAABB_FlipAABB_ChangingBoundingBoxExtents()
		{
			AABB3D testAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f),
				0.5f, 1.0f, 1.0f);
			AABB3D rotationBox = new AABB3D(Vector3.zero, 0.5f, 1.0f, 1.0f);
			float[][] rotationMatrix =
			{
				new[] {1.0f, 0.0f, 0.0f},
				new []{0.0f, 0.0f, -1.0f},
				new []{0.0f, 1.0f, 0.0f}
			};

			AABB3D expectedAABB = new AABB3D(new Vector3(1.0f, 1.0f, 1.0f), 0.5f, 1.0f, 1.0f);
			testAABB.UpdateAABB(rotationBox, rotationMatrix, Vector3.one, ref testAABB);

			Assert.AreEqual(expectedAABB, testAABB);
		}
	}
}