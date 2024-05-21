<template>
  <div class="official_item">
    <div class="official_full_card mb_0">
      <div class="attractive_score">{{ info.facialScore }}</div>
      <div class="location_tag">
        <div class="location_tag_text">
          <CdnImage
            src="@/assets/images/card/ic_card_official_location.svg"
            alt=""
          />
          {{ cityName(info.areaCode) }}
        </div>
      </div>
      <div class="photo large_card">
        <div class="photo_tag_rest" v-if="!info.isOpen">休息中</div>
        <AssetImage :item="setImageItem(info)" />
      </div>
      <div class="official_title">{{ info.title }}</div>
      <div class="info_co">
        <div class="info">
          <div class="info_text">{{ info.height }}cm</div>
          <div class="dot"></div>
          <div class="info_text">{{ info.age }}岁</div>
          <div class="dot"></div>
          <div class="info_text">{{ info.cup }}杯</div>
        </div>
      </div>
      <div class="official_price">期望收入：¥{{ info.lowPrice }}</div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { Tools, NavigateRule } from "@/mixins";
import { ImageItemModel, OfficialListModel } from "@/models";
import AssetImage from "../AssetImage.vue";
import CdnImage from "../CdnImage.vue";

export default defineComponent({
  components: { AssetImage, CdnImage },
  mixins: [Tools, NavigateRule],
  props: {
    info: {
      type: Object as () => OfficialListModel,
      required: true,
    },
    isEnsure: Boolean,
  },
  methods: {
    setImageItem(info: OfficialListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.coverUrl,
        class: "",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
  },
});
</script>
