using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extensions
{
	public interface IWeight
	{
		float Weight { get; }
	}
	public static class Array
	{
		/// <summary>
		/// Swap two elements in array
		/// </summary>
		public static void Swap<T>(this IList<T> array, int a, int b)
		{
			T x = array[a];
			array[a] = array[b];
			array[b] = x;
		}

		public static T GetPrevious<T>(this IList<T> array, int index, bool loop = false)
		{
			if (array.IsNull())
				throw new Exception("GetPrevious() Error");
			index--;
			if (index < 0)
				if (loop)
					index = array.Count - 1;
				else
					index = 0;
			return array[index];
		}

		public static T GetIndex<T>(this IList<T> array, int index, bool loop = false)
		{
			if (array.IsNull()) throw new Exception("GetIndex() Error");
			if (index < 0)
				if (loop)
					index = array.Count - 1;
				else
					index = 0;
			return array[index];
		}

		public static T GetNext<T>(this IList<T> array, int index, bool loop = false)
		{
			if (array.IsNull()) throw new Exception("GetNext() Error");
			index++;
			if (index >= array.Count)
				if (loop)
					index = 0;
				else
					index = array.Count - 1;
			return array[index];
		}

		/// <summary>
		/// (WARNING: Heavy) Check if all items are the same between list (even if scrambled)
		/// </summary>
		/// <typeparam name="T">List type</typeparam>
		/// <param name="list1"></param>
		/// <param name="list2"></param>
		/// <returns>if equals</returns>
		public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
		{
			var cnt = new Dictionary<T, int>();
			foreach (T s in list1)
			{
				if (cnt.ContainsKey(s))
					cnt[s]++;
				else
					cnt.Add(s, 1);
			}
			foreach (T s in list2)
			{
				if (cnt.ContainsKey(s))
					cnt[s]--;
				else
					return false;
			}
			return cnt.Values.All(c => c == 0);
		}

		public static bool IsNull<T>(this IList<T> list)
		{
			if (list == null || list.Count == 0) return true;
			return false;
		}

		public static T RandomItem<T>(this IList<T> list, int maxIndex = -1)
		{
			if (list.IsNull()) throw new Exception("RandomItem() Error");
			if (maxIndex < 0) return list[Random.Range(0, list.Count)];
			return list[Random.Range(0, System.Math.Min(maxIndex, list.Count))];
		}

		/// <summary>
		/// Random based on chance percent [IChance]
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static T RandomProportionalSelection<T>(this IList<T> items)
			where T : IWeight
		{
			// Calculate the summa of all portions.
			float poolSize = 0;
			for (int i = 0; i < items.Count; i++)
			{
				poolSize += items[i].Weight;
			}

			float randomNumber = Random.Range(0f, poolSize);

			// Detect the item, which corresponds to current random number.
			float accumulatedProbability = 0;
			for (int i = 0; i < items.Count; i++)
			{
				accumulatedProbability += items[i].Weight;
				if (randomNumber <= accumulatedProbability)
					return items[i];
			}
			return default;
		}

		public static IList<T> RandomItems<T>(this IList<T> list, int count, bool different = false)
		{
			if (list.IsNull() || (count > list.Count && different)) throw new Exception("RandomItems() Error");
			if (count == list.Count && different) return list;
			IList<T> items = new List<T>(count);
			while(items.Count < count)
			{
				T item = list.RandomItem();
				if (items.Contains(item) && different)
					continue;
				items.Add(item);
			}
			return items;
		}

        private static System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<T> Flatten<T>(this T e) where T : IEnumerable<T> =>
			e.SelectMany(c => c.Flatten()).Concat(e);
		
		        public static T[,] ResizeArray<T>(this T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = System.Math.Min(rows, original.GetLength(0));
            int minCols = System.Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
            for (int j = 0; j < minCols; j++)
                newArray[i, j] = original[i, j];
            return newArray;
        }

        public static T[] To1DArray<T>(this T[,] input)
        {
            // Step 1: get total size of 2D array, and allocate 1D array.
            int size = input.Length;
            T[] result = new T[size];
            int width = input.GetLength(0);
            int height = input.GetLength(1);

            // Step 2: copy 2D array elements into a 1D array.
            int write = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[write++] = input[x, y];
                }
            }
            // Step 3: return the new array.
            return result;
        }
        
        public static T[] ToInverse1DArray<T>(this T[,] input)
        {
            // Step 1: get total size of 2D array, and allocate 1D array.
            int size = input.Length;
            T[] result = new T[size];
            int width = input.GetLength(0);
            int height = input.GetLength(1);

            // Step 2: copy 2D array elements into a 1D array.
            int write = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    result[write++] = input[x, height-1-y];
                }
            }
            // Step 3: return the new array.
            return result;
        }

        public static T[,] To2DArray<T>(this T[] input, int height, int width)
        {
            if(input.Length == 0) return new T[,]{};
            T[,] output = new T[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    output[x, y] = input[y * width + x];
                }
            }

            return output;
        }
        
        public static T[,] InverseTo2DArray<T>(this T[] input, int height, int width)
        {
            if(input.Length == 0) return new T[,]{};
            T[,] output = new T[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    output[x, y] = input[(height-1-y) * width + x];
                }
            }

            return output;
        }

        public static List<Vector2Int> CellsInRadius(Vector2Int origin, float radius, Vector2Int canvasSize)
        {
            var ceiledRadius = Mathf.CeilToInt(radius);
            var index = new Vector2Int();
            var output = new List<Vector2Int>();
            for (int y = -ceiledRadius; y <= ceiledRadius; y++)
            {
                for (int x = -ceiledRadius; x <= ceiledRadius; x++)
                {
                    index.x = origin.x - x;
                    index.y = origin.y - y;

                    if (index.x > -1 && index.y > -1 &&
                        index.x < canvasSize.x && index.y < canvasSize.y &&
                        Vector2.Distance(index, origin) < radius)
                    {
                        output.Add(index);
                    }
                }
            }

            return output;
        }
        
        public static void FloodFill<T>(ref T[,] array, Vector2Int startIndex, T replacementValue)
        {
            var targetValue = array[startIndex.x, startIndex.y];
            if (targetValue.Equals(replacementValue))
            {
                return;
            }
 
            Stack<Vector2Int> targets = new Stack<Vector2Int>();
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            targets.Push(startIndex);
            while (targets.Count != 0)
            {
                var temp = targets.Pop();
                int y1 = temp.y;
                while (y1 >= 0 && array[temp.x, y1].Equals(targetValue))
                {
                    y1--;
                }
                y1++;
                bool spanLeft = false;
                bool spanRight = false;
                while (y1 < height && array[temp.x, y1].Equals(targetValue))
                {
                    array[temp.x, y1] = replacementValue;
 
                    if (!spanLeft && temp.x > 0 && array[temp.x - 1, y1].Equals(targetValue))
                    {
                        targets.Push(new Vector2Int(temp.x - 1, y1));
                        spanLeft = true;
                    }
                    else if(spanLeft && temp.x - 1 == 0 && !array[temp.x - 1, y1].Equals(targetValue))
                    {
                        spanLeft = false;
                    }
                    if (!spanRight && temp.x < width - 1 && array[temp.x + 1, y1].Equals(targetValue))
                    {
                        targets.Push(new Vector2Int(temp.x + 1, y1));
                        spanRight = true;
                    }
                    else if (spanRight && temp.x < width - 1 && !array[temp.x + 1, y1].Equals(targetValue))
                    {
                        spanRight = false;
                    } 
                    y1++;
                }
            }
        }
	}
}