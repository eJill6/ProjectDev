import { PageParamModel } from "./PageParamModel";
export interface PaginationModel<T> extends PageParamModel {
  /// 總頁數
  totalPage: number;
  /// 資料總筆數
  totalCount: number;
  /// 分頁資料
  data: T[];
}
