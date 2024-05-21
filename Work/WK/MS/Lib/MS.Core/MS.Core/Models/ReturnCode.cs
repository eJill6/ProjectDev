using System.Collections.Concurrent;

namespace MS.Core.Models
{
    public partial class ReturnCode
    {
        private static ConcurrentDictionary<string, ReturnCode> _defaultReturnCode = new ConcurrentDictionary<string, ReturnCode>();

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; private set; } = false;

        /// <summary>
        /// 錯誤碼
        /// </summary>
        public string Code { get; private set; } = string.Empty;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; private set; } = string.Empty;

        public ReturnCode(string code, bool isSuccess = false, string? message = null)
        {
            Code = code;
            IsSuccess = isSuccess;
            Message = message ?? string.Empty;
        }

        /// <summary>
        /// 回傳資訊
        /// </summary>
        /// <param name="code">錯誤碼</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="message">錯誤訊息</param>
        /// <returns></returns>
        protected static ReturnCode Factory(string code, bool isSuccess = false, string? message = null)
        {
            if (_defaultReturnCode.ContainsKey(code))
            {
                return _defaultReturnCode[code];
            }

            var result = new ReturnCode(code, isSuccess, message);
            _defaultReturnCode[code] = result;
            return result;
        }

        /// <summary>
        /// 初始值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ReturnCode GetDefault(string code)
        {
            if (_defaultReturnCode.ContainsKey(code))
            {
                return _defaultReturnCode[code];
            }
            return new ReturnCode(code, false, null);
        }

        // Service

        #region 系統服務方面的訊息

        /// <summary>
        /// 成功
        /// </summary>
        public static ReturnCode Success = Factory("000000", true);

        /// <summary>
        ///
        /// </summary>
        public static ReturnCode CustomizedMessage = Factory("E09999");

        /// <summary>
        /// 系統錯誤
        /// </summary>
        public static ReturnCode SystemError = Factory("E00001");

        /// <summary>
        /// 無效的參數
        /// </summary>
        public static ReturnCode ParameterIsInvalid = Factory("E00002");

        /// <summary>
        /// 無符合的值
        /// </summary>
        public static ReturnCode NonMatched = Factory("E00003");

        /// <summary>
        /// 資料已存在
        /// </summary>
        public static ReturnCode DataIsExist = Factory("E00005");

        /// <summary>
        /// 資料不存在
        /// </summary>
        public static ReturnCode DataIsNotExist = Factory("E00006");

        /// <summary>
        /// 資料不可使用
        /// </summary>
        public static ReturnCode DataNotUse = Factory("E00007");

        /// <summary>
        /// 資料未完成
        /// </summary>
        public static ReturnCode DataIsNotCompleted = Factory("E00012");

        /// <summary>
        /// 更新失敗
        /// </summary>
        public static ReturnCode UpdateFailed = Factory("E00013");

        /// <summary>
        /// 操作失敗
        /// </summary>
        public static ReturnCode OperationFailed = Factory("E00014");

        /// <summary>
        /// 搜尋結果為空
        /// </summary>
        public static ReturnCode SearchResultIsEmpty = Factory("E00021");

        /// <summary>
        /// 缺少必要參數
        /// </summary>
        public static ReturnCode MissingNecessaryParameter = Factory("E00022");

        /// <summary>
        /// 非法使用者
        /// </summary>
        public static ReturnCode IllegalUser = Factory("E00023");

        /// <summary>
        /// 已提交身份申請
        /// </summary>
        public static ReturnCode IsAlreadyIdentityApply = Factory("E00024", message: "已有身份申请，请耐心等候审核");

        /// <summary>
        /// 頻繁操作
        /// </summary>
        public static ReturnCode TooFrequent = Factory("E00025", message: "您操作过于频繁,请稍后重试");

        /// <summary>
        /// 手機尚未綁定
        /// </summary>
        public static ReturnCode PhoneHasNotBeenBound = Factory("E00026", message: "需要绑定手机号码才能申请觅经纪/觅老板");

        /// <summary>
        /// 取得電話認證失敗
        /// </summary>
        public static ReturnCode FailedToGetPhoneAuthentication = Factory("E00027");

        /// <summary>
        /// 您已有身份，無法再次申請
        /// </summary>
        public static ReturnCode AlreadyHaveIdentityCannotApplyAgain = Factory("E00028", message: "您已有身份，无法再次申请");

        /// <summary>
        /// 該會員不存在
        /// </summary>
        public static ReturnCode UserDoesNotExist = Factory("E00029");

        /// <summary>
        /// 審核中的贴子不可刪除
        /// </summary>
        public static ReturnCode UnderReviewPostCannotBeDeleted = Factory("E00030", message: "审核中的贴子不可删除");

        /// <summary>
        /// 正在進行中的訂單不可刪除
        /// </summary>
        public static ReturnCode OrdersInProgressCannotBeDeleted = Factory("E00031", message: "正在进行中的订单不可删除");

        /// <summary>
        /// 已提交退费申请
        /// </summary>
        public static ReturnCode RefundHasBeenSubmitted = Factory("E00032", message: "已提交退费申请");

        /// <summary>
        /// 沒有預約
        /// </summary>
        public static ReturnCode NoAppointment = Factory("E00033");

        /// <summary>
        /// 无此Apply ID
        /// </summary>
        public static ReturnCode UserDoesNotApplyID = Factory("E00034");

        /// <summary>
        /// 此Apply ID非觅老板
        /// </summary>
        public static ReturnCode UserDoesNotBossApplyID = Factory("E00035");

        /// <summary>
        /// 店铺关闭中不可上架
        /// </summary>

        public static ReturnCode StoreClosed = Factory("E00036", message: "店铺关闭中");

        /// <summary>
        /// 第三方 API 錯誤
        /// </summary>
        public static ReturnCode ThirdPartyApi = Factory("E00101");

        /// <summary>
        /// 第三方 API 錯誤 回傳 NULL
        /// </summary>
        public static ReturnCode ThirdPartyApiNull = Factory("E00102");

        /// <summary>
        /// 第三方 API 錯誤 回傳 失敗
        /// </summary>
        public static ReturnCode ThirdPartyApiNotSuccess = Factory("E00103");

        #endregion 系統服務方面的訊息

        // DataBase

        #region 提示訊息 D10001 ~ D19999

        /// <summary>
        /// 無效的使用者
        /// </summary>
        public static ReturnCode InvalidUser = Factory("D10001");

        /// <summary>
        /// 沒有符合的資料
        /// </summary>
        public static ReturnCode NotMatchData = Factory("D10002");

        /// <summary>
        /// 不是會員
        /// </summary>
        public static ReturnCode NotMember = Factory("D10003");

        /// <summary>
        /// 限制發佈
        /// </summary>
        public static ReturnCode LimitPublish = Factory("D10004");

        /// <summary>
        /// 已回報過
        /// </summary>
        public static ReturnCode IsAlreadyReport = Factory("D10005", true);

        /// <summary>
        /// 更新狀態失敗
        /// </summary>
        public static ReturnCode UpdateStatusFail = Factory("D10006");

        /// <summary>
        /// 資料已存在
        /// </summary>
        public static ReturnCode DataIsExists = Factory("D10007");

        /// <summary>
        /// 已達上限
        /// </summary>
        public static ReturnCode LimitReached = Factory("D10008");

        /// <summary>
        /// 照片是必須傳入的
        /// </summary>
        public static ReturnCode PhotoIsNecessary = Factory("D10009");

        /// <summary>
        /// 無效的圖片 (可能是盜用)
        /// </summary>
        public static ReturnCode InvalidPhoto = Factory("D10010");

        /// <summary>
        /// 無效的視頻 (可能是盜用)
        /// </summary>
        public static ReturnCode InvalidVideo = Factory("D10011");

        /// <summary>
        /// 無法投訴，超過時間限制
        /// </summary>
        public static ReturnCode UnableToComplain = Factory("D10012", message: "解锁超过72小时后不可投诉");

        /// <summary>
        /// 視頻是必須傳入的
        /// </summary>
        public static ReturnCode VideoIsNecessary = Factory("D10013");

        /// <summary>
        /// 无此贴子，请再次确认
        /// </summary>
        public static ReturnCode PostIsNotExist = Factory("D10101");

        /// <summary>
        /// 贴子重复，请再次确认
        /// </summary>
        public static ReturnCode PostRepeat = Factory("D10102");

        /// <summary>
        /// 权重重复，请再次确认
        /// </summary>
        public static ReturnCode WeightRepeat = Factory("D10103");

        /// <summary>
        /// 首页【广场】达设定上限50笔
        /// </summary>
        public static ReturnCode LimitSquare = Factory("D10111");

        /// <summary>
        /// 首页【担保】达设定上限50笔
        /// </summary>
        public static ReturnCode LimitAgency = Factory("D10112");

        /// <summary>
        /// 首页【官方】达设定上限50笔
        /// </summary>
        public static ReturnCode LimitOfficial = Factory("D10113");

        #endregion 提示訊息 D10001 ~ D19999

        #region 執行錯誤 D90001 ~ D99999

        /// <summary>
        /// 執行 SQL 發生錯誤
        /// </summary>
        public static ReturnCode RunSQLFail = Factory("D99998");

        /// <summary>
        /// 執行 Store procedure 發生錯誤
        /// </summary>
        public static ReturnCode RunProcedureFail = Factory("D99999");

        #endregion 執行錯誤 D90001 ~ D99999
    }
}