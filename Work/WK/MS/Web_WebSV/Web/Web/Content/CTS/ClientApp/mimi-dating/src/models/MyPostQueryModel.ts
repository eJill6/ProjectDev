import { PostType, ReviewStatusType, MyPostSortType } from "@/enums";
import { PageParamModel } from "./PageParamModel";

export interface MyPostQueryModel extends PageParamModel {
  postType: PostType;
  postStatus?: ReviewStatusType;
  sortType?: MyPostSortType;
}
