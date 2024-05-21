export interface ChatMessageViewModelExtend {
  //訊息發送人ID
  publishUserID: number;

  //訊息ID
  messageID: string;

  //訊息ID
  messageIDText: string;

  messageType: number;

  //訊息內容
  message: string;

  //訊息發送時間
  publishTimestamp: string;

  //訊息發送時間
  publishDateTimeText: string;

  showMessageTime: boolean;
}
