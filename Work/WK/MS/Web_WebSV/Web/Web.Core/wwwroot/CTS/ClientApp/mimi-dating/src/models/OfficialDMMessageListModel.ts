export interface OfficialDMMessageListModel {
  /// 房間號
  roomID?: string;

  /// 最后一笔讯息id
  lastMessageID?: string;

  lastPublishTimestamp?: string;

  // 信息加载顺序，1：顺序，2：逆序
  searchDirectionTypeValue: number;
}
