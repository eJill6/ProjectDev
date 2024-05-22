using AgDataBase.Common;
using AgDataBase.DLL;
using AgDataBase.Model;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgDataBase.BLL
{
    /// <summary>
    /// 手工同步用户余额
    /// </summary>
    public class UserBll : BaseService
    {
        private static readonly Lazy<IAgApi> _agApi = new Lazy<IAgApi>(() => DependencyUtil.ResolveService<IAgApi>());

        private readonly ITPGameAccountReadService _tpGameAccountReadService;

        public UserBll(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _tpGameAccountReadService = DependencyUtil.ResolveJxBackendService<ITPGameAccountReadService>(
                Merchant,
                EnvLoginUser,
                DbConnectionTypes.Slave);
        }

        public void RepaireNegativeAvailableScores()
        {
            List<AgUserInfo> userInfos = null;

            try
            {
                userInfos = UserDal.GetNegativeAvailableScoreUsers();
            }
            catch (Exception ex)
            {
                LogsManager.Info("获取负积分用户失败，详细信息：" + ex.Message);
                return;
            }

            LogsManager.Info("获取到 " + userInfos.Count.ToString() + " 个负积分用户");

            foreach (AgUserInfo userInfo in userInfos)
            {
                string tpGameAccount = _tpGameAccountReadService.GetTPGameAccountByRule(PlatformProduct.AG, userInfo.UserID);
                decimal availableScores = _agApi.Value.GetBalance(tpGameAccount, userInfo.UserID.ToString());

                if (availableScores != -9999)
                {
                    userInfo.AvailableScores = availableScores;

                    try
                    {
                        if (UserDal.UpdateAvailableScores(userInfo))
                        {
                            LogsManager.Info("更新负积分用户 " + userInfo.UserName + " 余额为 " + availableScores.ToString());
                        }
                        else
                        {
                            LogsManager.Info("更新负积分用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogsManager.Info("更新负积分用户 " + userInfo.UserName + " 余额失败，详细信息：" + ex.Message);
                    }
                }
                else
                {
                    LogsManager.Info("更新负积分用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                }

                Thread.Sleep(1000);
            }

            LogsManager.Info("更新负积分用户结束");
        }

        public void RefreshAvailableScores()
        {
            List<AgUserInfo> userInfos = null;

            try
            {
                userInfos = UserDal.GetUserInfo();
            }
            catch (Exception ex)
            {
                LogsManager.Info("获取待更新用户失败，详细信息：" + ex.Message);

                return;
            }

            LogsManager.Info("获取到 " + userInfos.Count.ToString() + " 个待更新用户");

            foreach (AgUserInfo userInfo in userInfos)
            {
                string tpGameAccount = _tpGameAccountReadService.GetTPGameAccountByRule(PlatformProduct.AG, userInfo.UserID);
                decimal availableScores = _agApi.Value.GetBalance(tpGameAccount, userInfo.UserID.ToString());

                if (availableScores != -9999)
                {
                    userInfo.AvailableScores = availableScores;

                    try
                    {
                        if (UserDal.UpdateAvailableScores(userInfo))
                        {
                            LogsManager.Info("更新用户 " + userInfo.UserName + " 余额为 " + availableScores.ToString());
                        }
                        else
                        {
                            LogsManager.Info("更新用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogsManager.Info("更新用户 " + userInfo.UserName + " 余额失败，详细信息：" + ex.Message);
                    }
                }
                else
                {
                    LogsManager.Info("获取用户 " + userInfo.UserName + " 余额失败，未能成功更新余额");
                }

                Thread.Sleep(1000);
            }

            LogsManager.Info("更新用户余额完毕");
        }
    }
}