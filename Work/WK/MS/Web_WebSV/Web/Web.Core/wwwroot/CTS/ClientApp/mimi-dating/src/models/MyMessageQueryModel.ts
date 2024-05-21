import { MyMessageViewType } from "@/enums";
import { PageParamModel } from "./PageParamModel";

export interface MyMessageQueryModel extends PageParamModel {
    messageInfoType: MyMessageViewType;
}
