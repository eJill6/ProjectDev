import { createRouter, createWebHashHistory, RouteRecordRaw } from 'vue-router';
import Views from './views';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    redirect: { name: 'Bet' }
  },
  {
    path: '/Bet',
    name: 'Bet',
    component: Views.BetViews.BetView,
    redirect: { name: 'Bet_Default' },
    children: [
      {
        path: '',
        redirect: { name: 'Bet_Default' }
      },
      {
        path: 'Default',
        name: 'Bet_Default',
        components: {
          nav: Views.BetViews.Nav.DefaultView
        }
      },
      {
        path: 'RecentOrderHistory',
        name: 'Bet_RecentOrderHistory',
        components: {
          nav: Views.BetViews.Nav.RecentOrderHistoryView,
          navContent: Views.BetViews.NavContent.RecentOrderHistoryView
        },
        meta: {
          useSmallIcon: false
        }
      },
      {
        path: 'RecentIssueHistory',
        name: 'Bet_RecentIssueHistory',
        components: {
          nav: Views.BetViews.Nav.RecentIssueHistoryView,
          navContent: Views.BetViews.NavContent.RecentIssueHistoryView
        },
        meta: {
          useSmallIcon: false
        }
      },
      {
        path: 'BaseAmountSelection',
        name: 'Bet_BaseAmountSelection',
        components: {
          nav: Views.BetViews.Nav.DefaultView,
          dialog: Views.BetViews.Dialogs.BaseAmountSelectionView
        }
      },
      {
        path: 'BaseAmountCustom',
        name: 'Bet_BaseAmountCustom',
        components: {
          nav: Views.BetViews.Nav.DefaultView,
          dialog: Views.BetViews.Dialogs.BaseAmountCustomView
        }
      }
    ]
  },
  {
    path: '/Description',
    name: 'Description',
    component: Views.DescriptionView
  },
  {
    path: '/OrderHistory',
    name: 'OrderHistory',
    component: Views.OrderHistoryView
  },
  {
    path: '/IssueHistory',
    name: 'IssueHistory',
    component: Views.IssueHistoryView
  },
  {
    path: '/ConfirmBet',
    name: 'ConfirmBet',
    component: Views.ConfirmBetView
  },
  {
    path: '/ReConfirmBet',
    name: 'ReConfirmBet',
    component: Views.ReConfirmBetView
  },
  {
    path: '/FollowPlan',
    name: 'FollowPlan',
    component: Views.FollowPlanView
  },
  {
    path: '/LongDragonPlan',
    name: 'LongDragonPlan',
    component: Views.LongDragonPlanView
  }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes
});

export default router;

declare module 'vue-router' {
  interface RouteMeta {
    useSmallIcon?: boolean
  }
}