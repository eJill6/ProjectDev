<template>
  <!-- 圖片放大 -->
  <div
    class="video_full_container"
    v-if="itemInfo.mediaType === MediaType.Video"
  >
    <AssetVideo
      :item="setVideoItem(itemInfo)"
      :showMediaControl="true"
      :bottomHight="66"
    ></AssetVideo>
  </div>
  <div
    class="video_delete"
    v-if="itemInfo.mediaType === MediaType.Video && showDeleteButton"
    @click="deleteEvent" style="z-index: 1;"
  >
    <CdnImage src="@/assets/images/element/ic_pic_delet.svg" />
    <p>删除</p>
  </div>
  <div class="pic_zoomin_outter" v-if="itemInfo.mediaType === MediaType.Image">
    <div class="pic_zoomin">
      <AssetImage :item="setImageItem(itemInfo)" />
    </div>
    <div class="pic_delete" v-if="showDeleteButton" @click="deleteEvent">
      <CdnImage src="@/assets/images/element/ic_pic_delet.svg" />
      <p>删除</p>
    </div>
  </div>
</template>
<script lang="ts">
import { MediaType } from "@/enums";
import CdnImage from "./CdnImage.vue";
import { ImageItemModel, MediaModel, VideoItemModel } from "@/models";
import { defineComponent } from "vue";
import AssetImage from "./AssetImage.vue";
import AssetVideo from "./AssetVideo.vue";

export default defineComponent({
  components: { AssetImage, AssetVideo, CdnImage },
  props: {
    image: {
      type: Object as () => MediaModel,
      required: true,
    },
    isEdit: Boolean,
  },
  emits: ["deleteImage"],
  data() {
    return {
      MediaType,
    };
  },
  methods: {
    deleteEvent() {
      this.$emit("deleteImage", this.itemInfo);
    },
    setImageItem(info: MediaModel) {
      let item: ImageItemModel = {
        id: info.id,
        subId: info.bytes,
        class: "box_image",
        src: info.bytes,
        alt: "",
      };
      return item;
    },
    setVideoItem(info: MediaModel) {
      let item: VideoItemModel = {
        id: info.id,
        class: "video_normal",
        src: info.bytes,
        alt: "",
        isCloud: info.isCloud || false,
        coverUrl: "",
      };
      return item;
    },
  },
  computed: {
    itemInfo() {
      return this.image;
    },
    showDeleteButton() {
      return this.isEdit;
    }
  },
});
</script>
