using System.Linq;

namespace LeetCodeSolution.Solution
{
    public class No739_DailyTemperatures : BaseSolution
    {
        public override object GetResult()
        {
            int[] input = { 73, 74, 75, 71, 69, 72, 76, 73 };
            return new Solution().DailyTemperatures(input);
        }

        public class Solution
        {
            public int[] DailyTemperatures(int[] temperatures)
            {
                int index = 0;
                int first = 0;
                int total = temperatures.Count();
                //List<int> result = new List<int>();
                int[] vs = new int[total];

                while (index < total)
                {
                    first = temperatures[index];
                    int tmp = 0;

                    for (int i = index + 1; i < total; i++)
                    {
                        if (temperatures[i] > first)
                        {
                            tmp = i - index;
                            break;
                        }
                    }
                    vs[index] = tmp;
                    index++;
                }

                return vs;

                //return result.ToArray();
            }
        }
    }
}