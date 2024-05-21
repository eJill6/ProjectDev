import {MyBossPostStatus,MyPostSortType } from "@/enums";
import { PageParamModel } from "./PageParamModel";

export interface MyOfficialPostQueryModel extends PageParamModel
{
   postStatus:MyBossPostStatus;
   sortType: MyPostSortType;
}