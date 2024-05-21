<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage
              src="@/assets/images/header/ic_header_chat_arrow.svg"
              alt=""
            />
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <!-- 圖片放大 -->
        <div class="pic_zoomin_outter">
          <div class="pic_zoomin">
            <AssetImage :item="imageItem"/>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { AssetImage, CdnImage } from "@/components";
import { defineComponent } from "vue";
import { NavigateRule } from "@/mixins";

export default defineComponent({
  components: { AssetImage, CdnImage },
  mixins: [NavigateRule],
  computed: {
    imageItem() {
      return this.$store.state.imageShowItem;
    },
  },
});
</script>
