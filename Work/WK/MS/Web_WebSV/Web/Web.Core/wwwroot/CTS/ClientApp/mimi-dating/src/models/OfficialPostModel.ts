import { BaseInfoModel } from "./BaseInfoModel";

export interface OfficialPostModel extends BaseInfoModel {
  /// 信息標題
  title: string;

  /// 期待收入
  lowPrice: number;

  /// 身高
  height: string;

  /// 年齡
  age: string;

  /// 罩杯
  cup: string;

  /// 基础浏览量
  viewBaseCount: number;

  areaCode: string;

  /// 浏览量
  views: number;
}