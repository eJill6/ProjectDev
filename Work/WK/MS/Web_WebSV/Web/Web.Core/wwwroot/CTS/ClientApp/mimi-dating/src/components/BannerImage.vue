<template>
  <template v-if="isContainer">
    <img :src="imageString" alt="" v-if="isHome" />
    <div class="cover_pic_outter" v-else>
      <img class="cover_pic" :src="imageString" alt="" />
    </div>
  </template>
  <template v-else>
    <CdnImage src="@/assets/images/index/banner_default.png" alt="" />
  </template>
</template>


<script lang="ts">
import { defineComponent } from "vue";
import { ScrollManager } from "@/mixins";
import { ImageItemModel } from "@/models";
import CdnImage from "./CdnImage.vue";

export default defineComponent({
  components: { CdnImage },
  mixins: [ScrollManager],
  props: {
    item: {
      type: Object as () => ImageItemModel,
      required: true,
    },
    isHome: Boolean,
  },
  data() {
    return {
      imageSrc: require("@/assets/images/index/banner_default.png") as string,
    };
  },
  computed: {
    isContainer(): boolean {
      const container = this.$store.state.imageCache.get(this.item.id);
      if (container) {
        return !!container.get(this.item.subId);
      }
      return false;
    },
    imageString() {
      const container = this.$store.state.imageCache.get(this.item.id);
      if (container) {
        return container.get(this.item.subId) || "";
      }
      return "";
    },
  },
});
</script>
