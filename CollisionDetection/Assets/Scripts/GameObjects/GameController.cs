using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.IntersectionTests;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.SweptTests;

namespace Assets.Scripts.GameObjects
{
	public class GameController : MonoBehaviour 
	{
		private Player player;
		private List<Ground> grounds = new List<Ground>();

		private AABBIntersection intersection = new AABBIntersection();
		private OBBIntersection obbIntersection = new OBBIntersection();
		private Sweep sweep = new Sweep();

		private const float constGravity = -10.0f;
		private Vector3 gravity = new Vector3(0.0f, -10.0f);

		private float previousTime = 0.0f;
		// Update is called once per frame
		void Update ()
		{
			float hitTime = 0f;
			bool dontFailWhenTrue = false;
			for (int i = 0; i < grounds.Count; i++)
			{
					if (sweep.TestMovingAABB(player.BoundingBox, player.BoundingBox.Velocity * Time.deltaTime,
					Time.time, Time.time + Time.deltaTime, grounds[i].BoundingBox, ref hitTime))
					{
						if (player.BoundingBox.Velocity.y != 0.0f)
						{
							player.transform.position -= (player.BoundingBox.Velocity) * hitTime;
						}
							
						gravity = new Vector2(0.0f, 0.0f);
						dontFailWhenTrue = true;
					}
					else if (!dontFailWhenTrue)
						gravity = new Vector2(0.0f, constGravity);
			}


			previousTime = Time.time;
			player.UpdatePosition(gravity);
		}

		public void AddCollisionObjects(ICollider collider)
		{
			if (collider is Player)
				player = (Player)collider;
			else if (collider is Ground)
				grounds.Add((Ground)collider);
		}
	}

}
