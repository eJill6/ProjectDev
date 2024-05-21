import { MyMessageViewType } from "../enums"

export interface MyMessageListModel{
    /// 消息ID
    id:string,
    /// 消息类型
    messageType:MyMessageViewType,
    /// 消息标题
    messageTitle:string,
    /// 发布时间
    publishTime:string,
    /// 消息内容
    messageContent:string,
    /// 是否已读
    isRead:boolean,
    /// 是否删除
    isDelete:boolean
}