<template>
  <div class="main_container ">
         <div class="main_container_flex">
             <!-- Header start -->
             <header class="header_height1 solid_color">
                 <div class="header_back" @click="navigateToPrevious">
                     <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt=""/></div>
                 </div>
                 <div class="header_title">我的订单</div>
             </header>
             <!-- Header end -->
             <!-- filter tab -->
             <div class="filter_tab hide_overflowx">
                 <ul>
                     <li  :class="{active: isActive(MyBookingStatusType.InService)}"   @click="bookingSelected(MyBookingStatusType.InService)" >
                         <div class="text">服务中</div>
                         <div class="tab_bottom_line"></div>
                     </li>

                     <li :class="{active: isActive(MyBookingStatusType.Refunding)}"   @click="bookingSelected(MyBookingStatusType.Refunding)">
                         <div class="text">退款中</div>
                         <div class="tab_bottom_line"></div>
                     </li>
                     <li :class="{active: isActive(MyBookingStatusType.Refunded)}"    @click="bookingSelected(MyBookingStatusType.Refunded)">
                         <div class="text">已退款</div>
                         <div class="tab_bottom_line"></div>
                     </li>
                     <li :class="{active: isActive(MyBookingStatusType.Completed)}"   @click="bookingSelected(MyBookingStatusType.Completed)">
                         <div class="text">已完成</div>
                         <div class="tab_bottom_line"></div>
                     </li>
                 </ul>
             </div>
             <!-- filter tab end -->
             <EmptyView :message="`还没有预约单~`" v-show="isEmpty"></EmptyView>

             <!-- 共同滑動區塊 -->
             <div class=" flex_height bg_personal" v-show="!isEmpty">
                 <div class="overflow no_scrollbar"   @scroll="onScroll"   ref="scrollContainer">
                  <div style="height: 8px;"></div>
                     <div class="padding_order "   :style="{'padding-top': scrollStatus.virtualScroll.paddingTop + 'px','padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px', }">

                         <!-- 服务中 start -->
                         <div class="me_order" v-for="item in orderList">
                           
                             <div class="top_info">
                                 <div class="user_info">
                                     <div class="avatar avatar_fit">
                                      <CdnImage :src="item.post.avatarUrl"  v-if="item.post.avatarUrl?.indexOf('aes') < 0"/>
                                      <AssetImage :item="setAvatarImageItem(item.post)" v-else />
                                     </div>
                                     <div class="info">
                                         <div class="first_co">
                                             <div class="basic">
                                                 <div class="name_second">  {{ item.post.nickname }}</div>
                                             </div>


                                         </div>

                                     </div>
                                     <div class="state">
                                         <!-- <div> {{ orderStatus(item.status, item.paymentStatus) }}</div> -->
                                         <div> {{ newOrderStatus(item.status) }}</div>
                                     </div>
                                 </div>
                             </div>
                            


                             <div class="order_info"    @click="navigateToOrderDetail(item.bookingId)">
                                 <div class="photo">
                                     <AssetImage :item="setImageItem(item)" />
                                 </div>
                                 <div class="content">
                                     <div class="name">   {{ item.post.title }}</div>
                                     <div class="city">{{cityName(item.post.areaCode)}}</div>
                                     <div class="payment">
                                         <div>{{ item.paymentStatus }}：</div>
                                         <div>{{ item.paymentMoney }}</div>
                                     </div>


                                 </div>

                             </div>
                             <div class="reserve_info_btn">
                                 <div class="btn" v-if="isPrivateButton(item.status)" @click="sendMessage(item.post)">私信</div>
                                 <div class="btn" v-if="isRefund(item.status,item.refusalOfRefund)" @click="checkRefund(item)">申请退款</div>
                                 <div class="btn" v-if="isOrderAgain(item.status)" @click="toAgain(item)">再次下单</div>
                                 <div class="btn" v-if="isOther(item.status)" @click="navigateToHome">查看其他</div>
                                 <div class="btn" v-if="isFinish(item.status)" @click="userToFinish(item)">确认完成</div>
                                 <div class="btn" v-if="isRefundProgress(item.status)" @click="navigateToOrderDetail(item.bookingId)">退款进度</div>
                             </div>
                         </div>
                     </div>
                 </div>
             </div>
         </div>
     </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {
ScrollManager,
VirtualScroll,
ImageCacheManager,
MyOrderTools,
PlayGame
} from "@/mixins";
import { MyBookingStatusType } from "@/enums";
import api from "@/api";
import { MyBookingOfficialModel, ResMyBookingModel,ImageItemModel,ResMyBookingPostModel,MediaResultModel,MessageDialogModel } from "@/models";
         
