import { OfficialShopModel } from "./OfficialShopModel";
import { ProductListModel } from "./ProductListModel";
import { BannerModel } from "./BannerModel";

export interface HomeStatusInfoModel {
  scrollTop: number;
  homeAgencyList: ProductListModel[];
  homeSquareList: ProductListModel[];
  homeOfficialList: OfficialShopModel[]; // as OfficialListModel[];
  homeShortcutList: BannerModel[];
}
