export interface RebatePro {
    numberOdds: {
        [key: string]: number
    },
    rebateText: string
    text: string
    value: {
        odds: number
        rebate: number
    }
}

export type IndexedRebatePro = {
    id: number
} & RebatePro