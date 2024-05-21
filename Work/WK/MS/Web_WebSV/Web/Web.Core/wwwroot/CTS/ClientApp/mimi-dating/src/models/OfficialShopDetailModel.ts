
import {
  OfficialShopModel,
  MediaResultModel
  } from "@/models";

export interface OfficialShopDetailModel extends OfficialShopModel {
  /// 
  bossId:string,
  /// 基础浏览量
  viewBaseCount: number;

  /// 浏览量
  views: number;

  /// 店龄
  shopYears: number;

  /// 成交订单数
  dealOrder: 0;

  /// 评分
  selfPopularity: number;

  /// 介绍文字
  introduction: string;

  /// 营业时间（日）
  businessDate: string;

  /// 营业时间（时）
  businessHour: string;
  
  /// 商家照片
  businessPhotoSource: string[];

  /// 当前用户是否收藏该店铺
  isFollow: boolean;

  /// 联系软件
  contactApp:string;
  
  /// 联系地址
  contactInfo:string;
  
  ///帖子地区编码列表
  areaCodes:string[];

  businessPhotoSourceViewModel:MediaResultModel[];
  //该店铺是否处于修改审核当中
  isEditAudit:boolean;
}
