using UnityEngine;

namespace Assets.Scripts.Maths.PointSearch.Interfaces
{
	public interface IGeometrySearch
	{
		void MostSeperatedPointsOnAABB(out int min, out int max, Vector3[] points);
		float Variance(float[] x);
		void CovarianceMatrix(ref float[][] covariance, Vector3[] points);
		void Jacobi(ref float[][] a, ref float[][] v);
		void SymSchurSqr(ref float[][] a, int p, int q, out float cos, out float sine);
		void ExtremePointsAlongDirection(Vector3 direction, Vector3[] points, out int imin, out int imax);
	}
}