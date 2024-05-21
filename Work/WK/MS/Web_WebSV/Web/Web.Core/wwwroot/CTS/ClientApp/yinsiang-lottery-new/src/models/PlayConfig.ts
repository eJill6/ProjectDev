import { PlayTypeInfo } from "./PlayTypeInfo";

export interface PlayConfig {
    playModeId: number
    playModeName: string
    playTypeInfos: PlayTypeInfo[]
}