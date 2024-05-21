<template>
  <img
    :class="item.class"
    :alt="item.alt"
    :src="imageString"
    @click="$emit('clickEvent')"
  />
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { DecryptoManager, ScrollManager } from "@/mixins";
import { DecryptoSourceType } from "@/enums";
import { ImageItemModel, ProductListModel } from "@/models";

export default defineComponent({
  mixins: [DecryptoManager, ScrollManager],
  props: {
    item: {
      type: Object as () => ImageItemModel,
      required: true,
    },
  },
  emits: ["clickEvent"],
  data() {
    return {
      isExist: false,
      defaultIamgeSrc:
        require("@/assets/images/element/defaultCoverImage.png") as string,
      imageSrc:
        require("@/assets/images/element/defaultCoverImage.png") as string,
    };
  },
  watch: {
    item(value) {
      if (Object.keys(value).length > 0) {
        this.setImageSrc();
      }
    },
  },
  methods: {
    //处理破图
    moveErrorImg(event: any) {
      event.currentTarget.src = this.defaultIamgeSrc;
      event.currentTarget.style.width = "auto";
      return true;
    },
    async downloadImage() {
      this.imageSrc = this.defaultIamgeSrc;
      const resultItem = await this.fetchImageFileDownload(this.item);
      if (
        resultItem.sourceType === DecryptoSourceType.MainList ||
        resultItem.sourceType === DecryptoSourceType.MyPostList
      ) {
        let list = this.scrollStatus.list as ProductListModel[];
        let imageResult = list.find((item) => item.postId === resultItem.id);

        if (imageResult) {
          imageResult.coverUrl = resultItem.src;
        }
      }
      this.imageSrc = resultItem.src;
    },
    async setImageSrc() {
      if (this.item.src.indexOf("http") === 0) {
        await this.downloadImage();
      } else {
        this.imageSrc = this.item.src;
      }
    },
  },
  mounted() {
    this.setImageSrc();
  },
  computed: {
    imageString() {
      return this.imageSrc;
    },
  },
});
</script>
