namespace JxBackendService.Common.Util
{
    public static class RequiredUtil
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
                        if (string.IsNullOrEmpty(obj.ToTrimString()))
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