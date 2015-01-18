using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.Maths.PointSearch;
using Assets.Scripts.Maths.PointSearch.Interfaces;
using CollisionDetection.UnitTests.CollisionBoxesTests.Math.Base;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.Math
{
	[TestFixture]
	public class GeometrySearchTests : CommonMathProperties
	{
		[Test]
		public void MostSeperatedPointsOnAABB_FindFarthestPointsApart_OutMinAndMax()
		{
			Vector3[] points =
			{
				new Vector3(-2.0f, -2.0f, -2.0f), 
				new Vector3(0.0f, 0.0f, 0.0f),
				new Vector3(2.0f, 2.0f, 2.0f), 
			};
			IGeometrySearch geometrySearch = new GeometrySearch();

			int expectedMin = 0, expectedMax = 2;
			int min, max;
			geometrySearch.MostSeperatedPointsOnAABB(out min, out max, points);

			Assert.AreEqual(expectedMin, min);
			Assert.AreEqual(expectedMax, max);
		}

		[Test]
		public void MostSeperatedPointsOnAABB_OffOfDiagaonal_OutMinAndMax()
		{
			Vector3[] points =
			{
				new Vector3(-2.0f, -5.0f, -1.0f), 
				new Vector3(0.0f, 1.0f, 2.0f),
				new Vector3(4.0f, 3.0f, 1.0f), 
			};
			IGeometrySearch geometrySearch = new GeometrySearch();

			int expectedMin = 0, expectedMax = 2;
			int min, max;
			geometrySearch.MostSeperatedPointsOnAABB(out min, out max, points);

			Assert.AreEqual(expectedMin, min);
			Assert.AreEqual(expectedMax, max);
		}

		// Technically not a mean, some other technical average
		[Test]
		public void Variance_CalculatePositveVariance_ReturnMean()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			float[] variance =
			{
				0.0f,
				1.0f,
				2.0f,
				4.0f
			};
			Sphere3D sphere3D = new Sphere3D();

			float expected = 2.1875f;
			float result = geometrySearch.Variance(variance);

			Assert.That(expected, Is.EqualTo(result).Using(floatComparerTolerance));
		}

		[Test]
		public void Variance_CalculateNegativeVariance_ReturnMean()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			float[] variance =
			{
				-1.0f,
				-2.0f,
				-4.0f,
				-5.0f
			};

			float expected = 2.5f;
			float result = geometrySearch.Variance(variance);

			Assert.That(expected, Is.EqualTo(result).Using(floatComparerTolerance));
		}

		[Test]
		public void CovarianceMatrix_CalculatCoVarianceOfPoints_RefFloat()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			float[][] covariance =
			{
				new float[3],
				new float[3],
				new float[3],
			};
			Vector3[] boxCorners =
			{
				new Vector3(0.0f, 0.0f, 0.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 0.0f, 0.0f),
 				new Vector3(1.0f, 1.0f, 0.0f),
 				new Vector3(0.0f, 1.0f, 1.0f),
 				new Vector3(0.0f, 0.0f, 1.0f), 
				new Vector3(0.0f, 1.0f, 0.0f),
				new Vector3(1.0f, 0.0f, 1.0f), 
			};
			Sphere3D sphere3D = new Sphere3D();

			float[][] expected =
			{
				new[] {0.25f, 0.0f, 0.0f},
				new []{0.0f, 0.25f, 0.0f},
				new []{0.0f, 0.0f, 0.25f}
			};
			geometrySearch.CovarianceMatrix(ref covariance, boxCorners);
			
			Assert.AreEqual(expected, covariance);
		}

		[Test]
		public void SymSchurSqr_FindSineCosPairAtSpecificIndexesLessThanOrEqualZero_OutCosAndSine()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			float[][] covariance =
			{
				new[] {0.25f, 0.0f, 0.0f},
				new []{0.0f, 0.25f, 0.0f},
				new []{0.0f, 0.0f, 0.25f}
			};
			Sphere3D sphere3D = new Sphere3D();

			float expectedCos = 1.0f, expectedSine = 0.0f;
			float cos, sine;
			geometrySearch.SymSchurSqr(ref covariance, 1, 2, out cos, out sine);

			Assert.AreEqual(expectedCos, cos);
			Assert.AreEqual(expectedSine, sine);
		}

		[Test]
		public void SymSchurSqr_FindSineCosPairAtSpecificIndexesGreaterThanZero_OutCosAndSine()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			float[][] covariance =
			{
				new[] {0.25f, 0.25f, 0.0f},
				new []{0.0f, 0.25f, 0.25f},
				new []{0.0f, 0.0f, 0.25f}
			};
			Sphere3D sphere3D = new Sphere3D();

			float expectedCos = 0.707106769f, expectedSine = 0.707106769f;
			float cos, sine;
			geometrySearch.SymSchurSqr(ref covariance, 1, 2, out cos, out sine);

			Assert.AreEqual(expectedCos, cos);
			Assert.AreEqual(expectedSine, sine);
		}

		[Test]
		public void Jacobi_EigenVectorsAndValues_RefArrayofVectorsAndValues()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			float[][] covariance =
			{
				new[] {0.25f, 0.25f, 0.0f},
				new []{0.0f, 0.25f, 0.25f},
				new []{0.0f, 0.0f, 0.25f}
			};
			float[][] actual =
			{
				new float[3],
 				new float[3],
 				new float[3] 
			};

			float[][] expected =
			{
				new[] {0.24999997f, 0.0f, 0.0f},
				new[] {0.0f, 0.24999997f, 0.0f},
				new[] {0.0f, 0.0f, 1.0f}
			};
			geometrySearch.Jacobi(ref covariance, ref actual);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ExtremePointsAlongDirection_FindIndexes_OutiMinMax()
		{
			IGeometrySearch geometrySearch = new GeometrySearch();
			Vector3 direction = new Vector3(1.0f, 0.0f, 0.0f);
			Vector3[] boxCorners =
			{
				new Vector3(0.0f, 0.0f, 0.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 0.0f, 0.0f),
 				new Vector3(1.0f, 1.0f, 0.0f),
 				new Vector3(0.0f, 1.0f, 1.0f),
 				new Vector3(0.0f, 0.0f, 1.0f), 
				new Vector3(0.0f, 1.0f, 0.0f),
				new Vector3(1.0f, 0.0f, 1.0f), 
			};

			int expectedImin = 0, expectedImax = 1;
			int imin, imax;
			geometrySearch.ExtremePointsAlongDirection(direction, boxCorners, out imin, out imax);

			Assert.AreEqual(expectedImin, imin);
			Assert.AreEqual(expectedImax, imax);
		}
	}
}