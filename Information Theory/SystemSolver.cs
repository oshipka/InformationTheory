using System;

namespace Information_Theory
{
	public class SystemSolver
	{


		internal static Vector3 CrossProduct(Vector3 a, Vector3 b)
		{
			var r = new Vector3 {x = a.y * b.z - a.z * b.y, y = a.z * b.x - a.x * b.z, z = a.x * b.y - a.y * b.x};

			return r;
		}

		Vector3 Multiply(Matrix3x3 A, Vector3 b)
		{
			var r = new Vector3 {x = 0, y = 0, z = 0};
			for (var i = 0; i < 3; i++)
			for (var j = 0; j < 3; j++)
				r[i] += A[i, j] * b[j];

			return r;
		}

		internal class Matrix3x3
		{
			private double[,] A = new double [3, 3];

			public Matrix3x3()
			{
				for (var i = 0; i < 3; i++)
				for (var j = 0; j < 3; j++)
					A[i, j] = 0;
			}

			public Matrix3x3(double[][] input)
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						A[i, j] = input[i][j];
					}
				}
			}
			
			public double this[int i, int j]
			{
				get
				{
					if (!((i < 0 || i >= 3) && (j < 0 || j <= 3)))
						return A[i, j];
					else
						return 0;
				}

				set
				{
					if (!((i < 0 || i >= 3) && (j < 0 || j <= 3)))
						A[i, j] = value;
				}
			}

			public double Determinant()
			{
				double d = 0;
				d += A[0, 0] * (A[1, 1] * A[2, 2] - A[1, 2] * A[2, 1]);
				d -= A[0, 1] * (A[1, 0] * A[2, 2] - A[1, 2] * A[2, 0]);
				d += A[0, 2] * (A[1, 0] * A[2, 1] - A[1, 1] * A[2, 0]);

				return d;
			}

			public static int SolveEquations(out Vector3 x,
				Matrix3x3 A, Vector3 b)
			{
				x = new Vector3();
				const double eps = 1.0E-12;
				var d = A.Determinant();
				if (Math.Abs(d) < eps)
					return 1;

				var M0 = new Matrix3x3();
				var M1 = new Matrix3x3();
				var M2 = new Matrix3x3();

				ChangeColumn(out M0, A, b, 0);
				ChangeColumn(out M1, A, b, 1);
				ChangeColumn(out M2, A, b, 2);

				x[0] = M0.Determinant() / d;
				x[1] = M1.Determinant() / d;
				x[2] = M2.Determinant() / d;

				return 0;
			}

			public static int ChangeColumn(out Matrix3x3 R,
				Matrix3x3 A, Vector3 b, int j)
			{
				R = new Matrix3x3();
				R.Copy(A);

				if (j < 0 || j >= 3)
					return 1;

				R[0, j] = b[0];
				R[1, j] = b[1];
				R[2, j] = b[2];

				return 0;
			}

			public void Copy(Matrix3x3 X)
			{
				for (var i = 0; i < 3; i++)
				for (var j = 0; j < 3; j++)
					this[i, j] = X[i, j];
			}
		}

		internal class Vector3
		{
			private double[] v = new double[3];

			public Vector3()
			{
				v[0] = v[1] = v[2] = 0;
			}

			public Vector3(double[] input)
			{
				v = input;
			}
			
			public double x
			{
				get { return v[0]; }
				set { v[0] = value; }
			}

			public double y
			{
				get { return v[1]; }
				set { v[1] = value; }
			}

			public double z
			{
				get { return v[2]; }
				set { v[2] = value; }
			}

			public double this[int i]
			{
				get
				{
					if (i < 0 || i >= 3)
						return 0;
					else
						return v[i];
				}

				set
				{
					if (!(i < 0 || i >= 3))
						v[i] = value;
				}
			}
		}
	}
}