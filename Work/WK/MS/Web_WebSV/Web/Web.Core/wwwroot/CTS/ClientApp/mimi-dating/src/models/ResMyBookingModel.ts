import { MyBookingStatusType } from "@/enums";
import { ResMyBookingPostModel } from "./ResMyBookingPostModel";

export interface ResMyBookingModel {
  post: ResMyBookingPostModel;
  bookingId: string;
  status: MyBookingStatusType;
  paymentStatus: string;
  paymentMoney: string;
  commentId: string;
  contact: string;
  refusalOfRefund:boolean;
}