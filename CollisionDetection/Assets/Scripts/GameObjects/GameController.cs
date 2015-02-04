using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.IntersectionTests;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.SweptTests;
using D = System.Diagnostics;

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
		private List<Vector3> previousCorrections = new List<Vector3>();
		private List<float> previousHitTime = new List<float>(); 
		private float previousTime = 0.0f;
		Vector3 previousVelocity = Vector3.zero;
		bool start = true;
		// Update is called once per frame
		void Update ()
		{
			float hitTime = 0f;
			bool dontFailWhenTrue = false;
			player.UpdatePosition(gravity);
			for (int i = 0; i < grounds.Count; i++)
			{
				if(start)
				{
					previousVelocity = player.BoundingBox.Velocity;
					start = false;
				}
					

					Vector3 velocity = player.BoundingBox.Velocity;
					float endTime = Time.time + 1f;
					
				//D.Debug.Assert(velocity == previousVelocity);
				if (sweep.TestMovingAABB(player.BoundingBox, velocity,
				Time.time, endTime, grounds[i].BoundingBox, ref hitTime))
				{
					dontFailWhenTrue = true;
						Debug.Log("HitTime: " + hitTime);
					if(dontFailWhenTrue)
					{
						float actualHitTime = 1.0f - hitTime;
						if(hitTime > .9f)
						{
							Debug.Log("HITTIME NEAR ONE");
							//hitTime = 1.0f;
						}
						Vector3 CollisionPoint = (grounds[i].BoundingBox.Center + 
						                          new Vector3(0.0f, grounds[i].BoundingBox.HalfHeight, 0.0f)) -
							(player.BoundingBox.Center - 
							 new Vector3(0.0f, player.BoundingBox.HalfHeight, 0.0f));
						Debug.Log("Point of Collision: " + CollisionPoint);
						Vector3 correctionDistance = -gravity * hitTime;
						previousCorrections.Add(correctionDistance);
						previousHitTime.Add(hitTime);
						Debug.Log("Fall Correction: " + correctionDistance);
						player.UpdatePosition(correctionDistance);
					}
				}
				else
				{
					Debug.Log("HitTime non-Collision: " + hitTime);
				}
			}

			previousVelocity = player.BoundingBox.Velocity * Time.deltaTime;
			previousTime = Time.time;

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
