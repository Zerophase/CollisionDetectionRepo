using UnityEngine;
using System.Collections;
using Assets.Scripts.CollisionBoxes.TwoD;
using NUnit.Framework;

namespace Assets.Scripts.GameObjects
{
	public interface ICollider
	{
		AABB2D BoundingBox { get; }

		void UpdatePosition(Vector2 position);
	}


	public class Ground : MonoBehaviour, ICollider
	{
		private AABB2D boundingBox;
		private AABB2D rotationBox;

		public AABB2D BoundingBox { get { return boundingBox; }}

		private Matrix4x4 matrix4X4;

		float[][] rotation =
			{
				new float[4],
				new float[4],
				new float[4],
				new float[4]
			};

		// Use this for initialization
		void Start()
		{
			var boxCollider = gameObject.GetComponent<BoxCollider>();
			boundingBox = new AABB2D(transform.position,
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y);
			rotationBox = new AABB2D(
				Vector2.zero,
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y);

			Debug.Log("Center of AABB is: " + boundingBox.Center +
				"Half Height is: " + boundingBox.HalfHeight +
				"Half Width is: " + boundingBox.HalfWidth);

			
			GameObject.Find("GameController").SendMessage("AddCollisionObjects", this);
		}

		public void Update()
		{
			matrix4X4 = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					rotation[i][j] = matrix4X4[i, j];
				}
			}

			boundingBox.UpdateAABB(rotationBox, rotation, transform.position, ref boundingBox);
			boundingBox.DrawBoundingBox();
		}
		
		public void UpdatePosition(Vector2 position)
		{
			throw new System.NotImplementedException();
		}
	}
}

