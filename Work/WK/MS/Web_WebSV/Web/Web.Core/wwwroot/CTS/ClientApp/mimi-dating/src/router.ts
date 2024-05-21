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
      {
        path: "/Official",
        name: "Official",
        component: views.Pages.OfficialView,
      },
      {
        path: "/Experience",
        name: "Experience",
        component: views.Pages.ExperienceView,
      },
    ],
  },
  {
    path: "/Detail",
    name: "Detail",
    component: views.SubPages.ProductDetailView,
  },
  {
    path: "/OfficialDetail",
    name: "OfficialDetail",
    component: views.SubPages.OfficialDetailView,
  },
  {
    path: "/OfficialSearch",
    name: "OfficialSearch",
    component: views.SubPages.OfficialSearchView,
  },
  {
    path: "/OfficialShopDetail",
    name: "OfficialShopDetail",
    component: views.SubPages.OfficialShopDetailView,
  },
  {
    path: "/Prevent",
    name: "Prevent",
    component: views.SubPages.PreventFraudView,
  },
  {
    path: "/Comment",
    name: "Comment",
    component: views.SubPages.CommentView,
  },
  {
    path: "/OfficialComment",
    name: "OfficialComment",
    component: views.SubPages.OfficialCommentView,
  },
  {
    path: "/Complaint",
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
    path: "/OfficialFrom",
    name: "OfficialFrom",
    component: views.Publish.OfficialFromView,
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
    path: "/MyOrder",
    name: "MyOrder",
    component: views.My.MyOrderView,
  },

  // {
  //   path: "/Member",
  //   name: "Member",
  //   component: views.Member.MemberView,
  // },
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
  {
    path: "/Private",
    name: "Private",
    component: views.SubPages.PrivateMessageView,
  },
  {
    path: "/PrivateDetial",
    name: "PrivateDetail",
    component: views.SubPages.PrivateMessageChatView,
  },
  {
    path: "/OfficialPayment",
    name: "OfficialPayment",
    component: views.SubPages.OfficialPaymentView,
  },
  {
    path: "/ImageDetail",
    name: "ImageDetail",
    component: views.SubPages.ImageDetailView,
  },
  {
    path: "/Overview",
    name: "Overview",
    component: views.My.MyPost.OverviewView,
    meta: {
      keepAlive: false,
    },
  },
  {
    path: "/BossShopOverView",
    name: "BossShopOverView",
    component: views.My.myMBoosShop.BossShopOverView,
    meta: {
      keepAlive: false,
    },
  },

  {
    path: "/MyMessage",
    name: "MyMessage",
    component: views.My.MyMessage.MessageView,
  },

  {
    path: "/Announcement",
    name: "Announcement",
    component: views.My.MyMessage.AnnouncementView,
  },
  {
    path: "/CollectView",
    name: "CollectView",
    component: views.My.myCollect.CollectView,
  },
  {
    path: "/ComplaintPost",
    name: "ComplaintPost",
    component: views.My.MyMessage.ComplaintPostView,
  },
  {
    path: "/Refund",
    name: "Refund",
    component: views.My.MyOrder.RefundView,
  },
  {
    path: "/OrderDetail",
    name: "OrderDetail",
    component: views.My.MyOrder.OrderDetailView,
  },
  // {
  //   path:"/applyBoss",
  //   name:"applyBoss",
  //   component:views.Publish.ApplyView,
  // }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

export default router;
