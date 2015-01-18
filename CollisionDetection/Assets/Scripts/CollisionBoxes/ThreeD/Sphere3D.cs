using System;
using System.Xml.Schema;
using Assets.Scripts.CollisionBoxes.ThreeD.BoundingGeneration;
using Assets.Scripts.EqualityComparison.Float;
using Assets.Scripts.Maths.PointSearch;
using Assets.Scripts.Maths.PointSearch.Interfaces;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class Sphere3D
	{
		private const float radiusEpsilon = 1e-4f;

		private Vector3 center;
		public Vector3 Center { get { return center; } set { center = value; } }
		private float radius;
		public float Radius { get { return radius; } set { radius = value; }}

		// TODO setup through Property injection
		public IGeometrySearch GeometrySearch = new GeometrySearch();
		public SphereGeneration SphereGenerator = new SphereGeneration();

		public Sphere3D(Vector3 center, float radius)
		{
			this.center = center;
			this.radius = radius;
		}

		public Sphere3D()
		{
			radius = -1f; // not valid
			center = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		}

		public Sphere3D(Vector3 O)
		{
			radius = 0 + radiusEpsilon;
			center = O;
		}

		public float VectorTimesVector(Vector3 c1, Vector3 c2)
		{
			return c1.x*c2.x + c1.y*c2.y + c1.z*c2.z;
		}
		public Sphere3D(Vector3 O, Vector3 A)
		{
			Vector3 a = A - O;
			Vector3 tempRad = 0.5f * a;

			radius = (float)Math.Sqrt(VectorTimesVector(tempRad, tempRad)) + radiusEpsilon;
			center = O + tempRad;
		}

		public Sphere3D(Vector3 O, Vector3 A, Vector3 B)
		{
			Vector3 a = A - O;
			Vector3 b = B - O;

			float denominator = 2.0f * (VectorTimesVector(Vector3.Cross(a, b), Vector3.Cross(a, b)));

			Vector3 tempRadius = (b.sqrMagnitude * (Vector3.Cross(Vector3.Cross(a, b), a)) +
								  (a.sqrMagnitude) * (Vector3.Cross(b, Vector3.Cross(a, b)))) / denominator;

			radius = tempRadius.magnitude + radiusEpsilon;
			center = O + tempRadius;
		}

		public Sphere3D(Vector3 O, Vector3 A, Vector3 B, Vector3 C)
		{
			Vector3 a = A - O;
			Vector3 b = B - O;
			Vector3 c = C - O;

			float denominator = 2.0f*determinate(a.x, a.y, a.z,
				b.x, b.y, b.z,
				c.x, c.y, c.z);

			Vector3 tempRad = ((c.sqrMagnitude) * Vector3.Cross(a, b) +
								(b.sqrMagnitude) * Vector3.Cross(c, a) +
								(a.sqrMagnitude) * Vector3.Cross(b, c)) / denominator;
			radius = tempRad.magnitude + radiusEpsilon;
			center = O + tempRad;
		}

		private float determinate(float m11, float m12, float m13,
			float m21, float m22, float m23,
			float m31, float m32, float m33)
		{
			return m11*(m22*m33 - m32*m23) -
			       m21*(m12*m33 - m32*m13) +
			       m31*(m12*m23 - m22*m13);
		}
		public override bool Equals(object obj)
		{
			var test = (Sphere3D) obj;
			var diff = Math.Abs(test.Center.sqrMagnitude - Center.sqrMagnitude);
			return diff < 0.1f &&
			       FloatComparer.Compare(test.Radius, Radius);
		}

		public void DrawCenterLines()
		{
			Debug.DrawLine(center, new Vector3(center.x + radius, center.y, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x - radius, center.y, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y + radius, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y - radius, center.z), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y, center.z + radius), Color.yellow);
			Debug.DrawLine(center, new Vector3(center.x, center.y, center.z - radius), Color.yellow);
			Debug.DrawLine(new Vector3(center.x + radius, center.y, center.z), new Vector3(center.x, center.y + radius, center.z));
		}
	}
}