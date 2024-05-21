import { PostType } from "@/enums";

export interface BaseUnlockAmountModel {
  postType: PostType;
  unlockAmount: number;
}