import { MutationType } from "@/store";
import { AssetImage, EmptyView, CdnImage } from "@/components";
import toast from "@/toast";

export default defineComponent({
mixins: [ScrollManager, VirtualScroll, ImageCacheManager, MyOrderTools, PlayGame],
components: { AssetImage, EmptyView, CdnImage },
data() {
 return {
   isEmpty: false,
   MyBookingStatusType,
   isImgLoadError: false,
   defaultAvatarUrl: require('@/assets/images/card/avatar_default.svg')
 };
},
methods: {
  sendMessage(model: ResMyBookingPostModel){
    const postModel = this.orderList.filter(x => x.post == model);
    if(this.userInfo.userId == Number.parseInt(model.userId))
    {
      toast("不能给自己发送私信");
    } else if(postModel && postModel.length > 0 && !postModel[0].post.canMessage) {
      toast("订单结束已清除对话");
    } else {
      this.navigateToPrivateDetail(model.userId, model.nickname, model.avatarUrl);
    }
  },
  async userToFinish(info: ResMyBookingModel){
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
        try{
          this.$store.commit(MutationType.SetIsLoading, true);
          await api.appointmentDone(info.bookingId);
          info.post.canMessage = false;
        }
        catch(e)
        {
          toast("频繁操作,请稍后重试");
        }
        finally
        {
          this.$store.commit(MutationType.SetIsLoading, false);
          await this.reload();
        }
      });

     
      
  },
  ///  申请退款
  isRefund(status: MyBookingStatusType,refusalOfRefund:boolean) {
        return status=== MyBookingStatusType.InService && !refusalOfRefund;
  },
  async $_loadData() {
    await this.reload();
  },
  async bookingSelected(type: MyBookingStatusType) {
    this.$store.commit(MutationType.SetMyOrderSelectPageType, type);
    await this.reload();
  },
  isActive(type: MyBookingStatusType) {
    return this.pageType === type;
  },
  setIsImgLoadError(){
    this.isImgLoadError = true;
  },

  setAvatarImageItem(model: ResMyBookingPostModel) {
      let item: ImageItemModel = {
        id: model.userId,
        subId: model.avatarUrl,
        class: "",
        src: model.avatarUrl,
        alt: "",
      };
      return item;
  },
  async reload() {
    this.resetPageInfo();
    this.resetScroll();
    await this.loadAsync();
  },
  async loadAsync() {
    if (
      this.isLoading ||
      this.scrollStatus.totalPage === this.pageInfo.pageNo || this.isPromising
    )
      return;

    try {
      this.$store.commit(MutationType.SetIsLoading, true);

      const nextPage = this.pageInfo.pageNo + 1;
      let body: MyBookingOfficialModel = {
        status: this.pageType,
        pageNo: nextPage,
      };
      const result = await api.getMyBooking(body);
      
      let orderDetailList = result.data.map((item) => item.post);

      let list:MediaResultModel[]=[];
      orderDetailList.map(c=> {
        if(c.avatarUrl.indexOf(".aes")>-1)
        {
          let media:MediaResultModel={
            id:c.userId,
            fullMediaUrl:c.avatarUrl,
          }
          list.push(media);
        }
        c.canMessage = true;
      });

      this.officialShopImage(list);
      
      this.orderImageDownload(orderDetailList);
      this.scrollStatus.totalPage = result.totalPage;
      this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
      this.isEmpty = !this.scrollStatus.list.length;
      this.pageInfo.pageNo = result.pageNo;
    } catch (e) {
      toast(e);
    } finally {
      this.$store.commit(MutationType.SetIsLoading, false);
    }
    this.calculateVirtualScroll();
  },

  // $_onScrollReload() {
  //   this.reload();
  // },
  $_onScrollToBottom() {
    this.loadAsync();
  },
  
  async checkDetailToReturn() {
    if (!Object.keys(this.scrollStatus.virtualScroll).length || this.scrollStatus.list.length === 0) {
      this.resetScroll();
      await this.loadAsync();
    }
  },
},
created(){
   this.reload();
   if (this.checkUserEmpty) {
    this.setUserInfo();
   }
},
computed: {
 $_virtualScrollItemElemHeight() {
   return 127;
 },
 isLoading() {
   return this.$store.state.isLoading;
 },
 orderList() {
   return this.scrollStatus.virtualScroll.list as ResMyBookingModel[];
 },
 pageType()
 {
  return this.$store.state.myOrderSelectPageType;
 }
},
});
</script>