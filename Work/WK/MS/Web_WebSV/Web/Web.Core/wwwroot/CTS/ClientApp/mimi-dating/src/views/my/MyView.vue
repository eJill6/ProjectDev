<template>
  <div class="main_container bg_list">
         <div class="main_container_flex">
             <!-- Header start -->
             <header class="header_height1">
                 <div class="header_back"  @click="navigateToHome">
                     <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt=""/></div>
                 </div>
                 <div class="header_title">个人中心</div>
             </header>
             <!-- Header end -->
             <!-- filter tab -->
             <div class="header_line">
                 <CdnImage src="@/assets/images/public/pic_line.png" alt=""/>
             </div>
             <!-- filter tab end -->

             <!-- 共同滑動區塊 -->
             <div class=" flex_height">
                 <div class="overflow no_scrollbar">
                     <div class="padding_order padding_distance">
                         <!-- list 我的订单 -->
                         <div class="me_list"  @click="navigateToMyOrder">
                             <div class="list_info">
                                 <div class="icon">
                                     <CdnImage src="@/assets/images/public/ic_order.png" alt=""/>
                                 </div>
                                 <div class="content">
                                     <div class="item_title">我的订单</div>
                                     <div class="item_text">
                                         <div>正在预约中{{userInfo.bookingCount }}{{userInfo.bookingCount>99?"+":"" }}单</div>
                                     </div>
                                 </div>
                                 <div class="item_btn" @click="navigateToMyOrder">
                                     <div class=" text">查看更多</div>
                                 </div>
                             </div>
                         </div>
                         
                         <!-- list 我解锁的帖子 -->
                         <div class="me_list" @click="navigateToMyUnLock">
                             <div class="list_info">
                                 <div class="icon">
                                    <CdnImage src="@/assets/images/public/ic_unlock.png" alt="" />
                      
                                 </div>
                                 <div class="content">
                                     <div class="item_title">我解锁的帖子</div>
                                     <div class="item_text">
                                         <div>已解锁：</div>
                                         <div>{{ userInfo.unlockCount }}{{ userInfo.unlockCount>99?"+":"" }}</div>
                                     </div>
                                 </div>
                                 <div class="item_btn" @click="navigateToMyUnLock">
                                     <div class=" text">查看更多</div>
                                 </div>
                             </div>
                         </div>

                         <!-- list 我发布的帖子 -->
                         <div class="me_list" @click="navigateToOverview">
                             <div class="list_info">
                                 <div class="icon">
                                   
                                     <CdnImage src="@/assets/images/public/ic_release.png" alt=""/>
                                 </div>
                                 <div class="content">
                                     <div class="item_title">我发布的帖子</div>
                                     <div class="item_text">
                                         <div class="flex_box">
                                           <div>帖数：</div>
                                           <div>{{ userInfo.putOnShelvesCount }}</div>
                                         </div>
                                         <div class="flex_box">
                                           <div>本月收益：</div>
                                           <div>{{ userInfo.income }}</div>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="item_btn" @click="navigateToOverview">
                                     <div class=" text">查看更多</div>
                                 </div>
                             </div>
                         </div>

                         <!-- list 我的消息 -->
                         <div class="me_list" @click="navigateToMyMessage">
                             <div class="list_info">
                                 <div class="icon">
                                     <CdnImage src="@/assets/images/public/ic_info.png" alt=""/>
                                 </div>
                                 <div class="content">
                                     <div class="item_title number_flex">
                                         我的消息<div class="number" v-if="userInfo.unreadMessage?.totalUnreadCount>0">{{showSumCountMessageCount()}}</div>
                                     </div>
                                     <div class="item_text">
                                         <div class="flex_box">
                                           <div>我的投诉：</div>
                                           <div>{{ showReportMessageCount() }}</div>
                                         </div>
                                         <div class="flex_box">
                                           <div>官方公告：</div>
                                           <div>{{ showAnnouncementMessageCount() }}</div>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="item_btn">
                                     <div class=" text">查看更多</div>
                                 </div>
                             </div>
                         </div>

                         <!-- list 我的个人收藏 -->
                         <div class="me_list" @click="navigateToMyCollect">
                             <div class="list_info">
                                 <div class="icon">
                                     <CdnImage src="@/assets/images/public/ic_collection.png" alt=""/>
                                 </div>
                                 <div class="content">
                                     <div class="item_title">我的个人收藏</div>
                                     <div class="item_text">
                                         <div class="flex_box">
                                           <div>广场：</div>
                                           <div>{{showCollectSquareCount()}}</div>
                                         </div>
                                         <div class="flex_box">
                                           <div>寻芳阁：</div>
                                           <div>{{ showCollectXfgCount() }}</div>
                                         </div>
                                         <div class="flex_box">
                                           <div>店铺：</div>
                                           <div>{{ showCollectShopCount() }}</div>
                                         </div>
                                     </div>
                                 </div>
                                 <div class="item_btn">
                                     <div class=" text">查看更多</div>
                                 </div>
                             </div>
                         </div>


                         <!-- list 觅老板的官方店铺 -->
                         <div class="me_list" @click="navigateToMBossShop" v-if="showBossShop()">
                             <div class="list_info">
                                 <div class="icon">
                                     <CdnImage src="@/assets/images/public/ic_shop.png" alt=""/>
                                 </div>
                                 <div class="content">
                                     <div class="item_title">觅老板的官方店铺</div>
                                     <div class="item_text">
                                         <div>申请成为觅老板，开通自己的官方店铺</div>
                                     </div>
                                 </div>
                                 <div class="item_btn">
                                     <div class=" text">查看更多</div>
                                 </div>
                             </div>
                         </div>

                     </div>
                 </div>
             </div>
         </div>
     </div>
