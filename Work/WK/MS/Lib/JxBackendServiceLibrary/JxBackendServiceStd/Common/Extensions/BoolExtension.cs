using JxBackendService.Resource.Element;

namespace JxBackendService.Common.Extensions
{
    public static class BoolExtension
    {
        public static string GetActionText(this bool isActive)
        {
            if (isActive)
            {
                return CommonElement.Open;
            }

            return CommonElement.CloseDown;
        }

        public static string GetUserIsActiveText(this bool isActive)
        {
            if (isActive)
            {
                return CommonElement.Activity;
            }

            return UserRelatedElement.Freeze;
        }

        public static string GetUserIsOnlineText(this bool isOnline)
        {
            if (isOnline)
            {
                return UserRelatedElement.Online;
            }

            return UserRelatedElement.Offline;
        }

        public static string GetYesNoText(this bool isYes)
        {
            if (isYes)
            {
                return CommonElement.Yes;
            }

            return CommonElement.No;
        }
    }
}