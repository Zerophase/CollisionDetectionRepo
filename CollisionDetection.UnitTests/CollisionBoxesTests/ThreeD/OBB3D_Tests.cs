using System;
using Assets.Scripts.CollisionBoxes.ThreeD;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.ThreeD
{
	[TestFixture]
	public class OBB3D_Tests
	{
		[Test]
		public void Constructor_BothInstancesTheSame_NewOBB3D()
		{
			Vector3[] orientationMatrix = new Vector3[3]
			{
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
 				new Vector3(1.0f, 1.0f, 1.0f)
			};
			Vector3 WidthExtents = new Vector3(1.0f, 1.0f, 1.0f);

			OBB3D expected = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);
			OBB3D actual = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), 
				orientationMatrix, WidthExtents);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Constructor_OverThreeVectorsInOrientationMatrix_ThrowsException()
		{
			Vector3[] orientationMatrix =
			{
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
 				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f) 
			};
			Vector3 WidthExtents = new Vector3(1.0f, 1.0f, 1.0f);

			Action a = () => new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);

			Assert.Throws<ArgumentException>(new TestDelegate(a));
		}

		[Test]
		public void Constructor_UnderThreeVectorsInOrientationMatrix_ThrowsException()
		{
			Vector3[] orientationMatrix =
			{
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
			};
			Vector3 WidthExtents = new Vector3(1.0f, 1.0f, 1.0f);

			Action a = () => new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);

			Assert.Throws<ArgumentException>(new TestDelegate(a));
		}

		[Test]
		public void Equals_OperatesCorrectly_ReturnsTrue()
		{
			Vector3[] orientationMatrix = new Vector3[3]
			{
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
 				new Vector3(1.0f, 1.0f, 1.0f)
			};
			Vector3 WidthExtents = new Vector3(1.0f, 1.0f, 1.0f);
			OBB3D compareer = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);
			OBB3D comparee = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);

			bool expected = true;
			bool actual = compareer.Equals(comparee);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Equals_OperatesCorrectly_ReturnsFalse()
		{
			Vector3[] orientationMatrix = new Vector3[3]
			{
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
 				new Vector3(1.0f, 1.0f, 1.0f)
			};
			Vector3 WidthExtents = new Vector3(1.0f, 1.0f, 1.0f);
			OBB3D compareer = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);
			OBB3D comparee = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				new Vector3(2.0f, 2.0f, 2.0f));

			bool expected = false;
			bool actual = compareer.Equals(comparee);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void HalfWidths_HalfOfValuePassed_ReturnsVector3()
		{
			Vector3[] orientationMatrix = new Vector3[3]
			{
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
 				new Vector3(1.0f, 1.0f, 1.0f)
			};
			Vector3 WidthExtents = new Vector3(1.0f, 1.0f, 1.0f);

			Vector3 expected = WidthExtents/2.0f;
			OBB3D actual = new OBB3D(new Vector3(1.0f, 1.0f, 1.0f), orientationMatrix,
				WidthExtents);

			Assert.AreEqual(expected, actual.HalfWidths);
		}

		
	}
}