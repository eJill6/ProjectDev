using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.User.Online
{
    public class UserOnlineService : IUserOnlineService
    {
        private readonly IJxCacheService _cacheService;

        public UserOnlineService(JxApplication jxApplication)
        {
            _cacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(jxApplication);
        }

        private List<UserOnlineDetail> GetUserOnlineDetails(int userId)
        {
            bool isSliding = true;
            List<UserOnlineDetail> userOnlineDetails = _cacheService.GetCache(
                CacheKey.GetUserOnlineDetails(userId),
                isSliding,
                () => new List<UserOnlineDetail>());

            return userOnlineDetails;
        }

        public bool IsExpired(int userId, string loginKey, out string expiredMessage)
        {
            List<UserOnlineDetail> userOnlineDetails = GetUserOnlineDetails(userId);

            if (!userOnlineDetails.AnyAndNotNull())
            {
                expiredMessage = MessageElement.LoginIsExpiredPleaseloginAgain;
                return true;
            }

            UserOnlineDetail userOnlineDetail = userOnlineDetails.Where(w => w.LoginKey == loginKey).SingleOrDefault();
            expiredMessage = null;

            if (userOnlineDetail != null)
            {
                userOnlineDetail.LastUpdateTime = DateTime.Now;
                SaveToCache(userId, userOnlineDetails);

                if (userOnlineDetail.UserOnlineStatus == UserOnlineStatuses.Online)
                {
                    return false;
                }
                else if (userOnlineDetail.UserOnlineStatus == UserOnlineStatuses.Expired)
                {
                    expiredMessage = userOnlineDetail.ExpiredMessage;
                }
            }

            if (expiredMessage.IsNullOrEmpty())
            {
                expiredMessage = MessageElement.LoginIsExpiredPleaseloginAgain;
            }

            return true;
        }

        public void Add(int userId, string loginKey)
        {
            List<UserOnlineDetail> userOnlineDetails = GetUserOnlineDetails(userId);

            if (userOnlineDetails == null)
            {
                userOnlineDetails = new List<UserOnlineDetail>();
            }

            var userOnlineDetail = userOnlineDetails.Where(w => w.LoginKey == loginKey).SingleOrDefault();

            if (userOnlineDetail == null)
            {
                userOnlineDetail = new UserOnlineDetail()
                {
                    LoginKey = loginKey
                };

                userOnlineDetails.Add(userOnlineDetail);
            }

            userOnlineDetail.LastUpdateTime = DateTime.Now;

            //移除用戶詳細資料已經不存在的資料的資料
            var removeKeys = new List<string>();

            userOnlineDetails.Where(w => DateTime.Now.Subtract(w.LastUpdateTime).TotalHours > 1).ToList().ForEach(f =>
            {
                if (_cacheService.GetCache<object>(CacheKey.GetBackSideUserInfoKey(f.LoginKey)) == null)
                {
                    removeKeys.Add(f.LoginKey);
                }
            });

            if (removeKeys.Any())
            {
                userOnlineDetails.RemoveAll(r => removeKeys.Contains(r.LoginKey));
            }

            SaveToCache(userId, userOnlineDetails);
        }

        private void SaveToCache(int userId, List<UserOnlineDetail> userOnlineDetails)
        {
            _cacheService.SetCache(new SetCacheParam()
            {
                Key = CacheKey.GetUserOnlineDetails(userId),
                IsSlidingExpiration = true,
            }, userOnlineDetails);
        }

        /// <summary>
        /// 讓用戶過期, 為了讓被踢出的用戶知道原因, 所以不刪除資料只改狀態
        /// </summary>       
        public void ExpireUser(int userId, string expiredMessage, string excludeLoginKey = null)
        {
            List<UserOnlineDetail> userOnlineDetails = GetUserOnlineDetails(userId);

            if (!userOnlineDetails.AnyAndNotNull())
            {
                return;
            }

            userOnlineDetails.ForEach(f =>
            {
                bool isSkip = false;
                if (!string.IsNullOrEmpty(excludeLoginKey) && f.LoginKey == excludeLoginKey)
                {
                    isSkip = true;
                }

                if (!isSkip)
                {
                    f.UserOnlineStatus = UserOnlineStatuses.Expired;
                    f.ExpiredMessage = expiredMessage;
                }
            });

            SaveToCache(userId, userOnlineDetails);
        }

        /// <summary>
        /// 刪除用戶狀態資料
        /// </summary>       
        public void RemoveUser(int userId, string loginKey)
        {
            List<UserOnlineDetail> userOnlineDetails = GetUserOnlineDetails(userId);

            if (!userOnlineDetails.AnyAndNotNull())
            {
                return;
            }

            int affectRows = userOnlineDetails.RemoveAll(r => r.LoginKey == loginKey);

            if (affectRows > 0)
            {
                SaveToCache(userId, userOnlineDetails);
            }
        }

        //private void Remove(string userName)
        //{
        //    if (!UserNames.ContainsKey(userName))
        //    {
        //        return;
        //    }

        //    UserNames.Remove(userName);
        //}

        //public void Remove(string userName, string loginKey)
        //{
        //    if (!UserNames.ContainsKey(userName))
        //    {
        //        return;
        //    }

        //    UserNames[userName].RemoveAll(r => r.LoginKey == loginKey);

        //    if (!UserNames[userName].Any())
        //    {
        //        Remove(userName);
        //    }
        //}

    }

    public class UserOnlineDetail
    {
        public string LoginKey { get; set; }
        public UserOnlineStatuses UserOnlineStatus { get; set; } = UserOnlineStatuses.Online;
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 由外部指定過期原因
        /// </summary>
        public string ExpiredMessage { get; set; }
    }

    public enum UserOnlineStatuses
    {
        Online,
        Expired
    }
}
