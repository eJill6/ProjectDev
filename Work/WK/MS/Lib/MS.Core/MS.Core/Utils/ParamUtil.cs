namespace MS.Core.Utils
{
    public static class ParamUtil
    {
        public static bool IsValidRequired(params object[] list)
        {
            bool isValid = true;

            if (list != null)
            {
                foreach (object obj in list)
                {
                    if (obj is string)
                    {
                        if (string.IsNullOrEmpty(ObjUtil.ToTrimString(obj)))
                        {
                            isValid = false;
                            break;
                        }
                    }
                    else
                    {
                        if (obj == null)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }

            return isValid;
        }
    }
}