</template>
<script lang="ts">
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { DialogControl, NavigateRule, PlayGame, ScrollManager } from "@/mixins";
import { IdentityType } from "@/enums";

export default defineComponent({
    components: { CdnImage },
    mixins: [DialogControl, NavigateRule, PlayGame, ScrollManager],
    async created() {
       
        this.resetPageInfo();
        this.resetScroll();

        await this.setUserInfo();
    },
    methods:{
        
        showBossShop(){
            return this.userInfo.identity === IdentityType.Boss || this.userInfo.identity === IdentityType.SuperBoss;
        },
        showSumCountMessageCount() {
            let sumCount:number=this.userInfo.unreadMessage?.totalUnreadCount;
            return sumCount>999?"999+":sumCount;
        },
        showAnnouncementMessageCount(){
            let count:number=this.userInfo.unreadMessage?.announcementCount;
            return count>999?"999+":count>99?"99+":count;
        },
        showReportMessageCount(){
            let count:number=this.userInfo.unreadMessage?.complaintCount;
            return count>999?"999+":count>99?"99+":count;
        },
        showCollectSquareCount() {   
            return this.userInfo.collectSquareCount>99?"99+":this.userInfo.collectSquareCount;
        },
        showCollectXfgCount(){
            return this.userInfo.collectXfgCount>99?"99+":this.userInfo.collectXfgCount;
        },
        showCollectShopCount(){
            return this.userInfo.collectShopCount>99?"99+":this.userInfo.collectShopCount;
        }      
        // showCollectSquareCount() {   
        //     let square=this.userInfo?.userFavorites.find(c=>c.favoriteEnum===UserFavoriteCategoryType.Square);
        //     let squareCount=0;
        //     squareCount=square?.favorites===undefined?0:square?.favorites;
        //     return squareCount>99?"99+":squareCount;
        // },
        // showCollectXfgCount(){
        //     let agency=this.userInfo?.userFavorites.find(c=>c.favoriteEnum===UserFavoriteCategoryType.Agency);
        //     let agencyCount=0;
        //     agencyCount=agency?.favorites===undefined?0:agency?.favorites;
        //     return agencyCount>99?"99+":agencyCount;
        // },
        // showCollectShopCount(){
        //     let shop=this.userInfo?.userFavorites.find(c=>c.favoriteEnum===UserFavoriteCategoryType.Shop);
        //     let shopCount=0;
        //     shopCount=shop?.favorites===undefined?0:shop?.favorites;
        //     return shopCount>99?"99+":shopCount;
        // }
    },
    computed:{
        
        // totalUnread()
        // {
        //     return this.userInfo.unreadMessage.totalUnread;
        // },
        // announcementUnreadCount()
        // {
        //     return this.userInfo.unreadMessage.announcementUnreadCount;
        // }
    }
});
</script>