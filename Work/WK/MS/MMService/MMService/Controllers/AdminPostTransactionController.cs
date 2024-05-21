using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPostTransaction;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models;
using MS.Core.Models.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace MMService.Controllers
{
    public class AdminPostTransactionController : ApiControllerBase
    {

        /// <summary>
        /// 解鎖相關服務
        /// </summary>
        private readonly IPostTransactionRepo _postTransactionRepo;

        /// <summary>
        /// 收益單相關Repo
        /// </summary>
        private readonly IIncomeExpenseRepo _incomeExpenseRepo;

        /// <summary>
        /// 型別轉換
        /// </summary>
        private readonly IMapper _mapper;

        public AdminPostTransactionController(IPostTransactionRepo postTransactionRepo,
            IIncomeExpenseRepo incomeExpenseRepo,
            IMapper mapper,
            ILogger logger) : base(logger)
        {
            _postTransactionRepo = postTransactionRepo;
            _incomeExpenseRepo = incomeExpenseRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// 解锁单记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminPostTransactionList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminPostTransactionListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminPostTransactionList>>();

                if (!string.IsNullOrWhiteSpace(param.PostId))
                {
                    var unlocks = await _postTransactionRepo.List(new string[1] { param.PostId }, 1);
                    if (unlocks.Length > 0)
                    {
                        result.DataModel = _mapper.Map<PageResultModel<AdminPostTransactionList>>(await _incomeExpenseRepo.List(new AdminIncomeExpenseListByTypeParam()
                        {
                            Ids = unlocks.Select(x => x.Id).ToArray(),
                            Type = IncomeExpenseTransactionTypeEnum.Expense,
                            IdType = 0,
                        }));
                        foreach (var item in result.DataModel.Data)
                        {
                            item.PostId = param.PostId;
                        }
                    }
                    else
                    {
                        result.DataModel = new PageResultModel<AdminPostTransactionList>()
                        {
                            Data = new AdminPostTransactionList[0]
                        };
                    }
                    result.SetCode(ReturnCode.Success);
                }
                else
                {
                    var rawData = await _incomeExpenseRepo.List(model);
                    result.DataModel = _mapper.Map<PageResultModel<AdminPostTransactionList>>(rawData);
                    if (result.DataModel.Data.Length > 0)
                    {
                        var transactionIds = rawData.Data.Select(x => x.SourceId).ToArray();
                        var transactions = await _postTransactionRepo.List(transactionIds, 0);
                        foreach (var item in result.DataModel.Data)
                        {
                            var transaction = transactions.FirstOrDefault(x => x.Id == item.Id);
                            if (transaction != null)
                            {
                                item.PostId = transaction.PostId;
                            }
                            else
                            {
                                _logger.LogError($"{MethodInfo.GetCurrentMethod()?.Name ?? string.Empty} {nameof(MMIncomeExpenseModel)} not have {nameof(MMPostTransactionModel)} item:{JsonConvert.SerializeObject(item)}");
                            }
                        }
                    }
                    result.SetCode(ReturnCode.Success);
                }

                if (result.IsSuccess && result.DataModel.Data.Length > 0)
                {

                    try
                    {
                        var rawData = await _incomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter()
                        {
                            SourceIds = result.DataModel.Data.Select(x => x.Id),
                            TransactionType = IncomeExpenseTransactionTypeEnum.Refund
                        });

                        var dic = rawData.GroupBy(x => x.SourceId).ToDictionary(x => x.Key, x => x.First());
                        foreach (var item in result.DataModel.Data)
                        {
                            if (dic.ContainsKey(item.Id))
                            {
                                item.RefundMemo = "已退款";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{MethodInfo.GetCurrentMethod()} Get RefundMemo fail");
                    }
                }
                return result;
            }, model));
        }
    }
}
