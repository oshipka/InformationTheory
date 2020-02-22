using System;

namespace Information_Theory.Labs
{
	class Lab1
	{
		public static double[] prob_t1 = {0.35, 0.15, 0.05, 0.25, 0.20};

		public static double[][] matr_t2 =
		{
			new[] {0.39, 0.005, 0.005},
			new[] {0.005, 0.19, 0.005},
			new[] {0.005, 0.005, 0.39}
		};

		public static double[][] matr_t3 =
		{
			new[] {0.95, 0.025, 0.025},
			new[] {0.025, 0.95, 0.025},
			new[] {0.025, 0.025, 0.95}
		};

		public static double[] prob_t3 = {0.50, 0.20, 0.30};

		public static void Task1()
		{
			H(prob_t1, out var h1);
			Ro(h1, Hmax(prob_t1.Length), out var ro1);
		}

		public static void Task2()
		{
			//HXandY(matr_t2);
			//HXifY(PXifY(matr_t2), px(matr_t2));
			var px_t2 = px(matr_t2);
			var py_t2 = py(matr_t2);
			Console.WriteLine("H(X)");
			H(px_t2, out var hX);
			Console.WriteLine("H(Y)");
			H(py_t2, out var hY);

			Console.WriteLine("ro_x");
			Ro(hX, Hmax(3), out var ro_x);
			Console.WriteLine("ro_y");
			Ro(hY, Hmax(3), out var ro_y);

			Console.WriteLine("P(Y|X)");
			var PYifX = Lab1.PYifX(matr_t2);
			HYifX(PYifX, px_t2);
			var hxandy = HXandY(matr_t2);
			AvgInfo(hX, hY, hxandy);
		}

		public static void Task3()
		{
			var PXandY = pXandY(prob_t3, matr_t3);
			var py_t3 = py(PXandY);
			Console.WriteLine("H(X)");
			H(prob_t3, out var hX);
			Console.WriteLine("H(Y)");
			H(py_t3, out var hY);

			Console.WriteLine("ro_x");
			Ro(hX, Hmax(3), out var ro_x);
			Console.WriteLine("ro_y");
			Ro(hY, Hmax(3), out var ro_y);
			HYifX(matr_t3, prob_t3);
			var hxandy = HXandY(PXandY);
			AvgInfo(hX, hY, hxandy);
		}

		public static void H(double[] pi, out double h)
		{
			h = 0.0;
			var output_calculated = "H(x) = -1 * (";
			var output_pre_calculated = "H(x) = -1 * (";
			foreach (var p in pi)
			{
				output_pre_calculated += p + "* Log2(" + p + ") + ";
				var s = p * Math.Log(p, 2);
				output_calculated += s + " + ";
				h += s;
			}

			output_pre_calculated = output_pre_calculated.Remove(output_pre_calculated.Length - 1);
			output_pre_calculated = output_pre_calculated.Remove(output_pre_calculated.Length - 1);
			output_pre_calculated += ")";
			output_calculated = output_calculated.Remove(output_calculated.Length - 1);
			output_calculated = output_calculated.Remove(output_calculated.Length - 1);
			output_calculated += ")";

			Console.WriteLine(output_pre_calculated);
			Console.WriteLine(output_calculated);

			h *= -1.0;
			Console.WriteLine("H(x) = " + h);
		}

		public static double Hmax(int k)
		{
			return Math.Log(k, 2);
		}

		public static void Ro(double h, double hmax, out double ro)
		{
			Console.WriteLine("ro = 1 - (" + h + "/" + hmax + ")");
			ro = 1 - h / hmax;
			Console.WriteLine("ro = " + ro);
		}

		public static double[][] pXandY(double[] px, double[][] PYifX)
		{
			var result = new double[PYifX.Length][];
			var precalculated = "";
			var calculated = "";
			for (int i = 0; i < PYifX.Length; i++)
			{
				result[i] = new double[PYifX[i].Length];
				for (int j = 0; j < result[i].Length; j++)
				{
					var value = PYifX[i][j] * px[i];
					precalculated += PYifX[i][j] + " * " + px[i] + "\t";
					calculated += value + "\t";
					result[i][j] = value;
				}

				precalculated += "\n";
				calculated += "\n";
			}

			Console.WriteLine("P(X, Y) = \n" + precalculated + "\n=\n" + calculated);
			return result;
		}

