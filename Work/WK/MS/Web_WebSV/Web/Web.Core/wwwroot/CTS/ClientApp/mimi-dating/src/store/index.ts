import { createStore, Store as VuexStore, CommitOptions, DispatchOptions } from 'vuex';
import { State, state } from './state';
import { Getters, getters } from './getters';
import { Mutations, mutations, MutationType } from './mutations';
import { Actions, actions, ActionType } from './actions';

export type Store = Omit<
  VuexStore<State>,
  'getters' | 'commit' | 'dispatch'
> & {
  commit<K extends keyof Mutations, P extends Parameters<Mutations[K]>[1]>(
    key: K,
    payload?: P,
    options?: CommitOptions
  ): ReturnType<Mutations[K]>
} & {
  dispatch<K extends keyof Actions>(
    key: K,
    payload?: Parameters<Actions[K]>[1],
    options?: DispatchOptions
  ): ReturnType<Actions[K]>
} & {
  getters: Getters
}

export const store = createStore({
  state,
  getters,
  mutations,
  actions
}) as Store;

export { MutationType, ActionType };