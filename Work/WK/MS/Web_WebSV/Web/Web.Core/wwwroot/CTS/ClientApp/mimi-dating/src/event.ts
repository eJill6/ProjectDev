import mitt, { Emitter } from 'mitt';
import { event } from '@/models';

type EventType = {   
    ReceiveMsg: event.ReceiveMsgArg, // 接收新消息
    DeleteRoom: event.DeleteRoomArg, //删除聊天
    isBossShopEdit:true //是否为觅老板修改
};

const emitter: Emitter<EventType> = mitt<EventType>();

export default {
    on: emitter.on,
    off: emitter.off,
    emit: emitter.emit
};