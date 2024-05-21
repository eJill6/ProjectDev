import { PostType } from "@/enums";
import { BaseInfoModel } from "./BaseInfoModel";

export interface OfficialListModel extends BaseInfoModel {
  /// 發帖類型
  postType: PostType;

  /// 標題
  title: string;

  /// 身高
  height: string;

  /// 年齡
  age: string;

  /// 罩杯
  cup: string;

  /// 地區編碼
  areaCode: string;

  /// 最低價格
  lowPrice: string;

  /// 顏值
  facialScore: string;

  /// 更新時間
  updateTime: string;

  /// 是否營業中。0：休息、1：營業中
  isOpen: boolean;
}
