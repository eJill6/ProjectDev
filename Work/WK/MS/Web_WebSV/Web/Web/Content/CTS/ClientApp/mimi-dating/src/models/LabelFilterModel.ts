import { PostLockStatus, PostSortType } from "@/enums";

export interface LabelFilterModel {
  sortType?: PostSortType | undefined;
  messageId?: Number | undefined;
  lockStatus?: PostLockStatus | undefined;
}
