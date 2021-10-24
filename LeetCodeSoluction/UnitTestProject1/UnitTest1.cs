using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            System.Diagnostics.Debug.WriteLine(OrderChars("ab", "ab").ToArray());
            Turn a ;

        }

        public enum Turn
        {
            L = 1,
            R
        }

        public void aa()
        {
            int i = 1;
        }

        private IEnumerable<char> OrderChars(params string[] strings)
        {
            return strings.SelectMany(str => str.ToCharArray()).Where(chr => Char.IsLetter(chr)).OrderBy(chr => chr);
        }
    }

    public class Problem
    {
        public static void Main(string[] args)
        {
            var weights = new[] { 1, 5 };
            var total = 8;

            // 4
            Console.WriteLine(GetMinWeightsCount(weights, total));
        }

        public static int GetMinWeightsCount(int[] weights, int total)
        {
            int tempNum = total;
            int totalCount = 0;

            while (true)
            {
                bool isFound = false;

                foreach (int num in weights.OrderByDescending(o=>o))
                {
                    if(tempNum>= num)
                    {
                        tempNum -= num;
                        totalCount++;
                        isFound = true;
                        break;
                    }
                }

                if (tempNum == 0)
                {
                    return totalCount;
                }

                if (!isFound)
                {
                    return -1;
                }
            }
        }


    }

    public class MaxSum
    {
        public static int FindMaxSum(List<int> list)
        {
            int? maxVal = null;
            int? secondMaxVal = null;

            foreach (int num in list)
            {
                if (!maxVal.HasValue)
                {
                    maxVal = num;
                }
                else
                {
                    if (maxVal.Value < num)
                    {
                        secondMaxVal = maxVal;
                        maxVal = num;
                    }
                    else if (!secondMaxVal.HasValue)
                    {
                        secondMaxVal = num;
                    }
                    else if (secondMaxVal.Value < num)
                    {
                        secondMaxVal = num;
                    }
                }
            }

            return maxVal.GetValueOrDefault() + secondMaxVal.GetValueOrDefault();
        }

        public static void Main(string[] args)
        {
            List<int> list = new List<int> { 5, 9, 7, 11 };
            Console.WriteLine(FindMaxSum(list));
        }
    }
}
