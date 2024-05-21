import { OptionItemModel } from "./OptionItemModel";

export interface PostTypeOptionsModel {
  /// <summary>
  /// 價格
  /// </summary>
  price: OptionItemModel[];

  /// <summary>
  /// 標籤
  /// </summary>
  label: OptionItemModel[];

  /// <summary>
  /// 訊息種類
  /// </summary>
  messageType: OptionItemModel[];

  /// <summary>
  /// 服務項目
  /// </summary>
  service: OptionItemModel[];

  /// <summary>
  /// 年齡
  /// </summary>
  age: OptionItemModel[];

  /// <summary>
  /// 身高
  /// </summary>
  bodyHeight: OptionItemModel[];

  /// <summary>
  /// Cup
  /// </summary>
  cup: OptionItemModel[];
}
