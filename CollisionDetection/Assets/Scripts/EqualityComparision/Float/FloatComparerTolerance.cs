using System.Collections.Generic;

namespace Assets.Scripts.EqualityComparison.Float
{
	public class FloatComparerTolerance : IEqualityComparer<float>
	{
		private float epsilon;
		public FloatComparerTolerance(float epsilon = float.Epsilon)
		{
			this.epsilon = epsilon;
		}

		public bool Equals(float x, float y)
		{
			return FloatComparer.Compare(x, y, epsilon);
		}

		public int GetHashCode(float obj)
		{
			throw new System.NotImplementedException();
		}
	}
}