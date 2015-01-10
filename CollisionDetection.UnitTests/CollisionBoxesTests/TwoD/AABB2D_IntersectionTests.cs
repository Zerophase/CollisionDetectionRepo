using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.CollisionBoxes.TwoD;
using Assets.Scripts.IntersectionTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.TwoD
{
	[TestFixture]
	public class AABB2D_IntersectionTests
	{
		[Test]
		public void Intersect_OnOrigin_ReturnTrue()
		{
			AABB2D a = new AABB2D(
				new Vector2(0.0f, 0.0f), 1.0f, 1.0f );
			AABB2D b = new AABB2D(
				new Vector2(1.0f, 1.0f), 1.0f, 1.0f );
			AABBIntersection intersection = new AABBIntersection();

			bool expected = true;
			bool actual = intersection.Intersect(a, b);

			Assert.That(expected == actual);
		}

		[Test]
		public void Intersect_OnOrigin_ReturnsFalse()
		{
			AABB2D a = new AABB2D(
				new Vector2(0.0f, 0.0f), 1.0f, 1.0f);
			AABB2D b = new AABB2D(
				new Vector2(1.5f, 1.5f), 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = false;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_AwayFromOrigin_ReturnsTrue()
		{
			AABB2D a = new AABB2D(
				new Vector2(20.0f, 20.0f), 1.0f, 1.0f);
			AABB2D b = new AABB2D(
				new Vector2(21.0f, 21.0f), 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = true;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_AwayFromOrigin_ReturnsFalse()
		{
			AABB2D a = new AABB2D(
				new Vector2(20.0f, 20.0f), 1.0f, 1.0f);
			AABB2D b = new AABB2D(
				new Vector2(22.0f, 22.0f), 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = false;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_DifferentLengthExtents_ReturnsTrue()
		{
			AABB2D a = new AABB2D(
				new Vector2(0.0f, 0.0f), 1.5f, 1.5f);
			AABB2D b = new AABB2D(
				new Vector2(1.0f, 1.0f), 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = true;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Intersect_DifferentLengthExtents_ReturnsFalse()
		{
			AABB2D a = new AABB2D(
				new Vector2(0.0f, 0.0f), 1.5f, 1.5f);
			AABB2D b = new AABB2D(
				new Vector2(2.0f, 2.0f), 1.0f, 1.0f);
			AABBIntersection intersection = new AABBIntersection();

			bool expected = false;
			bool actual = intersection.Intersect(a, b);

			Assert.AreEqual(expected, actual);
		}
	}
}