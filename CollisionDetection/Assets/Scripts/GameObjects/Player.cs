using UnityEngine;
using System.Collections;
using Assets.Scripts.CollisionBoxes.TwoD;

namespace Assets.Scripts.GameObjects
{
	public class Player : MonoBehaviour, ICollider
	{
		private AABB2D boundingBox;

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
		}


		public void UpdatePosition(Vector2 position)
		{
			boundingBox.Center += position * Time.deltaTime;
			transform.Translate(position * Time.deltaTime);
		}
	}
}

