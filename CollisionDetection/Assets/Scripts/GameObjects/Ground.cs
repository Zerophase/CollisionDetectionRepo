using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using Assets.Scripts.CollisionBoxes.ThreeD;
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
				Vector2.zero,
				boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);

			Debug.Log("Center of AABB is: " + boundingBox.Center +
				"Half Height is: " + boundingBox.HalfHeight +
				"Half Width is: " + boundingBox.HalfWidth);

			
			GameObject.Find("GameController").SendMessage("AddCollisionObjects", this);
		}

		//public static float[][] Matrix3x3(Quaternion rotation)
		//{
		//	float[][] matrix3x3 = new[]
		//	{
		//		new float[3],
		//		new float[3],
		//		new float[3]
		//	};

		//	matrix3x3[0][0] = rotation.x*2f;
		//	matrix3x3[0][1] = rotation.y*2.0f;
		//	matrix3x3[0][2] = rotation.z*2.0f;
		//	matrix3x3[1][0] = rotation.x*matrix3x3[0][0];
		//	matrix3x3[1][1] = rotation.y*matrix3x3[0][1];
		//	matrix3x3[1][2] = rotation.z*matrix3x3[0][2];
		//	matrix3x3[2][0] = rotation.

		//	return matrix3x3;
		//}

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

