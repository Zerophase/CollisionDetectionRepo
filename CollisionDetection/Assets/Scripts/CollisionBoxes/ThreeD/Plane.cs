﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class Plane
	{
		private Vector3 normal;
		public Vector3 Normal { get { return normal; } }
		private float d;
		public float D { get { return d; }}
		public Plane(Vector3 a, Vector3 b, Vector3 c)
		{
			normal = Vector3.Cross(b - a, c - a);
			normal.Normalize();
			d = Vector3.Dot(normal, a);
		}

		public void DrawNormal(Vector3 start, Color color)
		{
			Vector3 normalPosition = start + normal;
			Debug.DrawLine(start, normalPosition, color);	
		}
	}
}
