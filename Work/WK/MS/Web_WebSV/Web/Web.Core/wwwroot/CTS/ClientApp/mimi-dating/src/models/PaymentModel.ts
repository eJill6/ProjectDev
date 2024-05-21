import { PaymentType } from "@/enums";
import { VipCardModel } from "./VipCardModel";

export interface PaymentModel extends VipCardModel {
  payType: PaymentType;
  showDiamondImage: Boolean;
}
