using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Information_Theory.Labs
{
	public class Lab3
	{
		private const int t1_count_N = 16;
		private const int t1_len_n = 7;
		
		private const string t2_combination_A = "0101";
		private const string t2_combination_B = "1011";
		private const int t2_len_d = 2;
		
		private const string t3_combination_A = "11011011";
		private const string t3_combination_B = "01001010";
		private const string t3_combination_C = "01010101";
		private const string t3_combination_D = "11001101";
		private const int t3_len_n = 8;


		public static void Task1()
		{
			Redundancy(t1_count_N, t1_len_n);
		}

		public static void Task2()
		{
			CodeDistance(t2_combination_A, t2_combination_B);
			AllPossibleCombinationsOnLength(t2_combination_A, t2_len_d);
		}

		public static void Task3()
		{
			var min = int.MaxValue;
			var max = int.MinValue;
			var i = 0;
			i = CodeDistance(t3_combination_A, t3_combination_B);
			if (i > max)
			{
				max = i;
			}

			if (i < min)
			{
				min = i;
			}

			i = CodeDistance(t3_combination_A, t3_combination_C);
			if (i > max)
			{
				max = i;
			}

			if (i < min)
			{
				min = i;
			}

			i = CodeDistance(t3_combination_A, t3_combination_D);
			if (i > max)
			{
				max = i;
			}

			if (i < min)
			{
				min = i;
			}

			i = CodeDistance(t3_combination_B, t3_combination_C);
			if (i > max)
			{
				max = i;
			}

			if (i < min)
			{
				min = i;
			}

			i = CodeDistance(t3_combination_B, t3_combination_D);
			if (i > max)
			{
				max = i;
			}

			if (i < min)
			{
				min = i;
			}

			i = CodeDistance(t3_combination_C, t3_combination_D);
			if (i > max)
			{
				max = i;
			}

			if (i < min)
			{
				min = i;
			}

			Console.WriteLine("Biggest distance: " + max);
			Console.WriteLine("Least distance: " + min);
		}

		private static double CodeLengthNotRedundant(int symbolsCount)
		{
			var res = Math.Log(symbolsCount, 2);
			Console.WriteLine("k = log2("+symbolsCount+")=" + Math.Round(res, 3));
			return res;
		}

		private static double Redundancy(int symbolsCount, int codeLength)
		{
			var nonRedundant = CodeLengthNotRedundant(symbolsCount);
			var res = 1 - (nonRedundant / codeLength);
			Console.WriteLine("ro = 1-(" + Math.Round(nonRedundant, 3) + "/" + codeLength + ")=" + Math.Round(res, 3));
			return res;
		}

		private static int CodeDistance(string codeA, string codeB)
		{
			var sum = Xor(codeA, codeB);
			var distance = CombinationWeight(sum);
			Console.WriteLine("w="+distance);
			return distance;
		}

		private static int CombinationWeight(string combination)
		{
			if (combination.Replace("1","").Replace("0","").Length!=0)
			{
				throw new InvalidDataException("Incorrect string entered: "+combination);
			}
			return combination.Length - combination.Replace("1", "").Length;
		}

		private static string Xor(string a, string b)
		{
			if (a.Length != b.Length)
			{
				throw new InvalidDataException("Entered codes are not the same length.");
			}
			var res = "";
			var len = a.Length;
			for (var i = 0; i < len; i++)
			{
				if (a[i]!=b[i])
				{
					res += "1";
				}
				else
				{
					res += "0";
				}
			}

			Console.WriteLine(a + "\n" + b + "\n" + res);
			return res;
		}

		private static void AllPossibleCombinationsOnLength(string initial, int d)
		{
			var n = initial.Length;
			var combinations = Factorial(n) / (Factorial(d) * Factorial(n - d));
			Console.WriteLine(n + "!/(" + d + "!(" + n + "-" + d + ")!)="+Factorial(n) + "/(" + Factorial(d) + "*" + Factorial(n - d)+ ")");
			Console.WriteLine("Possible combinations count: "+combinations);
			var toXor = "";
			for (var i = 0; i < d; i++)
			{
				toXor += "1";
			}

			while (toXor.Length<n)
			{
				toXor += "0";
			}

			var possibleCombinations = Permutate(toXor);
			for (var i = 0; i < combinations; i++)
			{
				Console.WriteLine(i+1);
				Xor(initial, possibleCombinations[i]);
			}
		}

		private static int Factorial(int n)
		{
			var res = n;
			for (var i = n - 1; i >= 1; i--)
			{
				res *= i;
			}

			return res;
		}


		private static List<string> Permutate(string toPermutate)
		{
			var res = new List<string>();
			if (toPermutate.Length == 1)
			{
				res.Add(toPermutate);
				return res;
			}

			for (var i = 0; i < toPermutate.Length; i++)
			{
				var symbol = toPermutate[i];
				var permutations = Permutate(toPermutate.Remove(i, 1));
				foreach (var str in permutations.Select(permutation => symbol + permutation).Where(str => !res.Contains(str)))
				{
					res.Add(str);
				}
			}

			return res;
		}
	}
}