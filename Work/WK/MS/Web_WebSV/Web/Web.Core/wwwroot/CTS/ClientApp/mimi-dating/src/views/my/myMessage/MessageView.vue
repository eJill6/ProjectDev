<template>
     <div class="main_container ">
            <div class="main_container_flex bg_personal">
                <!-- Header start -->
                <header class="header_height1 solid_color">
                    <div class="header_back" @click="navigateToPrevious">
                        <div>  <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt=""/></div>
                    </div>
                    <div class="header_title">我的消息</div>
                </header>
                <!-- Header end -->

                <!-- filter tab -->
                <div class="filter_tab hide_overflowx">
                    <ul>
                        <li :class="{active:messageViewStatus===MyMessageViewType.ComplaintPost}" @click="changeMessageViewType(MyMessageViewType.ComplaintPost)">
                            <div class="text">已投诉帖子</div>
                            <div class="tab_bottom_line"></div>
                        </li>

                        <li :class="{active:messageViewStatus===MyMessageViewType.Announcement}" @click="changeMessageViewType(MyMessageViewType.Announcement)">
                            <div class="text">公告信息</div>
                            <div class="tab_bottom_line"></div>
                        </li>
                    </ul>
                </div>
                <!-- tab end -->
                <EmptyView :message="`暂无数据`" v-show="IsEmpty"></EmptyView>
                <!-- 共同滑動區塊 -->
                <div class=" flex_height" v-show="!IsEmpty">
                    <div class="overflow no_scrollbar" @scroll="onScroll" ref="scrollContainer">
                        <div class="padding_order " :style="{'padding-top': scrollStatus.virtualScroll.paddingTop + 'px','padding-bottom':scrollStatus.virtualScroll.paddingBottom + 'px',}">
                            <div style="height: 9px;"></div>
                            <div class="me_order me_order_second"   v-for="info in messageList"  @click="navigateToMyMessageDetail(info)" >
                                <div class="user_info user_flex">
                                    <div class="avatar_third avatar_fit" v-if="info.messageType===MyMessageViewType.ComplaintPost">
                                        <CdnImage src="@/assets/images/me/ic_me_24h.svg" alt="" />
                                    </div>
                                    <div class="avatar_third avatar_fit2" v-else>
                                        <CdnImage src="@/assets/images/me/ic_me_notify.svg" alt=""  />
                                    </div>
                                   
                                    <div class="info">
                                        <div class="first_co">
                                            <div class="basic">
                                                <div class="name_fourth">{{showTitle(info.messageTitle,info.messageType)}}</div>
                                            </div>
                                            <div class="basic">
                                                <div class="name_fourth">{{formatDate(info.publishTime)}}</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="state" v-if="!info.isRead && !userIsAllRead">
                                        <div class="text_red">●</div>
                                    </div>
                                </div>
                                <div class="add_text" v-html="info.messageContent">
                                  
                                </div>
                            </div>
                        </div>
                    </div>
                
                </div>
                <div class="sheet_btn_second" v-if="isShowAllRedAndAllDelete()">
                    <div class="btn_border_box" @click="userMessageOperation(MessageOperationType.UserRead)">
                        <div class="btn_read btn_read_bg">全部已读</div>
                    </div>
                    <div class="btn_border_box delete_bg" @click="userMessageOperation(MessageOperationType.UserDelete)">
                        <div class="btn_read btn_delete_bg">全部删除</div>
                    </div>
                </div>
        </div>
    </div>
</template>
<script lang="ts">
import { NavigateRule,DialogControl,VirtualScroll,PlayGame,ScrollManager,Tools,ImageCacheManager} from "@/mixins";

import {
  MyMessageListModel,
  MyMessageQueryModel,
  MessageOperationModel
} from "@/models";
import { defineComponent } from "vue";
import { MyMessageViewType,MessageOperationType } from '@/enums';
import {CdnImage, EmptyView, AssetImage} from '@/components'
import { MutationType } from "@/store";
import api from "@/api";
import toast from "@/toast";

