import { TimeRules } from "@/enums";

export interface DateTimeModel {
  timeRule: TimeRules;
  secondsTotal: number;
  secondsTenDigits: string;
  secondsDigits: string;
}
