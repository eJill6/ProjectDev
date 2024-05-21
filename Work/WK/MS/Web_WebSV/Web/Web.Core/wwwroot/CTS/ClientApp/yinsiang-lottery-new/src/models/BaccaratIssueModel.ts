import { JSBaccaratPoker } from "@/GameRules/JSBaccaratRule";

export interface BaccaratIssueModel {
  issueNo: string;
  player: JSBaccaratPoker;
  banker: JSBaccaratPoker;
}