<template>
  <EmptyView :message="`暂无数据～`" v-if="isEmpty"></EmptyView>
  <div class="flex_height bg_personal">
    <div class="overflow no_scrollbar" @scroll="onScroll" ref="scrollContainer">
      <div
        class="padding_order"
        :style="{
          'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
          'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
        }"
      >
        <div class="me_order me_order_second" v-for="dataItem in shopList">
          <div class="user_info user_flex">
            <div class="avatar_fourth avatar_fit">
              <div class="total_view">
                <div class="icon"></div>
                <div class="text">
                  {{ showView(dataItem.views, dataItem.viewBaseCount) }}
                </div>
              </div>
              <AssetImage :item="setImageItem(dataItem)" />
            </div>
            <div class="info" @click="shopDetails(dataItem.applyId)">
              <div class="first_co">
                <div class="basic">
                  <div class="name_info_inner">
                    <div
                      class="name_sixth"
                      :data-text="showTitle(dataItem.shopName)"
                    >
                      {{ showTitle(dataItem.shopName) }}
                    </div>
                  </div>
                </div>
                <div class="basic">
                  <div class="keywords_info_btn">
                    <div class="btn_one">店龄{{ dataItem.shopYears }}年</div>
                    <div class="btn_two">妹子{{ dataItem.girls }}位</div>
                    <div class="btn_three">{{ dataItem.dealOrder }}已约过</div>
                    <div class="btn_four">
                      评分{{ dataItem.selfPopularity }}
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="state" @click="userCanCelFavorite(dataItem.favoriteId)">
              <div>
                <CdnImage src="@/assets/images/me/ic_me_collect.svg" alt="" />
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
import { MutationType } from "@/store";
import {
  NavigateRule,
  DialogControl,
  PlayGame,
  Tools,
  ImageCacheManager,
  VirtualScroll,
  ScrollManager,
} from "@/mixins";
import toast from "@/toast";
import { EmptyView, AssetImage, CdnImage } from "@/components";
import api from "@/api";

import {
  MyFavoriteShopQueryParamModel,
  MyFavoriteShopModel,
  ImageItemModel,
} from "@/models";
export default defineComponent({
  mixins: [
    NavigateRule,
    DialogControl,
    PlayGame,
    Tools,
    ImageCacheManager,
    VirtualScroll,
    ScrollManager,
  ],
  components: {
    EmptyView,
    AssetImage,
    CdnImage,
  },
  data() {
    return {
      isEmpty: false,
    };
  },
  methods: {
    showView(view: number, viewBase: number) {
      return view + viewBase;
    },
    shopDetails(applyId: string) {
      this.navigateToOfficialShopDetail(applyId);
    },
    async loadAsync() {
      if (
        this.isLoading ||
        this.scrollStatus.totalPage === this.pageInfo.pageNo ||
        this.isPromising
      )
        return;

      this.$store.commit(MutationType.SetIsLoading, true);

      try {
        const pageSize = 10;
        const nextPage = this.pageInfo.pageNo + 1;
        const search: MyFavoriteShopQueryParamModel = {
          pageNo: nextPage,
          pageSize: pageSize,
          ts: "",
        };
        const result = await api.getMyFavoriteShop(search);

        this.scrollStatus.totalPage = result.totalPage;
        this.myFavoriteShopDownload(result.data);
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
    showTitle(title: string) {
      if (title !== null && title !== undefined) {
        return title.length > 12 ? title.substring(0, 12) : title;
      } else {
        return "";
      }
    },
    async reload() {
      this.resetPageInfo();
      this.resetScroll();
      await this.loadAsync();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    setImageItem(info: MyFavoriteShopModel) {
      let item: ImageItemModel = {
        id: info.favoriteId,
        subId: info.bossId,
        class: "",
        src: info.shopAvatarSource,
        alt: "",
      };
      return item;
    },
    async userCanCelFavorite(favoriteId: string) {
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        var result = await api.canCelFavorite(favoriteId);
      } catch (e) {
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.shopList.splice(
        this.shopList.findIndex((c) => c.favoriteId === favoriteId),
        1
      );
      this.isEmpty = !this.shopList.length;
    },
  },
  async created() {
    await this.reload();
  },
  computed: {
    currentOrderStatus() {
      return this.$store.state.myCollectViewTypeStatus;
    },
    $_virtualScrollItemElemHeight() {
      return 155;
    },
    shopList() {
      return this.scrollStatus.virtualScroll.list as MyFavoriteShopModel[];
    },
  },
});
</script>
