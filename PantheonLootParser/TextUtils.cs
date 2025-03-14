using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PantheonLootParser
{
	internal static class TextUtils
	{
		private static String CorrectRecogning(String inputString)
		{
			String[] words = inputString.Split(' ');
			for(Int32 index = 0; index < words.Length; index++)
			{
				var receivedDistance = DamerauLevenshteinDistance(words[index], "received", Int32.MaxValue);
				if(receivedDistance > 0 && receivedDistance <= 2)
					words[index] = "received";
				var lootedDistance = DamerauLevenshteinDistance(words[index], "looted", Int32.MaxValue);
				if(lootedDistance > 0 && lootedDistance <= 2)
					words[index] = "looted";
			}
			return String.Join(" ", words);
		}

		public static String MakeReplacements(String input, Dictionary<String, String> replacements)
		{
			foreach(String key in replacements.Keys)
				if(input.Contains(key))
					input = input.Replace(key, replacements[key]);
			return input;
		}

		public static String[] FindByLootPatterns(String inputString)
		{
			inputString = String.Join(' ', inputString.Split(new String[] { " ", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries));
			inputString = CorrectRecogning(inputString);

			List<String> result = new List<String>();

			foreach (Match itemTitle in Regex.Matches(inputString, "You received ([^.\\[]+)[.\\[]+"))
				result.Add(itemTitle.Groups[1].Value);

			foreach(Match itemTitle in Regex.Matches(inputString, "]\\s?([^\\s]+) looted ([^.\\[]+)[.\\[]+"))
				result.Add(itemTitle.Groups[2].Value);

			result.RemoveAll(p => String.IsNullOrWhiteSpace(p));
			result.Distinct();

			return result.ToArray();
		}

		public static Int32 DamerauLevenshteinDistance(String source, String target, Int32 threshold)
		{
			Int32[] sourceChars = source.Trim().ToLower().Select(p => (int)p).ToArray();
			Int32[] targetChars = target.Trim().ToLower().Select(p => (int)p).ToArray();
			return DamerauLevenshteinDistance(sourceChars, targetChars, threshold);
		}

		/// <summary>
		/// Computes the Damerau-Levenshtein Distance between two strings, represented as arrays of
		/// integers, where each integer represents the code point of a character in the source string.
		/// Includes an optional threshhold which can be used to indicate the maximum allowable distance.
		/// </summary>
		/// <param name="source">An array of the code points of the first string</param>
		/// <param name="target">An array of the code points of the second string</param>
		/// <param name="threshold">Maximum allowable distance</param>
		/// <returns>Int.MaxValue if threshhold exceeded; otherwise the Damerau-Leveshteim distance between the strings</returns>
		private static int DamerauLevenshteinDistance(int[] source, int[] target, int threshold)
		{

			int length1 = source.Length;
			int length2 = target.Length;

			// Return trivial case - difference in string lengths exceeds threshhold
			if(Math.Abs(length1 - length2) > threshold)
			{ return int.MaxValue; }

			// Ensure arrays [i] / length1 use shorter length 
			if(length1 > length2)
			{
				Swap(ref target, ref source);
				Swap(ref length1, ref length2);
			}

			int maxi = length1;
			int maxj = length2;

			int[] dCurrent = new int[maxi + 1];
			int[] dMinus1 = new int[maxi + 1];
			int[] dMinus2 = new int[maxi + 1];
			int[] dSwap;

			for(int i = 0; i <= maxi; i++)
			{ dCurrent[i] = i; }

			int jm1 = 0, im1 = 0, im2 = -1;

			for(int j = 1; j <= maxj; j++)
			{

				// Rotate
				dSwap = dMinus2;
				dMinus2 = dMinus1;
				dMinus1 = dCurrent;
				dCurrent = dSwap;

				// Initialize
				int minDistance = int.MaxValue;
				dCurrent[0] = j;
				im1 = 0;
				im2 = -1;

				for(int i = 1; i <= maxi; i++)
				{

					int cost = source[im1] == target[jm1] ? 0 : 1;

					int del = dCurrent[im1] + 1;
					int ins = dMinus1[i] + 1;
					int sub = dMinus1[im1] + cost;

					//Fastest execution for min value of 3 integers
					int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

					if(i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
						min = Math.Min(min, dMinus2[im2] + cost);

					dCurrent[i] = min;
					if(min < minDistance)
					{ minDistance = min; }
					im1++;
					im2++;
				}
				jm1++;
				if(minDistance > threshold)
				{ return int.MaxValue; }
			}

			int result = dCurrent[maxi];
			return (result > threshold) ? int.MaxValue : result;
		}
		static void Swap<T>(ref T arg1, ref T arg2)
		{
			T temp = arg1;
			arg1 = arg2;
			arg2 = temp;
		}
	}
}
