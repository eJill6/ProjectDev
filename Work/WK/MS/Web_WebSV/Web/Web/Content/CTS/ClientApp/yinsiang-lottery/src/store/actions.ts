import { ActionTree, ActionContext } from 'vuex';
import { State } from './state';
import { Mutations } from './mutations';
import { MutationType } from './mutations';
import api from '@/api';

export enum ActionType {
    RefreshBalanceAsync = 'A_RefreshBalanceAsync'
};

type AugmentedActionContext = {
    commit<K extends keyof Mutations>(
        key: K,
        payload: Parameters<Mutations[K]>[1]
    ): ReturnType<Mutations[K]>
} & Omit<ActionContext<State, State>, 'commit'>

export interface Actions {
    [ActionType.RefreshBalanceAsync](
        { commit }: AugmentedActionContext,
    ): Promise<void>
}

export const actions: ActionTree<State, State> & Actions = {
    async [ActionType.RefreshBalanceAsync]({ commit }) {
        let result = await api.getBalanceAsync();

        commit(MutationType.SetBalance, result);
    },
}