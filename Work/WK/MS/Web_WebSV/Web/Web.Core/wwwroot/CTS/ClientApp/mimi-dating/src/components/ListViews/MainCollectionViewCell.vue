<template>
  <div class="official_pd">
    <li class="official_full_card">
      <div class="tag">
        <CdnImage src="@/assets/images/card/ic_wallet_square" alt="" />
      </div>

      <div class="photo">
        <AssetImage :item="setImageItem(info)"></AssetImage>
        <div class="tag_rest" v-if="!info.isOpen">休息中</div>
      </div>
      <div class="official_title">{{ info.title }}</div>
      <div class="official_price">期望收入：¥{{ info.lowPrice }}</div>
    </li>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { ImageItemModel, OfficialListModel, ProductListModel } from "@/models";
import { Tools, NavigateRule } from "@/mixins";
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
