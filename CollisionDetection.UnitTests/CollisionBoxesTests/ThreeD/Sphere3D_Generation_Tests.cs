using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.CollisionBoxes.ThreeD.BoundingGeneration;
using Assets.Scripts.EqualityComparison.Float;
using Assets.Scripts.Maths.PointSearch;
using Assets.Scripts.Maths.PointSearch.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	[TestFixture]
	public class Sphere3D_Generation_Tests
	{
		[Test]
		public void SphereFromDistantPoint_CreateSphereByFindingMostDistantPoints_CreateSphere()
		{
			SphereGeneration sphereGenerator = new SphereGeneration();
			int min, max;
			Vector3[] points =
			{
				Vector3.zero,
				new Vector3(2.0f, 2.0f, 2.0f), 
			};
			IGeometrySearch geometrySearch = Substitute.For<IGeometrySearch>();
			geometrySearch.When(x => x.MostSeperatedPointsOnAABB(out min, out max, points))
				.Do(x =>
				{
					x[0] = 0;
					x[1] = 1;
				});
			sphereGenerator.GeometrySearch = geometrySearch;

			Sphere3D expected = new Sphere3D(new Vector3(1.0f, 1.0f, 1.0f), 1.73205078f);
			Sphere3D result = new Sphere3D();
			sphereGenerator.Sphere3D = result;
			sphereGenerator.SphereFromDistantPoints(points);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void SphereOfSphereAndPoint_ModifySphereSize_ReturnNewSphere()
		{
			Vector3 point = Vector3.zero;
			Sphere3D originalSphere = new Sphere3D(
				new Vector3(-1.0f, -1.0f, -1.0f), 0.5f);
			SphereGeneration sphereGenerator = new SphereGeneration();

			Sphere3D expected = new Sphere3D(new Vector3(-0.7f, -0.7f, -0.7f), 0.957106769f);
			sphereGenerator.Sphere3D = originalSphere;
			sphereGenerator.SphereOfSphereAndPoint(point);

			Assert.AreEqual(expected, originalSphere);
		}

		[Test]
		public void RitterSphere_FullMethodCallForRitterGeneration_CreateAccurateSphere()
		{
			SphereGeneration sphereGenerator = new SphereGeneration();
			int min, max;
			Vector3[] points =
			{
				Vector3.zero,
				new Vector3(2.0f, 2.0f, 2.0f),
				new Vector3(1.0f, 1.5f, 0.0f) 
			};
			IGeometrySearch geometrySearch = Substitute.For<IGeometrySearch>();
			geometrySearch.When(x => x.MostSeperatedPointsOnAABB(out min, out max, points))
				.Do(x =>
				{
					x[0] = 0;
					x[1] = 1;
				});
			sphereGenerator.GeometrySearch = geometrySearch;

			Sphere3D expected = new Sphere3D(new Vector3(1.0f, 1.0f, 1.0f), 1.73205078f);
			Sphere3D result = new Sphere3D();
			sphereGenerator.Sphere3D = result;
			sphereGenerator.RitterSphere(points);

			Assert.AreEqual(expected, result);
		}

		[Test]
		public void EigenSphere_GenerateSPhereBasedOnEigenAlgorithim_SphereAssign()
		{
			SphereGeneration sphereGenerator = new SphereGeneration(); 
			IGeometrySearch geometrySearch = Substitute.For<IGeometrySearch>();
			Vector3[] points =
			{
				Vector3.zero,
				new Vector3(2.0f, 2.0f, 2.0f),
				new Vector3(1.0f, 1.5f, 0.0f) 
			};
			
			int imin, imax;
			geometrySearch.When(x => x.ExtremePointsAlongDirection(Arg.Any<Vector3>(),
				Arg.Any<Vector3[]>(), out imin , out imax))
				.Do(x =>
				{
					x[2] = 0;
					x[3] = 1;
				});

			Sphere3D expected = new Sphere3D(new Vector3(1.0f, 1.0f, 1.0f), 1.73205078f);
			Sphere3D actual = new Sphere3D();
			sphereGenerator.Sphere3D = actual;
			sphereGenerator.EigenSphere(points);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void RitterEigenSphere_GenerateSphereBasedonEigenAndRitterAlg_SphereAssign()
		{
			SphereGeneration sphereGenerator = new SphereGeneration();
			IGeometrySearch geometrySearch = Substitute.For<IGeometrySearch>();
			Vector3[] points =
			{
				Vector3.zero,
				new Vector3(2.0f, 2.0f, 2.0f),
				new Vector3(1.0f, 1.5f, 0.0f) 
			};

			int imin, imax;
			geometrySearch.When(x => x.ExtremePointsAlongDirection(Arg.Any<Vector3>(),
				Arg.Any<Vector3[]>(), out imin, out imax))
				.Do(x =>
				{
					x[2] = 0;
					x[3] = 1;
				});
			Sphere3D expected = new Sphere3D(new Vector3(1.0f, 1.0f, 1.0f), 1.73205078f);
			Sphere3D actual = new Sphere3D();
			sphereGenerator.Sphere3D = actual;
			sphereGenerator.RitterEigenSphere(points);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void RitterIterative_GenerateSphereThroughIteration_SizeSphereDown()
		{
			SphereGeneration sphereGenerator = new SphereGeneration();
			System.Random random = Substitute.For<System.Random>();
			random.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(0);
			sphereGenerator.Random = random;

			Vector3[] points =
			{
				Vector3.zero,
				new Vector3(2.0f, 2.0f, 2.0f),
				new Vector3(1.0f, 1.5f, 0.0f),
				new Vector3(-1.0f, -1.0f, -1.0f),
 				new Vector3(9.0f, 9.0f, 9.0f),
 				new Vector3(3.0f, 4.0f, 1.0f),
 				new Vector3(-2.0f, -0.0f, -1.0f),
 				new Vector3(-8.0f, -3.0f, -5.0f) 
			};

			Sphere3D expected = new Sphere3D(new Vector3(-0.4f, 2.4f, 1.3f), 9.361788f);
			Sphere3D actual = new Sphere3D();
			sphereGenerator.Sphere3D = actual;
			
			sphereGenerator.RitterIterative(points);

			Assert.AreEqual(expected, actual);
		}
	}
}