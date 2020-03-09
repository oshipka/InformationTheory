using System;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Information_Theory.Labs
{
	public class Lab2
	{
		private static double[][] matrix_t1 =
		{
			new[] {0.2175, 0.0225, 0.01},
			new[] {0.016, 0.3480, 0.036},
			new[] {0.0315, 0.0140, 0.3045}
		};
		/*private static double[][] matrix_t1 =
		{
			new[] {0.1275, 0.009, 0.0135},
			new[] {0.0225, 0.2125, 0.015},
			new[] {0.036, 0.054, 0.51}
		};*/
		private static int v0_t1 = 1200;
		//private static int v0_t1 = 150;

		private static double p_t2 = 0.83;
		private static double q_t2 = 0.01;
		private static double pb_t2 = 0.16;
		private static int v0_t2 = 2400;

		private static double[][] matrix_t3 =
		{
			new[] {0.81, 0.19},
			new[] {0.05, 0.95}
		};
		private static int v0_t3 = 1000;
		
		public static void Task1()
		{
			Console.WriteLine("I(X;Y)");
			var I = Lab2.I(matrix_t1);
			Console.WriteLine("V");
			var v = Lab2.V(v0_t1, I );
			Console.WriteLine("P(X|Y)");
			var pXifY = Lab1.Pxify(matrix_t1, Lab1.Py(matrix_t1));
			Console.WriteLine("P(Y|X)");
			var pYifX = Lab1.PYifX(matrix_t1);
            Console.WriteLine("C");
			var c = Lab2.C(v0_t1, pYifX);
			string[] val = {"I(X;Y)", "V", "C"};
			AnsverTable(val, new []{I, v, c});
		}

		public static void Task2()
		{
			Console.WriteLine("C");
			C(p_t2, q_t2, pb_t2, v0_t2);
		}

		public static void Task3()
		{

		}
		private static double I(double[][] matrix_pxandy)
		{
			var px  = Lab1.Px(matrix_pxandy);
			Lab1.H(px, out var Hx);
			var py  = Lab1.Py(matrix_pxandy);
			Lab1.H(py, out var Hy);
			var hXandY = Lab1.HXandY(matrix_pxandy);
			var res = Hx + Hy - hXandY;
			Console.WriteLine("I(X;Y) = " + Hx + " + " + Hy + " - " + hXandY + " = " + res);
			return res;
		}

		private static double C(int v0, double[][] matrix_pyifx)
		{
			var max = Lab1.Hmax(3);
			var res = 0.0;
			var precalc = max + " + ";
			var calc = max + " + ";
			for (int i = 0; i < 3; i++)
			{
				var val = matrix_pyifx[1][i];
				var interm = val * Math.Log(val, 2);
				precalc += val + "log2 " + val + " + ";
				calc += interm + " + ";
				res += interm;
			}

			res += max;
			res *= v0;
			Console.WriteLine("C =\n "+v0 + "*("+precalc+")=\n "+v0 + "*("+calc+")="+Math.Round(res, 3));
			return res;
			
		}

		private static double C(double p, double q, double pb, int v0)
		{
			var res = 0.0;
			var res_str = "v0 * (plog2p + qlog2q + (1-pb)*(1-log2(1-pb)) = \n";
			res_str += v0 +
			           " * (" +
			           p +
			           "log2" +
			           p +
			           " + " +
			           q +
			           "log2" +
			           q +
			           " + (1-" +
			           pb +
			           ")*log2(1-" +
			           pb +
			           ") = \n" +
			           v0 +
			           " * (";
			var v = p * Math.Log(p, 2);
			res_str += v + " + ";
			res += v;
			v = q * Math.Log(q, 2);
			res_str += v + " + ";
			res += v;
			v = (1-pb) * (1-Math.Log((1-pb), 2));
			res_str += v + ") = \n";
			res += v;
			res *= v0;
			Console.WriteLine("C =\n "+res_str+")="+Math.Round(res, 3));
			return res;
		}
		internal static double Tau(int v0)
		{
			return 1 / (v0 * 1.0);
		}
		
		public static double V(double v0, double IXY)
		{
			var val = v0 * IXY;
			Console.WriteLine("V = " + v0 + " * " + IXY + " = " + val);
			return val;
		}
		
		public static void AnsverTable(string[] variables, double[] values)
		{
			if (variables.Length != values.Length)
			{
				throw new InvalidDataException("Arrays not the same length");
			}
			foreach (var _var in variables)
			{
				Console.Write("|"+_var+"\t");	
			}
			Console.Write("\n");	
			foreach (var _var in values)
			{
				Console.Write("|"+Math.Round(_var, 3)+"\t");	
			}


		}
	}
}