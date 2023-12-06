namespace ShuffleString
{
    public static class Solution
    {
        public static int[] Shuffle(int[] nums, int n)
        {
            int first = 0;
            int second = n;

            int[] results = new int[nums.Length];

            for (int i = 0; i < nums.Length; i += 2)
            {
                results[i] = nums[first];
                results[i + 1] = nums[second];
                first++;
                second++;
            }

            return results;
        }
    }
}