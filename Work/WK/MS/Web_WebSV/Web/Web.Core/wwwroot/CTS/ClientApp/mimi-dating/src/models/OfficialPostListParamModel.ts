import { PageParamModel } from "./PageParamModel";

export interface OfficialPostListParamModel extends PageParamModel {
  // 店铺Id
  applyId: string;

  /// 使用者编号
  userId?: number;

  // 地区码
  areaCode: string[];
}
