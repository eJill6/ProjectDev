import mitt, { Emitter } from 'mitt';
import { event } from '@/models';

type EventType = {
    issueNoChanged: event.IssueNoChangedArg // 期號變更    
    lotteryDraw: event.LotteryDrawArg // 開獎獎號
    kj: void // 開獎算獎
};

const emitter: Emitter<EventType> = mitt<EventType>();

export default {
    on: emitter.on,
    off: emitter.off,
    emit: emitter.emit
};