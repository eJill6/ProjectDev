<template>
  <!-- 共同滑動區塊 -->
  <div class="flex_height">
    <div
      class="overflow no_scrollbar"
      ref="scrollContainer"
      @scroll="scrollEvent"
      @touchstart="onTouchStartEvent"
      @touchmove="onTouchMoveEvent"
      @mousedown="onMouseDownEvent"
      @mouseup="onMouseUpEvent"
      @touchend="endEventAction"
    >
      <div class="padding_basic pt_0" >
        <label-filter
          :post-type="$_PostType"
          @filter="filterCondition"
        ></label-filter>

        <div class="waterfall_view" v-if="isDoubleRow" ref="waterfallView">
          <div class="waterfall_section" ref="leftWaterfall">
            <WaterfallViewCell
              :info="info"
              v-for="info in leftWaterfallSection"
              @click="navigateToProductDetail(info.postId)"
              :key="info.watterfallId"
            ></WaterfallViewCell>
          </div>
          <div class="waterfall_section" ref="rightWaterfall">
            <WaterfallViewCell
              :info="info"
              v-for="info in rightWaterfallSection"
              @click="navigateToProductDetail(info.postId)"
              :key="info.watterfallId"
            ></WaterfallViewCell>
          </div>
        </div>
        <div
          class="loading_bar"
          v-if="showWaterfallBottomLoadingBar && isDoubleRow"
        >
          <div class="loading-dot"></div>
          <div class="loading-dot"></div>
          <div class="loading-dot"></div>
        </div>

        <div class="product_view" v-if="!isDoubleRow">
          <ul
            class="full_cards"
            :style="{
              'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
              'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
            }"
          >
            <MainTableViewCell
              :info="info"
              :isEnsure="isAgency"
              v-for="info in orderList"
              :key="info.postId"
              @click="navigateToProductDetail(info.postId)"
            ></MainTableViewCell>
          </ul>
          <div class="loading_bar" v-if="showBottomLoadingBar">
            <div class="loading-dot"></div>
            <div class="loading-dot"></div>
            <div class="loading-dot"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PageManager, WaterfallManager } from "@/mixins";
import {
  LabelFilter,
  MainTableViewCell,
  CdnImage,
  WaterfallViewCell,
} from "@/components";
import { PostModeType, PostType } from "@/enums";
import { ImageItemModel, LabelFilterModel, ProductListModel } from "@/models";
import AssetImage from "@/components/AssetImage.vue";
import { MutationType } from "@/store";

export default defineComponent({
  mixins: [PageManager, WaterfallManager],
  data() {
    return {};
  },
  components: {
    LabelFilter,
    MainTableViewCell,
    CdnImage,
    AssetImage,
    WaterfallViewCell,
  },
  props: {
    lastPostType: {
      type: Number as () => PostType,
      required: true,
    },
  },
  watch: {
    async searchStatus(value: Boolean) {
      if (value) {
        this.$store.commit(MutationType.SetSearchStatus, false);
        if (this.isDoubleRow) {
          await this.reloadWaterfall();
        } else {
          await this.reload();
        }
      }
    },
  },
  methods: {
    scrollEvent(e: Event) {
      if (!this.isDoubleRow) {
        this.onScroll(e);
      } else {
        this.onWaterfallScroll(e);
      }
    },
    onTouchStartEvent(e: TouchEvent) {
      if (!this.isDoubleRow) {
        this.onTouchStart(e);
      } else {
        this.onWaterfallTouchStart(e);
      }
    },
    onTouchMoveEvent(e: TouchEvent) {
      if (!this.isDoubleRow) {
        this.onTouchMove(e);
      } else {
        this.onWaterfallTouchMove(e);
      }
    },
    onMouseDownEvent(e: MouseEvent) {
      if (!this.isDoubleRow) {
        this.onMouseDown(e);
      } else {
        this.onWaterfallMouseDown(e);
      }
    },
    onMouseUpEvent(e: MouseEvent) {
      if (!this.isDoubleRow) {
        this.onMouseUp(e);
      } else {
        this.onWaterfallMouseUp(e);
      }
    },
    endEventAction() {
      if (!this.isDoubleRow) {
        this.endEvent();
      } else {
        this.endWaterfallEvent();
      }
    },
    async filterCondition(condition: LabelFilterModel) {
      this.$store.commit(MutationType.SetFilter, condition);
      this.$store.commit(MutationType.SetWaterfallTop, 0);
      if (condition.modeType === PostModeType.doubleRow) {
        await this.reloadWaterfall();
      } else {
        await this.reload();
      }
    },
    setImageItem(info: ProductListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.coverUrl,
        class: "photo",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
    viewCountStr(info: ProductListModel) {
      const views = Number(info.views) || 0;
      if (views < 100000) {
        return views;
      }
      return (views / 10000).toFixed(1).replace(".0", "") + "w";
    },
  },
  async mounted() {
    if (this.lastPostType !== this.$_PostType) {
      this.scrollStatus.list = [];
      if (this.isDoubleRow) {
        this.resetWaterfallModel();
      }
    }
    if (this.isDoubleRow) {
      await this.checkTopToReturn();
    } else {
      await this.checkDetailToReturn();
    }
  },
  updated() {
    let body = document.getElementsByTagName("body")[0];
    body.style.fontSize = "12px";    
  },
  computed: {
    searchStatus() {
      return this.$store.state.searchStatus;
    },
    isDoubleRow() {
      return this.$store.state.filter.modeType === PostModeType.doubleRow;
    },
    $_virtualScrollItemElemHeight() {
      return 144;
    },
    isAgency() {
      return this.$store.state.postType === PostType.Agency;
    },
    orderList() {
      return (this.scrollStatus.virtualScroll.list || []) as ProductListModel[];
    },
  },
});
</script>
