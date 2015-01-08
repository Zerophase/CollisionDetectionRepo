using UnityEngine;

namespace Assets.Scripts.Policies.Interfaces
{
	public interface IVectorPolicy // Only use with Vectors no interface to restrict access
	{
		Vector2 Add(Vector2 a, Vector2 b );
		Vector2 Subtract(Vector2 a, Vector2 b);
		float Dot(Vector2 lhs, Vector2 rhs);
	}
}