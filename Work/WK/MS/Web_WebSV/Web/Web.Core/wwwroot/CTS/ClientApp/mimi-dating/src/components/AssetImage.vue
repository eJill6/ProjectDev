<template>
  <template v-if="isShowImage()">
    <img :class="item.class" :alt="item.alt" ref="myAssetImage" :src="imageString" @click="$emit('clickEvent')" />
  </template>
  <template v-else>
    <CdnImage src="@/assets/images/element/defaultCoverImage.png" alt="" />
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
  },
  emits: ["clickEvent"],
  data() {
    return {
      imageSrc:
        require("@/assets/images/element/defaultCoverImage.png") as string,
    };
  },
  methods: {
    //处理破图
    moveErrorImg(event: any) {
      event.currentTarget.src = this.imageSrc;
      event.currentTarget.style.width = "auto";
      return true;
    },
    isShowImage(){
      const container = this.$store.state.imageCache.get(this.item.id);
      return this.isContainer || !!this.imageString;
    }
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
        const src = container.get(this.item.subId);
        if((src?.indexOf('data:image') ?? 0) >= 0) {
          this.imageSrc = container.get(this.item.subId) || "";
        }
      }

      if (this.item.src && this.item.src.indexOf("http") !== 0) {
        this.imageSrc = this.item.src;
      }
      return this.imageSrc;
    },
  },
});
</script>
