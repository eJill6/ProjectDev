namespace MS.Core.Utils
{
    public static class ObjUtil
    {
        public static string ToTrimString(object obj)
        {
            return ToNonNullString(obj).Trim();
        }

        public static string ToNonNullString(object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
