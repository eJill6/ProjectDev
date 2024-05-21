import { createRouter, createWebHashHistory, RouteRecordRaw } from "vue-router";
import views from "@/views";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Index",
    component: views.NavigationView,
    redirect: { name: "Home" },
    children: [
      {
        path: "/Home",
        name: "Home",
        component: views.Pages.HomeView,
      },

      {
        path: "/Square",
        name: "Square",
        component: views.Pages.SquareView,
      },
      {
        path: "/Agency",
        name: "Agency",
        component: views.Pages.AgencyView,
      },
    ],
  },
  {
    path: "/Detai/:postId",
    name: "Detail",
    component: views.SubPages.ProductDetailView,
  },
  {
    path: "/Prevent",
    name: "Prevent",
    component: views.SubPages.PreventFraudView,
  },
  {
    path: "/Comment/:commentId",
    name: "Comment",
    component: views.SubPages.CommentView,
  },
  {
    path: "/Complaint/:postId",
    name: "Complaint",
    component: views.SubPages.ComplaintView,
  },
  {
    path: "/Introduction",
    name: "Introduction",
    component: views.Publish.IntroductionView,
  },
  {
    path: "/PublishRule",
    name: "PublishRule",
    component: views.Publish.PublishRuleView,
  },
  {
    path: "/From",
    name: "From",
    component: views.Publish.ProductFromView,
  },  
  {
    path: "/Apply",
    name: "Apply",
    component: views.Publish.ApplyView,
  },
  {
    path: "/My",
    name: "My",
    component: views.My.MyView,
  },
  {
    path: "/MyUnLock",
    name: "MyUnLock",
    component: views.My.MyUnLockView,
  },
  {
    path: "/MyPost",
    name: "MyPost",
    component: views.My.Post.MyPostView,
  },
  {
    path: "/MyOverview",
    name: "MyOverview",
    component: views.My.Post.MyOverviewView,
  },
  {
    path: "/MyOrder",
    name: "MyOrder",
    component: views.My.MyOrderView,
  },
  {
    path: "/MyOrder",
    name: "MyOrder",
    component: views.My.MyOrderView,
  },
  {
    path: "/Member",
    name: "Member",
    component: views.Member.MemberView,
  },
  {
    path: "/VipHistory",
    name: "VipHistory",
    component: views.Member.VipHistoryView,
  },
  {
    path: "/Wallet",
    name: "Wallet",
    component: views.Wallet.WalletView,
  },
  {
    path: "/ProfitDetail",
    name: "ProfitDetail",
    component: views.Wallet.ProfitDetailView,
  },
  {
    path: "/ProfitRule",
    name: "ProfitRule",
    component: views.Wallet.ProfitRuleView,
  },
  {
    path: "/PaymentHistory",
    name: "PaymentHistory",
    component: views.Wallet.PaymentHistoryView,
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
