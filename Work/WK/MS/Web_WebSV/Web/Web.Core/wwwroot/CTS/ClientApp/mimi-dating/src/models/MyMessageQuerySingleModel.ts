import { MyMessageViewType,MessageOperationType } from "@/enums";
export interface MyMessageQuerySingleModel {
    /// <summary>
    /// 消息ID数组
    /// </summary>
    messageId: string
  
    /// <summary>
    /// 消息类型
    /// </summary>
    messageType: MyMessageViewType;
    
     /// <summary>
    /// 消息的操作类型
    /// </summary>
    messageOperationType:MessageOperationType;
  }