import { OptionItemModel } from "./OptionItemModel";
import { PriceLowAndHighModel } from "./PriceLowAndHighModel";

export interface PostFilterOptionsModel {  
  /// 年齡
  age: { [name: string]: number[] };

  /// 身高
  height: { [name: string]: number[] };

  /// 價格
  price: { [name: string]: PriceLowAndHighModel };

  /// 罩杯
  cup: OptionItemModel[];

  /// 服務項目
  service: OptionItemModel[];
}
