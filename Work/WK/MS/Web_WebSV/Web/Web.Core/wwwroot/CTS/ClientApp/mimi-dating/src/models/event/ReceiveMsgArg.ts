import { MqChatNotificationType } from "@/enums";

export interface ReceiveMsgArg {
  ChatNotificationType: MqChatNotificationType;
  RoomID: string;
  MessageID: string;
  MessageIDText: string;
  PublishTimestamp: string;
  PublishDateTimeText: string;
  Message: string;
}
