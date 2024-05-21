
import { PageParamModel } from "./PageParamModel";

export interface MyOfficialPostQueryParamModel extends PageParamModel {
    /// 使用者编号
    userId?: number;
  
    // 地区码
    areaCode: string[];
  }
  