using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}