using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.User;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Menu;
using Newtonsoft.Json;
using RestSharp;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Helpers;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using SLPolyGame.Web.MSSeal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BLL = SLPolyGame.Web.BLL;

using Model = SLPolyGame.Web.Model;

namespace WebApiImpl
{
    public abstract class BaseSLPolyGameService : BaseWebApiService, ISLPolyGameWebSVService
    {
        private readonly Lazy<BLL.PalyInfo> _playInfoService;

        private readonly Lazy<BLL.SysSettings> _sysSettings;

        public BaseSLPolyGameService()
        {
            _playInfoService = DependencyUtil.ResolveService<BLL.PalyInfo>();
            _sysSettings = DependencyUtil.ResolveService<BLL.SysSettings>();
        }

        public async Task<string> CancelOrder(PalyInfo model)
        {
            if (EnvLoginUser.LoginUser.UserKey.IsNullOrEmpty())
            {
                return "002";
            }

            model.UserID = EnvLoginUser.LoginUser.UserId;

            try
            {
                return await Task.FromResult(_playInfoService.Value.CancelOrder(model));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"撤单时异常:{ex.Message}, ex:{ex}");

                return await Task.FromResult("002");
            }
        }

        public async Task<CursorPagination<CurrentLotteryInfo>> GetCursorPaginationDrawResult(int lotteryId, DateTime start, DateTime end, int count, string cursor)
        {
            try
            {
                var cli = new BLL.CurrentLotteryInfo();

                return await Task.FromResult(cli.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetCursorPaginationDrawResult异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<CursorPagination<PalyInfo>> GetFollowBet(string palyId, int lottertId)
        {
            try
            {
                return await Task.FromResult(_playInfoService.Value.GetFollowBet(palyId, lottertId));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetFollowBet异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<List<MenuInnerInfo>> GetMenuInnerInfos()
        {
            var frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave).Value;

            return await Task.FromResult(frontsideMenuService.GetMenuInnerInfos());
        }

        public async Task<WebGameCenterViewModel> GetWebGameCenterViewModel()
        {
            var frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave).Value;

            return await Task.FromResult(frontsideMenuService.GetWebGameCenterViewModel());
        }

        public async Task<List<string>> GetLatestWinningList(string period)
        {
            try
            {
                return await Task.FromResult(_playInfoService.Value.GetLatestWinningList(period));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetBateList异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<List<WinningListItem>> GetLatestWinningListItems(string period)
        {
            try
            {
                return await Task.FromResult(_playInfoService.Value.GetLatestWinningListItems(period));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetBateList异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<PalyInfo> GetPalyIDPalyBet(string value)
        {
            if (EnvLoginUser.LoginUser.UserKey.IsNullOrEmpty())
            {
                return null;
            }

            int userId = EnvLoginUser.LoginUser.UserId;

            try
            {
                int playID = int.Parse(value);
                return await Task.FromResult(_playInfoService.Value.GetPalyIDPalyBet(playID, userId));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetPalyIDPalyBet异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<PalyInfo> GetPlayBetByAnonymous(string playId)
        {
            try
            {
                return await Task.FromResult(_playInfoService.Value.GetPlayBetByAnonymous(playId));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetPlayBetByAnonymous异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<PalyInfo[]> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            try
            {
                return await Task.FromResult(_playInfoService.Value.GetPlayBetsByAnonymous(startTime, endTime, gameId));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetPlayBetsByAnonymous异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<DateTime> GetServerCurrentTime()
        {
            return await Task.FromResult(DateTime.Now);
        }

        public async Task<CursorPaginationTotalData<PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime searchDate, string cursor, int pageSize, string roomId)
        {
            try
            {
                return await _playInfoService.Value.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize, roomId);
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetSpecifyOrderList异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<SysSettings> GetSysSettings()
        {
            try
            {
                return await Task.FromResult(_sysSettings.Value.GetSysSettings());
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"获取系统配置时异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<Model.UserInfo> GetUserInfo()
        {
            if (EnvLoginUser.LoginUser.UserKey.IsNullOrEmpty())
            {
                return null;
            }

            int userId = EnvLoginUser.LoginUser.UserId;

            try
            {
                var us = new BLL.UserInfo();
                var user = us.GetModel(userId);

                var client = new RestClient(baseUrl: $"{GlobalCache.MSSealAddress}");
                var requestModel = new BalanceRequest()
                {
                    UserId = userId,
                    Salt = GlobalCache.MSSealSalt
                };

                var request = requestModel.GetRequest();
                var rawResult = client.Execute(request);
                if (rawResult.IsSuccessful)
                {
                    var result = new BalanceResult();
                    try
                    {
                        result = JsonConvert.DeserializeObject<BalanceResult>(rawResult.Content);

                        if (result?.Data != null
                            && !string.IsNullOrWhiteSpace(result?.Data?.Balance)
                            && result.Success)
                        {
                            user.Available = Convert.ToDecimal(result.Data.Balance);
                        }
                        else
                        {
                            LogUtilService.Error($"获取用户信息时异常 By Api: " +
                                $"Msg: {result?.Msg ?? string.Empty}, " +
                                $"Request:{JsonConvert.SerializeObject(requestModel)}, " +
                                $"Content: {rawResult.Content}, Result: {JsonConvert.SerializeObject(result)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtilService.Error($"获取用户信息时异常 By Api: " +
                            $"ErrorMessage: {rawResult.ErrorMessage}, " +
                            $"Request:{JsonConvert.SerializeObject(requestModel)}, Content: {rawResult.Content}, " +
                            $"Result: {JsonConvert.SerializeObject(result)}, ex:{ex}");
                    }
                }
                else
                {
                    LogUtilService.Error($"获取用户信息时异常 By Api: " +
                        $"ErrorMessage: {rawResult.ErrorMessage}, " +
                        $"Request:{JsonConvert.SerializeObject(requestModel)}, " +
                        $"Content: {rawResult.Content}");
                }

                return await Task.FromResult(user);
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"获取用户信息时异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<Model.UserInfo> GetUserInfoByUserID(int UserID)
        {
            try
            {
                var us = new BLL.UserInfo();

                return await Task.FromResult(us.GetModel(UserID));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetUserInfoByUserID时异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        /// <summary> 下单 </summary>

        public async Task<Model.PalyInfo> InsertPlayInfo(Model.PalyInfo model)
        {
            Model.UserInfoToken user = GetUserInfoToken();

            if (user == null)
            {
                return null;
            }
            else
            {
                model.UserID = user.UserId;
                model.UserName = user.UserName;
            }

            try
            {
                return await Task.FromResult(_playInfoService.Value.InsertPlayInfo(model));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"下单时异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }

        public async Task<bool> IsFrontsideMenuActive(FrontSideMainMenu frontSideMainMenu)
        {
            //遊戲開關要以主選單為主, 因為主選單關閉後，所屬的熱門遊戲會在前台隱藏，此時熱門遊戲有可能是active，會造成誤判
            var frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave).Value;

            IEnumerable<FrontsideMenu> frontsideMenus = frontsideMenuService.GetActiveFrontsideMenus()
                .Where(w =>
                    w.ProductCode == frontSideMainMenu.ProductCode &&
                    w.Type != FrontsideMenuTypeSetting.Hot);

            //大廳類無法從remote code回推GameCode(會有多筆), 所以判斷只要同product下有任一開啟，就當做開啟
            if (!frontSideMainMenu.GameCode.IsNullOrEmpty())
            {
                frontsideMenus = frontsideMenus.Where(w => w.GameCode.ToNonNullString() == frontSideMainMenu.GameCode.ToNonNullString());
            }

            if (!frontsideMenus.Any())
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }

        public async Task<MessageEntity<UserAuthInformation>> ValidateLogin(LoginRequestParam param)
        {
            try
            {
                var cacheBase = DependencyUtil.ResolveService<ICacheBase>().Value;
                string userKey = CreateUserKey(param);

                if (param.IsSlidingExpiration)
                {
                    UserAuthInformation cacheUserAuth = cacheBase.GetCache(userKey).Deserialize<UserAuthInformation>();

                    if (cacheUserAuth != null)
                    {
                        return await Task.FromResult(CreateSuccessResult(cacheUserAuth));
                    }
                }

                bool isUserExists = JxCacheService.GetCache(
                      CacheKey.UserExists(param.UserId),
                      () =>
                      {
                          var userInfoRelatedReadService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(
                              new EnvironmentUser
                              {
                                  Application = Application,
                                  LoginUser = new BasicUserInfo { }
                              },
                              DbConnectionTypes.Slave).Value;

                          var userInfo = userInfoRelatedReadService.GetUserInfo(param.UserId);

                          if (userInfo != null)
                          {
                              return string.Empty;
                          }

                          return null; //不寫入cache
                      }) != null;

                if (!isUserExists)
                {
                    int dbReturnCode = JxCacheService.DoWorkWithRemoteLock(
                        CacheKey.UserInfoLock(param.UserId),
                        () =>
                        {
                            var user = new BLL.UserInfo();

                            return user.Add(new Model.UserInfo()
                            {
                                UserId = param.UserId,
                                UserName = param.UserName,
                                RebatePro = 0.077M
                            });
                        });

                    if (new int[] { 2, 3 }.Contains(dbReturnCode) == false)
                    {
                        return await Task.FromResult(CreateFailResult(dbReturnCode));
                    };
                }

                // 存在或是創建成功
                var logonMode = LogonMode.GetSingle(param.LogonMode);

                if (logonMode == null)
                {
                    logonMode = LogonMode.Native;
                }

                var userAuthInformation = new UserAuthInformation()
                {
                    DepositUrl = param.DepositUrl,
                    GameID = param.GameID,
                    RoomNo = param.RoomNo,
                    UserId = param.UserId,
                    UserName = param.UserName,
                    Key = userKey,
                    LogonMode = logonMode.Value,
                    ExpiredTimestamp = DateTime.UtcNow.AddMinutes(param.UserKeyExpiredMinutes).ToUnixOfTime()
                };

                int cacheSeconds = param.UserKeyExpiredMinutes * 60;

                CacheObj cacheObj = cacheBase.CreateCacheObj(
                    CacheKeyHelper.GetUserTokenKey(userKey),
                    userAuthInformation.ToJsonString(),
                    cacheSeconds,
                    isSliding: param.IsSlidingExpiration);

                JxCacheService.SetCache(new SetCacheParam()
                {
                    Key = CacheKey.GetFrontSideUserInfoKey(userKey),
                    CacheSeconds = cacheSeconds,
                    IsSlidingExpiration = param.IsSlidingExpiration,
                }, cacheObj);

                return await Task.FromResult(CreateSuccessResult(userAuthInformation));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"登录验证时异常:{ex.Message}, ex:{ex}");

                return await Task.FromResult(CreateFailResult(-1));
            }
        }

        public async Task<int> GetOrCreateGeneratorId(string machineName)
        {
            var idGeneratorService = DependencyUtil.ResolveJxBackendService<IIdGeneratorService>(EnvLoginUser, DbConnectionTypes.Master).Value;

            return await Task.FromResult(idGeneratorService.GetOrCreateGeneratorId(machineName));
        }

        private MessageEntity<UserAuthInformation> CreateSuccessResult(UserAuthInformation userAuthInformation)
        {
            return new MessageEntity<UserAuthInformation>()
            {
                Code = 0,
                Msg = "SUCCESS",
                Data = userAuthInformation
            };
        }

        private MessageEntity<UserAuthInformation> CreateFailResult(int code)
        {
            return new MessageEntity<UserAuthInformation>()
            {
                Code = code,
                Msg = string.Empty
            };
        }

        private string CreateUserKey(LoginRequestParam param)
        {
            List<string> hashItems = new List<string>()
            {
                param.UserId.ToString(),
                HttpUtility.UrlEncode(param.UserName),
                param.GameID,
                param.RoomNo,
                HttpUtility.UrlEncode(param.DepositUrl),
                param.LogonMode.ToString()
            };

            if (!param.IsSlidingExpiration)
            {
                hashItems.Add(DateTime.UtcNow.ToUnixOfTime().ToString());
            }

            string hashContent = string.Join("-", hashItems);
            MD5 md5 = MD5.Create();
            byte[] contentBytes = Encoding.UTF8.GetBytes(hashContent);
            string userKey = string.Join("", md5.ComputeHash(contentBytes).Select(x => x.ToString("x")));

            return userKey;
        }

        public async Task<Model.UserInfo> GetUserInfoWithoutAvailable(int userId)
        {
            try
            {
                var us = new BLL.UserInfo();
                var user = us.GetModel(userId);

                return await Task.FromResult(user);
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"获取用户信息时异常:{ex.Message}, ex:{ex}");

                return null;
            }
        }
    }
}