
import { MyMessageViewType,MessageOperationType } from "@/enums";

export interface MessageOperationModel {
    /// <summary>
    /// 消息ID数组
    /// </summary>
    messageIds: string[] | null;
  
    /// <summary>
    /// 消息类型
    /// </summary>
    messageType: MyMessageViewType;
    
     /// <summary>
    /// 消息的操作类型
    /// </summary>
    messageOperationType:MessageOperationType;
  }
  