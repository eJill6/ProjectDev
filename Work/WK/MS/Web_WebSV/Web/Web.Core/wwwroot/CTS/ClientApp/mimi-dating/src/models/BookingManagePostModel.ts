import { ManageBookingStatusType } from "@/enums";
import { PageParamModel } from "./PageParamModel";
export interface BookingManagePostModel extends PageParamModel{
  status: ManageBookingStatusType;

  pageNo: number;
}
