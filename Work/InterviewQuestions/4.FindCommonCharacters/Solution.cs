namespace FindCommonCharacters
{
    public static class Solution
    {
        public static IList<string> CommonChars(string[] words)
        {
            string first = words[0];
            var result = new List<string>();

            for (int i = 0; i < first.Length; i++)
            {
                char ch = first[i];

                int check = 0;

                for (int j = 1; j < words.Length; j++)
                {
                    string wd = words[j];

                    if (wd.Contains(ch))
                    {
                        check++;

                        int count = wd.IndexOf(ch);

                        if (count > -1)
                        {
                            words[j] = wd.Remove(count, 1);
                        }
                    }

                    if (check == words.Length - 1)
                    {
                        result.Add(ch.ToString());
                    }
                }
            }

            return result;
        }
    }
}