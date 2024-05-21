import { defineComponent } from "vue";
import ScrollManager from "./ScrollManager";
import { NavigateRule, Tools, DialogControl } from "@/mixins";
import api from "@/api";
import {
  ImageItemModel,
  MessageDialogModel,
  ResMyBookingModel,
} from "@/models";
import {ReviewStatusType,MyBookingStatusType, IdentityType } from "@/enums";
import toast from "@/toast";

export default defineComponent({
  mixins: [NavigateRule, Tools, DialogControl, ScrollManager],
  data() {
      return{
       
      }
  },
  methods: {
    ///共用函數 start
      /// 服务中 待接单,服务中,拒绝退款                                                           btn:私信，申请退款，确认完成
      /// 退款中 申请退款中                                                                      btn:退款进度，查看其他，确认完成
      /// 已退款 退款成功                                                                        btn:查看其他
      /// 已完成 待评价,评价审核中,交易完成，评价审核未通过,订单已取消, 超时未接单，超时未接单处理中   btn:在次下单，查看其他
    ///  私信 
    isPrivateButton(status: MyBookingStatusType) {
      return status===MyBookingStatusType.InService;
    },
    ///  申请退款
    isRefund(status: MyBookingStatusType) {
      return status=== MyBookingStatusType.InService;
    },
    ///  确认完成
    isFinish(status: MyBookingStatusType) {
      return status=== MyBookingStatusType.InService;
    },
    ///  查看其他
    isOther(status: MyBookingStatusType) {
      return status===MyBookingStatusType.Completed || status===MyBookingStatusType.Refunded || status===MyBookingStatusType.Refunding ;
    },
    /// 再次下单
    isOrderAgain(status: MyBookingStatusType) {
      return status===MyBookingStatusType.Completed;
    },
    /// 退款进度
    isRefundProgress(status:MyBookingStatusType){
      return status===MyBookingStatusType.Refunding;
    },
    getBookingStatusType(status:MyBookingStatusType) {
        // if(this.InServiceArray.includes(status)) return MyBookingStatusType.InService;
        // else if(this.BeingRefundedArray.includes(status)) return MyBookingStatusType.Refunding;
        // else if(this.AlreadyRefundArray.includes(status)) return MyBookingStatusType.Refunded;
        // else return MyBookingStatusType.Completed;
    },
    toFinish(info: ResMyBookingModel) {

      if (info.status === MyBookingStatusType.Refunding) {
        toast("申请退款当中不允许确认完成");
        return;
      }
      const messageModel: MessageDialogModel = {
        message: "确认完成该笔订单?",
        cancelTitle: "",
        buttonTitle: "确认完成",
      };

     
      this.showMessageDialog(messageModel, async () => {
        try {
          var result= await api.appointmentDone(info.bookingId);
          this.resetScroll();
          this.resetPageInfo();
          this.$_loadData()
        } catch(e) {
          toast("您操作过于频繁,请稍后重试");
        }
      });
    },


    async checkRefund(info: ResMyBookingModel) {
   
      if(info.status===MyBookingStatusType.InService){
        this.navigateToRefund(info.bookingId);
      }
    },
    toComment(info: ResMyBookingModel) {
      this.navigateToOfficialComment(
        info.commentId || "",
        info.post.postId,
        info.bookingId
      );
    },
    async toAgain(info: ResMyBookingModel) {
      try {
        const postDetail = await api.getOfficialPostDetail(info.post.postId);
        if (
          Object.keys(postDetail).length <= 0 ||
          postDetail.postStatus == ReviewStatusType.UnderReview ||
          (postDetail.userIdentity != IdentityType.Boss && postDetail.userIdentity != IdentityType.SuperBoss)
        ) {
          toast("此贴已不存在");
        }
        else {
          this.navigateToOfficialDetail(info.post.postId);
        }
      } catch (e) {
        toast("此贴已不存在");
      }
    },
    async toDelete(info: ResMyBookingModel) {
      const messageModel: MessageDialogModel = {
        title: "确认删除订单?",
        message: "删除之后将无法恢复订单",
        cancelTitle: "",
        buttonTitle: "确认",
      };
      this.showMessageDialog(messageModel, async () => {
        try {
          await api.appointmentDelete(info.bookingId);
          this.resetScroll();
          this.resetPageInfo();
          this.navigateToPrevious();
        } catch {}
      });
    },
    // sendMessage(userId:string,nickName:string,avatarUrl:string){
    //    if(this.userInfo?.userId?.toString() === userId){
    //       toast("无法给自己发送私信");
    //       return;
    //    }
    //    this.navigateToPrivateDetail(userId.toString(), nickName, avatarUrl);
    // },
    setImageItem(info: ResMyBookingModel) {
      const subId = this.getImageID(info.post.coverUrl);
      let item: ImageItemModel = {
        id: info.post.postId,
        subId: subId,
        class: "",
        src: info.post.coverUrl,
        alt: "",
      };
      return item;
    },
    /// 共用函數 end
    async $_loadData() {},
  },
});
