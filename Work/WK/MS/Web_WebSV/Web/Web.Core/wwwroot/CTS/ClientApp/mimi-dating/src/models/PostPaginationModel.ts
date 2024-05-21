import { PageParamModel } from "./PageParamModel";
import {NextPagePostCoverModel} from "./NextPagePostCoverModel";
export interface PostPaginationModel<T> extends PageParamModel {
  /// 總頁數
  totalPage: number;
  /// 資料總筆數
  totalCount: number;
  /// 分頁資料
  data: T[];
  /// 下一页postId
  nextPagePost:NextPagePostCoverModel[];
}