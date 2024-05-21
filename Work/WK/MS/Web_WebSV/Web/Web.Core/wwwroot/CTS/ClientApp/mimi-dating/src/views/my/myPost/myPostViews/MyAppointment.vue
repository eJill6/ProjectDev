<template>
  <!-- Header end -->    
  <div class="me_filter hide_overflowx">
      <ul>
          <li :class="{ active: isStatusSelected(ReviewStatusType.None) }"        @click="selectedStatus(ReviewStatusType.None)">
              <div class="text">全部</div>
          </li>
          <li  :class="{ active: isStatusSelected(ReviewStatusType.Approval) }"    @click="selectedStatus(ReviewStatusType.Approval)">
              <div class="text">展示中</div>
          </li>
          <li :class="{ active: isStatusSelected(ReviewStatusType.UnderReview) }" @click="selectedStatus(ReviewStatusType.UnderReview)">
              <div class="text">审核中</div>
          </li>
          <li :class="{ active: isStatusSelected(ReviewStatusType.NotApproved) }" @click="selectedStatus(ReviewStatusType.NotApproved)">
              <div class="text">未通过</div>
          </li>
      </ul>
  </div>
  <div class="table_view th style_a">
      <div class="column_1 h_center v_center">封面</div>
      <div class="column_2 h_center v_center">标题</div>
      <div class="column_3 h_center v_center" @click="sortCount">
          <div class="sort">
              解锁次数
              <div class="icon"><CdnImage :src="sortImage(unlockCountSort)" alt="" /></div>
          </div>
      </div>
      <div class="column_4 h_center v_center" @click="sortTime">
          <div class="sort">
              上传时间
              <div class="icon"> <CdnImage :src="sortImage(uploadTimeSort)" alt="" /></div>
          </div>
      </div>
      <div class="column_5 h_center v_center">操作</div>
  </div>
  <!--filter tag end -->

  <!-- filter text -->
  <!--filter tag end -->
  <EmptyView :message="`还没有发布过帖子哟～`" v-if="isEmpty"></EmptyView>
  <!-- 共同滑動區塊 -->
  <div class=" flex_height overflow no_scrollbar" v-show="!isEmpty" @scroll="onScroll" ref="scrollContainer">
      <div :style="{'padding-top': scrollStatus.virtualScroll.paddingTop + 'px','padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',}">
          <div class="table_view td style_a" v-for="n in orderList" @click="selectedId(n.postId)">
                  <div class="column_1">
                      <div class="photo">
                          <AssetImage :item="setImageItem(n)" />
                          <div class="tag inprogress" v-if="n.status === ReviewStatusType.UnderReview">审核中</div>
                          <div class="tag failed"     v-if="n.status === ReviewStatusType.NotApproved">未通过</div>
                          <div class="tag finish"     v-if="n.status === ReviewStatusType.Approval">   展示中</div>
                      </div>
                  </div>
                  <div class="column_2 v_center" style="text-align:center;">
                      <div class="ellipsis">{{ n.title }}</div>
                      <div class="alert" v-if="n.status===ReviewStatusType.NotApproved">{{showMemo(n.memo)}}</div>
                  </div>
                  <div class="column_3 h_center v_center">{{ unlockCount(n) }}</div>
                  <div class="column_4 h_center v_center">{{ n.createTime }}</div>
                  <div class="column_5 h_center v_center" @click="editPost(n)" v-if="(postTypeStatus !== PostType.Official && n.status !== ReviewStatusType.UnderReview) ||(postTypeStatus === PostType.Official && n.status === ReviewStatusType.NotApproved)">
                      <div class="icon_btn">  
                          <CdnImage src="@/assets/images/public/ic_list_edit.svg" alt=""/>
                      </div>
                  </div>
                  <div class="column_5 h_center v_center" v-else>
                      <div class="icon_btn"> </div>
                  </div>
              </div>
              
      </div>
  </div>
  <div class="release_btn" style="position: relative">
      <div class="btn_default" @click="postEvent">发帖+</div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import {
NavigateRule,
PlayGame,
DialogControl,
VirtualScroll,
OverviewMode,
ScrollManager,
Tools,
ImageCacheManager,
} from "@/mixins";
import { EmptyView, AssetImage, CdnImage } from "@/components";
import api from "@/api";
import {
MyPostQueryModel,
MyPostListModel,
ImageItemModel,
MessageDialogModel,
TipInfo
} from "@/models";
import {
IdentityType,
MyPostSortType,
PostType,
ReviewStatusType,TipType
} from "@/enums";
import { MutationType } from "@/store";
import toast from "@/toast";

