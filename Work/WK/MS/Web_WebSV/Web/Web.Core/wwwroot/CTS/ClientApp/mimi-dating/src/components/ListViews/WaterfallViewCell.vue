<template>
  <div class="waterfall_wrapper">
    <div class="item">
      <div class="pin_outter">
        <div class="pin">
          <CdnImage src="@/assets/images/post/ic_post_location.svg" />
        </div>
        <div class="pin_text">{{ shortCityName(info.areaCode) }}</div>
      </div>
      <div class="library">
        <div class="certification" v-if="info.isCertified">
          <CdnImage src="@/assets/images/card/ic_waterfall_certification.svg" />
        </div>
        <AssetImage :item="setImageItem(info)" />
        <div class="view_outer">
          <div>
            <CdnImage
              class="view"
              src="@/assets/images/card/ic_card_num_watch.svg"
            />
          </div>
          <div class="view_text">{{ viewCountStr }}</div>
        </div>
      </div>
      <div class="outer">
        <div class="name">{{ info.title }}</div>
        <div class="info">
          <div class="icon">
            <CdnImage src="@/assets/images/card/ic_card_body_info.svg" />
          </div>
          <div class="subtext">{{ info.height }}cm</div>
          <div class="dot"></div>
          <div class="subtext">{{ info.age }}岁</div>
          <div class="dot"></div>
          <div class="subtext">{{ info.cup }}杯</div>
        </div>
        <div class="text">期望收入：¥{{ info.lowPrice }}</div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { ImageItemModel, ProductListModel } from "@/models";
import { Tools, NavigateRule } from "@/mixins";
import { AssetImage, CdnImage } from "@/components";

export default defineComponent({
  components: { AssetImage, CdnImage },
  mixins: [Tools, NavigateRule],
  props: {
    info: {
      type: Object as () => ProductListModel,
      required: true,
    },
  },
  methods: {
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
  },
  computed: {
    viewCountStr() {
      const views = Number(this.info.views) || 0;
      if (views < 100000) {
        return views;
      }
      return (views / 10000).toFixed(1).replace(".0", "") + "w";
    },
  },
});
</script>
