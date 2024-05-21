<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">已解锁</div>
      </header>
      <!-- Header end -->
      <!-- filter tab -->
      <div class="filter_tab">
        <ul>
          <li
            :class="{ active: postTypeStatus === PostType.Square }" 
            @click="changePostType(PostType.Square)"
          >
            <div class="text">广场区</div>
            <div class="tab_bottom_line"></div>
          </li>
          <li
            :class="{ active: postTypeStatus === PostType.Agency }"
            @click="changePostType(PostType.Agency)"
          >
            <div class="text">担保区</div>
            <div class="tab_bottom_line"></div>
          </li>
        </ul>
      </div>
      <!-- filter tab end -->

      <!-- 共同滑動區塊 -->
      <EmptyView :message="`还没有解锁帖子哟～`" v-show="isEmpty"></EmptyView>
      <div class="flex_height" v-show="!isEmpty">
        <div
          class="overflow no_scrollbar"
          @scroll="onScroll"
          ref="scrollContainer"
        >
          <div class="text_notice normal" v-if="tip">
            {{ tip }}
          </div>
          <div class="padding_basic">
            <div class="me_product_view">
              <ul
                class="full_cards"
                :style="{
                  'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
                  'padding-bottom':
                    scrollStatus.virtualScroll.paddingBottom + 'px',
                }"
              >
                <li
                  class="full_card no_premium_tag with_status"
                  v-for="info in orderList"                  
                >
                  <div class="card_content" @click="navigateToProductDetail(info.postId)">
                    <div class="location_tag">
                      <div>
                        <img
                          src="@/assets/images/card/ic_card_location.svg"
                          alt=""
                        />
                        {{ cityName(info.areaCode) }}
                      </div>
                    </div>
                    <div class="card_text">
                      <h1>{{ info.title }}</h1>
                      <div class="info_co">
                        <div class="info">
                          <div class="icon">
                            <img
                              src="@/assets/images/card/ic_card_body_info.svg"
                              alt=""
                            />
                          </div>
                          <div>{{ info.height }}cm</div>
                          <div class="dot"></div>
                          <div>{{ info.age }}岁</div>
                          <div class="dot"></div>
                          <div>{{ info.cup }}杯</div>
                        </div>
                      </div>
                      <div class="info_co">
                        <div class="info hobby">
                          <div class="icon">
                            <img
                              src="@/assets/images/card/ic_card_hobby.svg"
                              alt=""
                            />
                          </div>
                          <div
                            v-for="n in favoriteInfo(info.serviceItem)"
                            :class="{ dot: !n }"
                          >
                            {{ n }}
                          </div>
                        </div>
                      </div>
                      <p>职业：{{ info.job }}</p>
                      <p>期望收入：¥{{ info.lowPrice }}</p>
                      <div class="user_view">
                        <div class="num_total">
                          <ul>
                            <li>
                              <img
                                src="@/assets/images/card/ic_card_num_like.svg"
                                alt=""
                              />{{ info.favorites }}
                            </li>
                            <li>
                              <img
                                src="@/assets/images/card/ic_card_num_chat.svg"
                                alt=""
                              />{{ info.comments }}
                            </li>
                            <li>
                              <img
                                src="@/assets/images/card/ic_card_num_watch.svg"
                                alt=""
                              />{{ info.views }}
                            </li>
                          </ul>
                        </div>
                        <div class="num_time">{{ info.updateTime }}</div>
                      </div>
                    </div>
                    <div class="photo">
                      <AssetImage :item="setImageItem(info)"/>
                    </div>
                  </div>
                  <div
                    class="card_status btn justify_center"
                    v-if="
                      !info.commentId && info.status !== statusType.UnderReview
                    "
                    @click="toComment(info)"
                  >
                    立即评论
                  </div>

                  <div
                    class="card_status space_between"
                    v-else-if="info.status === statusType.UnderReview"
                  >
                    <div class="title">评论审核中</div>
                  </div>

                  <div
                    class="card_status space_between"
                    v-else-if="info.status === statusType.Approval"
                  >
                    <div class="title">已评论</div>
                    <div class="sub">序号：{{ info.commentId }}</div>
                  </div>

                  <div
                    class="card_status space_between"
                    v-else-if="info.status === statusType.NotApproved"
                  >
                    <div class="title">
                      审核不通过
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_attention.svg"
                          alt=""
                        />
                      </div>
                    </div>
                    <div class="sub" @click="reComment(info)">
                      重新编辑评论
                      <div class="icon">
                        <img
                          src="@/assets/images/card/ic_card_edit_arrow.svg"
                          alt=""
                        />
                      </div>
                    </div>
                  </div>
                </li>
              </ul>
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
  NavigateRule,
  DialogControl,
  VirtualScroll,
  PlayGame,
  ScrollManager,
  Tools
} from "@/mixins";
import { EmptyView, AssetImage } from "@/components";
import api from "@/api";
import {
ImageItemModel,
  MessageDialogModel,
  MyPostQueryModel,
  MyUnlockPosModel,
  ProductDetailModel,
} from "@/models";
import { DecryptoSourceType, PostType, ReviewStatusType } from "@/enums";
import toast from "@/toast";
import { MutationType } from "@/store";

export default defineComponent({
  mixins: [NavigateRule, DialogControl, VirtualScroll, PlayGame, ScrollManager, Tools],
  components: { EmptyView, AssetImage },
  data() {
    return {
      PostType,
      totalPage: 1,
      isEmpty: false,
      tip: "",
      postTypeStatus: PostType.Square,
    };
  },
  methods: {
    setImageItem(info: MyUnlockPosModel) {
      let item: ImageItemModel = {
        id: info.postId,
        sourceType: DecryptoSourceType.UnlockList,
        class: "loading-image",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
    reload() {
      this.totalPage = 1;
      this.scrollStatus.list = [];
      this.pageInfo.pageNo = 0;
      this.loadAsync();
      this.resetScroll();
    },
    reComment(item: MyUnlockPosModel) {
      const msg = item.commentMemo;
      const title = "重新编辑";
      const messageModel: MessageDialogModel = {
        message: msg,
        cancelTitle: "",
        buttonTitle: title,
      };
      this.showMessageDialog(messageModel, async () => {
        this.toComment(item);
      });
    },
    toComment(item: MyUnlockPosModel) {
      const commentId = item.commentId ? item.commentId : " ";
      let productDetail = {} as ProductDetailModel;
      productDetail.postId = item.postId;
      this.$store.commit(MutationType.SetProductDetail, productDetail);
      this.navigateToComment(commentId);
    },
    changePostType(postType: PostType) {
      this.postTypeStatus = postType;
      this.reload();
    },
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        const nextPage = this.pageInfo.pageNo + 1;
        const count = 30;
        const model: MyPostQueryModel = {
          postType: this.postTypeStatus,
          pageNo: nextPage,
          pageSize: count,
          page: 0,
          ts: "",
        };
        let result = await api.unlockPostList(model);
        this.tip = result.tip;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.isEmpty = !this.scrollStatus.list.length;
        this.totalPage = result.totalPage;
        this.pageInfo.pageNo = result.pageNo;
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    $_onScrollReload() {
      this.reload();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
  },
  async created() {
    await this.loadAsync();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 180;
    },
    statusType() {
      return ReviewStatusType;
    },
    orderList() {
      return this.scrollStatus.virtualScroll.list as MyUnlockPosModel[];
    },
  },
});
</script>
