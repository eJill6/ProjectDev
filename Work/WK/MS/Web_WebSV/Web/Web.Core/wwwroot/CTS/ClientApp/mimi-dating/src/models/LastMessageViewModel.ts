export interface LastMessageViewModel {
  //房間號
  roomID: string;

  //顯示名稱
  roomName: string;

  //頭像網址
  avatarUrl: string;

  messageID: number;

  //最後一則訊息ID
  messageIDText: string;

  messageType: number;

  //最後一則訊息內容
  message: string;

  //最後一則訊息發送時間
  publishTimestamp: number;

  //最後一則訊息顯示文字時間
  publishDateTimeText: string;

  //未讀數量
  unreadCount: number;
}
