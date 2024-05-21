import { event as eventModel } from '@/models';
import event from './event';
const w = window as any;

// MQ訂閱
(() => {

    const messqgeQueue = w.messageQueue;

    const messageTypes = w.ExchangeType;

    messqgeQueue.subscribe(messageTypes.RefreshLottery, (arg: eventModel.LotteryDrawArg) => event.emit('lotteryDraw', arg));

    messqgeQueue.subscribe(messageTypes.Client, () => event.emit('kj'));
})();

const toTokenPath = (path : string) : string => w.toTokenPath(path);
const openUrl=(logonMode:Number,url:string,win:object|null)=>w.openUrl(logonMode,url,win);

export default {
    toTokenPath,
    openUrl   
}