using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Assets.Scripts.IntersectionTests;
using Assets.Scripts.CollisionBoxes.ThreeD;
using Assets.Scripts.EqualityComparison.Float;
using Assets.Scripts.SweptTests;
using D = System.Diagnostics;

namespace Assets.Scripts.GameObjects
{
	public class GameController : MonoBehaviour 
	{
		private List<Player> players = new List<Player>();
		private List<Ground> grounds = new List<Ground>();

		private AABBIntersection intersection = new AABBIntersection();
		private OBBIntersection obbIntersection = new OBBIntersection();
		private Sweep sweep = new Sweep();

		private const float constGravity = -10.0f;
		public static Vector3 gravity = new Vector3(0.0f, -10.0f);
		private List<Vector3> previousCorrections = new List<Vector3>();
		private List<float> previousHitTime = new List<float>(); 
		private float previousTime = 0.0f;
		Vector3 previousVelocity = Vector3.zero;
		bool start = true;
		// Update is called once per frame
		void Update ()
		{
			float hitTime = 0f;
			
			sweep.ResetRectangles();
			for (int k = 0; k < players.Count; k++)
			{
				players[k].UpdateVelocity(gravity);
				for (int i = 0; i < grounds.Count; i++)
				{
					AABB3D playerBoundingBox = players[k].BoundingBox;
					AABB3D groundBoundingBox = grounds[i].BoundingBox;
					float endTime = Time.time + Time.deltaTime;
					if (sweep.TestMovingAABB(playerBoundingBox,
						playerBoundingBox.Velocity * Time.deltaTime, 0f, 1f,
						groundBoundingBox, ref hitTime))
					{
						Vector3 collisionPoint = new Vector3
							(
								playerBoundingBox.Center.x - groundBoundingBox.Center.x,
								playerBoundingBox.Center.y - groundBoundingBox.Center.y,
								playerBoundingBox.Center.z - groundBoundingBox.Center.z
							);
						float ax = Math.Abs(collisionPoint.x);
						float ay = Math.Abs(collisionPoint.y);
						float az = Math.Abs(collisionPoint.z);

						float sx = playerBoundingBox.Center.x < groundBoundingBox.Center.x ? -1.0f : 1.0f;
						float sy = playerBoundingBox.Center.y < groundBoundingBox.Center.y ? -1.0f : 1.0f;
						float sz = playerBoundingBox.Center.z < groundBoundingBox.Center.z ? -1.0f : 1.0f;

						Vector3 collisionNormal = Vector3.zero;
						if(ax <= ay && ax <= az)
							collisionNormal = new Vector3(sx, 0.0f, 0.0f);
						else if(ay <= az)
							collisionNormal = new Vector3(0.0f, sy, 0.0f);
						else
						{
							collisionNormal = new Vector3(0.0f, 0.0f, sz);
						}

						float actualHittime = 1.0f - hitTime;
						if (collisionNormal.x > 0.0f)
						{
							Vector3 velocity = new Vector3(playerBoundingBox.Velocity.x, 0.0f, 0.0f);
							players[k].UpdatePosition(-velocity * actualHittime);
						}

						if (collisionNormal.y < 0.0f)
						{
							Vector3 velocity = new Vector3(0.0f, playerBoundingBox.Velocity.y, 0.0f);
							players[k].UpdatePosition(velocity * actualHittime);
						}
						if (collisionNormal.y > 0.0f)
						{
							players[k].UpdatePosition(-gravity * actualHittime);
						}
							
						Debug.Log("Collision with Sweep");
							grounds[i].BoundingBox.DrawBoundingBox(Color.green);
							
					}
				}

				players[k].UpdatePosition();
				players[k].ResetVelocity();
				previousVelocity = players[k].BoundingBox.Velocity * Time.deltaTime;
				previousTime = Time.time;
			}
			
		}

		public void AddCollisionObjects(ICollider collider)
		{
			if (collider is Player)
				players.Add((Player)collider);
			else if (collider is Ground)
				grounds.Add((Ground)collider);
		}
	}

}
