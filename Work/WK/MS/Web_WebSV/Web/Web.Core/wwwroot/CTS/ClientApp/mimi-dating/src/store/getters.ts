import { GetterTree } from "vuex";
import { State } from "./state";

export type Getters = {
  [K in keyof GetterType]: ReturnType<GetterType[K]>
};

type GetterType = {};

export const getters: GetterTree<State, State> & GetterType = {};
