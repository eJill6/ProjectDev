import { MqChatNotificationType } from "@/enums";

export interface DeleteRoomArg {
  ChatNotificationType: MqChatNotificationType;
  Message: string;
  MessageID: string;
  MessageIDText: string;
  PublishDateTimeText: string;
  PublishTimestamp: number;
  RoomID: string;
}
