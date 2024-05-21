namespace JxBackendService.Service.ThirdPartyTransfer.Old
{
    ///// <summary>
    ///// 把舊版錢包的服務獨立出來
    ///// </summary>
    //public abstract class BaseOldTPGameWalletApiService<CheckAccountResultType, BalanceResultType> : BaseService, IOldTPGameApiService, IOldTPGameApiReadService
    //{
    //    private readonly ITPGameApiReadService _tpGameApiReadService;

    //    private readonly ITPGameAccountService _tpGameAccountService;

    //    private readonly ITPGameAccountReadService _tpGameAccountReadService;

    //    private readonly ITPGameStoredProcedureRep _tpGameStoredProcedureRep;

    //    protected abstract PlatformProduct Product { get; }

    //    protected abstract BaseReturnDataModel<UserScore> GetRemoteUserScore(BasicUserInfo loginUser);

    //    protected abstract CheckAccountResultType DoCheckOrCreateAccount(string tpGameAccount);

    //    protected abstract CheckAccountResultType GetCheckAccountFailResult(string errorMessage);

    //    protected abstract BalanceResultType GetBalance(string tpGameAccount, CheckAccountResultType checkRemoteAccountResult);

    //    public BaseOldTPGameWalletApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
    //    {
    //        _tpGameApiReadService = ResolveJxBackendService<ITPGameApiReadService>(Product);
    //        _tpGameAccountService = ResolveJxBackendService<ITPGameAccountService>(SharedAppSettings.PlatformMerchant);
    //        _tpGameAccountReadService = ResolveJxBackendService<ITPGameAccountReadService>(SharedAppSettings.PlatformMerchant);
    //        _tpGameStoredProcedureRep = ResolveJxBackendService<ITPGameStoredProcedureRep>(Product);
    //    }

    //    protected CheckAccountResultType CheckOrCreateAccount()
    //    {
    //        return CheckOrCreateAccount(out string tpGameAccount);
    //    }

    //    public BalanceResultType GetBalanceWithCheckAccount()
    //    {
    //        CheckAccountResultType checkResult = CheckOrCreateAccount(out string tpGameAccount);

    //        return GetBalance(tpGameAccount, checkResult);
    //    }

    //    protected CheckAccountResultType CheckOrCreateAccount(out string tpGameAccount)
    //    {
    //        tpGameAccount = null;
    //        BaseReturnDataModel<string> returnModel = GetAllowCreateOrderAndLocalAccountResult();

    //        if (!returnModel.IsSuccess)
    //        {
    //            return GetCheckAccountFailResult(returnModel.Message);
    //        }

    //        tpGameAccount = returnModel.DataModel;

    //        return DoCheckOrCreateAccount(tpGameAccount);
    //    }

    //    public BaseReturnModel UpdateUserScoreFromRemote()
    //    {
    //        BaseReturnDataModel<UserScore> userScoreReturnDataModel = GetRemoteUserScore(EnvLoginUser.LoginUser);

    //        if (userScoreReturnDataModel.IsSuccess)
    //        {
    //            //todo 更新餘額
    //            throw new NotImplementedException();

    //            //return new BaseReturnModel(ReturnCode.Success);
    //        }

    //        return userScoreReturnDataModel.CastByJson<BaseReturnModel>();
    //    }

    //    /// <summary>
    //    /// 取得是否可以建立轉帳訂單與第三方帳號名稱
    //    /// </summary>
    //    protected BaseReturnDataModel<string> GetAllowCreateOrderAndLocalAccountResult()
    //    {
    //        BaseReturnModel returnModel = _tpGameApiReadService.GetAllowCreateTransferOrderResult();

    //        if (!returnModel.IsSuccess)
    //        {
    //            return new BaseReturnDataModel<string>(returnModel.Message, null);
    //        }

    //        ThirdPartyUserAccount thirdPartyUserAccount = _tpGameAccountReadService.GetThirdPartyUserAccount(EnvLoginUser.LoginUser.UserId, Product);
    //        string tpGameAccount;

    //        if (thirdPartyUserAccount == null)
    //        {
    //            tpGameAccount = _tpGameAccountReadService.GetTPGameAccountByRule(
    //                Product,
    //                EnvLoginUser.LoginUser.UserId);

    //            _tpGameAccountService.Create(
    //                EnvLoginUser.LoginUser.UserId,
    //                Product,
    //                tpGameAccount);
    //        }
    //        else
    //        {
    //            tpGameAccount = thirdPartyUserAccount.Account;
    //        }

    //        return new BaseReturnDataModel<string>(ReturnCode.Success, tpGameAccount);
    //    }

    //    /// <summary>
    //    /// 建立轉帳單前的檢查
    //    /// </summary>
    //    private BaseReturnModel CreateTransferBeforeCheck(decimal amount)
    //    {
    //        TPTransferAmountBound tpTransferAmountBound = GlobalVariables.TPTransferAmountBound;

    //        if (amount < tpTransferAmountBound.MinTPGameTransferAmount)
    //        {
    //            return new BaseReturnModel(string.Format(ThirdPartyGameElement.TransferMoneyMustThanAmount, tpTransferAmountBound.MinTPGameTransferAmount));
    //        }
    //        else if (amount > tpTransferAmountBound.MaxTPGameTransferAmount)
    //        {
    //            return new BaseReturnModel(ThirdPartyGameElement.TransferMoneyMustUnderLimitation);
    //        }

    //        return new BaseReturnModel(ReturnCode.Success);
    //    }

    //    /// <summary>
    //    /// 建立轉入單
    //    /// </summary>
    //    protected TPGameTransferMoneyResult CreateTransferInInfo(int userID, decimal amount, string tpGameAccount)
    //    {
    //        BaseReturnModel checkModel = CreateTransferBeforeCheck(amount);

    //        if (!checkModel.IsSuccess)
    //        {
    //            return new TPGameTransferMoneyResult() { ErrorMsg = checkModel.Message };
    //        }

    //        amount = decimal.Round(amount, 2);

    //        return _tpGameStoredProcedureRep.CreateMoneyInOrder(userID, amount, tpGameAccount, TPGameMoneyInOrderStatus.Unprocessed);
    //    }

    //    /// <summary>
    //    /// 建立轉出單
    //    /// </summary>
    //    protected TPGameTransferMoneyResult CreateTransferOutInfo(int userID, decimal amount, string tpGameAccount)
    //    {
    //        amount = decimal.Round(amount, 2);
    //        BaseReturnModel checkModel = CreateTransferBeforeCheck(amount);

    //        if (!checkModel.IsSuccess)
    //        {
    //            return new TPGameTransferMoneyResult() { ErrorMsg = checkModel.Message };
    //        }

    //        return _tpGameStoredProcedureRep.CreateMoneyOutOrder(new CreateTransferOutOrderParam()
    //        {
    //            UserId = userID,
    //            Amount = amount,
    //            TPGameAccount = tpGameAccount,
    //            TransferOutStatus = TPGameMoneyOutOrderStatus.Unprocessed,
    //        });
    //    }
    //}
}