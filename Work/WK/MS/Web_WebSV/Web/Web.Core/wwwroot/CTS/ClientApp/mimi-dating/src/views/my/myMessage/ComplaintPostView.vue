<template>
            <div class="main_container ">
            <div class="main_container_flex">
                <!-- Header start -->
                <header class="header_height1 solid_color solid_line">
                    <div class="header_back"  @click="navigateToPrevious">
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
                            <div class="me_order me_order_third">
                                <div class="me_content">
                                <div class="user_info user_flex">
                                    <div class="avatar_third avatar_fit">
                                        <CdnImage src="@/assets/images/me/ic_me_24h.svg" alt=""/>
                                    </div>
                                    <div class="info">
                                        <div class="first_co">
                                            <div class="basic">
                                                <div class="name_fourth">投诉反馈回复</div>
                                            </div>
                                            <div class="basic">
                                                <div class="name_fourth" v-if="isShowTime">{{ formatDate(itemInfo.createTimeText) }}</div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="add_text">
                                    {{ itemInfo.memo }}
                                </div>
                            </div>
                            
                            <div class="distance_con">
                                <div class="user_info user_flex">

                                    <div class="info">
                                        <div class="first_co">
                                            <div class="basic">
                                                <div class="name_fifth">我的投诉：<span></span></div>
                                            </div>
                                            <div class="basic">
                                                <div class="name_fifth">举报原因：<span>{{itemInfo.reportTypeText}}</span></div>
                                            </div>
                                            <div class="basic">
                                                <div class="name_fifth">截图证据：<span></span></div>
                                            </div>
                                            <div class="basic_img" >
                                                <div class="img_fifth" v-for="image in itemInfo.photoIds" >
                                                    <AssetImage :item="setImageItem(image)" />
                                                </div>
                                            </div>
                                           

                                        </div>
                                    </div>
                                    <div class="state_btn" @click="openPostDetail(itemInfo.postId)">
                                        <div>进入帖子</div>
                                    </div>
                                </div>
                                    <div class="add_text2">详情描述：<br>
                                        <span>{{ itemInfo.describe }}</span>
                                </div>
                            </div>

                                </div>
                            </div>
                    
                </div>
                
            </div>

        </div>
    </div>
</template>
<style scoped>

</style>
<script lang="ts">

import { defineComponent } from "vue";
import api from "@/api";
import { NavigateRule,DialogControl,VirtualScroll,PlayGame,ScrollManager,Tools,ImageCacheManager} from "@/mixins";
import { MyMessageViewType,MessageOperationType, PostType,ReviewStatusType } from '@/enums';
import {
    MessageOperationModel,ReportDetailModel,ImageItemModel
} from "@/models";
import { MutationType } from "@/store";
import dayjs from "dayjs";
import { AssetImage, CdnImage } from "@/components";
import toast from "@/toast";
export default defineComponent({
    components: { AssetImage, CdnImage },
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
            item:{} as ReportDetailModel,
            isShowTime:false,
        }
    },
    watch:{

        item(newValue,oldValue)
        {
            this.isShowTime=true;
        }
    },
    methods:{
        async getMessageDetail(){

            if (this.isLoading) return;

            this.$store.commit(MutationType.SetIsLoading, true);

            try{
                const result = await api.getReportDetail(this.messageId);
                this.item=result;
                this.complaintPostDownload(this.item);
            }
            catch (error) {
                console.error(error);
            } finally {
                this.$store.commit(MutationType.SetIsLoading, false);
            }
        },
        setImageItem(info: string) {
            const subId = this.getImageID(info);
            let item: ImageItemModel = {
                id: this.item.reportId,
                subId: subId,
                class: "",
                src: info,
                alt: "",
            };
            return item;
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
        },
        openPostDetail(postId:string){

            if(this.itemInfo.postType===PostType.Official){

                if(this.itemInfo.postStatus!=ReviewStatusType.Approval || this.itemInfo.postIsDelete){
                    toast("很抱歉，当前帖子已下架或审核中");
                    return;
                }
                this.navigateToOfficialDetail(postId);
            }
            else{

                if(this.itemInfo.postStatus!=ReviewStatusType.Approval){
                    toast("很抱歉，当前帖子已下架或审核中");
                    return;
                }
                this.navigateToProductDetail(postId);
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
                    messageType:MyMessageViewType.ComplaintPost,
                    messageOperationType:MessageOperationType.UserRead
                }
            let result= await api.userToMessageOperation(model);
        },
       
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
        },
        // imageList(){
        //     const imageArrayList=[];
        //     const photoIds=this.itemInfo?.photoIds;
        //     let photoIdsLenth=this.itemInfo?.photoIds?.length;
        //     if(photoIdsLenth>0){
        //         let starIndex=0;
        //         while(starIndex<photoIdsLenth)
        //         {
        //             const end=Math.min(starIndex+3,photoIdsLenth);
        //             imageArrayList.push(photoIds.slice(starIndex,end));
        //             starIndex=end;
        //         }
        //     }
        //     return imageArrayList;
        // }
    },
    async created()
    {
        await this.getMessageDetail();
        await this.writeMessageInfo();
    },
})
</script>