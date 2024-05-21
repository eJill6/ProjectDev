<template>
  <!-- 共同滑動區塊 -->
  <div class="flex_height">
    <div
      class="overflow no_scrollbar"
      ref="scrollContainer"
      @scroll="onScroll"
      @touchstart="onTouchStart"
      @touchmove="onTouchMove"
      @touchend="endEvent"
      @mousedown="onMouseDown"
      @mouseup="onMouseUp"
    >
      <div class="padding_basic pt_0">
        <label-filter
          :post-type="$_PostType"
          @filter="filterCondition"
        ></label-filter>

        <div class="product_view">
          <ul
            class="full_cards"
            :style="{
              'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
              'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
            }"
          >
            <MainTableViewCell
              :info="info"
              v-for="info in orderList"
              @click="navigateToProductDetail(info.postId)"
            ></MainTableViewCell>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PageManager } from "@/mixins";
import { LabelFilter, MainTableViewCell } from "@/components";

export default defineComponent({
  mixins: [PageManager],
  components: { LabelFilter, MainTableViewCell },
  async created() {
    await this.checkDetailToReturn();
  },
});
</script>
