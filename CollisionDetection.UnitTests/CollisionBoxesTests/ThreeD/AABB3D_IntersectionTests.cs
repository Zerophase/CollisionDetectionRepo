using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.IntersectionTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	[TestFixture]
	public class AABB3D_IntersectionTests
	{
		[Test]
		public void Intersect_OnOrigin_ReturnTrue()
		{
			AABB3D a = new AABB3D(
				new Vector3(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 1.0f);
			AABB3D b = new AABB3D(
				new Vector3(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = true;
			bool actual = intersection.Intersect(a, b);

			Assert.That(expected == actual);
		}

		[Test]
		public void Intersect_OnOrigin_ReturnsFalse()
		{
			AABB3D a = new AABB3D(
				new Vector3(0.0f, 0.0f, 0.0f), 1.0f, 1.0f, 1.0f);
			AABB3D b = new AABB3D(
				new Vector3(1.5f, 1.5f, 1.5f), 1.0f, 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = false;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_AwayFromOrigin_ReturnsTrue()
		{
			AABB3D a = new AABB3D(
				new Vector3(20.0f, 20.0f, 20.0f), 1.0f, 1.0f, 1.0f);
			AABB3D b = new AABB3D(
				new Vector3(21.0f, 21.0f, 21.0f), 1.0f, 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = true;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_AwayFromOrigin_ReturnsFalse()
		{
			AABB3D a = new AABB3D(
				new Vector3(20.0f, 20.0f, 20.0f), 1.0f, 1.0f, 1.0f);
			AABB3D b = new AABB3D(
				new Vector3(22.0f, 22.0f, 22.0f), 1.0f, 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = false;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_DifferentLengthExtents_ReturnsTrue()
		{
			AABB3D a = new AABB3D(
				new Vector3(0.0f, 0.0f, 0.0f), 1.5f, 1.5f, 1.5f);
			AABB3D b = new AABB3D(
				new Vector3(1.0f, 1.0f, 1.0f), 1.0f, 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = true;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_DifferentLengthExtents_ReturnsFalse()
		{
			AABB3D a = new AABB3D(
				new Vector3(0.0f, 0.0f, 0.0f), 1.5f, 1.5f, 1.5f);
			AABB3D b = new AABB3D(
				new Vector3(2.0f, 2.0f, 2.0f), 1.0f, 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = false;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}
	}
}