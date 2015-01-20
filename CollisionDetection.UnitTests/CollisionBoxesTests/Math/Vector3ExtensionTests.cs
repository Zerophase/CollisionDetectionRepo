using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Maths;
using NUnit.Framework;
using UnityEngine;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.Math
{
	[TestFixture]
	public class Vector3ExtensionTests
	{
		[Test]
		public void Multiply_Multiply2Vector3s_ReturnfloatTotal()
		{
			Vector3 firstVector = new Vector3(1.0f, 1.0f, 1.0f);
			Vector3 secondVector = new Vector3(1.0f, 1.0f, 1.0f);

			float expected = 3.0f;
			float actual = firstVector.Multiply(secondVector);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Multiply_TwoVectorsofDifferentSizes_ReturnsFloatTotal()
		{
			Vector3 firstVector = new Vector3(2.0f, 2.0f, 2.0f);
			Vector3 secondVector = new Vector3(3.0f, 3.0f, 3.0f);

			float expected = 18.0f;
			float actual = firstVector.Multiply(secondVector);

			Assert.AreEqual(expected, actual);
		}
	}
}
