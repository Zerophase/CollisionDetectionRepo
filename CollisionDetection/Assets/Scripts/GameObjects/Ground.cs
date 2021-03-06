﻿using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.CollisionBoxes.ThreeD.BoundingGeneration;
using Assets.Scripts.Maths;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Assets.Scripts.GameObjects
{
	public interface ICollider
	{
		AABB3D BoundingBox { get; }
		Sphere3D Sphere3D { get; }
		Sphere3D[] Sphere3Ds { get; }
		void UpdatePosition();
	}


	public class Ground : MonoBehaviour, ICollider
	{
		private AABB3D boundingBox;
		private AABB3D rotationBox;

		public AABB3D BoundingBox { get { return boundingBox; }}

		private Sphere3D sphere3D;
		private Sphere3D[] sphere3DArray;
		public Sphere3D Sphere3D
		{
			get { return sphere3D; }
		}

		public Sphere3D[] Sphere3Ds { get {return sphere3DArray;} }

		private SphereGeneration sphereGenerator = new SphereGeneration();

		private Matrix4x4 matrix4X4;

		float[][] rotation =
			{
				new float[4],
				new float[4],
				new float[4],
				new float[4]
			};

		private OBB3D orientedBoundingBox;
		public OBB3D OrientedBoundingBox
		{
			get { return orientedBoundingBox; }
			set { orientedBoundingBox = value; }
		}
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

			float[][] rm = transform.localRotation.QuaternionTo3x3();
			Vector3[] rotationVector3 =
			{
				new Vector3(rm[0][0], rm[0][1], rm[0][2]),
				new Vector3(rm[1][0], rm[1][1], rm[1][2]),
				new Vector3(rm[2][0], rm[2][1], rm[2][2]), 
			};

			Vector3 widths = new Vector3(boxCollider.size.x * transform.lossyScale.x,
				boxCollider.size.y * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);
			Debug.Log("Ground Center: " + transform.position);
			Debug.Log("Ground Widths: " + widths);

			orientedBoundingBox = new OBB3D(transform.position, rotationVector3,
				widths);

			sphere3DArray = new Sphere3D[(int)(boundingBox.HalfWidth * 2) - 1];
			for (int i = 0; i < sphere3DArray.Length; i++)
			{
				sphere3DArray[i] = new Sphere3D();
			}

			Vector3[][] boxCornerCorners = new Vector3[6][];
			//for (int i = 0; i < sphere3DArray.Length; i++)
			//{
			//	float halfWidth = boundingBox.HalfWidth/boundingBox.HalfWidth;
			//	Vector3 center = new Vector3((boundingBox.Center.x - boundingBox.HalfWidth)
			//		+ (halfWidth * (i + 1)),
			//		boundingBox.Center.y, boundingBox.Center.z);
			//	boxCornerCorners[i] = new Vector3[]
			//	{
			//		new Vector3(center.x + halfWidth,
			//			center.y + boundingBox.HalfHeight,
			//			center.z + boundingBox.HalfDepth), // (+,+, +)
			//		new Vector3(center.x + halfWidth,
			//			center.y - boundingBox.HalfHeight,
			//			center.z + boundingBox.HalfDepth), // (+, -, +)
			//		new Vector3(center.x - halfWidth,
			//			center.y + boundingBox.HalfHeight,
			//			center.z + boundingBox.HalfDepth), // (-, +, +)
			//		new Vector3(center.x - halfWidth,
			//			center.y - boundingBox.HalfHeight,
			//			center.z + boundingBox.HalfDepth), // (-,-, +)
			//		new Vector3(center.x - halfWidth,
			//			center.y - boundingBox.HalfHeight,
			//			center.z - boundingBox.HalfDepth), // (-,-,-)
			//		new Vector3(center.x + halfWidth,
			//			center.y - boundingBox.HalfHeight,
			//			center.z - boundingBox.HalfDepth), // (+,-,-)
			//		new Vector3(center.x - halfWidth,
			//			center.y + boundingBox.HalfHeight,
			//			center.z + boundingBox.HalfDepth), // (-, +, +)
			//		new Vector3(center.x - halfWidth,
			//			center.y + boundingBox.HalfHeight,
			//			center.z - boundingBox.HalfDepth), // (-, +, -)
			//	};
			//}

			Vector3[] boxCorners = new Vector3[]
			{
				new Vector3(boundingBox.Center.x + boundingBox.HalfWidth,
					boundingBox.Center.y + boundingBox.HalfHeight, 
					boundingBox.Center.z + boundingBox.HalfDepth), // (+,+, +)
 				new Vector3(boundingBox.Center.x + boundingBox.HalfWidth,
					boundingBox.Center.y - boundingBox.HalfHeight, 
					boundingBox.Center.z + boundingBox.HalfDepth), // (+, -, +)
 				new Vector3(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y + boundingBox.HalfHeight,
					boundingBox.Center.z + boundingBox.HalfDepth), // (-, +, +)
 				new Vector3(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y - boundingBox.HalfHeight,
					boundingBox.Center.z + boundingBox.HalfDepth), // (-,-, +)
				new Vector3(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y - boundingBox.HalfHeight,
					boundingBox.Center.z - boundingBox.HalfDepth),	// (-,-,-)
				new Vector3(boundingBox.Center.x + boundingBox.HalfWidth,
					boundingBox.Center.y - boundingBox.HalfHeight,
					boundingBox.Center.z - boundingBox.HalfDepth), 	// (+,-,-)
				new Vector3(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y + boundingBox.HalfHeight,
					boundingBox.Center.z + boundingBox.HalfDepth), 	// (-, +, +)
				new Vector3(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y + boundingBox.HalfHeight,
					boundingBox.Center.z - boundingBox.HalfDepth), 	// (-, +, -)
			};

			//sphere3D = new Sphere3D();
			//sphereGenerator.Sphere3D = sphere3D;
			//Vector3[] sos = new Vector3[4];
			//sphere3D = sphereGenerator.WelzlSphere(boxCorners, 8, sos);

			//for (int i = 0; i < sphere3DArray.Length; i++)
			//{
			//	sphereGenerator.Sphere3D = sphere3DArray[i];
			//	sphere3DArray[i] = sphereGenerator.WelzlSphere(boxCornerCorners[i],
			//		8, sos);
			//}
			

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

			//float[][] rm = transform.rotation.QuaternionTo3x3();
			//Vector3[] rotationVector3 =
			//{
			//	new Vector3(rm[0][0], rm[0][1], rm[0][2]),
			//	new Vector3(rm[1][0], rm[1][1], rm[1][2]),
			//	new Vector3(rm[2][0], rm[2][1], rm[2][2]), 
			//};
			//orientedBoundingBox.UpdateRotation(rotationVector3);
			//orientedBoundingBox.DrawBoundingBox();

			//sphere3D.DrawCenterLines();
			boundingBox.UpdateAABB(rotationBox, transform.localRotation.QuaternionTo3x3(), transform.position, ref boundingBox);
			boundingBox.DrawBoundingBox();
			// point at collision 2.894078
		}
		
		public void UpdatePosition()
		{
			throw new System.NotImplementedException();
		}
	}
}

