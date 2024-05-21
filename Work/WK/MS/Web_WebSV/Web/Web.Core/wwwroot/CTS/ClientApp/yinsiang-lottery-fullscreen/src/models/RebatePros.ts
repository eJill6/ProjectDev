export interface RebateProsDetail {
    gameTypeId: number
    gameTypeName: string
    lotteryId: number
    playTypeId: number
    playTypeRadioId: number
    numberOdds: {
        [key: string]: number
    },
}

export type RebatePros = {
    [key: number]: RebateProsDetail[]
}


