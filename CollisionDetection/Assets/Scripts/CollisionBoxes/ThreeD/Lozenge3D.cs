using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class Lozenge3D
	{
		Vector3 center;
		Vector3[] edgeAxis = new Vector3[2];
		float radius;

		public Lozenge3D(Vector3 center, Vector3[] edgeAxis, float radius)
		{
			this.center = center;
			if (edgeAxis.Length != 2)
				throw new ArgumentException("Lozenge3D member variable edgeAxis " +
											"requires a Vector3[2]");
			this.edgeAxis = edgeAxis;
		}
	}
}
