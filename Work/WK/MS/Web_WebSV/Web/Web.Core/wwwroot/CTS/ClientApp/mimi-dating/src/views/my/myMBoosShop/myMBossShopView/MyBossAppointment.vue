<template>
    
<div class="me_filter hide_overflowx">
    <ul>
        <li :class="{active:isActive(orderStatus.type)}" v-for="orderStatus in headerTab" @click="selectOrderStatus(orderStatus.type)">
            <div class="text">{{orderStatus.name}}</div>
        </li>
    </ul>
</div>

<EmptyView :message="`暂无数据～`" v-show="isEmpty"></EmptyView>

<div class="flex_height" v-show="!isEmpty">
                                       
    <div class="overflow no_scrollbar" @scroll="onScroll" ref="scrollContainer">

        <div class="padding_order " :style="{'padding-top': scrollStatus.virtualScroll.paddingTop + 'px','padding-bottom':scrollStatus.virtualScroll.paddingBottom + 'px',}">

            <div class="me_order" v-for="item in postList" style="margin-top: 9px;">
                    <div class="user_info user_flex">
                        <div class="avatar_second avatar_fit">
                            <AssetImage :item="setImageItem(item)" />
                        </div>
                        <div class="info">
                            <div class="first_co">
                                <div class="basic">
                                    <div class="name">{{ item.nickname }}</div>
                                </div>

                            </div>
 
                        </div>
                        <div class="state">
                            <div :class="{text_red:isTextRed(item.status)}">{{ showStatusText(item.status) }}</div>
                        </div>
                    </div>
                    <div class="order_info order_flex">

                        <div class="reserve_info">
                            <div>预约贴标题：</div>
                            <div>{{ item.title }}</div>
                        </div>
                        <div class="reserve_info">
                            <div>下单时间：</div>
                            <div>{{ item.bookingTime }}</div>
                        </div>
                        <div class="reserve_info">
                            <div>{{showBookingPaymentTypeText(item.paymentType,item.status)}}：</div>
                            <div>{{item.paymentMoney}}</div>
                        </div>
                </div>
  <!-- paymentType: BookingPaymentType; -->
            
                <div class="reserve_info_btn" v-if="isShowPrivateLetter(item.status)" @click="sendMessage(item.userId.toString(),item.nickname,item.avatarUrl)">
                    <div class="btn">私信</div>
                </div>
            </div>
    </div>
</div>

</div>

</template>

<script lang="ts">
import { defineComponent } from "vue";

import { NavigateRule, DialogControl, PlayGame,VirtualScroll,ImageCacheManager,MyOrderTools } from "@/mixins";
import { MyBossAppointmentType,ManageBookingStatusType,MyBookingStatusType,BookingPaymentType } from "@/enums";
import { MutationType } from "@/store";
import {
    BookingManagePostModel, BookingManageModel,ImageItemModel
} from "@/models";
import api from "@/api";
import toast from "@/toast";
import { EmptyView, AssetImage, CdnImage } from "@/components";

export default defineComponent({
    mixins: [NavigateRule, DialogControl, PlayGame,VirtualScroll,ImageCacheManager,MyOrderTools],
    components: {CdnImage ,EmptyView, AssetImage},
    data() {
        return{
            isEmpty: false,
            MyBossAppointmentType,
            headerTab:[
                {
                    type:MyBossAppointmentType.TakeOrders,
                    name:"接单中"
                },
                {
                    type:MyBossAppointmentType.Refunded,
                    name:"已退款"
                },
                {
                    type:MyBossAppointmentType.Accomplish,
                    name:"已完成"
                }
            ]
        }
    },
    methods:{
        sendMessage(userId:string,nickName:string,avatarUrl:string){
            if(this.userInfo.userId===Number.parseInt(userId))
            {
            toast("不能给自己发送私信");
            return;
            }
            this.navigateToPrivateDetail(userId, nickName, avatarUrl);
        },
        showStatusText(state:MyBookingStatusType)
        {
            if(state===MyBookingStatusType.InService || state===MyBookingStatusType.Refunding) return "接单中";
            else if(state===MyBookingStatusType.Refunded) return "已退款";
            else if(state===MyBookingStatusType.Completed) return "已完成";
        },
        isShowPrivateLetter(status:MyBookingStatusType)
        {
            return status===MyBookingStatusType.InService ||  status===MyBookingStatusType.Refunding;
        },
        privateMessage(){},
            async checkDetailToReturn() {
        },
        isActive(type: MyBossAppointmentType) {
            return this.currentOrderStatus === type;
        },
        isTextRed(type:number){
            return type===ManageBookingStatusType.RefundSuccessful;
        },
        selectOrderStatus(type:MyBossAppointmentType){
            this.$store.commit(MutationType.SetMyBossAppointmentName, type);
            this.reload();
        },
        setImageItem(info: BookingManageModel) {
            const subId = this.getImageID(info.avatarUrl);
            let item: ImageItemModel = {
                id: info.bookingId,
                subId: subId,
                class: "",
                src: info.avatarUrl,
                alt: "",
            };
            return item;
        },
        async loadAsync()
        {
            if (this.isLoading || this.scrollStatus.totalPage === this.pageInfo.pageNo)
                return;

            this.$store.commit(MutationType.SetIsLoading, true);
            try
            {
                let status=this.MyBossAppointmentTypeToManageBookingStatusType();
                const nextPage = this.pageInfo.pageNo + 1;
                const pageSize = 10;
                let body:BookingManagePostModel={
                    status:status,
                    pageNo: nextPage,
                    pageSize:pageSize,
                    ts:""
                }
                let result = await api.manageBooking(body);
                this.scrollStatus.totalPage = result.totalPage;
                this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
                this.manageBossBookingDownload(result.data);
                this.isEmpty = !this.scrollStatus.list.length;
                this.pageInfo.pageNo = result.pageNo;



            }
            catch(e){
                toast(e);
            }
            finally{
                this.$store.commit(MutationType.SetIsLoading, false);
            }
            this.calculateVirtualScroll();
        },
        async reload() {
            this.resetPageInfo();
            this.resetScroll();
            await this.loadAsync();
        },
        async $_onScrollToBottom() {
           await this.loadAsync();
        },
        MyBossAppointmentTypeToManageBookingStatusType()
        {
            let status;
            switch(this.currentOrderStatus)
            {
                case MyBossAppointmentType.Accomplish:
                    status= ManageBookingStatusType.TransactionCompleted;
                    break;
                case MyBossAppointmentType.Refunded:
                    status= ManageBookingStatusType.RefundSuccessful;
                    break;
                case MyBossAppointmentType.TakeOrders:
                    status=ManageBookingStatusType.InService;
                    break;
                default:
                    status=ManageBookingStatusType.InService;
                    break;
            }
            return status;
        },
        showBookingPaymentTypeText(type:BookingPaymentType,status:MyBookingStatusType)
        {
            if(status===MyBookingStatusType.Refunded){
                return type===BookingPaymentType.Booking?"已退还预约金":"已退还全额";
            }
            return type===BookingPaymentType.Booking?"已支付预约金":"已全额支付";
        }
    },
    async created(){
        await this.reload();
    },
    computed:{

        currentOrderStatus(){
            return this.$store.state.myBossShopAppointmentOrderStatus;
        }, 
        $_virtualScrollItemElemHeight() {
            return 71;
        },
        postList(){
            return this.scrollStatus.virtualScroll.list as BookingManageModel[];
        }

    }
})
</script>
