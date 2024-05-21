import { VirtualScrollModel } from "./VirtualScrollModel";
export interface ScrollStatusModel {
  list: unknown[];
  virtualScroll: VirtualScrollModel;
  isTop: boolean;
  scrollTop: number;
}
