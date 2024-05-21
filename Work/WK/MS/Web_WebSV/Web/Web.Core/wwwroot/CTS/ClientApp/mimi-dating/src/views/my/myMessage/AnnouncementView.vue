<template>

<div class="main_container ">
            <div class="main_container_flex">
                <!-- Header start -->
                <header class="header_height1 solid_color solid_line">
                    <div class="header_back" @click="navigateToPrevious">
                        <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt=""/></div>
                    </div>
                    <div class="header_title">我的消息</div>
                </header>
                <!-- Header end -->

                <!-- filter tab -->

                <!-- tab end -->

                <!-- 共同滑動區塊 -->
                <div class=" flex_height bg_personal">
                    <div class="overflow no_scrollbar">
                        <div class="padding_order ">
                            <div class="me_order me_order_second">
                                <div class="user_info user_flex">
                                    <div class="avatar_third avatar_fit2">
                                        <CdnImage src="@/assets/images/me/ic_me_notify.svg" alt=""/>
                                    </div>
                                    <div class="info">
                                        <div class="first_co">
                                            <div class="basic">
                                                <div class="name_fourth">{{ itemInfo.title }}</div>
                                            </div>
                                            <div class="basic">
                                                <div class="name_fourth" v-if="isShowTime">{{ formatDate(itemInfo.createDateText)}}</div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- <div class="state">
                                        <div class="text_red">●</div>
                                    </div> -->
                                </div>
                                <div class="add_text" v-html="itemInfo.homeContent">
                                 
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
import api from "@/api";
import { NavigateRule,DialogControl,VirtualScroll,PlayGame,ScrollManager,Tools,ImageCacheManager} from "@/mixins";
import { MyMessageViewType,MessageOperationType } from '@/enums';
import {
    MyAnnouncementModel,MessageOperationModel
} from "@/models";
import { MutationType } from "@/store";
import {CdnImage, EmptyView, AssetImage} from '@/components'
export default defineComponent({
    mixins:[
    NavigateRule
   ,DialogControl
   ,VirtualScroll
   ,PlayGame
   ,ScrollManager
   ,Tools
   ,ImageCacheManager],
   components:{ EmptyView, AssetImage, CdnImage},
    data(){
        return {
            item:{} as MyAnnouncementModel,
            MyMessageViewType,
            MessageOperationType,
            isShowTime:false,
        }
    },
    async created()
    {
        await this.getMessageDetail();
        await this.writeMessageInfo();
    },
    watch:{
        item(newValue,oldValue){
             this.isShowTime=true;
        }
    },
    methods:{
      
        async $_loadData(){
        },
        async getMessageDetail(){

            if (this.isLoading) return;

            this.$store.commit(MutationType.SetIsLoading, true);

            try{
                const result = await api.getAnnouncementInfo(this.messageId);
                this.item=result;
            }
            catch (error) {
                console.error(error);
            } finally {
                this.$store.commit(MutationType.SetIsLoading, false);
            }
        },
       
        async writeMessageInfo(){
            if(this.messageIsRed===1){
                return;
            }
            let messageIdArray:string[]=[];
            messageIdArray.push(this.messageId);

            const model:MessageOperationModel={
                messageIds:messageIdArray,
                messageType:MyMessageViewType.Announcement,
                messageOperationType:MessageOperationType.UserRead
            }
            let result= await api.userToMessageOperation(model);
          
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
    computed:{
        
        messageId(){
            return (this.$route.query.messageId as unknown as string) || "";
        }, 
        messageIsRed(){
            return (this.$route.query.isRead as unknown as number) || "";
        },
        itemInfo() {
            return this.item;
        }
    },
})
</script>