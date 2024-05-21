import { PlayTypeRadioInfo } from "./PlayTypeRadioInfo";

export interface PlayTypeInfo {
    basePlayTypeId: number
    info: {
        lotteryID: number
        playTypeID: number
        playTypeName: string
        userType: number
    }
    playTypeEnum: string
    playTypeRadioInfos: { [key: string]: PlayTypeRadioInfo[] }
}