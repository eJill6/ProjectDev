export interface OfficialDMRoomListModel {
  /// 当前用户id
  ownerUserID?: number;

  /// 房間號
  roomId?: string;

  /// 最后一笔讯息id
  lastMessageID?: string;

  publishTimestamp?: string;
}
