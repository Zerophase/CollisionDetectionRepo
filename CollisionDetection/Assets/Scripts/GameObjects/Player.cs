using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.CollisionBoxes.ThreeD;

namespace Assets.Scripts.GameObjects
{
	public class Player : MonoBehaviour, ICollider
	{
		private AABB3D boundingBox;
		private Sphere3D sphere2D;

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
			Vector3[] squareCorners = new Vector3[]
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

			sphere2D = new Sphere3D();
			sphere2D.RitterSphere(squareCorners);
			// below are more accurate for larger amounts of points
			//sphere2D.EigenSphere(ref sphere2D, squareCorners);
			//sphere2D.RitterEigenSphere(ref sphere2D, squareCorners, 8);
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
			sphere2D.DrawCenterLines();
		}


		public void UpdatePosition(Vector2 position)
		{
			//boundingBox.Center += position * Time.deltaTime;
			//sphere2D.Center += position *Time.deltaTime;
			//transform.Translate(position * Time.deltaTime);
		}
	}
}

