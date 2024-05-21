import { PostLockStatus, PostSortType, PostType } from "@/enums";

export interface SquareSearchModel {
  postType: PostType;
  isEssence: boolean;
  sortType: PostSortType;
  messageId: number;
  lockStatus: PostLockStatus;
}
