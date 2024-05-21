import { PostType } from "@/enums";
import { LabelFilterModel } from "./LabelFilterModel";
import { PriceLowAndHighModel } from "./PriceLowAndHighModel";

export interface SearchModel extends LabelFilterModel {
  page: Number;
  pageSize?: Number | undefined;
  postType?: PostType | undefined;
  isRecommend?: Boolean | undefined;
  areaCode: string;
  labelIds: Number[];  
  /// 篩選的年齡
  age: Number[];

  /// 篩選的年齡
  height: Number[];

  /// 篩選的年齡
  cup: Number[];

  /// 價格設定
  price: PriceLowAndHighModel[];

  /// 篩選的服務項目
  serviceIds: Number[];
  ts: string;
}
