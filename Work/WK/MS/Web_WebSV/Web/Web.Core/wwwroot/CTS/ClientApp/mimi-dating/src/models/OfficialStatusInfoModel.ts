import { OfficialShopModel } from "./OfficialShopModel";
import { OfficialShopListModel } from "./OfficialShopListModel";

export interface OfficialStatusInfoModel {
  officialGoldenShopList: OfficialShopModel[];
  officialShopList: OfficialShopListModel[];
}
