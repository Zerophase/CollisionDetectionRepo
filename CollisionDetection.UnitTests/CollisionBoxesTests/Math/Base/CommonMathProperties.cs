using Assets.Scripts.EqualityComparison.Float;

namespace CollisionDetection.UnitTests.CollisionBoxesTests.Math.Base
{
	public abstract class CommonMathProperties
	{
		protected FloatComparerTolerance floatComparerTolerance =
		   new FloatComparerTolerance(0.01f);
	}
}