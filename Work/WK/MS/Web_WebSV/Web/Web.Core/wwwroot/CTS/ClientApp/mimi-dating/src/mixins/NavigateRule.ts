import { MyPostType, PostType,MyMessageViewType } from "@/enums";
import { MutationType } from "@/store";
import { defineComponent } from "vue";
import PlayGame from "./PlayGame";
import {
  MyMessageListModel
} from "@/models";

export default defineComponent({
  mixins: [ PlayGame],
  methods: {
    navigateToSquare() {
      this.$router.push("Square");
    },
    navigateToAgency() {
      this.$router.push("Agency");
    },
    navigateToOfficial() {
      this.$router.push("Official");
    },
    navigateToExperience() {
      this.$router.push("Experience");
    },
    navigateToHome() {
      this.$router.push({ name: "Home" });
    },
    navigateToRule() {
      this.$router.push({ name: "PublishRule" });
    },
    navigateToComplaint(postId: string) {
      this.$router.push({
        name: "Complaint",
        query: {
          postId,
        },
      });
    },
    navigateToMy() {
      this.$router.push({ name: "My" });
    },
    navigateToWallet() {
      this.$router.push({ name: "Wallet" });
    },
    navigateToMember() {
      // toast("会员中心正在升级中，更多特权，敬请期待");
      this.goVipCenter();
      // this.$router.push({ name: "Member" });
    },
    navigateToVipHistory() {
      this.$router.push({ name: "VipHistory" });
    },
    navigateToPaymentHistory() {
      this.$router.push({ name: "PaymentHistory" });
    },
    navigateToProfitDetail() {
      this.$router.push({ name: "ProfitDetail" });
    },
    navigateToProfitRule() {
      this.$router.push({ name: "ProfitRule" });
    },
    navigateToOverview() {
      this.$store.commit(MutationType.SetMyPostViewName, MyPostType.MyOverview);
      this.$router.push({ name: "Overview" });
    },
    navigateToMyUnLock() {
      this.$router.push({ name: "MyUnLock" });
    },
    navigateToMyMessage(){
      this.$router.push({ name: "MyMessage" });
    },
    navigateToMBossShop(){
      this.$router.push({ name: "BossShopOverView" });
    },
    navigateToMyCollect(){
      this.$router.push({ name: "CollectView" });
    },
    navigateToMyMessageDetail(item:MyMessageListModel){
    
      this.$router.push({
        name: item.messageType==MyMessageViewType.Announcement?"Announcement":"ComplaintPost",
        query: {
          messageId: item.id,
          isRed:item.isRead?1:0
        },
      });
      
    },
    navigateToApplyBossShopEdit(applyId:string)
    {
      this.$router.push({
        name: "Introduction", 
        query: {
          applyId: applyId,
        },
      });
    },
    navigateToIntroduction() {
      this.$router.push({ name: "Introduction" });
    },
    navigateToMyOrder() {
      this.$router.push({ name: "MyOrder" });
    },
    navigateToApplyBoss(){
      this.$router.push({ name: "Apply" });
    },
    navigateToPrevious() {
      const isGoHome =
        this.$router?.options?.history?.state?.back !== null &&
        this.$router?.options?.history?.state?.back !== undefined &&
        this.$router?.options?.history?.state?.back !== "" &&
        this.$router?.options?.history?.state?.back !== "/Home";

      if (isGoHome) {
        this.$router.go(-1);
      } else {
        this.navigateToHome();
      }
    },
    navigateToComment(commentId: string) {
      this.$router.push({
        name: "Comment",
        query: {
          commentId: commentId,
        },
      });
    },
    navigateToOfficialComment(
      commentId: string,
      postId: string,
      bookingId: string
    ) {
      this.$router.push({
        name: "OfficialComment",
        query: {
          commentId: commentId,
          postId: postId,
          bookingId: bookingId,
        },
      });
    },
    navigateToPrevent() {
      this.$router.push({ name: "Prevent" });
    },
    // navigateToAppointment() {
    //   this.resetPageInfo();
    //   this.resetScroll();
    //   this.$router.push({ name: "Appointment" });
    // },
    navigateToPrivate() {
      this.$router.push({
        name: "Private"
      });
    },
    navigateToOfficialPayment(postId: string, userIdentity: number) {
      this.$router.push({
        name: "OfficialPayment",
        query: {
          postId: postId,
          userIdentity: userIdentity
        },
      });
    },
    navigateToFrom(postType: PostType, postId?: string) {
      this.$router.push({
        name: "From",
        query: {
          postId: postId,
          postType: postType,
        },
      });
    },
    navigateToOfficialFrom(postId?: string) {
      this.$router.push({
        name: "OfficialFrom",
        query: {
          postId: postId,
          postType: PostType.Official,
        },
      });
    },
    navigateToRefund(bookingId: string) {
      this.$router.push({
        name: "Refund",
        query: {
          bookingId: bookingId,
        },
      });
    },
    navigateToOrderDetail(bookingId: string) {
      this.$router.push({
        name: "OrderDetail",
        query: {
          bookingId: bookingId,
        },
      });
    },
    navigateToApply(title: string) {
      this.$router.push({
        name: "Apply",
        query: {
          title: title,
        },
      });
    },
    navigateToProductDetail(postId: string) {
      this.$store.commit(MutationType.SetBanner, []);
      this.$router.push({
        name: "Detail",
        query: {
          postId: postId,
        },
      });
    },
    navigateToOfficialDetail(postId: string) {
      this.$store.commit(MutationType.SetBanner, []);
      this.$router.push({
        name: "OfficialDetail",
        query: {
          postId: postId,
        },
      });
    },
    navigateToOfficialSearch(keyword: string) {
      this.$router.push({
        name: "OfficialSearch",
        query: {
          keyword: keyword,
        },
      });
    },
    navigateToOfficialShopDetail(applyId: string){
      this.$router.push({
        name: "OfficialShopDetail",
        query: {
          applyId: applyId,
        },
      });
    },
    navigateToPrivateDetail(roomId: string, shopName?: string, shopAvatar?: string){
      this.$store.commit(MutationType.SetPrivateMessageAvatar, shopAvatar);
      this.$store.commit(MutationType.SetPrivateMessageShopName, shopName);
      this.$router.push({
        name: "PrivateDetail",
        query: {
          roomId: roomId,
        },
      });
    }
  },
});
