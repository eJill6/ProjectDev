import { LotteryInfo } from './LotteryInfo';
import { PlayConfig } from './PlayConfig';
import { Settings } from './Settings';
import {MsSetting} from './MsSetting';
export interface AppData {
    lotteryInfo: LotteryInfo
    playConfigs: PlayConfig[]
    settings: Settings
    msSetting: MsSetting
}