using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.Maths;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Assets.Scripts.GameObjects
{
	public interface ICollider
	{
		AABB3D BoundingBox { get; }

		void UpdatePosition(Vector2 position);
	}


	public class Ground : MonoBehaviour, ICollider
	{
		private AABB3D boundingBox;
		private AABB3D rotationBox;

		public AABB3D BoundingBox { get { return boundingBox; }}

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
			boundingBox = new AABB3D(transform.position,
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);
			rotationBox = new AABB3D(
				Vector3.zero,
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);

			GameObject.Find("GameController").SendMessage("AddCollisionObjects", this);
		}

		public void Update()
		{
			//matrix4X4 = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

			//for (int i = 0; i < 4; i++)
			//{
			//	for (int j = 0; j < 4; j++)
			//	{
			//		rotation[i][j] = matrix4X4[i, j];
			//	}
			//}

			boundingBox.UpdateAABB(rotationBox, transform.rotation.QuaternionTo3x3(), transform.position, ref boundingBox);
			boundingBox.DrawBoundingBox();
		}
		
		public void UpdatePosition(Vector2 position)
		{
			throw new System.NotImplementedException();
		}
	}
}

