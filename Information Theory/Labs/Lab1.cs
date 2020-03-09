using System;
using System.Collections.Generic;

namespace Information_Theory.Labs
{
	internal static class Lab1
	{
		private static readonly double[] ProbT1 = {0.35, 0.15, 0.05, 0.25, 0.20};

		private static readonly double[][] MatrT2 =
		{
			new[] {0.39, 0.005, 0.005},
			new[] {0.005, 0.19, 0.005},
			new[] {0.005, 0.005, 0.39}
		};

		/*public static double[][] matr_t3 =
		{
			new[] {0.95, 0.025, 0.025},
			new[] {0.025, 0.95, 0.025},
			new[] {0.025, 0.025, 0.95}
		};

		public static double[] prob_t3 = {0.50, 0.20, 0.30};*/
		private static readonly double[][] MatrT3 =
		{
			new[] {0.82, 0.09, 0.09},
			new[] {0.07, 0.9, 0.03},
			new[] {0.15, 0.1, 0.75}
		};

		private static readonly double[] ProbT3 = {0.75, 0.1, 0.15};

		public static void Task1()
		{
			H(ProbT1, out var h1);
			Ro(h1, Hmax(ProbT1.Length), out _);
		}

		public static void Task2()
		{
			//HXandY(matr_t2);
			//HXifY(PXifY(matr_t2), px(matr_t2));
			var pxT2 = Px(MatrT2);
			var pyT2 = Py(MatrT2);
			Console.WriteLine("H(X)");
			H(pxT2, out var hX);
			Console.WriteLine("H(Y)");
			H(pyT2, out var hY);

			Console.WriteLine("ro_x");
			Ro(hX, Hmax(3), out _);
			Console.WriteLine("ro_y");
			Ro(hY, Hmax(3), out _);

			Console.WriteLine("P(Y|X)");
			var pYifX = Lab1.PYifX(MatrT2);
			HYifX(pYifX, pxT2);
			var hxandy = HXandY(MatrT2);
			AvgInfo(hX, hY, hxandy);
		}

		public static void Task3()
		{
			var pXandY = PXandY(ProbT3, MatrT3);
			var pyT3 = Py(pXandY);
			Console.WriteLine("H(X)");
			H(ProbT3, out var hX);
			Console.WriteLine("H(Y)");
			H(pyT3, out var hY);

			Console.WriteLine("ro_x");
			Ro(hX, Hmax(3), out _);
			Console.WriteLine("ro_y");
			Ro(hY, Hmax(3), out _);
			HYifX(MatrT3, ProbT3);
			var hxandy = HXandY(pXandY);
			AvgInfo(hX, hY, hxandy);
		}

		internal static void H(IEnumerable<double> pi, out double h)
		{
			h = 0.0;
			var outputCalculated = "H(x) = -1 * (";
			var outputPreCalculated = "H(x) = -1 * (";
			foreach (var p in pi)
			{
				outputPreCalculated += p + "* Log2(" + p + ") + ";
				var s = p * Math.Log(p, 2);
				outputCalculated += s + " + ";
				h += s;
			}

			outputPreCalculated = outputPreCalculated.Remove(outputPreCalculated.Length - 1);
			outputPreCalculated = outputPreCalculated.Remove(outputPreCalculated.Length - 1);
			outputPreCalculated += ")";
			outputCalculated = outputCalculated.Remove(outputCalculated.Length - 1);
			outputCalculated = outputCalculated.Remove(outputCalculated.Length - 1);
			outputCalculated += ")";

			Console.WriteLine(outputPreCalculated);
			Console.WriteLine(outputCalculated);

			h *= -1.0;
			Console.WriteLine("H(x) = " + h);
		}

		internal static double Hmax(int k)
		{
			return Math.Log(k, 2);
		}

		private static void Ro(double h, double hmax, out double ro)
		{
			Console.WriteLine("ro = 1 - (" + h + "/" + hmax + ")");
			ro = 1 - h / hmax;
			Console.WriteLine("ro = " + ro);
		}

		private static double[][] PXandY(IReadOnlyList<double> px, IReadOnlyList<double[]> pYifX)
		{
			var result = new double[pYifX.Count][];
			var precalculated = "";
			var calculated = "";
			for (var i = 0; i < pYifX.Count; i++)
			{
				result[i] = new double[pYifX[i].Length];
				for (var j = 0; j < result[i].Length; j++)
				{
					var value = pYifX[i][j] * px[i];
					precalculated += pYifX[i][j] + " * " + px[i] + "\t";
					calculated += value + "\t";
					result[i][j] = value;
				}

				precalculated += "\n";
				calculated += "\n";
			}

			Console.WriteLine("P(X, Y) = \n" + precalculated + "\n=\n" + calculated);
			return result;
		}

