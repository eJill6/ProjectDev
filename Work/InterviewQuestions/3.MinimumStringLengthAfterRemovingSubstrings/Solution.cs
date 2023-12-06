using System.Linq;

namespace MinimumStringLengthAfterRemovingSubstrings
{
    public static class Solution
    {
        public static int MinLength(string s)
        {
            var matches = new List<string> { "AB", "CD" };

            while (true)
            {
                if (!matches.Any(keyword => s.Contains(keyword)))
                {
                    break;
                }

                foreach (string match in matches)
                {
                    s = s.Replace(match, string.Empty);
                }
            }

            return s.Length;
        }
    }
}