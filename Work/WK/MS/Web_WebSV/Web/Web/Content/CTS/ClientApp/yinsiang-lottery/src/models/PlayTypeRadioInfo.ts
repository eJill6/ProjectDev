export interface PlayTypeRadioInfo {
    basePlayTypeRadioId: number
    canPlayAfter: boolean
    fields: {
        numbers: string[]
        prompt: string
    }[],
    info: {
        playDescription: string
        playTypeID: number
        playTypeRadioID: number
        playTypeRadioName: string
        priority: number
        typeModel: string | null
        userType: number
        winExample: string
    },
    playTypeRadioEnum: string
}