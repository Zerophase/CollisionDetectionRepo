using UnityEngine;
using System.Collections;
using Assets.Scripts.IntersectionTests;
using Assets.Scripts.CollisionBoxes.ThreeD;

namespace Assets.Scripts.GameObjects
{
	public class GameController : MonoBehaviour 
	{
		private Player player;
		private Ground ground;

		private AABBIntersection intersection = new AABBIntersection();
		private OBBIntersection obbIntersection = new OBBIntersection();

		private Vector3 gravity = new Vector3(0.0f, -1.0f);

		// Update is called once per frame
		void Update () 
		{
			if (obbIntersection.TestOBBOBB(player.OrientedBoundingBox, ground.OrientedBoundingBox))
			{
				gravity = new Vector2(0.0f, 0.0f);
			}
			else
				gravity = new Vector2(0.0f, -1.0f);

			player.UpdatePosition(gravity);
		}

		public void AddCollisionObjects(ICollider collider)
		{
			if (collider is Player)
				player = (Player)collider;
			else if (collider is Ground)
				ground = (Ground)collider;
		}
	}

}
