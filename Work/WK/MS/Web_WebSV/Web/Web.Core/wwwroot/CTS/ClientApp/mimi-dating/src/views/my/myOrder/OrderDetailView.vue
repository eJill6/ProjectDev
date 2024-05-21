<template>
  <div class="main_container ">
         <div class="main_container_flex">
             <!-- Header start -->
             <header class="header_height1 solid_color solid_line">
                 <div class="header_back" @click="navigateToPrevious">
                     <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt=""/></div>
                 </div>
                 <div class="header_title">  {{ newOrderStatus(itemInfo.status) }}</div>
             </header>
             <!-- Header end -->


             <!-- 共同滑動區塊 -->
             <div class=" flex_height bg_personal" v-if="itemInfo">
                 <div class="overflow no_scrollbar"   v-if="Object.keys(itemInfo).length !== 0">
                     <div class="padding_order ">
                         <div class="me_order">
                             <div class="user_info user_flex">
                                 <div class="avatar_second avatar_fit">
                                     <!-- <CdnImage :src="isImgLoadError ? defaultAvatarUrl : itemInfo.post.avatarUrl" v-on:error="setIsImgLoadError" alt="" /> -->
                                      <CdnImage :src="item.post.avatarUrl"  v-if="item.post.avatarUrl?.indexOf('aes') < 0"/>
                                      <AssetImage :item="setAvatarImageItem(item.post)" v-else />
                                 </div>
                                 <div class="info">
                                     <div class="first_co">
                                         <div class="basic">
                                             <div class="name">{{ itemInfo.post.nickname }}</div>
                                         </div>

                                     </div>
                                   
                                 </div>
                                <!-- state -->
                                 <div class="state">
                                     <div :class="{text_red: isTextRed()}"> {{ newOrderStatus(itemInfo.status) }}</div>
                                 </div>
                             </div>
                             <div class="order_info order_flex">
 
                                 <div class="reserve_info">
                                     <div>预约贴标题：</div>
                                     <div>  {{ itemInfo.post.title }}</div>
                                 </div>
                                 <div class="reserve_info">
                                     <div>下单时间：</div>
                                     <div>{{ itemInfo.bookingTime }}</div>
                                 </div>
                                 <div class="reserve_info">
                                     <div>{{ itemInfo.paymentStatus }}:</div>
                                     <div>{{ itemInfo.paymentMoney }}</div>
                                 </div>
                                
                         </div>
                        </div>
                         <!-- 退款中 -->
                         <div class="me_order" v-if="isShowBeingRefunded()">
                            <div class="reject_order">
                                <h1 class="text_bold">退款进度</h1>
                                <div class="order_info order_flex">
                                    <div class="refund_progress">
                                        <div class="hollow"></div>
                                        <div style="color: #BA9B5E;">申请退款</div>
                                    </div>
                                    <div class="progress_line">
                                        <div class="beeline"></div>
                                    </div>
                                    <div class="refund_progress">
                                        <div class="filled"></div>
                                        <div class="active">请等待管理员进行处理</div>
                                    </div>
                                    <div class="progress_line">
                                        <div class="beeline"></div>
                                    </div>
                                    <div class="refund_progress">
                                        <div class="filled_grey"></div>
                                        <div class="grey">退款成功</div>
                                    </div>
                                
                              </div>
                            </div>
                          </div>
                          <!-- 退款成功 -->
                          <div class="me_order" v-if="isAlreadyRefund()">
                            <div class="reject_order">
                                <h1 class="text_bold">退款进度</h1>
                                <div class="order_info order_flex">
                                    <div class="refund_progress">
                                        <div class="hollow"></div>
                                        <div style="color: #BA9B5E;">申请退款</div>
                                    </div>
                                    <div class="progress_line">
                                        <div class="beeline"></div>
                                    </div>
                                    <div class="refund_progress">
                                        <div class="hollow"></div>
                                        <div style="color: #BA9B5E;">请等待管理员进行处理</div>
                                    </div>
                                    <div class="progress_line">
                                        <div class="beeline"></div>
                                    </div>
                                    <div class="refund_progress">
                                        <div class="filled"></div>
                                        <div class="active">退款成功</div>
                                    </div>
                              </div>
                            </div>
                          </div>
                           <!-- 拒绝退款 -->
                          <div class="me_order" v-if="isRefusalOfRefund()">
                            <div class="reject_order">
                                <h1 class="text_bold">退款进度</h1>
                                <div class="order_info order_flex">
                                  <div class="refund_progress">
                                        <div class="hollow"></div>
                                        <div style="color: #BA9B5E;">申请退款</div>
                                    </div>
                                    <div class="progress_line">
                                        <div class="beeline"></div>
                                    </div>
                                    <div class="refund_progress">
                                        <div class="hollow"></div>
                                        <div style="color: #BA9B5E;">请等待管理员进行处理</div>
                                    </div>
                                    <div class="progress_line">
                                        <div class="beeline"></div>
                                    </div>
                                    <div class="refund_progress">
                                        <div class="filled"></div>
                                        <div class="active">拒绝退款</div>
                                    </div>
                                  <div class="reject_detail">{{itemInfo.memo}}</div>
                              </div>
                            </div>
                          </div>
                          

                         <div class="me_order favorite_pd">
                             <div class="favorite">
                                 <h1 class="text_bold">您可能喜欢</h1>
                                 <div class="card_group scroll-x no_scrollbar">
                                     <ul>
                                     <li class="card"  v-for="info in officialList" @click="navigateToOfficialDetail(info.postId)">
                                         <div class="photo_area">
                                             <div class="photo"> <AssetImage :item="setRecommendImageItem(info)" /></div>
                                             <div class="location_tag">
                                                 <div><CdnImage src="@/assets/images/card/ic_card_location.svg" alt=""/>{{ cityName(info.areaCode) }}</div>
                                             </div>
                                         </div>
                                         <div class="title">{{ info.title }}</div>
                                     </li>

                                     
                                 </ul>
                                     
                                    
                                 </div>
                             </div>
                             
                         </div>
                       
                     </div>
                     <div class="bottom_btn_order">
                         <div class="btn_more"></div>
                         <div class="btn_grop">

                                 <div class="btn" v-if="isPrivateButton()" @click="sendMessage(item.post.userId,item.post.nickname,item.post.avatarUrl)">私信</div>
                                 <div class="btn" v-if="isRefund()"        @click="checkRefund(item)">申请退款</div>
                                 <div class="btn" v-if="isOrderAgain()"    @click="toAgain(item)">再次下单</div>
                                 <div class="btn" v-if="isOther()"         @click="navigateToHome">查看其他</div>
                                 <div class="btn" v-if="isFinish()"        @click="toFinish(item)">确认完成</div>
                                 <!-- <div class="btn" v-if="isRefundProgress(item.status)" @click="navigateToOrderDetail(item.bookingId)">退款进度</div> -->
                         </div>
                     </div>
                     <!-- <div class="delete_order">
                         <div class="btn">删除订单</div>
                     </div> -->
                 </div>
                 
             </div>
             
         </div>

  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { ImageCacheManager, MyOrderTools } from "@/mixins";
