﻿using System;
using Assets.Scripts.EqualityComparison.Float;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.ThreeD
{
	public class AABB3D
	{
		private Vector3 center;
		public Vector3 Center { get { return center; } set { center = value; } }

		private readonly float[] halfWidth;

		public float HalfHeight
		{
			get { return halfWidth[1]; } set { halfWidth[1] = value; }
		}

		public float HalfWidth
		{
			get { return halfWidth[0]; } set { halfWidth[0] = value; }
		}

		public float HalfDepth
		{
			get { return halfWidth[2]; }
			set { halfWidth[2] = value; }
		}

		public float[] HalfWidths
		{
			get { return halfWidth; }
		}

		public Vector3 Velocity = Vector3.zero;

		private Plane top;
		public Plane Top { get { return top; } }
		private Plane bottom;
		public Plane Bottom { get { return bottom; } }
		private Plane left;
		public Plane Left { get { return left;} }
		private Plane right;
		public Plane Right { get { return right; } }
		private Plane front;
		public Plane Front { get { return front; } }
		private Plane back;
		public Plane Back { get { return back;} }

		public AABB3D(Vector3 center, float width, float height, float depth)
		{
			this.center = center;

			halfWidth = new float[3] {width/2.0f, height/2.0f, depth/ 2.0f};

			Vector3 topLeft = new Vector3(Center.x - HalfWidth, Center.y + HalfHeight, Center.z - HalfDepth);
			Vector3 topCenter = new Vector3(Center.x + HalfWidth, Center.y + HalfHeight, Center.z - HalfDepth);
			Vector3 topRight = new Vector3(Center.x + HalfWidth, Center.y + HalfHeight, Center.z + HalfDepth);
			top = new Plane(topCenter, topLeft, topRight);

			Vector3 bottomLeft = new Vector3(Center.x - HalfWidth, Center.y - HalfHeight, Center.z - HalfDepth);
			Vector3 bottomCenter = new Vector3(Center.x - HalfWidth, Center.y - HalfHeight, Center.z + HalfDepth);
			Vector3 bottomRight = new Vector3(Center.x + HalfWidth, Center.y - HalfHeight, Center.z + HalfDepth);
			bottom = new Plane(bottomCenter, bottomLeft, bottomRight);

			Vector3 leftTop = new Vector3(Center.x - HalfWidth, Center.y + HalfHeight, Center.z - HalfDepth);
			Vector3 leftCenter = new Vector3(Center.x - HalfWidth, Center.y - HalfHeight, Center.z + HalfDepth);
			Vector3 leftBottom = new Vector3(Center.x - HalfWidth, Center.y - HalfHeight, Center.z - HalfDepth);
			left = new Plane(leftCenter, leftTop, leftBottom);

			Vector3 rightTop = new Vector3(Center.x + HalfWidth, Center.y + HalfHeight, Center.z - HalfDepth);
			Vector3 rightCenter = new Vector3(Center.x + HalfWidth, Center.y - HalfHeight, Center.z - HalfDepth);
			Vector3 rightBottom = new Vector3(Center.x + HalfWidth, Center.y - HalfHeight, Center.z + HalfDepth);
			right = new Plane(rightCenter, rightTop, rightBottom);

			Vector3 frontTop = new Vector3(Center.x + HalfWidth, Center.y + HalfHeight, Center.z - HalfDepth);
			Vector3 frontCenter = new Vector3(Center.x - HalfWidth, Center.y + HalfHeight, Center.z - HalfDepth);
			Vector3 frontBottom = new Vector3(Center.x + HalfWidth, Center.y - HalfHeight, Center.z - HalfDepth);
			front = new Plane(frontCenter, frontTop, frontBottom);

			Vector3 backTop = new Vector3(Center.x + HalfWidth, Center.y + HalfHeight, Center.z + HalfDepth);
			Vector3 backCenter = new Vector3(Center.x - HalfWidth, Center.y - HalfHeight, Center.z + HalfDepth);
			Vector3 backBottom = new Vector3(Center.x - HalfWidth, Center.y + HalfHeight, Center.z + HalfDepth);
			back = new Plane(backCenter, backTop, backBottom);
		}
		public override bool Equals(object obj)
		{
			AABB3D test = (AABB3D) obj;
			if (test.Center == Center &&
			    FloatComparer.Compare(test.HalfHeight, HalfHeight) &&
			    FloatComparer.Compare(test.HalfWidth, HalfWidth) &&
				FloatComparer.Compare(test.HalfDepth, HalfDepth))
				return true;

			return false;
		}

		float clamp(float val, float min, float max)
		{
			if (val < min)
				return min;
			if (val > max)
				return max;
			return val;
		}
		public AABB3D AdjustForHitTime(AABB3D movingBox, 
			Vector3 velocity, float hitTime)
		{
			//hitTime = 1.0f - clamp(hitTime - float.Epsilon, 0f, 1f);
			Vector3 adjustedCenter = movingBox.Center + (velocity*hitTime);
			float width = (movingBox.HalfWidth*2) + (velocity.x*hitTime);
			float height = (movingBox.HalfHeight * 2) + (velocity.y * hitTime);
			float depth = (movingBox.HalfDepth*2) + (velocity.z*hitTime);
			AABB3D test = new AABB3D(adjustedCenter, width,
					height,
					depth);
			//Vector3 direction = velocity;
			//direction.Normalize();
			//test.Center += new Vector3(direction.x * test.HalfWidth, direction.y * test.HalfHeight,
			//	direction.z * test.HalfDepth);
			if (hitTime < 1.0f)
			{
				Debug.Log("---------------" + hitTime);
			}
			test.DrawBoundingBox(Color.blue);
			//if (Velocity != Vector3.zero)
			//{
			//	Debug.Log("Center hit point: " + test.center.ToString());
			//	Debug.Log("Actual Center: " + center.ToString());
			//}
				
			return test;
		}
		
		public void CalculateNormal(AABB3D collided)
		{
			
		}
		public void CLosestPointptAABB(Vector3 p, AABB3D b, ref Vector3 q)
		{
			for (int i = 0; i < 3; i++)
			{
				float v = p[i];
				if (v < b.HalfWidths[i]*2)
					v = b.Center[i] - b.HalfWidths[i];
				if (v > b.HalfWidths[i]*2)
					v = b.Center[i] + b.HalfWidths[i];
				q[i] = v;
			}
		}

		public void DrawBoundingBox(Color color)
		{
			// back face
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z + HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z + HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z + HalfDepth),
				color);
			Debug.DrawLine(new Vector3(Center.x - HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z + HalfDepth),
				color);

			//// front face
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z - HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z - HalfDepth), new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z - HalfDepth),
				color);

			//// side face
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z + HalfDepth), new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z - HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z - HalfDepth), new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				color);

			//// Other side face
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z - HalfDepth),
				color);
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z - HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				color);

			drawNormals();
		}

		public void DrawBoundingBox()
		{
			// back face
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z + HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z + HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z + HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(Center.x - HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z + HalfDepth),
				Color.magenta);

			//// front face
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z - HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z - HalfDepth), new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z - HalfDepth),
				Color.magenta);

			//// side face
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z + HalfDepth), new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z - HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x - HalfWidth, center.y - HalfHeight, center.z - HalfDepth), new Vector3(center.x - HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				Color.magenta);

			//// Other side face
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z + HalfDepth), new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z - HalfDepth),
				Color.magenta);
			Debug.DrawLine(new Vector3(center.x + HalfWidth, center.y - HalfHeight, center.z - HalfDepth), new Vector3(center.x + HalfWidth, center.y + HalfHeight, center.z - HalfDepth),
				Color.magenta);

			drawNormals();
		}

		private void drawNormals()
		{
			Vector3 topPosition = new Vector3(Center.x, center.y + HalfHeight, Center.z);
			top.DrawNormal(topPosition);

			Vector3 bottomPosition = new Vector3(Center.x, Center.y - HalfHeight, Center.z);
			bottom.DrawNormal(bottomPosition);

			Vector3 leftPosition = new Vector3(Center.x - HalfWidth, Center.y, Center.z);
			left.DrawNormal(leftPosition);

			Vector3 rightPosition = new Vector3(Center.x + HalfWidth, Center.y, Center.z);
			right.DrawNormal(rightPosition);

			Vector3 fontPosition = new Vector3(Center.x, Center.y, Center.z - HalfDepth);
			front.DrawNormal(fontPosition);

			Vector3 backPosition = new Vector3(Center.x, Center.y, Center.z + HalfDepth);
			back.DrawNormal(backPosition);
		}

		public void UpdateAABB(AABB3D a, float[][] m, Vector3 t, ref AABB3D b)
		{
			Vector3 modify = b.Center;
			modify.x = t.x;
			modify.y = t.y;
			modify.z = t.z;
			b.HalfHeight = 0.0f;
			b.HalfWidth = 0.0f;
			b.HalfDepth = 0.0f;
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (i == 0)
					{
						if(j == 0)
						{
							modify.x += m[i][j] * a.Center.x;
							b.HalfWidth += Math.Abs(m[i][j]) * a.HalfWidth;
						}
						else if (j == 1)
						{
							modify.x += m[i][j]*a.center.y;
							b.HalfWidth += Math.Abs(m[i][j]) * a.HalfHeight;
						}
						else if (j == 2)
						{
							modify.x += m[i][j]*a.center.z;
							b.HalfWidth += Math.Abs(m[i][j])*a.HalfDepth;
						}
					}
					else if (i == 1)
					{
						if(j == 0)
						{
							modify.y += m[i][j] * a.Center.x;
							b.HalfHeight += Math.Abs(m[i][j]) * a.HalfWidth;
						}
						else if(j == 1)
						{
							modify.y += m[i][j] * a.Center.y;
							b.HalfHeight += Math.Abs(m[i][j]) * a.HalfHeight;
						}
						else if (j == 2)
						{
							modify.y += m[i][j]*a.Center.z;
							b.HalfHeight += Math.Abs(m[i][j])*a.HalfDepth;
						}
						
					}
					else if (i == 2)
					{
						if (j == 0)
						{
							modify.z += m[i][j]*a.Center.x;
							b.HalfDepth += Math.Abs(m[i][j])*a.HalfWidth;
						}
						else if (j == 1)
						{
							modify.z += m[i][j]*a.Center.y;
							b.HalfDepth += Math.Abs(m[i][j])*a.HalfHeight;
						}
						else if (j == 2)
						{
							modify.z += m[i][j]*a.Center.z;
							b.HalfDepth += Math.Abs(m[i][j])*a.HalfDepth;
						}
					}
				}
			}

			b.Center = modify;
		}
	}
}