export default defineComponent({
mixins: [
  NavigateRule,
  PlayGame,
  DialogControl,
  VirtualScroll,
  OverviewMode,
  ScrollManager,
  Tools,
  ImageCacheManager,
],
components: {
  EmptyView,
  AssetImage,
  CdnImage,
},

data() {
  return {
    isEmpty: false,
    unlockCountSort: false,
    uploadTimeSort: true,
    PostType,
    dataSortType: MyPostSortType.CreateDateDesc,
    ReviewStatusType,
    reviewStatus: ReviewStatusType.None,
    selectedValue: [] as string[],
  };
  
},

methods: {
  showMemo(memo:string)
  {
    return memo.length>15?memo.substring(0,15)+"...":memo;
  },
  backEvent() {
    this.$store.commit(MutationType.SetUnlockViewStatus, PostType.Square);
    this.$store.commit(
      MutationType.SetPostManageStatus,
      ReviewStatusType.None
    );
    this.navigateToMy();
  },
  selectedPostType(type: PostType) {
    this.$store.commit(MutationType.SetUnlockViewStatus, type);
    this.reload();
  },
  isPostTypeSelected(type: PostType) {
    return this.postTypeStatus === type;
  },
  sortCount() {
    this.unlockCountSort = !this.unlockCountSort;
    this.dataSortType = this.unlockCountSort
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
  sortImage(type: boolean) {
    return type ? this.arrowDownImage : this.arrowUpImage;
  },
  unlockCount(item: MyPostListModel) {
    return !item.unlockCount ? "-" : `${item.unlockCount}次`;
  },
  isStatusSelected(type: ReviewStatusType) {
    return this.reviewType === type;
  },
  selectedStatus(type: ReviewStatusType) {
    this.$store.commit(MutationType.SetPostManageStatus, type);
    this.selectedValue = [];
    this.reload();
  },
  postEvent() {
    
    //寻芳阁
    if (this.userInfo.identity === IdentityType.General) {
        toast("身份不符，无法在此发帖");
      }
      else if (this.userInfo.identity === IdentityType.Boss || this.userInfo.identity === IdentityType.SuperBoss) {
        if (this.userInfo.quantity?.remainingSend <= 0) {
          toast("您没有发帖次数，请联系管理员");
        } else {
          this.navigateToFrom(PostType.Agency);
        }
      }
      else if (this.userInfo.identity === IdentityType.Agent) {
        this.navigateToFrom(PostType.Agency);
      }

  },
  async loadAsync() {
    
    if (
      this.isLoading ||
      this.scrollStatus.totalPage === this.pageInfo.pageNo
    )
      return;


    try {
      this.$store.commit(MutationType.SetIsLoading, true);
      const count = 30;
      const status =
        this.reviewType === ReviewStatusType.None
          ? undefined
          : this.reviewType;

      const nextPage = this.pageInfo.pageNo + 1;
      let body: MyPostQueryModel = {
        postType:this.getPostTypeStatus,
        postStatus: status,
        sortType: this.dataSortType,
        pageNo: nextPage,
        pageSize: count,
        ts: "",
      };
      const result = await api.managePost(body);
      this.scrollStatus.totalPage = result.totalPage;
      this.managerDownload(result.data);
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
  async reload() {
    this.resetPageInfo();
    this.resetScroll();
    await this.loadAsync();
  },
  $_onScrollToBottom() {
    this.loadAsync();
  },
  editPost(item: MyPostListModel) {
    if (this.postTypeStatus === PostType.Official) {
      this.navigateToOfficialFrom(item.postId);
    }
    else{

      this.navigateToFrom(this.getPostTypeStatus, item.postId);
    }
  },
  setImageItem(info: MyPostListModel) {
    const subId = this.getImageID(info.coverUrl);
    let item: ImageItemModel = {
      id: info.postId,
      subId: info.coverUrl,
      class: "",
      src: info.coverUrl,
      alt: "",
    };
    return item;
  },
  async checkDetailToReturn() {
    
    if (
      !Object.keys(this.scrollStatus.virtualScroll).length ||
      this.scrollStatus.list.length === 0
    ) {
        await this.reload();
    }else
    {
      this.selectedStatus(this.reviewType);
    }
  
  },
  selectedId(postId: string) {
    const index = this.selectedValue.indexOf(postId);
    if (index > -1) {
      this.selectedValue.splice(index, 1);
    } else {
      this.selectedValue.push(postId);
    }
  },
  isChecked(postId: string) {
    const index = this.selectedValue.indexOf(postId);
    return index > -1;
  },
  async submitDeleteList() {
    if (
      !this.selectedValue ||
      !this.selectedValue.length ||
      this.postTypeStatus !== PostType.Official
    )
      return;

    const messageModel: MessageDialogModel = {
      message: "确认删除帖子吗",
      cancelTitle: "",
      buttonTitle: "确认",
    };
    this.showMessageDialog(messageModel, async () => {
      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        await api.deleteOfficialPost(this.selectedValue);
        toast("已删除成功");
        this.$store.commit(MutationType.SetIsLoading, false);
        await this.reload();
      } catch (e) {
        this.$store.commit(MutationType.SetIsLoading, false);
        toast(e);
      }
    });
  },
},
async created() {

  await this.checkDetailToReturn()
},
computed: {
  $_virtualScrollItemElemHeight() {
    return 71;
  },
  arrowDownImage() {
    return "@/assets/images/public/ic_sort_arrow_down.svg";
  },
  arrowUpImage() {
    return "@/assets/images/public/ic_sort_arrow_up.svg";
  },
  orderList() {
    return this.scrollStatus.virtualScroll.list as MyPostListModel[];
  },
  reviewType() {
    return this.$store.state.postManageStatus;
  }, 
  postTypeStatus() {
    return this.$store.state.unlockViewStatus as PostType;
  },
  getPostTypeStatus()
  {
    return this.$store.state.selectPostDataWherePostType as PostType;
  }
},
});
</script>