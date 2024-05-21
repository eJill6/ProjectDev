import {OrderHistory} from './OrderHistory'
export interface ResultHistory {
    totalBetCount: number,
    totalPrizeMoney: number,
    totalWinMoney: number, nextCursor: string, dataDetail: OrderHistory[]
};