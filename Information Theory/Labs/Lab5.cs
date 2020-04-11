using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Information_Theory.Labs
{
	public class Lab5
	{
		private static Dictionary<string, bool> convert = new Dictionary<string, bool>() {{"1", true}, {"0", false}};
		private static Dictionary<bool, string> convert_reverse = new Dictionary<bool, string>()
			{{true, "1"}, {false, "0"}};

		private static string t1_sequenceX = "10111001011";
		private static string[] t1_sequenceY1 = new[] {"1011011001001001", "1101101111011010"};
		private static string[] t1_sequenceY2 = new[] {"001101110", "011100100"};

		private static string t2_sequenceX = "110111001011";
		private static string t2_sequenceY = "11001111101000110100";

		private static int[][][] t3_matrix = new[]
		{
			new[]
			{
				new[] {1, 0, 0, 0, 0},
				new[] {0, 1, 0, 0, 0},
				new[] {0, 0, 1, 0, 0},
				new[] {0, 0, 0, 1, 0},
				new[] {0, 0, 0, 0, 1}
			},
			new[]
			{
				new[] {0, 0, 1, 1},
				new[] {0, 1, 0, 1},
				new[] {0, 1, 1, 0},
				new[] {1, 0, 0, 1},
				new[] {1, 0, 1, 0}
			}
		};
		private static string[] t3_sequences = new[] {"111110011", "101111100", "000011111"};

		private static int[][][] t4_matrix = new[]
		{
			new[]
			{
				new[] {0, 0, 0, 1, 1},
				new[] {0, 1, 1, 0, 0},
				new[] {1, 0, 1, 0, 1},
				new[] {1, 1, 0, 1, 0}
			},
			new[]
			{
				new[] {1, 0, 0, 0},
				new[] {0, 1, 0, 0},
				new[] {0, 0, 1, 0},
				new[] {0, 0, 0, 1}
			}
		};
		private static string[] t4_sequences = new[] {"011011001","100001010","000000110"};

		private static int t5_dmin1 = 3;
		private static string t5_sequenceX1 = "10101";
		private static string t5_sequenceY1 = "1111110001111";
		private static int t5_dmin2 = 4;
		private static string t5_sequenceX2 = "10011011001";
		private static string t5_sequenceY2 = "111111111100110001100";


		public static void Task1()
		{
			InverseEncode(t1_sequenceX);
			EvenCheckEncode(t1_sequenceX);
			foreach (var s in t1_sequenceY1)
			{
				Console.WriteLine(InverseFindIfErrorExists(s)
					? "There is no error in sequence"
					: "There is an error in sequence");
			}

			foreach (var s in t1_sequenceY2)
			{
				Console.WriteLine(EvenCheckFindIfErrorExists(s)
					? "There is no error in sequence"
					: "There is an error in sequence");
			}
		}
		public static void Task2()
		{
			IterativeEncode(t2_sequenceX);
			IterativeDecodeAndFixError(t2_sequenceY);
		}

		public static void Task3()
		{
			var checkMatrix = GetCheckMatrix(t3_matrix);
			LinearBlockCheckAndFix(t3_sequences, checkMatrix);
		}
		public static void Task4()
		{
			LinearBlockCheckAndFix(t4_sequences, t4_matrix);
		}
		public static void Task5(){}

		private static string InverseEncode(string input)
		{
			var secondHalf = "";
			if (NumberOfOnesIsEven(input))
			{
				secondHalf = input;
			}
			else
			{
				foreach (var character in input)
				{
					if (character == '1')
					{
						secondHalf += "0";
					}
					else
					{
						secondHalf += "1";
					}
				}
			}

			Console.WriteLine("Input sequence: \t" + input + ";\nControl sequence: \t" + secondHalf);
			Console.WriteLine("Result sequence:\t" + input + secondHalf);
			return input + secondHalf;
		}
		private static bool InverseFindIfErrorExists(string input)
		{
			var inputSequence = input.Substring(0, input.Length / 2);
			var controlSequence = input.Substring(input.Length / 2, input.Length / 2);
			Console.WriteLine("Checking sequence: \t"+input);
			Console.WriteLine("Input sequence: \t"+inputSequence);
			Console.WriteLine("Control sequence: \t" + controlSequence);
			Console.WriteLine(InverseEncode(inputSequence));
			if (NumberOfOnesIsEven(inputSequence))
			{
				for (int i = 0; i < input.Length/2; i++)
				{
					if (inputSequence[i] != controlSequence[i]) return false;
				}	
			}
			else
			{
				for (int i = 0; i < input.Length/2; i++)
				{
					if (inputSequence[i] == controlSequence[i]) return false;
				}
			}

			return true;
		}

		private static string EvenCheckEncode(string input)
		{
			var controlSymbol = "";
			controlSymbol = NumberOfOnesIsEven(input) ? "0" : "1";
			Console.WriteLine("Input sequence: " + input + "; Control symbol: " + controlSymbol);
			return input + controlSymbol;
		}
		private static bool EvenCheckFindIfErrorExists(string input)
		{
			var inputSequence = input.Remove(input.Length - 1, 1);
			var controlSymbol = input.Substring(input.Length - 1, 1);
			Console.WriteLine("Checking sequence: \t"+input);
			Console.WriteLine("Input sequence: "+inputSequence +";  Control symbol: "+controlSymbol);
			Console.WriteLine("expected control sequence: \t" + NumberOfOnesIsEven(inputSequence));
			return NumberOfOnesIsEven(inputSequence) == convert[controlSymbol];
		}

		public static string IterativeEncode(string input)
		{
			if (input.Length != 12)
			{
				throw new InvalidDataException("Unexpected input length. Expected Length=12; Received Length="+input.Length);
			}

			var k = 12; //const = rows*columns in information matrix
			var n = 20; //resulting code length
			var R = 1 - k / (n * 1.0);
			var firstRow = input.Substring(0, 4);
			var secondRow = input.Substring(4, 4);
			var thirdRow = input.Substring(8, 4);
			var b1 = convert_reverse[!NumberOfOnesIsEven(firstRow)];
			var b2 = convert_reverse[!NumberOfOnesIsEven(secondRow)];
			var b3 = convert_reverse[!NumberOfOnesIsEven(thirdRow)];
			var firstColumn = firstRow.Substring(0,1) + secondRow.Substring(0,1) + thirdRow.Substring(0,1);
			var secondColumn = firstRow.Substring(1,1) + secondRow.Substring(1,1) + thirdRow.Substring(1,1);
			var thirdColumn = firstRow.Substring(2,1) + secondRow.Substring(2,1) + thirdRow.Substring(2,1);
			var fourthColumn = firstRow.Substring(3,1) + secondRow.Substring(3,1) + thirdRow.Substring(3,1);
			var b4 = convert_reverse[!NumberOfOnesIsEven(firstColumn)];
			var b5 = convert_reverse[!NumberOfOnesIsEven(secondColumn)];
			var b6 = convert_reverse[!NumberOfOnesIsEven(thirdColumn)];
			var b7 = convert_reverse[!NumberOfOnesIsEven(fourthColumn)];
			var b8 = convert_reverse[!NumberOfOnesIsEven(b1+b2+b3)];
			var res = firstRow+b1+secondRow+b2+thirdRow+b3 + b4 + b5 + b6 + b7 + b8;
			Console.WriteLine("Надмірність коду: "+R);
			Console.WriteLine(firstRow + "| "+b1+" (b1)");
			Console.WriteLine(secondRow + "| "+b2+" (b2)");
			Console.WriteLine(thirdRow + "| "+b3+" (b3)");
			Console.WriteLine("____________");
			Console.WriteLine(b4+b5+b6+b7+"| "+b8);
			Console.WriteLine("b4b5b6b7 b8");
			Console.WriteLine("Result sequence: "+res);
			return res;
		}
		public static string IterativeDecodeAndFixError(string input)
		{
			var rows=new string[4];
			for (int i = 0; i < 4; i++)
			{
				rows[i] = input.Substring(5*i, 5);
			}
			var columns = new string[5];
			for (int i = 0; i < 5; i++)
			{
				columns[i]= rows[0].Substring(i,1) + rows[1].Substring(i,1) + rows[2].Substring(i,1)+ rows[3].Substring(i,1);
			}

			Console.WriteLine("Checking input: " + input);
			Console.WriteLine(rows[0]);
			Console.WriteLine(rows[1]);
			Console.WriteLine(rows[2]);
			Console.WriteLine(rows[3]);
			
			var rowError = new string[4];
			for (int i = 0; i < 4; i++)
			{
				rowError[i] = convert_reverse[!NumberOfOnesIsEven(rows[i])];
			}
			var columnError = new string[5];
			for (int i = 0; i < 5; i++)
			{
				columnError[i] = convert_reverse[!NumberOfOnesIsEven(columns[i])];
			}

			Console.WriteLine("\n"+rows[0]+"| "+rowError[0]);
			Console.WriteLine(rows[1]+"| "+rowError[1]);
			Console.WriteLine(rows[2]+"| "+rowError[2]);
			Console.WriteLine(rows[3]+"| "+rowError[3]);
			Console.WriteLine("____________");
			Console.WriteLine(columnError[0]+columnError[1]+columnError[2]+columnError[3]+columnError[4] );


			if (rowError.Contains("1") && columnError.Contains("1"))
			{
				var errorRowIndex = Array.IndexOf(rowError, "1");
				var errorColumnIndex = Array.IndexOf(columnError, "1");
				Console.WriteLine("Error located in " + errorRowIndex + " row, " + errorColumnIndex + " column.");

				var rowToFix = rows[errorRowIndex];
				var error = rowToFix.Substring(errorColumnIndex, 1);
				var fixedRow = rowToFix.Substring(0, errorColumnIndex) +
				               convert_reverse[!convert[error]] +
				               rowToFix.Substring(errorColumnIndex + 1, rowToFix.Length - errorColumnIndex - 1);
				rows[errorRowIndex] = fixedRow;
				var fixedres = "";
				foreach (var row in rows)
				{
					fixedres += row;
				}

				Console.WriteLine("Fixed input: "+fixedres);
				
				return fixedres;
			}

			if (!(rowError.Contains("1") && columnError.Contains("1")))
			{
				Console.WriteLine("Input contains no errors");
				return input;
			}

			throw new Exception("I don't know what went wrong and I ain't gonna fix it");
		}
		
		public static int[][][] GetCheckMatrix(int[][][] creationMatrix)
		{
			var checkMatrix = new int[2][][];
			var checkMatrix_str = "";
			var h = creationMatrix[1][0].Length;
			var w = creationMatrix[1].Length;
			checkMatrix[0] = new int[h][];
			checkMatrix[1] = new int[h][];
			Console.WriteLine("Calculating check matrix");
			for (var i=0; i<h; i++)
			{
				checkMatrix[0][i] = new int[w];
				checkMatrix[1][i] = new int[h];
			}
			for(var i =0; i<h; i++)
			{
				for(var j=0; j<w; j++)
				{
					checkMatrix[0][i][j] = creationMatrix[1][j][i];
					checkMatrix_str += creationMatrix[1][j][i] + " ";
				}
				checkMatrix_str += " | ";
				for (var j = 0; j < h; j++)
				{
					if(i==j)
					{
						checkMatrix[1][i][j] = 1;
						checkMatrix_str += 1 + " ";
					}
					else
					{
						checkMatrix[1][i][j] = 0;
						checkMatrix_str += 0 + " ";
					}
					 
				}
				checkMatrix_str += "\n";
			}
			Console.WriteLine(checkMatrix_str);
			return checkMatrix;
		}

		public static void LinearBlockCheckAndFix(string[] inputs, int[][][] checkMatrix)
		{
			var k = checkMatrix[0][0].Length;
			var r = checkMatrix[0].Length;
			foreach(var combination in inputs)
			{
				Check(combination, checkMatrix[0], k, r);				
			}
			
		}

		public static void Check(string combination, int[][] checkMatrix_a, int k, int r)
		{
			var ai = combination.Substring(0, k);
			var bi = combination.Substring(k, r);
			var si = new string[r];
			var syndrom = "";
			Console.WriteLine("Calculating si-ths for "+combination);
			var hasOne = false;
			for(var j=0; j<r; j++)
			{
				si[j] = "";
				var pre = "";
				for(var i=0; i<k; i++)
				{
					var str = (ai.Substring(i, 1) + checkMatrix_a[j][i]).ToString();
					pre += str + "+";
					si[j] += Multiply(str);
				}
				si[j] += bi[j];
				pre += bi[j];
				Console.WriteLine(pre);
				Console.WriteLine(si[j]);
				si[j] = XorEach(si[j]);
				if(si[j]=="1")
				{
					hasOne = true;
				}
				Console.WriteLine(si[j]);
				syndrom += si[j];
			}
			Console.WriteLine("Syndrom: " + syndrom);
			if(hasOne)
			{
				Console.WriteLine("Code has errors");
				for(var i=0; i<k; i++)
				{
					var toCheck = "";
					for(var j=0; j<r; j++)
					{
						toCheck += checkMatrix_a[j][i];
					}
					Console.WriteLine("Comparing {0} and {1}", toCheck, syndrom);
					if(toCheck==syndrom)
					{
						var tofix = i;
						var s = combination.Substring(i, 1);
						var f = "";
						if (s == "1")
							f = "0";
						else
						{
							f = "1";
						}
						Console.WriteLine("Wrong combination: " + combination);
						Console.WriteLine("Error located in {0} element", i+1);
						combination = combination.Substring(0, i) + f + combination.Substring(i + 1, combination.Length - i - 1);
						Console.WriteLine("fixed combination: " + combination);
						break;

					}
				}

			}
			else
			{
				Console.WriteLine("Code is correct");
			}
		}

		private static bool NumberOfOnesIsEven(string input)
		{
			var res = true;
			foreach (var character in input)
			{
				if (character=='1')
				{
					res = !res;
				}
			}

			return res;
		}

		private static string Multiply(string str)
		{
			if (str.Substring(0, 1) == "1" && str.Substring(1, 1) == "1")
			{
				return "1";
			}
			return "0";
		}

		private static string XorEach(string str)
		{
			while (str.Length!=1)
			{
				var rest = str.Substring(2, str.Length - 2);
				var s = "";
				if(str.Substring(0, 1) == str.Substring(1, 1))
				{
					s = "0";
				}
				else
				{
					s = "1";
				}
				str = s + rest;
			}
			return str;
		}

	}
}