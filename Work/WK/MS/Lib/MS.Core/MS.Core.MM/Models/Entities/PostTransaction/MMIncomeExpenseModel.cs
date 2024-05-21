using MMService.DBTools;
using MS.Core.Attributes;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.Models;
using System.Data;

namespace MS.Core.MM.Models.Entities.PostTransaction;

/// <summary>
/// 收支(交易)紀錄
/// </summary>
public class MMIncomeExpenseModel : BaseDBModel
{
    /// <summary>
    /// ID
    /// </summary>
    [PrimaryKey]
    [EntityType(DbType.String)]
    public string Id { get; set; }

    /// <summary>
    /// 交易類型(收入/支出)
    /// </summary>
    public IncomeExpenseTransactionTypeEnum TransactionType { get; set; }

    /// <summary>
    /// 類型(贴子、購買會員)
    /// </summary>
    [EntityType(DbType.Byte)]
    public IncomeExpenseCategoryEnum Category { get; set; }

    /// <summary>
    /// 支付方式(覓錢包、其他)
    /// </summary>
    public IncomeExpensePayType PayType { get; set; }

    /// <summary>
    /// 來源ID
    /// </summary>
    [EntityType(DbType.String)]
    public string SourceId { get; set; }

    /// <summary>
    /// 目標 ID (支出單、收益單、預約支出單、預約收益單)
    /// </summary>
    public string TargetId { get; set; }

    /// <summary>
    /// 會員ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [EntityType(DbType.String, StringType.Nvarchar)]
    public string Title { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string Memo { get; set; }

    /// <summary>
    /// (支出)原始解鎖鑽石
    /// (收入)實際解鎖鑽石
    /// PS.單位使用鑽石
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public IncomeExpenseStatusEnum Status { get; set; }

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 派發時間
    /// </summary>
    public DateTime? DistributeTime { get; set; }

    /// <summary>
    /// (支出)折扣成數(免費為0)
    /// (支出)無折扣(1)
    /// (收益)解鎖鑽石抽成(0.06)
    /// (收益)預約鑽石計算(0.1)
    /// 超觅老板单独使用mmboss的拆账比值
    /// </summary>
    public decimal Rebate { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 異常備註
    /// </summary>
    public string UnusualMemo { get; set; } = null!;
}