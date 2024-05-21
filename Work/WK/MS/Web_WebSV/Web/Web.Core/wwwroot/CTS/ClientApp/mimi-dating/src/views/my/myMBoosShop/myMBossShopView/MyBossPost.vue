<template>
<div class="me_filter hide_overflowx">
<ul>
    <li :class="{active:isActive(liItem.type)}" v-for="liItem in headerTab" @click="selectPostStatus(liItem.type)">
        <div class="text">{{liItem.name}}</div>
    </li>
</ul>
</div>
<div class="table_view th style_a">
    <div class="column_1 h_center v_center">封面</div>
    <div class="column_2 h_center v_center">标题</div>
    <div class="column_3 h_center v_center" @click="sortCount">
        <div class="sort">
            预约次数
            <div class="icon"><CdnImage :src="sortImage(appointmentCountSort)" alt="" /></div>
        </div>
    </div>
    <div class="column_4 h_center v_center" @click="sortTime">
        <div class="sort">
            上传时间
            <div class="icon"><CdnImage :src="sortImage(uploadTimeSort)" alt="" /></div>
        </div>
    </div>
    <div class="column_5 h_center v_center">操作</div>
</div>


<EmptyView :message="`暂无数据～`" v-if="isEmpty"></EmptyView>
<div class=" flex_height overflow no_scrollbar" v-show="!isEmpty" @scroll="onScroll" ref="scrollContainer">
    <div class="padding_order " :style="{'padding-top': scrollStatus.virtualScroll.paddingTop + 'px','padding-bottom':scrollStatus.virtualScroll.paddingBottom + 'px',}" >
        <div class="table_view td style_a" v-for=" item in postList">
            <div class="column_1">
                <div class="photo">
                    <AssetImage :item="setImageItem(item)" />
                    <div :class="statusCss(item)">{{statusText(item)}}</div>
                </div>
            </div>
            <div class="column_2 v_center">
                <div class="ellipsis">{{showTitle(item.title)}}</div>
            </div>
            <div class="column_3 h_center v_center">{{ unlockCount(item)  }}</div>
            <div class="column_4 h_center v_center">{{ showTimeText(item) }}</div>

            <div class="column_5 h_center v_center">
                <div class="up_shelves_btn" v-if="isShelfOperation(item)" @click="shelfPost(item.postId,0)">
                    <div class=" text">上架</div>
                </div>

                <div class="up_shelves_btn" v-if="isRemoveAPost(item)" @click="shelfPost(item.postId,1)">
                    <div class=" text">下架</div>
                </div>

                <div class="icon_btn" v-if="isEditShow(item.status)" @click="editPost(item.postId)">
                    <CdnImage src="@/assets/images/public/ic_list_edit.svg" alt="" />
                </div>
            </div>
        </div>
    </div>
</div>
<div class="release_btn" @click="postEvent">
    <div class="btn_default">发帖+</div>
