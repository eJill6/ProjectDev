import { PostType } from "@/enums";
import { MutationType } from "@/store";
import { defineComponent } from "vue";

export default defineComponent({
  methods: {
    navigateToSquare() {
      this.$router.push("Square");
    },
    navigateToAgency() {
      this.$router.push("Agency");
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
        params: {
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
      this.$router.push({ name: "Member" });
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
    navigateToMyPost() {
      this.$router.push({ name: "MyPost" });
    },
    navigateToMyOverview() {
      this.$router.push({ name: "MyOverview" });
    },
    navigateToMyUnLock() {
      this.$router.push({ name: "MyUnLock" });
    },
    navigateToIntroduction() {
      this.$router.push({ name: "Introduction" });
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
        params: {
          commentId: commentId,
        },
      });
    },
    navigateToPrevent() {
      this.$router.push({ name: "Prevent" });
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
        params: {
          postId: postId,
        },
      });
    },
  },
});