export default defineComponent({
   components:{ EmptyView, AssetImage, CdnImage},
   mixins:[
    NavigateRule
   ,DialogControl
   ,VirtualScroll
   ,PlayGame
   ,ScrollManager
   ,Tools
   ,ImageCacheManager],
   data(){
    return{
        MyMessageViewType,
        MessageOperationType,
        isEmpty: false,
        redMessageIds:[] as string[],
        userIsAllRead:false,
    }
   },
   methods:{
    backEvent() {
      this.$store.commit(MutationType.SetMessageViewType, MyMessageViewType.Announcement);
      this.navigateToPrevious();
    },
    isShowAllRedAndAllDelete()
    {
        return this.messageViewStatus===MyMessageViewType.ComplaintPost;
    },
    isShow(model:MyMessageListModel)
    {
        return model.isDelete;
    },
    //全部或者已读
    async userMessageOperation(type:MessageOperationType){

     
        this.$store.commit(MutationType.SetIsLoading, true);

        const model:MessageOperationModel={
            messageIds:this.redMessageIds,
            messageType:this.messageViewStatus,
            messageOperationType:type
        }

        try{
            await api.userToMessageOperation(model);
        }
        catch(e)
        {
            toast(e);
        }
        finally
        {
            this.$store.commit(MutationType.SetIsLoading, false);

            if(type===MessageOperationType.UserDelete){
        
                this.scrollStatus.virtualScroll.list=[];
                
            }else{
                this.userIsAllRead=true;
            }
        }
     },
     showContent(strContent:string)
     {
        return strContent.length>23?strContent.substring(0,23)+"...":strContent;
     },
     showTitle(title:string,messageViewType:MyMessageViewType){

        if(messageViewType===MyMessageViewType.Announcement)
            return title;
        else
           return "投诉反馈回复";
    },
    changeMessageViewType(type:MyMessageViewType){
        this.$store.commit(MutationType.SetMessageViewType,type);
        this.reload();
    },
    reload()
    {
        this.resetScroll();
        this.resetPageInfo();
        this.loadAsync();
    },
    async loadAsync()
    {
        if (this.isLoading || this.scrollStatus.totalPage === this.pageInfo.pageNo) return;

        try{
            this.$store.commit(MutationType.SetIsLoading, true);
            const nextPage = this.pageInfo.pageNo + 1;
            const count = 20;

            const model: MyMessageQueryModel = {
                messageInfoType: this.messageViewStatus,
                pageNo: nextPage,
                pageSize: count,
                ts: ""
            };

            let result = await api.getMyMessageListRequest(model);
            this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
            this.scrollStatus.totalPage = result.totalPage;
            this.pageInfo.pageNo = result.pageNo;

        } catch (e) {
            toast(e);
        } finally {
            
            this.$store.commit(MutationType.SetIsLoading, false);
        }

        this.calculateVirtualScroll();
    },
    async $_onScrollToBottom() {
      await this.loadAsync();
    },
    formatDate(originalDate: string): string {
            const date = new Date(originalDate);
            const year = date.getFullYear();
            const month = (date.getMonth() + 1).toString().padStart(2, '0');
            const day = date.getDate().toString().padStart(2, '0');
            const hours = date.getHours().toString().padStart(2, '0');
            const minutes = date.getMinutes().toString().padStart(2, '0');
            const seconds = date.getSeconds().toString().padStart(2, '0');

            return `${year}年${month}月${day}日 ${hours}:${minutes}:${seconds}`;
    }

   },
   async created() {
        await this.reload();
    },
   computed:{
    $_virtualScrollItemElemHeight() {
      return 101;
    },
    messageViewStatus(){
        return this.$store.state.messageViewStatus as MyMessageViewType;
    },
    messageList() {
      return this.scrollStatus.virtualScroll.list as MyMessageListModel[];
    },
    IsEmpty(){
     return !this.messageList.filter(c=>c.messageType===this.messageViewStatus && !c.isDelete).length;
    } 
   }
})
</script>