</div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { MyBossPostStatus,ReviewStatusType,OfficialPostOnTheShelves,MyPostSortType,IdentityType} from "@/enums";
import { MutationType } from "@/store";
import { NavigateRule, DialogControl, PlayGame,Tools,ImageCacheManager,VirtualScroll, ScrollManager } from "@/mixins";
import {
ImageItemModel,
MyOfficialPostQueryModel,
MyOfficialPostListModel,
} from "@/models";
import { EmptyView, AssetImage, CdnImage } from "@/components";
import api from "@/api";
import toast from "@/toast";
export default defineComponent({
    mixins: [NavigateRule, DialogControl, PlayGame,Tools,ImageCacheManager,VirtualScroll, ScrollManager],
    components: {
        EmptyView,
        AssetImage,
        CdnImage,
    },
    data(){
        return{
            headerTab:[
                {
                    type:MyBossPostStatus.RemovedFromShelves,
                    name:"已下架"
                },
                {
                    type:MyBossPostStatus.OnDisplay,
                    name:"展示中"
                },
                {
                    type:MyBossPostStatus.UnderReview,
                    name:"审核中"
                },
                {
                    type:MyBossPostStatus.DidNotPass,
                    name:"未通过"
                }
            ],
            isEmpty: false,
            dataSortType:MyPostSortType.CreateDateDesc,
            uploadTimeSort:true,
            appointmentCountSort:false,
        }
    },
    methods:{
        postEvent() {
           
            if (
                this.userInfo.identity != IdentityType.Boss && this.userInfo.identity != IdentityType.SuperBoss
            ) {
                toast("抱歉,您的身份不是觅老板,不能发帖!");
                return;
            }

            this.navigateToOfficialFrom();
        },
        showTitle(title:string){
            return title.length>5?`${title.substring(0,5)}...`:title;
        },
        sortImage(type: boolean) {
            return type ? this.arrowDownImage : this.arrowUpImage;
        },
        unlockCount(item: MyOfficialPostListModel) {
            return item.appointmentCount==0 ? "-" : `${item.appointmentCount}次`;
        },
        sortCount() {
            this.appointmentCountSort = !this.appointmentCountSort;
            this.dataSortType = this.appointmentCountSort
                ? MyPostSortType.UolockCountDesc
                : MyPostSortType.UolockCountAsc;
            this.reload();
        },
        sortTime() {
            this.uploadTimeSort = !this.uploadTimeSort;
            this.dataSortType = this.uploadTimeSort
                ? MyPostSortType.CreateDateDesc
                : MyPostSortType.CreateDateAsc;
            this.reload();
        },
        editPost(postId:string)
        {
            this.navigateToOfficialFrom(postId);
        },
        async shelfPost(postId:string,isDelete:number)
        {
            this.$store.commit(MutationType.SetIsLoading, true);
            try {
                let postIds:string[]=[postId];
                await api.setShelfOfficialPost(postIds,isDelete);
                toast("操作成功");
            } catch (e) {
                toast(e);
            }
            finally{
                this.$store.commit(MutationType.SetIsLoading, false);
                await this.reload();
            }
        },
        isEditShow(statues:ReviewStatusType){
            return statues===ReviewStatusType.NotApproved;
        },
        isShelfOperation(item:MyOfficialPostListModel){
           return item.isDelete && item.status==ReviewStatusType.Approval;
        },
        isRemoveAPost(item:MyOfficialPostListModel){
            return !item.isDelete && item.status==ReviewStatusType.Approval;
        },
        statusCss(item:MyOfficialPostListModel){
            let statusText;
    
            if(item.status==ReviewStatusType.Approval && item.isDelete)
            {
                statusText="tag removed";
            }else if(item.status==ReviewStatusType.Approval && !item.isDelete)
            {
                statusText="tag finish";
            }else
            {
                switch(item.status)
                {
                    case ReviewStatusType.UnderReview:
                        statusText="tag inprogress";
                        break;
                    case ReviewStatusType.NotApproved:
                        statusText="tag failed";
                        break
                }
            }

            return statusText;

        },
        statusText(item:MyOfficialPostListModel){
           
            let statusText;

            if(item.status==ReviewStatusType.Approval && item.isDelete)
            {
                statusText="已下架";
            }else if(item.status==ReviewStatusType.Approval && !item.isDelete)
            {
                statusText="展示中";
            }else
            {
                switch(item.status)
                {
                    case ReviewStatusType.UnderReview:
                        statusText="审核中";
                        break;
                    case ReviewStatusType.NotApproved:
                        statusText="不通过";
                        break
                }
            }
          
            
            return statusText;
        },
        async checkDetailToReturn() {
            if (
                !Object.keys(this.scrollStatus.virtualScroll).length ||
                this.scrollStatus.list.length === 0
            ) {
                this.resetScroll();
                await this.loadAsync();
            }
        },
        isActive(type:MyBossPostStatus){
            return type===this.currentDataStatus;
        },
        async selectPostStatus(type:MyBossPostStatus){
            this.$store.commit(MutationType.SetMyBossPostStatusName, type);
            await this.reload();
        },
        async reload(){
            this.resetPageInfo();
            this.resetScroll();
            await this.loadAsync();
        },
        async loadAsync(){
            if (this.isLoading || this.scrollStatus.totalPage === this.pageInfo.pageNo || this.isPromising)
                return;

            this.$store.commit(MutationType.SetIsLoading, true);
            const nextPage = this.pageInfo.pageNo + 1;
            try{
                const pageSize = 10;
                const search: MyOfficialPostQueryModel = {
                    pageNo: nextPage,
                    pageSize: pageSize,
                    postStatus:this.currentDataStatus,
                    sortType:this.dataSortType,
                    ts:""
                }

                const result = await api.officialBossManagePost(search);
                this.scrollStatus.totalPage = result.totalPage;
                this.managerBossOfficialDownload(result.data);
                this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
                this.isEmpty = !this.scrollStatus.list.length;
                this.pageInfo.pageNo = result.pageNo;

            }catch(e){
                toast(e);
            }finally{
                this.$store.commit(MutationType.SetIsLoading, false);
            }

            this.calculateVirtualScroll();
        },
        getOnTheShelves(type:MyBossPostStatus)
        {
            return type===MyBossPostStatus.RemovedFromShelves?OfficialPostOnTheShelves.RemovedFromShelves:undefined;
        },
        getPostStatus(type:MyBossPostStatus)
        {
            if(type===MyBossPostStatus.RemovedFromShelves) return undefined;

            let status;
            switch(type)
            {
                case MyBossPostStatus.OnDisplay:
                    status= ReviewStatusType.Approval;
                    break;
                case MyBossPostStatus.UnderReview:
                    status=ReviewStatusType.UnderReview;
                    break;
                case MyBossPostStatus.DidNotPass:
                    status=ReviewStatusType.NotApproved;
                    break;
            }
            return status;
        },
        $_onScrollToBottom() {
            this.loadAsync();
        },
        setImageItem(info: MyOfficialPostListModel) {
            const subId = this.getImageID(info.coverUrl);
            let item: ImageItemModel = {
                id: info.postId,
                subId: subId,
                class: "",
                src: info.coverUrl,
                alt: "",
            };
            return item;
        },
        showTimeText(info: MyOfficialPostListModel)
        {
            return info.createTimeText==""?"-":info.createTimeText;
        }
    },
    async created(){
        await this.checkDetailToReturn();
    },
    computed:{
        currentDataStatus(){
            return this.$store.state.myBossShopPostStatus;
        },
        $_virtualScrollItemElemHeight() {
            return 71;
        },
        arrowDownImage() {
            return "@/assets/images/public/ic_sort_arrow_down.svg";
        },
        arrowUpImage() {
            return "@/assets/images/public/ic_sort_arrow_up.svg";
        },
        postList(){
            return this.scrollStatus.virtualScroll.list as MyOfficialPostListModel[];
        }
    }
})
</script>