import api from "@/api";
import {
ImageItemModel,
OfficialListModel,
OfficialSearchModel,
ResMyBookingDetailModel,
MediaResultModel,
ResMyBookingPostModel
} from "@/models";
import { OfficialPostStatus,MyBookingStatusType } from "@/enums";
import { AssetImage, CdnImage } from "@/components";
import { MutationType } from "@/store";
import toast from "@/toast";

export default defineComponent({
components: { AssetImage, CdnImage },
mixins: [ImageCacheManager, MyOrderTools],
data() {
 return {
   showDeleteButton: false,
   item: {} as ResMyBookingDetailModel,
   list: [] as OfficialListModel[],
   showMore: false,
   overStatusButtonMaxCount: 3,
   isImgLoadError: false,
   defaultAvatarUrl: require('@/assets/images/card/avatar_default.svg'),
   bookingStatus:MyBookingStatusType
 };
},
methods: {
  sendMessage(userId:string,nickName:string,avatarUrl:string){
    if(this.userInfo.userId===Number.parseInt(userId))
    {
      toast("不能给自己发送私信");
      return;
    }
    this.navigateToPrivateDetail(userId, nickName, avatarUrl);
  },
  isPrivateButton(){
     if(this.item.status!=MyBookingStatusType.InService){
      return false;
     }else{
      return this.item.status===MyBookingStatusType.InService || this.item.refusalOfRefund;
     }
  },
  isRefund(){
    return this.item.status===MyBookingStatusType.InService  && !this.item.refusalOfRefund;
  },
  isFinish(){
    return this.item.status===MyBookingStatusType.InService;
  },
  isOther(){
    return  this.item.status===MyBookingStatusType.Refunded || this.item.status===MyBookingStatusType.Completed || this.item.status===MyBookingStatusType.Refunding;
  },
  isOrderAgain(){
    return this.item.status===MyBookingStatusType.Completed;
  },

 async $_loadData() {
   await this.getBookingDetail();
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
 async getBookingDetail() {
   if (this.isLoading) return;
   this.$store.commit(MutationType.SetIsLoading, true);
   try {

     this.item = await api.getMyBookingDetail(this.bookingId);

     if(this.item.post.avatarUrl.indexOf("aes")>-1){
        let list:MediaResultModel[]=[];
        let media:MediaResultModel={
            id:this.item.post.userId,
            fullMediaUrl:this.item.post.avatarUrl,
        };

        list.push(media);
        this.officialShopImage(list);
     }
     

   } catch {
      toast("帖子不存在或者处于审核状态!");
      setTimeout(()=>{
        this.navigateToPrevious();
      },1000);

   } finally {
     this.$store.commit(MutationType.SetIsLoading, false);
     this.loadRecommend();
   }
 },
 async loadRecommend() {
   if (this.isLoading) return;
   try {
     this.$store.commit(MutationType.SetIsLoading, true);
     const pageSize = 30;
     const search: OfficialSearchModel = {
       pageNo: 1,
       pageSize: pageSize,
       areaCode: this.item.post.areaCode,
       age: [],
       height: [],
       cup: [],
       price: [],
       serviceIds: [],
       ts: "",
       status:OfficialPostStatus.blanking,
       isDelete:-1
     };

     const result = await api.getOfficialProductList(search);
     this.officialListDownload(result.data);
     this.list = result.data;
   } catch (error) {
     console.error(error);
   } finally {
     this.$store.commit(MutationType.SetIsLoading, false);
   }
 },
 setIsImgLoadError(){
   this.isImgLoadError = true;
 },
 isTextRed()
 {
    return this.itemInfo.status===MyBookingStatusType.Refunded;
 },
 isRefusalOfRefund(){
  return this.itemInfo.refusalOfRefund && this.itemInfo.status!=MyBookingStatusType.Refunded;
 },
 isAlreadyRefund(){
  return this.itemInfo.status === MyBookingStatusType.Refunded ;
 },
 isShowBeingRefunded(){
   return this.itemInfo.status === MyBookingStatusType.Refunding;
 },
 isShowActive(){
   return this.itemInfo.status === (MyBookingStatusType.Refunding && (MyBookingStatusType.Refunded) ) ?"active":"grey";
 },
 isShowFilled(){
   return this.itemInfo.status === MyBookingStatusType.Refunding?"filled":"filled_grey";
 },
 refundStatusText(item:ResMyBookingDetailModel)
 {
     var result="";

     if(item.refusalOfRefund)
     {
      result="拒绝退款";
     }else
     {

      switch(item.status)
        {

          case MyBookingStatusType.Refunding:
          result= "退款中";
            break;
          case MyBookingStatusType.Refunded:
          result= "已退款";
            break;
        }

     }

     return result;
 },

 setRecommendImageItem(info: OfficialListModel): ImageItemModel {
   return {
     id: info.postId,
     subId: info.coverUrl,
     class: "",
     src: info.coverUrl,
     alt: "",
   };
 },
 showDeleteButtonAction() {
   if (!this.showMore) return;
   this.showDeleteButton = !this.showDeleteButton;
 },
},
async mounted() {
 await this.getBookingDetail();
},
computed: {
 bookingId() {
   return (this.$route.query.bookingId as unknown as string) || "";
 },
 isLoading() {
   return this.$store.state.isLoading;
 },
 officialList() {
   return this.list;
 },
 itemInfo() {
   return this.item;
 },
 showExtraButtonString() {
   return this.showMore ? "更多" : "";
 }
},
});
</script>