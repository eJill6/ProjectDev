import { ResMyBookingModel } from "./ResMyBookingModel";

export interface ResMyBookingDetailModel extends ResMyBookingModel {
  bookingTime: "";
  acceptTime: "";
  finishTime: "";
  memo: "";
}
