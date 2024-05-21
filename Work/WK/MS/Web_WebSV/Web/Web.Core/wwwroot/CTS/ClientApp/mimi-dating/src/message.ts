import { event as eventModel } from '@/models';
import event from './event';
const w = window as any;

// MQ訂閱
(() => {

    const messageQueue = w.messageQueue;

    const messageTypes = w.ExchangeType;

    messageQueue.subscribe(messageTypes.MiseLiveChat, (arg: eventModel.ReceiveMsgArg) => event.emit('ReceiveMsg', arg));

    messageQueue.subscribe(messageTypes.MiseLiveChat, (arg: eventModel.DeleteRoomArg) => event.emit('DeleteRoom', arg));
})();
