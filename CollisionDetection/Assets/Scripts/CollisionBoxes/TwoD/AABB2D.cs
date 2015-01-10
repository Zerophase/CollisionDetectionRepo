using System;
using Assets.Scripts.EqualityComparison.Float;
using UnityEngine;

namespace Assets.Scripts.CollisionBoxes.TwoD
{
	public class AABB2D
	{
		private Vector2 center;
		public Vector2 Center { get { return center; } set { center = value; } }

		private readonly float[] halfWidth;

		public float HalfHeight
		{
			get { return halfWidth[1]; } set { halfWidth[1] = value; }
		}

		public float HalfWidth
		{
			get { return halfWidth[0]; } set { halfWidth[0] = value; }
		}


		public AABB2D(Vector2 center, float width, float height)
		{
			this.center = center;

			halfWidth = new float[2] {width/2, height/2};
		}

		public override bool Equals(object obj)
		{
			AABB2D test = (AABB2D) obj;
			if (test.Center == Center &&
			    FloatComparer.Compare(test.HalfHeight, HalfHeight) &&
			    FloatComparer.Compare(test.HalfWidth, HalfWidth))
				return true;

			return false;
		}

		public void DrawBoundingBox()
		{
			Debug.DrawLine(new Vector2(center.x - HalfWidth, center.y + HalfHeight), new Vector2(center.x + HalfWidth , center.y + HalfHeight),
				Color.magenta);
			Debug.DrawLine(new Vector2(center.x - HalfWidth, center.y - HalfHeight), new Vector2(center.x + HalfWidth, center.y - HalfHeight),
				Color.magenta);
			Debug.DrawLine(new Vector2(center.x + HalfWidth, center.y - HalfHeight), new Vector2(center.x + HalfWidth, center.y + HalfHeight),
				Color.magenta);
			Debug.DrawLine(new Vector2(Center.x - HalfWidth, center.y - HalfHeight), new Vector2(center.x - HalfWidth, center.y + HalfHeight),
				Color.magenta);
		}

		public void UpdateAABB(AABB2D a, float[][] m, Vector2 t, ref AABB2D b)
		{
			Vector2 modify = b.Center;
			modify.x = t.x;
			modify.y = t.y;
			b.HalfHeight = 0.0f;
			b.HalfWidth = 0.0f;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					if (i == 0)
					{
						if(j == 0)
						{
							modify.x += m[j][i] * a.Center.x;
							b.HalfWidth += Math.Abs(m[j][i]) * a.HalfWidth;
						}
						else if (j == 1)
						{
							modify.x += m[j][i]*a.center.y;
							b.HalfWidth += Math.Abs(m[j][i]) * a.HalfHeight;
						}
						
					}

					else if (i == 1)
					{
						if(j == 0)
						{
							modify.y += m[j][i] * a.Center.x;
							b.HalfHeight += Math.Abs(m[j][i]) * a.HalfWidth;
						}
						else
						{
							modify.y += m[j][i] * a.Center.y;
							b.HalfHeight += Math.Abs(m[j][i]) * a.HalfHeight;
						}
						
					}
				}
			}
			b.Center = modify;
		}
	}
}