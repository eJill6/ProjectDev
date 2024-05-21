import { LongDragonDetailInfo } from "./LongDragonDetailInfo"

export interface LongDragonInfo {
    currentIssueNo: string
    gameTypeId: number
    lotteryID: number
    lotteryTypeName: string
    longInfo: LongDragonDetailInfo[]
    lotteryTime: string,
    gameTypeName: string
}