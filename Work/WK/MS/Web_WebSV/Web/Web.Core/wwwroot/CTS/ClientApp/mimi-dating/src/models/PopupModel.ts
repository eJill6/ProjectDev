import { OptionItemModel } from "./OptionItemModel";

export interface PopupModel {
  title: string;
  content: OptionItemModel[];  
  isMultiple :boolean
}
