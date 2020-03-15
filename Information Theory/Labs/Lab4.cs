using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Information_Theory.Labs
{
	public class Lab4
	{
		private static readonly string[] alphabet = {"A", "B", "C"};
		
		private static List<Symbol> t1_symbols = new List<Symbol> {
			new Symbol("1", 0.32),
			new Symbol("6", 0.22),
			new Symbol("4", 0.15),
			new Symbol("2", 0.09),
			new Symbol("7", 0.08),
			new Symbol("8", 0.07),
			new Symbol("3", 0.05),
			new Symbol("5", 0.02)};
		
		private static List<Symbol> t2_symbols = new List<Symbol> {
			new Symbol("A", 0.42),
			new Symbol("B", 0.37),
			new Symbol("C", 0.18),
			new Symbol("D", 0.03)};
		private static string t2_sequence = "BBBCBABBBAACCBCCAAACABBABADAAA";
		
		private static double[][] t3_matrix =
		{
			new[] {0.01, 0.75, 0.24},
			new[] {0.33, 0.22, 0.45},
			new[] {0.01, 0.83, 0.16}
		};

		private static string t3_fragment = "ACBBABCBBABCCBABCBCB";
		public static void Task1()
		{
			var coded = ShannonFano(t1_symbols);
			PullLists(coded, out var p1, out var l1);
			AverageLength(p1, l1);
			var coded2 = Huffman(t1_symbols);
			PullLists(coded2, out var p2, out var l2);
			AverageLength(p2, l2);
		}
		public static void Task2()
		{
			var coded = ShannonFano(t2_symbols);
			RelativeDifference(coded, 1);
			var codedPairs = MakePairs(t2_symbols, 'S');
			RelativeDifference(codedPairs, 2);
			var singular = Encode(coded, t2_sequence);
			var pairs = Encode(codedPairs, t2_sequence);
			Console.WriteLine(singular+"\nCode length: "+singular.Length);
			Console.WriteLine(pairs+"\nCode length: "+pairs.Length);
		}
		public static void Task3()
		{
			CalculateCodesForMatrix(t3_matrix, out var nonConditional,  out var ifA,  out var ifB,  out var ifC);
			var pairs = MakePairs(nonConditional, 'S');
			var encoded = Encode(nonConditional, t3_fragment);
			Console.WriteLine("Encoded nonconditional");
			Console.WriteLine(encoded);
			Console.WriteLine("Code length: " + encoded.Length);
			encoded = Encode(pairs, t3_fragment);
			Console.WriteLine("Encoded double words");
			Console.WriteLine(encoded);
			Console.WriteLine("Code length: " + encoded.Length);
			encoded = EncodeConditional(nonConditional, new List<List<Symbol>>{ifA, ifB, ifC}, t3_fragment);
			Console.WriteLine("Encoded conditionl");
			Console.WriteLine(encoded);
			Console.WriteLine("Code length: " + encoded.Length);
		}

		private static double AverageLength(double[] pi, int[] li)
		{
			if (pi.Length!=li.Length)
			{
				throw new InvalidDataException("Arrays are of incorrect length");
			}
			var precalc = "";
			var calc = "";
			var len = pi.Length;
			var res = 0.0;
			for (int i = 0; i < len; i++)
			{
				precalc += pi[i] + "*" + li[i] + "+";
				calc += pi[i] * li[i] + "+";
				res += li[i] * pi[i];
			}
			Console.WriteLine(precalc+"=\n"+calc+"=\n"+res);
			return res;
		}
		private static double RelativeDifference(List<Symbol> coded, int wordLength)
		{
			PullLists(coded, out var pi, out var li);
			var l = AverageLength(pi, li)/wordLength;
			Lab1.H(pi, out var entr);
			var res = ((l - entr) / entr) * 100.0;
			Console.WriteLine("((" +
			                  Math.Round(l, 3) +
			                  "-" +
			                  Math.Round(entr, 3) +
			                  ")/" +
			                  entr +
			                  ")*100%=" +
			                  Math.Round(res, 3) +
			                  "%");
			return res;
		}
		private static string Encode(List<Symbol> codes, string toEncode)
		{
			var res = "";
			var toPull = codes[0].Name.Length;
			while (toEncode!="")
			{
				var toMatch = toEncode.Substring(0, toPull);
				toEncode = toEncode.Remove(0, toPull);
				res += Match(toMatch, codes);
			}

			return res;
		}
		private static string EncodeConditional(List<Symbol> nonConditional, List<List<Symbol>> codes, string toEncode)
		{
			var res = "";
			var toPull = 1;
			var previous = toEncode.Substring(0, toPull);
			toEncode = toEncode.Remove(0, toPull);
			res += Match(previous, nonConditional);
			while (toEncode!="")
			{
				var toMatch = toEncode.Substring(0, toPull);
				toEncode = toEncode.Remove(0, toPull);
				res += Match(toMatch, codes[Array.IndexOf(alphabet, previous)]);
				previous = toMatch;
			}

			return res;
		}

		private static void CalculateCodesForMatrix(double[][] matrix, out List<Symbol> nonConditional, out List<Symbol> ifA, out List<Symbol> ifB, out List<Symbol> ifC)
		{
			ifA = new List<Symbol>();
			ifB = new List<Symbol>();
			ifC = new List<Symbol>();
			for (int i = 0; i < 3; i++)
			{
				ifA.Add(new Symbol(alphabet[i], matrix[0][i]));
				ifB.Add(new Symbol(alphabet[i], matrix[1][i]));
				ifC.Add(new Symbol(alphabet[i], matrix[2][i]));
			}

			ifA=ifA.OrderByDescending(x => x.Probability).ToList();
			ifB=ifB.OrderByDescending(x => x.Probability).ToList();
			ifC=ifC.OrderByDescending(x => x.Probability).ToList();

			Console.WriteLine("After A");
			ifA = ShannonFano(ifA);
			Console.WriteLine("After B");
			ifB = ShannonFano(ifB);
			Console.WriteLine("After C");
			ifC = ShannonFano(ifC);
			
			Console.WriteLine("Average for A");
			PullLists(ifA, out var pa, out var la);
			var avgla = AverageLength(pa, la);
			Console.WriteLine("Average for B");
			PullLists(ifB, out var pb, out var lb);
			var avglb =AverageLength(pb, lb);
			Console.WriteLine("Average for C");
			PullLists(ifC, out var pc, out var lc);
			var avglc =AverageLength(pc, lc);

			double[][] input =
			{
				new[] {matrix[0][0] - 1.0, matrix[1][0], matrix[2][0]},
				new[] {matrix[1][0], matrix[1][1] - 1, matrix[2][1]},
				new[] {1.0, 1.0, 1.0}
			};
			SystemSolver.Matrix3x3.SolveEquations(out var sol,
				new SystemSolver.Matrix3x3(input),
				new SystemSolver.Vector3(new []{0.0, 0.0, 1.0}));

			var avglTotal = avgla * sol.x + avglb * sol.y + avglc * sol.z;
			Console.WriteLine(Math.Round(avgla, 3) +
			                  "*" +
			                  Math.Round(sol.x, 3) +
			                  "+" +
			                  Math.Round(avglb, 3) +
			                  "*" +
			                  Math.Round(sol.y, 3) +
			                  "+" +
			                  Math.Round(avglc, 3) +
			                  "*" +
			                  Math.Round(sol.z, 3) +
			                  "=" +
			                  Math.Round(avgla * sol.x, 3) +
			                  "+" +
			                  Math.Round(avglb * sol.y, 3) +
			                  "+" +
			                  Math.Round(avglc * sol.z, 3) +
			                  "=" +
			                  Math.Round(avglTotal, 3));

			var Hmem = 0.0;
			var memPrecalc = "";
			var memCalc = "";
			for (int i = 0; i < 3; i++)
			{
				var Hinterm = 0.0;
				var intermPrecalc = "";
				var intermCalc = "";
				for (int j = 0; j < 3; j++)
				{
					var c =  matrix[i][j] * Math.Log(matrix[i][j], 2);
					Hinterm += c;
					intermPrecalc += matrix[i][j] + "log2(" + matrix[i][j] + ")+";
					intermCalc += Math.Round(c, 3) + "+";
				}

				Console.WriteLine("H(X|x" + i + ")=" + intermPrecalc + "=" + intermCalc + "=" + Math.Round(Hinterm,3));
				var cc= sol[i] * Hinterm;
				Hmem += cc;
				memPrecalc += Math.Round(sol[i], 3) + "*" + Math.Round(Hinterm, 3);
				memCalc += Math.Round(cc,3) + "+";
			}

			Hmem *= -1;
			Console.WriteLine("Hmem(X)= -(" + memPrecalc + ")=-(" + memCalc + ")=" + Math.Round(Hmem,3));
			
			var relDiff = ((avglTotal - Hmem) / Hmem) * 100.0;
			Console.WriteLine("((" +
			                  Math.Round(avglTotal, 3) +
			                  "-" +
			                  Math.Round(Hmem, 3) +
			                  ")/" +
			                  Hmem +
			                  ")*100%=" +
			                  Math.Round(relDiff, 3) +
			                  "%");
			
			nonConditional = new List<Symbol>{new Symbol("A", sol[0]), new Symbol("B", sol[1]), new Symbol("C", sol[2])};
			nonConditional = nonConditional.OrderByDescending(x => x.Probability).ToList();
			nonConditional = ShannonFano(nonConditional);
		}
		private static List<Symbol> MakePairs(List<Symbol> symbols, char encoding)
		{
			var res = new List<Symbol>();
			for (int i = 0; i < symbols.Count; i++)
			{
				for (int j = 0; j < symbols.Count; j++)
				{
					var name = symbols[i].Name + symbols[j].Name;
					var prob = symbols[i].Probability * symbols[j].Probability;
					Console.WriteLine("p("+name+")="+Math.Round(symbols[i].Probability,3)+"*"+Math.Round(symbols[j].Probability,3)+"="+Math.Round(prob,3));
					res.Add(new Symbol(name, prob));
				}
			}

			res = res.OrderByDescending(x => x.Probability).ToList();
			
			if (encoding == 'H')
			{
				return Huffman(res);
			}
			if (encoding == 'S')
			{
				return ShannonFano(res);
			}
			throw new InvalidDataException("Incorrect Encoding Selected");
		}
		
		private static List<Symbol> ShannonFano(List<Symbol> symbols)
		{
			var measuredSymbols = CalculateSymbolsCodes_SF(symbols);

			Console.WriteLine("Name\tProbability\tCode");
			foreach (var symbol in measuredSymbols)
				Console.WriteLine($"{symbol.Name}\t{Math.Round(symbol.Probability,3)}\t\t{symbol.Code}");
			return measuredSymbols;
		}
		private static List<Symbol> Huffman(List<Symbol> symbols)
		{
			var measuredSymbols = CalculateSymbolsCodes_H(symbols);

			Console.WriteLine("Name\tProbability\tCode");
			foreach (var symbol in measuredSymbols)
				Console.WriteLine($"{symbol.Name}\t{Math.Round(symbol.Probability,3)}\t\t{symbol.Code}");
			return measuredSymbols;
		}
		
		private static List<Symbol> CalculateSymbolsCodes_SF(List<Symbol> symbols)
		{
			var separationPoint = GetSeparationIndex(symbols) + 1;

			var firstList = symbols.GetRange(0, separationPoint);
			var secondList = symbols.GetRange(separationPoint, symbols.Count - separationPoint);

			firstList.ForEach(symbol => symbol.Code += "0");
			if (firstList.Count > 1) CalculateSymbolsCodes_SF(firstList);

			secondList.ForEach(symbol => symbol.Code += "1");
			if (secondList.Count > 1) CalculateSymbolsCodes_SF(secondList);

			firstList.AddRange(secondList);

			return firstList;
		}
		private static List<Symbol> CalculateSymbolsCodes_H(List<Symbol> symbols)
		{
			var tree = new List<Node>();
			foreach (var symbol in symbols)
			{
				var n = new Node();
				n.Symbol = symbol.Name;
				n.Probability = symbol.Probability;
				tree.Add(n);
			}

			while (tree.Count>1)
			{
				tree = tree.OrderBy(x => x.Probability).ToList();
				var newNode = new Node();
				newNode.Symbol = "_";
				newNode.Probability = tree[0].Probability + tree[1].Probability;
				Console.WriteLine("left: " + tree[1].Probability +" right: "+tree[0].Probability + " into " + newNode.Probability);
				newNode.Left = tree[1];
				newNode.Right = tree[0];
				tree.Remove(tree[0]);
				tree.Remove(tree[0]);
				tree.Add(newNode);
			}

			var root = tree.First();
			
			foreach (var symbol in symbols)
			{
				symbol.Code = root.Traverse(symbol.Name, "");
			}

			return symbols;
		}

		// Returns the index of the element closest to total/2
		private static int GetSeparationIndex(List<Symbol> symbols)
		{
			var differences = new SortedList<double, int>();
			var half = symbols.Sum(s => s.Probability) / 2;
			var sum = 0.0;
			for (var i = 0; i < symbols.Count; i++)
			{
				sum += symbols[i].Probability;
				var diff = Math.Abs(half - sum);
				if (!differences.ContainsKey(diff))
				{
					differences.Add(diff, i);
				}
			}
			return differences.First().Value;
		}

		private static string Match(string toMatch, List<Symbol> codes)
		{
			foreach (var symbol in codes.Where(symbol => toMatch==symbol.Name))
			{
				return symbol.Code;
			}

			throw new InvalidDataException("No such substring in codes");
		}
		private static void PullLists(List<Symbol> symbols, out double[] pi, out int[] li)
		{
			pi = new double[symbols.Count];
			li = new int[symbols.Count];
			for (int i = 0; i < symbols.Count; i++)
			{
				pi[i] = symbols[i].Probability;
				li[i] = symbols[i].CodeLength;
			}
		}





	}
	class Symbol
	{
		string name;
		string code;
		double probability;

		public Symbol(string name, double probability)
		{
			this.name = name;
			this.probability = probability;
			code = "";
		}

		public string Name { get => name; }
		public double Probability { get => probability; }
		public string Code { get => code; set => code = value; }
		public int CodeLength { get => code.Length; }
	}

	class Node
	{
		public string Symbol { get; set;}
		
		public double Probability { get; set; }
        public Node Right { get; set; }
        public Node Left { get; set; }
        public string Traverse(string symbol, string code)
        {
	        // Leaf
	        if (Right == null && Left == null)
	        {
		        return symbol.Equals(Symbol) ? code : null;
	        }

	        string left = null;
	        string right = null;
 
	        if (Left != null)
	        {
		        var leftPath = "";
		        leftPath+=code;
		        leftPath+=0;
 
		        left = Left.Traverse(symbol, leftPath);
	        }
 
	        if (Right != null)
	        {
		        var rightPath = "";
		        rightPath+=code;
		        rightPath+=1;
		        right = Right.Traverse(symbol, rightPath);
	        }
 
	        return left ?? right;
        }
	}
}