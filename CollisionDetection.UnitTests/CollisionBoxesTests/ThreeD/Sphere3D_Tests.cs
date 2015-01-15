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
	}
}