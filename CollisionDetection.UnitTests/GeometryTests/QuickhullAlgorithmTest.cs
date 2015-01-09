using Assets.Scripts.CoordinateTests;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.GeometryTests
{
	[TestFixture]
	public class QuickhullAlgorithmTest
	{
		[Test]
		public void PointFarthestFromEdge_PointFarthestToTheLeftOfAngledUpEdge_ReturnIndex()
		{
			Vector2 a = new Vector2(0.0f, 0.0f);
			Vector2 b = new Vector2(1.0f, 1.0f);
			Vector2[] p = new Vector2[]
			{
				new Vector2(2.0f, 2.0f),
				new Vector2(3.0f, 2.0f),
 				new Vector2(-1.0f, -1.0f),
 				new Vector2(-1.0f, 0.0f),
 				new Vector2(-2.0f, -2.0f),
				new Vector2(-1.0f, 2.0f), 
			};

			var expected = 5;
			int actual = GeometryChecks.PointFarthestFromEdge(a, b, p);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void PointFarthestFromEdge_PointFarthestToTheLeftOfAngledDownEdge_ReturnIndex()
		{
			Vector2 a = new Vector2(0.0f, 0.0f);
			Vector2 b = new Vector2(-1.0f, -1.0f);
			Vector2[] p = new Vector2[]
			{
				new Vector2(2.0f, 2.0f),
				new Vector2(3.0f, 2.0f),
 				new Vector2(-1.0f, -1.0f),
 				new Vector2(-1.0f, 0.0f),
 				new Vector2(-2.0f, -2.0f),
				new Vector2(-1.0f, 2.0f), 
			};

			var expected = 1;
			int actual = GeometryChecks.PointFarthestFromEdge(a, b, p);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void PointFarthestFromEdge_PointFarthestToTheLeftVerticalEdge_ReturnIndex()
		{
			Vector2 a = new Vector2(0.0f, 0.0f);
			Vector2 b = new Vector2(0.0f, 1.0f);
			Vector2[] p = new Vector2[]
			{
				new Vector2(2.0f, 2.0f),
				new Vector2(3.0f, 2.0f),
 				new Vector2(-1.0f, -1.0f),
 				new Vector2(-1.0f, 0.0f),
 				new Vector2(-2.0f, -2.0f),
				new Vector2(-1.0f, 2.0f), 
			};

			var expected = 4;
			int actual = GeometryChecks.PointFarthestFromEdge(a, b, p);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void PointFarthestFromEdge_PointFarthestToTheLeftHorizontalEdge_ReturnIndex()
		{
			Vector2 a = new Vector2(0.0f, 0.0f);
			Vector2 b = new Vector2(1.0f, 0.0f);
			Vector2[] p = new Vector2[]
			{
				new Vector2(2.0f, 2.0f),
				new Vector2(3.0f, 2.0f),
 				new Vector2(-1.0f, -1.0f),
 				new Vector2(-1.0f, 0.0f),
 				new Vector2(-2.0f, -2.0f),
				new Vector2(-1.0f, 2.0f)
			};

			var expected = 5;
			int actual = GeometryChecks.PointFarthestFromEdge(a, b, p);

			Assert.AreEqual(expected, actual);
		}
	}
}