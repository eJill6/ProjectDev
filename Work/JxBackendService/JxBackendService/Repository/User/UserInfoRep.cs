using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Model.db;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.User;
using JxBackendService.Repository.Base;
using JxBackendService.Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JxBackendService.Repository.User
{
    public class UserInfoRep : BaseDbRepository<UserInfo>, IUserInfoRep
    {
        private static readonly string UpdatePasswordSuccessMsg = "密码修改成功"; //相容舊SP回傳值
        private static readonly string LowMoneyInSuccessMsg = "本次转账成功完成"; //相容舊SP回傳值
        private static readonly int SetNicknameSuccessId = 0; //保存成功
        //private static readonly int RegisterSuccessId = 2; // 添加成功

        public UserInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType) { }

        public int? GetFrontSideUserId(string userName)
        {
            string sql = GetAllQuerySQL(new GetAllQuerySQLParam()
            {
                TopRow = 1,
                DbType = InlodbType.Inlodb,
                ColumnNameFilters = new List<string> { nameof(UserInfo.UserID) }
            }) + "WHERE UserName = @userName AND IsPlay = 1 ORDER BY IsActive DESC, UserID";

            return DbHelper.QuerySingleOrDefault<int?>(sql, new { userName = userName.ToNVarchar(50) });
        }

        public string GetFrontSideUserName(int userId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.UserName) }) + "WHERE UserID = @userId AND IsPlay = 1 ";
            return DbHelper.QuerySingleOrDefault<string>(sql, new { userId });
        }

        public int? GetBackSideUserId(string userName)
        {
            return BaseGetUserId(userName, false);
        }

        public string GetBackSideUserName(int userId)
        {
            return BaseGetUserName(userId, false);
        }

        private int? BaseGetUserId(string userName, bool isPlay)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.UserID) }) + "WHERE UserName = @userName AND IsPlay = @isPlay ";
            return DbHelper.QueryFirstOrDefault<int?>(sql, new { userName = userName.ToNVarchar(50), isPlay });
        }

        private string BaseGetUserName(int userId, bool isPlay)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.UserName) }) + "WHERE UserID = @userId AND IsPlay = @isPlay ";
            return DbHelper.QuerySingleOrDefault<string>(sql, new { userId, isPlay });
        }

        public string GetUserName(int userId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.UserName) }) + "WHERE UserID = @userId ";
            return DbHelper.QuerySingleOrDefault<string>(sql, new { userId });
        }

        public List<string> GetUserNames(List<int> userIds)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.UserName) }) + $"WHERE {nameof(UserInfo.UserID)} IN @userIds ";
            return DbHelper.QueryList<string>(sql, new { userIds});
        }

        public string GetUserBindPhoneNumber(int userId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.PhoneNumber) }) + "WHERE UserID = @userId AND IsPhoneRotection = 1 ";
            return DbHelper.QuerySingleOrDefault<string>(sql, new { userId });
        }

        public string GetUserEmail(int userId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.Email) }) + "WHERE UserID = @userId ";
            return DbHelper.QuerySingleOrDefault<string>(sql, new { userId });
        }

        public bool IsHasBindPhoneNumber(string encryptPhoneNumber)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, 1, new List<string> { nameof(UserInfo.PhoneNumber) }) +
                "WHERE PhoneNumber = @encryptPhoneNumber AND IsPhoneRotection = 1 ";
            return DbHelper.QueryList<string>(sql, new { encryptPhoneNumber }).Count() > 0;
        }

        public bool IsHasEmail(string encryptEmail)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, 1, new List<string> { nameof(UserInfo.Email) }) + "WHERE Email = @encryptEmail ";
            return DbHelper.QueryList<string>(sql, new { encryptEmail }).Count() > 0;
        }

        /// <summary>
        /// 用戶是否完成初始化設定
        /// 邮箱，密保问题，资金密码，电话号码(非必填)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsInitializeUser(int userId)
        {
            string sql = $@"
SELECT  IIF(COUNT(0) = 0, 1, 0)
{GetFromTableSQL(InlodbType.Inlodb)}
WHERE   UserID = @userId AND ( 
        ISNULL(MoneyPwd, '') = '' OR
        ISNULL(IsPwdRotection, 0) = 0) ";

            return DbHelper.QuerySingleOrDefault<bool>(sql, new { userId });
        }

        public List<UserBankInfo> GetUserBankInfos(int userId)
        {
            string sql = $"EXEC {InlodbType.Inlodb}.dbo.Pro_GetUserBankInfo @UserID = @userId ";
            return DbHelper.QueryList<UserBankInfo>(sql, new { userId });
        }

        public BaseReturnDataModel<string> UpdateUserInfoData(SpUpdateUserInfoDataParam spUpdateUserInfo)
        {
            string sql = $@"
EXEC {InlodbType.Inlodb}.dbo.Pro_UpdateUserInfoData @UserID = @UserID, 
                            @ModifyUserDataType = @ModifyUserDataType,
                            @EncryptContent = @EncryptContent,
                            @RC_Success = @RC_Success,
                            @RC_DataIsExist = @RC_DataIsExist,
                            @RC_DataIsNotCompleted = @RC_DataIsNotCompleted,
                            @RC_UpdateFailed = @RC_UpdateFailed,
                            @RC_UserInitializeIncomplete = @RC_UserInitializeIncomplete,
                            @RC_SystemError = @RC_SystemError ";

            string returnCode = DbHelper.QueryFirst<string>(sql, spUpdateUserInfo);

            return new BaseReturnDataModel<string>(ReturnCode.GetSingle(returnCode));
        }

        /// <summary>
        /// 取得第一層下級
        /// </summary>        
        public List<int> GetAllFirstChild(int parentId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string> { nameof(UserInfo.UserID) }) + "WHERE ParentId = @parentId ";
            return DbHelper.QueryList<int>(sql, new { parentId });
        }

        public UserInfo GetUserInfo(string userName)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE UserName = @UserName ";
            return DbHelper.QueryFirstOrDefault<UserInfo>(sql, new { UserName = userName.ToNVarchar(50) });
        }


        public List<UserInfo> GetAllFirstChildUserInfo(int parentId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE ParentId = @parentId AND IsActive = 1 ";
            return DbHelper.QueryList<UserInfo>(sql, new { parentId });
        }

        public List<UserInfo> GetUserInfos(List<int> userIds)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + "WHERE UserID IN @userIds ";
            return DbHelper.QueryList<UserInfo>(sql, new { userIds });
        }

        public BaseReturnModel UpdatePassword(SavePasswordParam saveMoneyPasswordParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UpdatePwd";
            object param = new
            {
                OldPwd = saveMoneyPasswordParam.OldPasswordHash.ToNVarchar(50),
                NewPwd = saveMoneyPasswordParam.NewPasswordHash.ToNVarchar(50),
                saveMoneyPasswordParam.UserID,
                Type = saveMoneyPasswordParam.SavePasswordType.Value
            };

            string returnMsg = DbHelper.ExecuteScalar<string>(sql, param, CommandType.StoredProcedure);

            if (returnMsg == UpdatePasswordSuccessMsg)
            {
                return new BaseReturnModel(new SuccessMessage(returnMsg));
            }
            else
            {
                return new BaseReturnModel(returnMsg);
            }
        }

        public BaseReturnModel UpdatePasswordByOtherWay(SavePasswordByOtherWayParam param)
        {
            var userInfo = GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = param.UserID });

            if (userInfo == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UpdatePwd";

            object spParam = new
            {
                OldPwd = userInfo.GetPassword(param.SavePasswordType).ToNVarchar(50),
                NewPwd = param.NewPasswordHash.ToNVarchar(50),
                param.UserID,
                Type = param.SavePasswordType.Value
            };

            string returnMsg = DbHelper.ExecuteScalar<string>(sql, spParam, CommandType.StoredProcedure);

            if (returnMsg == UpdatePasswordSuccessMsg)
            {
                return new BaseReturnModel(new SuccessMessage(returnMsg));
            }
            else
            {
                return new BaseReturnModel(returnMsg);
            }
        }

        public bool UpdateUserActive(int userId, bool isActive)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UpdateUserInfoActive";
            return DbHelper.Execute(sql, new { userId, isActive }, CommandType.StoredProcedure) > 0;
        }

        public BaseReturnDataModel<int> LowMoneyIn(TransferToChildParam param)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_LowMoneyIn";
            string returnString = DbHelper.ExecuteScalar<string>(
                 sql,
                 new
                 {
                     Money = param.TransferAmount,
                     CurrentUserID = param.LoginUser.UserId,
                     Username = param.ChildUserName.ToNVarchar(50),
                     Pwd = param.MoneyPasswordHash.ToNVarchar(50)
                 },
                 CommandType.StoredProcedure);

            if (returnString.StartsWith(LowMoneyInSuccessMsg))
            {
                string[] returnData = returnString.Split('|');

                var returnDataModel = new BaseReturnDataModel<int>(ReturnCode.Success, returnData[1].ToInt32())
                {
                    Message = returnData[0]
                };

                return returnDataModel;
            }
            else
            {
                return new BaseReturnDataModel<int>(returnString, 0);
            }
        }

        /// <summary>
        /// 更新用戶暱稱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickname"></param>
        /// <returns></returns>
        public BaseReturnModel SaveUserNickname(int userId, string nickname)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_Member_SetNickname";
            object spParam = new
            {
                UserID = userId,
                Nickname = nickname,
            };

            int returnId = DbHelper.ExecuteScalar<int>(sql, spParam, CommandType.StoredProcedure);

            if (returnId == SetNicknameSuccessId)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel(ReturnCode.DuplicateUserNickname);
        }

        /// <summary>
        /// 查詢用戶暱稱
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserNickname(int userId)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_Member_GetNickname";

            return DbHelper.ExecuteScalar<string>(sql, new { UserID = userId },
                CommandType.StoredProcedure).ToTrimString();
        }

        public BaseReturnModel UpdateBlockChainInfo(BlockChainInfoParam param)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UpdateBlockChainInfo";
            ProUpdateBlockChainInfoReturnModel returnModel = DbHelper.QuerySingle<ProUpdateBlockChainInfoReturnModel>(
                 sql,
                 new
                 {
                     param.LoginUser.UserId,
                     WalletAddr = param.WalletAddr.ToVarchar(160),
                     param.IsActive
                 },
                 CommandType.StoredProcedure);

            if (returnModel.IsSuccess)
            {
                return new BaseReturnModel(new SuccessMessage(returnModel.ReturnMsg));
            }
            else
            {
                return new BaseReturnModel(returnModel.ReturnMsg);
            }
        }

        public BaseReturnModel SaveUserSecurityInfo(SpSaveUserSecurityInfoParam spSaveUserSecurityInfoParam)
        {
            string sql = $@"
EXEC {InlodbType.Inlodb}.dbo.Pro_SaveUserSecurityInfo @UserID = @UserID, 
                            @MoneyPasswordHash = @MoneyPasswordHash,
                            @EmailEncrypt = @EmailEncrypt,
                            @FirstQuestionId = @FirstQuestionId,
                            @FirstAnswer = @FirstAnswer,
                            @SecondQuestionId = @SecondQuestionId,
                            @SecondAnswer = @SecondAnswer,
                            @RC_Success = @RC_Success,
                            @RC_UserNotFound = @RC_UserNotFound,
                            @RC_UpdateFailed = @RC_UpdateFailed,
                            @RC_UserIsFrozen = @RC_UserIsFrozen,
                            @RC_SamePassword = @RC_SamePassword,
                            @RC_InitHasFinished = @RC_InitHasFinished ";

            string returnCode = DbHelper.QueryFirst<string>(sql, spSaveUserSecurityInfoParam);

            return new BaseReturnModel(ReturnCode.GetSingle(returnCode));
        }

        public bool HasUserActiveBankInfo(int userId)
        {
            string sql = $@"
SELECT COUNT(0)
FROM 
(
	SELECT TOP 1 UserID 
	FROM	{InlodbType.Inlodb.Value}.dbo.ABCBankInfo WITH(NOLOCK) 
	WHERE	UserID = @userId AND IsActive = 1
	UNION ALL  
	SELECT TOP 1 UserID 
	FROM	{InlodbType.Inlodb.Value}.dbo.BankInfo WITH(NOLOCK) 
	WHERE	UserID = @userId AND IsActive = 1
) UserBankInfo
GROUP BY UserBankInfo.UserID
";
            int? result = DbHelper.QueryFirstOrDefault<int?>(sql, new { userId });

            return result.HasValue;
        }

        public List<ProSelectBankResult> GetUserAllBankCard(int userId)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_SelectBank";
            return DbHelper.QueryList<ProSelectBankResult>(sql, new { userId }, CommandType.StoredProcedure);
        }

        public int GetUserLevel(int userId)
        {
            string sql = $@" SELECT TOP 1 Level 
	                      FROM	{InlodbType.Inlodb.Value}.dbo.UserLevel WITH(NOLOCK) 
	                      WHERE	UserID = @userId ";

            int result = DbHelper.QueryFirstOrDefault<int>(sql, new { userId });

            return result;
        }

        public List<ActivityLog> GetUserActivityLogByIpInOneDay(string ip)
        {
            string sql = $@"
                SELECT ID, PrizeMoney, ActyDate, IP, UserID
                , Msg, NotIssuingType
                FROM {InlodbType.Inlodb.Value}.dbo.Activity_log WITH(NOLOCK) 
			    WHERE IP = @ip 
                AND ActyDate >= @startDateTime
				AND ActyDate <= @endDateTime 
                AND NotIssuingType = 0
            ";

            var now = DateTime.Now;
            var startDateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0);
            var endDateTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);

            return DbHelper.QueryList<ActivityLog>(sql, new { ip , startDateTime , endDateTime });
        }

        public bool AddUserActivityLog(ActivityLog model)
        {
            var sql = $@"
                INSERT INTO {InlodbType.Inlodb.Value}.dbo.Activity_log
                       ([PrizeMoney]
                       ,[ActyDate]
                       ,[IP]
                       ,[UserID]
                       ,[Msg]
                       ,[NotIssuingType])
                 VALUES
                       (@PrizeMoney
                       ,@ActyDate
                       ,@IP
                       ,@UserID
                       ,@Msg
                       ,@NotIssuingType)
            ";

            var count = DbHelper.Execute(sql, new { model.PrizeMoney, model.ActyDate, model.IP , 
                                                    model.UserID , model.Msg , model.NotIssuingType });

            return count == 1;
        }

        /// <summary>
        /// 根据邮箱找到用户名
        /// </summary>
        /// <param name="emailHash"></param>
        /// <returns></returns>
        public List<SearchUserEmailModel> GetUserNameByEmail(string emailHash)
        {
            string sql = $@"
                SELECT ROW_NUMBER() OVER (ORDER BY UserName) AS SeqNo, UserName, Email 
                FROM {InlodbType.Inlodb.Value}.dbo.UserInfo WITH(NOLOCK)
                WHERE Email = @EmailHash
                ORDER BY UserName
            ";

            return DbHelper.QueryList<SearchUserEmailModel>(sql, new { emailHash });
        }

        /// <summary>
        /// 會員註冊
        /// </summary>
        /// <param name="userRegisterParam"></param>
        /// <returns></returns>
        public BaseReturnModel UrlRegisterUser(UserRegisterParam userRegisterParam)
        {
            string sql = $"{InlodbType.Inlodb}.dbo.Pro_UrlRegisterByCustomerType";
            string returnCode = DbHelper.ExecuteScalar<string>(sql, userRegisterParam, CommandType.StoredProcedure);

            return new BaseReturnModel(ReturnCode.GetSingle(returnCode));
        }
    }
}
