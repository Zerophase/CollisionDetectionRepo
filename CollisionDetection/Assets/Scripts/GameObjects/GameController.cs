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

		private AABB3D playerBoundingBox;
		private AABB3D groundBoundingBox;
		private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
		// Update is called once per frame
		void Update ()
		{
			float hitTime = 0f;
			//sweep.ResetRectangles();
			for (int k = 0; k < players.Count; k++)
			{
				players[k].UpdateVelocity(gravity);
				for (int i = 0; i < grounds.Count; i++)
				{
					playerBoundingBox = players[k].BoundingBox;
					groundBoundingBox = grounds[i].BoundingBox;
					if (sweep.TestMovingAABB(playerBoundingBox,
						playerBoundingBox.Velocity * Time.deltaTime, 0f, 1f,
						groundBoundingBox, ref hitTime))
					{
						float actualHittime = 1.0f - hitTime;
						if (playerBoundingBox.NormalCollision[0].x > 0.0f)
						{
							velocity.x = playerBoundingBox.Velocity.x;
							velocity.y = 0.0f;
							velocity.z = 0.0f;
							//velocity = new Vector3(playerBoundingBox.Velocity.x, 0.0f, 0.0f);
							//Debug.Log("Player x Velocity going right: " + playerBoundingBox.Velocity.x);
							players[k].UpdateVelocity(-velocity * actualHittime);
						}
						else if (playerBoundingBox.NormalCollision[0].x < 0.0f)
						{
							velocity.x = playerBoundingBox.Velocity.x;
							velocity.y = 0.0f;
							velocity.z = 0.0f;
							//velocity = new Vector3(playerBoundingBox.Velocity.x, 0.0f, 0.0f);
							//Debug.Log("Player x Velocity going left: " + playerBoundingBox.Velocity.x);
							players[k].UpdateVelocity(-velocity * actualHittime);
						}

						if (playerBoundingBox.NormalCollision[1].y < 0.0f)
						{
							velocity.x = 0.0f;
							velocity.y = playerBoundingBox.Velocity.y;
							velocity.z = 0.0f;
							//velocity = new Vector3(0.0f, playerBoundingBox.Velocity.y, 0.0f);
							players[k].UpdateVelocity(-velocity * actualHittime);
						}
							
					}
				}

				players[k].UpdatePosition();
				players[k].ResetVelocity();
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