		internal static double[] Py(IReadOnlyList<double[]> pXandY)
		{
			Console.WriteLine("Calculating p(y)");
			var res = new double[pXandY.Count];
			for (var j = 0; j < pXandY.Count; j++)
			{
				var resElt = 0.0;
				Console.Write("p(y" + j + ") = ");
				for (var i = 0; i < pXandY[0].Length; i++)
				{
					Console.Write(pXandY[i][j] + " + ");
					resElt += pXandY[i][j];
				}

				Console.Write(" = " + resElt + "\n");
				res[j] = resElt;
			}

			return res;
		}

		internal static double[] Px(IReadOnlyList<double[]> pXandY)
		{
			var res = new double[pXandY.Count];
			Console.WriteLine("Calculating p(x)");
			for (var i = 0; i < pXandY.Count; i++)
			{
				var resElt = 0.0;
				Console.Write("p(x" + i + ") = ");
				for (var j = 0; j < pXandY[0].Length; j++)
				{
					Console.Write(pXandY[i][j] + " + ");
					resElt += pXandY[i][j];
				}

				Console.Write(" = " + resElt + "\n");
				res[i] = resElt;
			}

			return res;
		}

		private static double Pxify(double pxandy, double py)
		{
			var res = pxandy / py;
			Console.WriteLine(pxandy + " / " + py + " = " + res);
			return res;
		}

		internal static double[][] Pxify(double[][] pxandy, double[] py)
		{
			var len = pxandy.Length;
			var res = new double[len][];
			for (int i = 0; i <len; i++)
			{
				res[i]=new double[len];
				for (int j = 0; j < len; j++)
				{
					res[i][j] = Pxify(pxandy[i][j], py[i]);
				}
			}
			Console.WriteLine("P(X|Y) = ");
			for (int i = 0; i < len; i++)
			{
				for (int j = 0; j < len; j++)
				{
					Console.Write("\t"+Math.Round(res[i][j], 2));
				}
				Console.Write("\n");
			}
			return res;
		}

		
		
		internal static double[][] PYifX(double[][] pXandY)
		{
			var result = new double[pXandY.Length][];
			var px = Lab1.Px(pXandY);
			var precalculated = "";
			var calculated = "";
			var len = pXandY.Length;
			for (var i = 0; i < len; i++)
			{
				result[i] = new double[len];
				for (var j = 0; j < len; j++)
				{
					var probability = Pxify(pXandY[i][j], px[i]);
					precalculated += pXandY[i][j] + " / " + px[i] + "\t";
					calculated += Math.Round(probability, 2) + "\t";
					result[i][j] = probability;
				}

				precalculated += "\n";
				calculated += "\n";
			}

			Console.WriteLine("P(Y|X) = ");
			for (int i = 0; i < len; i++)
			{
				for (int j = 0; j < len; j++)
				{
					Console.Write("\t"+Math.Round(result[i][j], 2));
				}
				Console.Write("\n");
			}
			
			Console.WriteLine("P(Y|X) = \n" + precalculated + "\n=\n" + calculated);
			return result;
		}

		internal static double HXandY(IEnumerable<double[]> pXandY)
		{
			var result = 0.0;
			var precalculated = "";
			var calculated = "";
			foreach (var row in pXandY)
			{
				foreach (var element in row)
				{
					var value = element * Math.Log(element, 2) * -1.0;
					calculated += value + " + ";
					precalculated += element + "Log2(" + element + ") + ";
					result += value;
				}
			}

			Console.WriteLine("H(X, Y) = " + precalculated + " = " + calculated + " = " + result);
			return result;
		}

		private static void HYifX(IReadOnlyList<double[]> pYifX, IReadOnlyList<double> pi)
		{
			var result = 0.0;
			var entr = "H(Y|X) = ";
			var entrPrecalc = "";
			var entrCalc = "";
			for (var i = 0; i < pYifX.Count; i++)
			{
				var hYifxi = 0.0;
				var partialEntr = "H(Y|x" + i + ") = ";
				var partialEntrPrecalc = "";
				var partialEntrCalc = "";
				for (var j = 0; j < pYifX[i].Length; j++)
				{
					var value = pYifX[i][j] * Math.Log(pYifX[i][j], 2);
					partialEntrPrecalc += pYifX[i][j] + "*Log2(" + pYifX[i][j] + ") + ";
					partialEntrCalc += value + " + ";
					hYifxi += value * -1.0;
				}

				partialEntr += partialEntrPrecalc + " = " + partialEntrCalc + " = " + hYifxi;
				Console.WriteLine(partialEntr);
				entrPrecalc += pi[i] + "*" + hYifxi + " + ";
				var val = pi[i] * hYifxi;
				entrCalc += val + " + ";
				result += val;
			}

			entr += entrPrecalc + " = " + entrCalc + " = " + result;
			Console.WriteLine(entr);
		}

		internal static double AvgInfo(double hx, double hy, double hxandy)
		{
			var result = hx + hy - hxandy;
			Console.WriteLine("I(X; Y) = " + hx + " + " + hy + " - " + hxandy + " = " + result);
			return result;
		}
	}
}