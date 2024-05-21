import { RedirectType } from "@/enums/RedirectType";

export interface PageRedirectInfo {
    /// 轉導的頁面
    redrectType: RedirectType;
    /// 詳細頁的編號
    refId?: string;
  }

export interface RedirectParam{
    /// 轉導頁面內容
    redrectPage: string;

    /// 需要參數的名稱
    queryName?: string;
}

export const redirectParamInfo = new Map<RedirectType, RedirectParam>([
    [RedirectType.Official, {
        redrectPage: "Official"
    }],
    [RedirectType.Agency, {
        redrectPage: "Agency"
    }],
    [RedirectType.Square, {
        redrectPage: "Square"
    }],
    [RedirectType.OfficialShopDetail, {
        redrectPage: "OfficialShopDetail",
        queryName:"applyId", 
    }],
    [RedirectType.OfficialDetail, {
        redrectPage: "OfficialDetail",
        queryName:"postId",
    }],
    [RedirectType.Detail, {
        redrectPage: "Detail",
        queryName:"postId",
    }],
])
  