		public static double[] py(double[][] PXandY)
		{
			Console.WriteLine("Calculating p(y)");
			var res = new double[PXandY.Length];
			for (int j = 0; j < PXandY.Length; j++)
			{
				var res_elt = 0.0;
				Console.Write("p(y" + j + ") = ");
				for (int i = 0; i < PXandY[0].Length; i++)
				{
					Console.Write(PXandY[i][j] + " + ");
					res_elt += PXandY[i][j];
				}

				Console.Write(" = " + res_elt + "\n");
				res[j] = res_elt;
			}

			return res;
		}

		public static double[] px(double[][] PXandY)
		{
			var res = new double[PXandY.Length];
			Console.WriteLine("Calculating p(x)");
			for (int i = 0; i < PXandY.Length; i++)
			{
				var res_elt = 0.0;
				Console.Write("p(x" + i + ") = ");
				for (int j = 0; j < PXandY[0].Length; j++)
				{
					Console.Write(PXandY[i][j] + " + ");
					res_elt += PXandY[i][j];
				}

				Console.Write(" = " + res_elt + "\n");
				res[i] = res_elt;
			}

			return res;
		}

		public static double pxify(double pxandy, double py)
		{
			return pxandy / py;
		}

		public static double[][] PYifX(double[][] PXandY)
		{
			var result = new double[PXandY.Length][];
			var py = Lab1.py(PXandY);
			var precalculated = "";
			var calculated = "";
			for (int i = 0; i < PXandY.Length; i++)
			{
				result[i] = new double[PXandY[i].Length];
				for (int j = 0; j < result[i].Length; j++)
				{
					var probability = pxify(PXandY[i][j], py[i]);
					precalculated += PXandY[i][j] + " / " + py[i] + "\t";
					calculated += probability + "\t";
					result[i][j] = probability;
				}

				precalculated += "\n";
				calculated += "\n";
			}

			Console.WriteLine("P(Y|X) = \n" + precalculated + "\n=\n" + calculated);
			return result;
		}

		public static double HXandY(double[][] PXandY)
		{
			var result = 0.0;
			var precalculated = "";
			var calculated = "";
			foreach (var row in PXandY)
			{
				for (int j = 0; j < row.Length; j++)
				{
					var value = row[j] * Math.Log(row[j], 2) * -1.0;
					calculated += value + " + ";
					precalculated += row[j] + "Log2(" + row[j] + ") + ";
					result += value;
				}
			}

			Console.WriteLine("H(X, Y) = " + precalculated + " = " + calculated + " = " + result);
			return result;
		}

		public static double HYifX(double[][] PYifX, double[] pi)
		{
			var result = 0.0;
			var Entr = "H(Y|X) = ";
			var Entr_precalc = "";
			var Entr_calc = "";
			for (int i = 0; i < PYifX.Length; i++)
			{
				var HYifxi = 0.0;
				var partialEntr = "H(Y|x" + i + ") = ";
				var partialEntr_precalc = "";
				var partialEntr_calc = "";
				for (int j = 0; j < PYifX[i].Length; j++)
				{
					var value = PYifX[i][j] * Math.Log(PYifX[i][j], 2);
					partialEntr_precalc += PYifX[i][j] + "*Log2(" + PYifX[i][j] + ") + ";
					partialEntr_calc += value + " + ";
					HYifxi += value * -1.0;
				}

				partialEntr += partialEntr_precalc + " = " + partialEntr_calc + " = " + HYifxi;
				Console.WriteLine(partialEntr);
				Entr_precalc += pi[i] + "*" + HYifxi + " + ";
				var val = pi[i] * HYifxi;
				Entr_calc += val + " + ";
				result += val;
			}

			Entr += Entr_precalc + " = " + Entr_calc + " = " + result;
			Console.WriteLine(Entr);
			return result;
		}

		public static double AvgInfo(double hx, double hy, double hxandy)
		{
			var result = hx + hy - hxandy;
			Console.WriteLine("I(X; Y) = " + hx + " + " + hy + " - " + hxandy + " = " + result);
			return result;
		}
	}
}