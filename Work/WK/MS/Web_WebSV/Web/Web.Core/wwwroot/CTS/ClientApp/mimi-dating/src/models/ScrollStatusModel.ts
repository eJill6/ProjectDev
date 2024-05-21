import { VirtualScrollModel } from "./VirtualScrollModel";
export interface ScrollStatusModel {
  totalPage: number;
  list: unknown[];
  virtualScroll: VirtualScrollModel;
  isTop: boolean;
  scrollTop: number;
}
