using System;
using Assets.Scripts.Maths.PointSearch.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Maths.PointSearch
{
	public class GeometrySearch : IGeometrySearch
	{
		public void MostSeperatedPointsOnAABB(out int min, out int max, Vector3[] points)
		{
			int minx = 0, maxx = 0, miny = 0, maxy = 0, minz = 0, maxz = 0;
			for (int i = 0; i < points.Length; i++)
			{
				if (points[i].x < points[minx].x)
					minx = i;
				if (points[i].x > points[maxx].x)
					maxx = i;
				if (points[i].y < points[miny].y)
					miny = i;
				if (points[i].y > points[maxy].y)
					maxy = i;
				if (points[i].z < points[minz].z)
					minz = i;
				if (points[i].z > points[maxz].z)
					maxz = i;
			}

			float distanceSqrX = Vector3.Dot(points[maxx] - points[minx],
				points[maxx] - points[minx]);
			float distanceSqrY = Vector3.Dot(points[maxy] - points[miny],
				points[maxy] - points[miny]);
			float distanceSqrZ = Vector3.Dot(points[maxz] - points[minz],
				points[maxz] - points[minz]);

			min = minx;
			max = maxx;

			if (distanceSqrY > distanceSqrX && distanceSqrY > distanceSqrZ)
			{
				max = maxy;
				min = miny;
			}
			if (distanceSqrZ > distanceSqrX && distanceSqrZ > distanceSqrY)
			{
				max = maxz;
				min = minz;
			}
		}

		// TODO Add to Variance Search class?
		public float Variance(float[] x)
		{
			float u = 0.0f;
			for (int i = 0; i < x.Length; i++)
				u += x[i];
			u /= x.Length;
			float sqr = 0.0f;
			for (int i = 0; i < x.Length; i++)
				sqr += (x[i] - u) * (x[i] - u);
			return sqr / x.Length;
		}

		public void CovarianceMatrix(ref float[][] covariance, Vector3[] points)
		{
			float oon = 1.0f / (float)points.Length;
			Vector3 c = Vector3.zero;
			float e00, e11, e22, e01, e02, e12;

			for (int i = 0; i < points.Length; i++)
			{
				c += points[i];
			}
			c *= oon;

			// compute covariance elements
			e00 = e11 = e22 = e01 = e02 = e12 = 0.0f;
			for (int i = 0; i < points.Length; i++)
			{
				Vector3 p = points[i] - c;
				e00 += p.x * p.x;
				e11 += p.y * p.y;
				e22 += p.z * p.z;
				e01 += p.x * p.y;
				e02 += p.x * p.z;
				e12 += p.y * p.z;
			}

			covariance[0][0] = e00 * oon;
			covariance[1][1] = e11 * oon;
			covariance[2][2] = e22 * oon;
			covariance[0][1] = covariance[1][0] = e01 * oon;
			covariance[0][2] = covariance[2][0] = e02 * oon;
			covariance[1][2] = covariance[2][1] = e12 * oon;
		}

		// (This code can be optimized for the three different cases of rotation)
		public void SymSchurSqr(ref float[][] a, int p, int q, out float cos, out float sine)
		{
			if (Math.Abs(a[p][q]) > 0.0001f)
			{
				float r = (a[q][q] - a[p][p]) / (2.0f * a[p][q]);
				float t;
				if (r >= 0.0f)
					t = 1.0f / (r + (float)Math.Sqrt(1.0f + r * r));
				else
					t = -1.0f / (-r + (float)Math.Sqrt(1.0f + r * r));
				cos = 1.0f / (float)Math.Sqrt(1.0f + t * t);
				sine = t * cos;
			}
			else
			{
				cos = 1.0f;
				sine = 0.0f;
			}
		}

		public void Jacobi(ref float[][] a, ref float[][] v)
		{
			int i, j, n, p, q;
			p = 0;
			q = 0;
			float prevoff = float.MaxValue;
			float c, s;
			float[][] J =
			{
				new float[3],
				new float[3],
				new float[3],
			};
			float[][] b =
			{
				new float[3],
				new float[3],
				new float[3],
			};
			float[][] t =
			{
				new float[3],
				new float[3],
				new float[3],
			};

			// initialize v to identity matrix
			for (i = 0; i < 3; i++)
			{
				v[i][0] = v[i][1] = v[i][2] = 0.0f;
				v[i][i] = 1.0f;
			}

			// Repeat for some maximum number of iterations
			int maxIterations = 50;
			for (n = 0; n < maxIterations; n++)
			{
				// Find largest off-diagonal absolute element a[p][q]
				p = 0;
				q = 1;
				for (i = 0; i < 3; i++)
				{
					for (j = 0; j < 3; j++)
					{
						if (i == j)
							continue;
						if (Math.Abs(a[i][j]) > Math.Abs(a[p][q]))
						{
							p = i;
							q = j;
						}
					}
				}

				// Compute the Jacobi rotation matrix J(p, q, theta)
				// (This code can be optimized for the three different cases of rotation)
				SymSchurSqr(ref a, p, q, out c, out s);
				for (i = 0; i < 3; i++)
				{
					J[i][0] = J[i][1] = J[i][2] = 0.0f;
					J[i][i] = 1.0f;
				}
				J[p][p] = c;
				J[p][q] = s;
				J[q][p] = -s;
				J[q][q] = c;

				// Cumulate rotations into what will contain the eigenvectors
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						v[l][k] = v[l][k] * J[l][k];
					}
				}

				// Make 'a' diagonal, until just eigenvalues remain on diagonal
				for (int k = 0; k < 3; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						a[k][l] = (J[l][k] * a[k][l]) * J[k][l];
					}
				}

				// Compute "norm" of off-diagonal elements
				float off = 0.0f;
				for (i = 0; i < 3; i++)
				{
					for (j = 0; j < 3; j++)
					{
						if (i == j)
							continue;
						off += a[i][j] * a[i][j];
					}
				}
				// off = sqrt(off); NotConvertedAttribute needed for norm comparison

				// stop when norm no longer decreasing
				if (n > 2 && off >= prevoff)
					return;
				prevoff = 0.0f;
			}
		}

		public void ExtremePointsAlongDirection(Vector3 direction, Vector3[] points, out int imin, out int imax)
		{
			float minproj = float.MaxValue;
			float maxproj = float.MinValue;
			imin = imax = 0;
			for (int i = 0; i < points.Length; i++)
			{
				// project vector from origin to point onto direction vector
				float proj = Vector3.Dot(points[i], direction);
				// keep track of least distant point along direciton vector
				if (proj < minproj)
				{
					minproj = proj;
					imin = i;
				}

				// keep track of most distant point along direction vector
				if (proj > maxproj)
				{
					maxproj = proj;
					imax = i;
				}
			}
		}
	}
}