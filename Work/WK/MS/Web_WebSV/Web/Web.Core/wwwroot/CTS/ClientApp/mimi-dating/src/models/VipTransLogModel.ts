export interface VipTransLogModel {
    title:string
    transactionTime:string
    
    /// 狀態(IncomeExpenseStatusEnum.cs)
    status:number
    
    /// 支付方式(IncomeExpensePayType.cs)
    payType:number
    
    /// 訂單編號
    orderID:string
    
    /// 金額
    amount:string
}