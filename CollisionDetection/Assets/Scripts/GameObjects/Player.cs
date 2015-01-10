using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.CollisionBoxes.TwoD;

namespace Assets.Scripts.GameObjects
{
	public class Player : MonoBehaviour, ICollider
	{
		private AABB2D boundingBox;
		private Sphere2D sphere2D;

		Vector2[] corners = new Vector2[4];

		public AABB2D BoundingBox
		{
			get { return boundingBox; }
			set { boundingBox = value; }
		}
		// Use this for initialization
		void Start()
		{
			var boxCollider = gameObject.GetComponent<BoxCollider>();
			boundingBox = new AABB2D(transform.position,
				(boxCollider.size.x)* transform.lossyScale.x,
				(boxCollider.size.y) * transform.lossyScale.y);

			Debug.Log("Center of AABB is: " + boundingBox.Center +
				"Half Height is: " + boundingBox.HalfHeight +
				"Half Width is: " + boundingBox.HalfWidth);

			GameObject.Find("GameController").SendMessage("AddCollisionObjects", this);
			Vector2[] squareCorners = new Vector2[]
			{
				new Vector2(boundingBox.Center.x + boundingBox.HalfWidth,
					boundingBox.Center.y + boundingBox.HalfHeight), // (+,+)
 				new Vector2(boundingBox.Center.x + boundingBox.HalfWidth,
					boundingBox.Center.y - boundingBox.HalfHeight), // (+, -)
 				new Vector2(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y + boundingBox.HalfHeight), // (-, +)
 				new Vector2(boundingBox.Center.x - boundingBox.HalfWidth,
					boundingBox.Center.y - boundingBox.HalfHeight) // (-,-)
			};

			corners = squareCorners;

			sphere2D = new Sphere2D();
			sphere2D.RitterSphere(squareCorners);
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

