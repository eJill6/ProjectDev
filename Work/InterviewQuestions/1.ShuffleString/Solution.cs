namespace ShuffleString
{
    public static class Solution
    {
        public static string RestoreString(string s, int[] indices)
        {
            int length = s.Length;
            char[] array = new char[length];

            for (int i = 0; i < indices.Length; i++)
            {
                int num = indices[i];
                array[num] = s[i];
            }

            return new string(array);
        }
    }
}