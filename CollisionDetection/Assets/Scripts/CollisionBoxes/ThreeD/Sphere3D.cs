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