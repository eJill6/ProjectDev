import { OrderStatus } from '@/enums';

export interface OrderHistory {
    lotteryId: number
    lotteryType: string
    issueNo: string
    playTypeName: string
    playTypeRadioName: string
    odds: string
    noteTime: string
    noteMoneyText: string
    prizeMoney:number
    prizeMoneyText:string    
    status: OrderStatus
    statusText: string
    palyNum: string
    playModeId: string
    playTypeId: number
    playTypeRadioId: number
}