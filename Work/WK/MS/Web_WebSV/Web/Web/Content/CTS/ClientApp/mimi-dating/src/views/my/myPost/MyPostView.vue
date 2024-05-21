<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToMy">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_tab">
          <ul>
            <li @click="navigateToMyOverview">
              <div class="text">总览</div>
              <div class="tab_bottom_line"></div>
            </li>

            <li class="active">
              <div class="text">发帖管理</div>
              <div class="tab_bottom_line"></div>
            </li>
            <li @click="showComingSoon">
              <div class="text">预约管理</div>
              <div class="tab_bottom_line"></div>
            </li>
          </ul>
        </div>
        <div class="header_message">
          <div>
            <img src="@/assets/images/me/ic_me_message.svg" />
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- filter tag -->
      <div class="filter_tag filter_tag_display">
        <ul>
          <li
            :class="{ active: isPostTypeSelected(PostType.Square) }"
            @click="selectedPostType(PostType.Square)"
          >
            广场
          </li>
          <li
            :class="{ active: isPostTypeSelected(PostType.Agency) }"
            @click="selectedPostType(PostType.Agency)"
          >
            担保
          </li>
          <li @click="showComingSoon">官方</li>
          <li @click="showComingSoon">体验</li>
        </ul>
        <div class="filter_search">
          <img src="@/assets/images/public/ic_sort_search.svg" alt="" />
        </div>
      </div>
      <!--filter tag end -->

      <!-- filter text -->
      <div class="filter_text">
        <ul>
          <li
            :class="{ active: isStatusSelected(statusType.None) }"
            @click="selectedStatus(statusType.None)"
          >
            全部
          </li>
          <li
            :class="{ active: isStatusSelected(statusType.Approval) }"
            @click="selectedStatus(statusType.Approval)"
          >
            展示中
          </li>
          <li
            :class="{ active: isStatusSelected(statusType.UnderReview) }"
            @click="selectedStatus(statusType.UnderReview)"
          >
            审核中
          </li>
          <li
            :class="{ active: isStatusSelected(statusType.NotApproved) }"
            @click="selectedStatus(statusType.NotApproved)"
          >
            未通过
          </li>
        </ul>
      </div>
      <!--filter tag end -->

      <div class="table_view th style_a">
        <div class="column_1 h_center v_center">封面</div>
        <div class="column_2 h_center v_center">标题</div>
        <div class="column_3 h_center v_center" @click="sortCount">
          <div class="sort">
            解锁次数
            <div class="icon">
              <img :src="sortImage(unlockCountSort)" alt="" />
            </div>
          </div>
        </div>
        <div class="column_4 h_center v_center" @click="sortTime">
          <div class="sort">
            上传时间
            <div class="icon">
              <img :src="sortImage(uploadTimeSort)" alt="" />
            </div>
          </div>
        </div>
        <div class="column_5 h_center v_center">操作</div>
      </div>

      <!-- 共同滑動區塊 -->
      <EmptyView :message="`还没有发布过帖子哟～`" v-if="isEmpty"></EmptyView>
      <div
        class="flex_height overflow no_scrollbar"
        v-show="!isEmpty"
        @scroll="onScroll"
        ref="scrollContainer"
      >
        <div
          :style="{
            'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
            'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
          }"
        >
          <div class="table_view td style_a" v-for="n in orderList">
            <div class="column_1">
              <div class="photo">
                <AssetImage :item="setImageItem(n)" />
                <div
                  class="tag inprogress"
                  v-if="n.status === statusType.UnderReview"
                >
                  审核中
                </div>
                <div
                  class="tag failed"
                  v-if="n.status === statusType.NotApproved"
                >
                  未通过
                </div>
                <div class="tag finish" v-if="n.status === statusType.Approval">
                  展示中
                </div>
              </div>
            </div>
            <div class="column_2 v_center">
              <div class="ellipsis">{{ n.title }}</div>
              <div class="alert">{{ n.memo }}</div>
            </div>
            <div class="column_3 h_center v_center">{{ unlockCount(n) }}</div>
            <div class="column_4 h_center v_center">{{ n.createTime }}</div>
            <div
              class="column_5 h_center v_center"
              @click="editPost(n)"
              v-if="
                n.status === statusType.NotApproved ||
                n.status === statusType.Approval
              "
            >
              <div class="icon_btn">
                <img src="@/assets/images/public/ic_list_edit.svg" alt="" />
              </div>
            </div>
          </div>
        </div>
      </div>
      <!-- 浮動按鈕 -->
      <div
        class="bottom_btn_section"
        @click="postEvent"
        style="position: relative"
      >
        <div class="fixed_bottom_btn">
          <h1>发帖+</h1>
          <p>剩余发帖次数：{{ memberInfo.remainPublish }}</p>
        </div>
        <!-- <div class="delect_bottom_btn">
          <h1>刪除</h1>
        </div> -->
      </div>
    </div>
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
} from "@/mixins";
import { EmptyView, AssetImage } from "@/components";
import api from "@/api";
import { MyPostQueryModel, MyPostListModel, ImageItemModel } from "@/models";
import {
  DecryptoSourceType,
  IdentityType,
  MyPostSortType,
  PostType,
  ReviewStatusType,
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
  ],
  components: {
    EmptyView,
    AssetImage,
  },
  data() {
    return {
      isEmpty: false,
      totalPage: 1,
      reviewStatusType: ReviewStatusType.None,
      unlockCountSort: false,
      uploadTimeSort: true,
      postTypeStatus: PostType.Square,
      PostType,
      dataSortType: MyPostSortType.CreateDateDesc,
    };
  },
  methods: {
    selectedPostType(type: PostType) {
      this.postTypeStatus = type;
      this.reload();
    },
    isPostTypeSelected(type: PostType) {
      return this.postTypeStatus === type;
    },
    sortCount() {
      this.unlockCountSort = !this.unlockCountSort;
      if (this.unlockCountSort) {
        this.dataSortType = MyPostSortType.UolockCountDesc;
      } else {
        this.dataSortType = MyPostSortType.UolockCountAsc;
      }
      this.reload();
    },
    sortTime() {
      this.uploadTimeSort = !this.uploadTimeSort;
      if (this.uploadTimeSort) {
        this.dataSortType = MyPostSortType.CreateDateDesc;
      } else {
        this.dataSortType = MyPostSortType.CreateDateAsc;
      }
      this.reload();
    },
    sortImage(type: boolean) {
      return type ? this.arrowDownImage : this.arrowUpImage;
    },
    unlockCount(item: MyPostListModel) {
      return !item.unlockCount ? "-" : `${item.unlockCount}次`;
    },
    isStatusSelected(type: ReviewStatusType) {
      return this.reviewStatusType === type;
    },
    selectedStatus(type: ReviewStatusType) {
      this.reviewStatusType = type;
      this.reload();
    },
    postEvent() {
      if (this.canPost(this.postTypeStatus)) {
        this.navigateToFrom(this.postTypeStatus);
      } else if (this.userInfo.quantity.remainingSend <= 0) {
        toast("发帖剩余次数不足，请洽客服");
      } else if (
        this.postTypeStatus === PostType.Agency &&
        this.userInfo.identity !== IdentityType.Agent &&
        this.userInfo.identity !== IdentityType.Boss
      ) {
        toast("权限不足，需有觅经纪或以上身分");
      } else if (this.postTypeStatus === PostType.Square) {
        toast("权限不足，需有会员卡或身分");
      }
    },
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        const count = 30;
        const status =
          this.reviewStatusType === ReviewStatusType.None
            ? undefined
            : this.reviewStatusType;

        const nextPage = this.pageInfo.pageNo + 1;
        let body: MyPostQueryModel = {
          postType: this.postTypeStatus,
          postStatus: status,
          sortType: this.dataSortType,
          pageNo: nextPage,
          pageSize: count,
          page: nextPage,
          ts: "",
        };
        const result = await api.managePost(body);
        this.totalPage = result.totalPage;
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
    reload() {
      this.totalPage = 1;
      this.resetPageInfo();
      this.resetScroll();
      this.loadAsync();
    },
    $_onScrollReload() {
      this.reload();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    editPost(item: MyPostListModel) {
      this.navigateToFrom(this.postTypeStatus, item.postId);
    },
    setImageItem(info: MyPostListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        sourceType: DecryptoSourceType.MyPostList,
        class: "loading-image",
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
        this.resetScroll();
        await this.loadAsync();
      }
    },
  },
  async created() {
    await this.checkDetailToReturn();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 71;
    },
    statusType() {
      return ReviewStatusType;
    },
    arrowDownImage() {
      return require(`@/assets/images/public/ic_sort_arrow_down.svg`);
    },
    arrowUpImage() {
      return require(`@/assets/images/public/ic_sort_arrow_up.svg`);
    },
    orderList() {
      return this.scrollStatus.virtualScroll.list as MyPostListModel[];
    },
  },
});
</script>
