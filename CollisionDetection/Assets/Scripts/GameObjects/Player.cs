﻿using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.CollisionBoxes.ThreeD.BoundingGeneration;

namespace Assets.Scripts.GameObjects
{
	public class Player : MonoBehaviour, ICollider
	{
		private AABB3D boundingBox;
		private Sphere3D sphere3D;
		private SphereGeneration sphereGenerator = new SphereGeneration();

		public AABB3D BoundingBox
		{
			get { return boundingBox; }
			set { boundingBox = value; }
		}
		// Use this for initialization
		void Start()
		{
			var boxCollider = gameObject.GetComponent<BoxCollider>();
			boundingBox = new AABB3D(transform.position,
				(boxCollider.size.x)* transform.lossyScale.x,
				(boxCollider.size.y) * transform.lossyScale.y,
				boxCollider.size.z * transform.lossyScale.z);

			Debug.Log("Center of AABB is: " + boundingBox.Center +
				"Half Height is: " + boundingBox.HalfHeight +
				"Half Width is: " + boundingBox.HalfWidth);
	
			GameObject.Find("GameController").SendMessage("AddCollisionObjects", this);
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

			// .8661254
			sphere3D = new Sphere3D();
			sphereGenerator.Sphere3D = sphere3D;
			Vector3[] sos = new Vector3[4];
			// even tighter sphere
			sphere3D = sphereGenerator.WelzlSphere(boxCorners, 8, sos);
			// very tight boxes
			//sphereGenerator.RitterIterative(boxCorners);
			//sphereGenerator.RitterSphere(boxCorners);
			// below are more accurate for larger amounts of points
			//sphereGenerator.EigenSphere(boxCorners);
			//sphereGenerator.RitterEigenSphere(boxCorners);
			Debug.Log("Center : " + sphere3D.Center + "Radius: " + sphere3D.Radius);
		}

		Vector2 moveRight = new Vector2(1.0f, 0.0f);
		// Update is called once per frame
		void Update()
		{
			if(Input.GetKey(KeyCode.RightArrow))
				UpdatePosition(moveRight * 5f);
			else if(Input.GetKey(KeyCode.LeftArrow))
				UpdatePosition(-moveRight * 5f);
			boundingBox.DrawBoundingBox();
			sphere3D.DrawCenterLines();
		}


		public void UpdatePosition(Vector2 position)
		{
			//boundingBox.Center += position * Time.deltaTime;
			//sphere2D.Center += position *Time.deltaTime;
			//transform.Translate(position * Time.deltaTime);
		}
	}
}

