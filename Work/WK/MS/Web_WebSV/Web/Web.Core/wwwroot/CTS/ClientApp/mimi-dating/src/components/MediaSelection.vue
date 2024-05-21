<template>
  <div class="sheet_main full_width">
    <div class="title title_full_width" :class="extendsClass">
      {{ titleName }}
      <template v-if="showAnnotation != ''"></template> 
      <span class="title_red">*</span>
      <span class="annotation_red">{{ showAnnotation }}</span>
    </div>
    <div class="content_full">
      <div
        class="upload_image_outter"
        v-for="(itemList, index) in outterImageList"
      >
        <div class="box spacing" v-for="item in itemList" @click="showImageZoom(item)">
          <AssetImage
            :item="setImageItem(item)"
            v-if="mediaType === MediaType.Image"
          />
          <AssetVideo :item="setVideoItem(item)" v-else></AssetVideo>
        </div>
        <div class="box spacing" v-if="showButton(itemList)">
          <form>
            <label
              :for="btnName"
              class="upload_btn"
              :class="appendPlusBoxClass"
            >
              <div class="icon">
                <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
              </div>
            </label>
            <input
              :id="btnName"
              type="file"
              :accept="acceptKind"
              multiple
              @change="onFileChanged($event, sourceType, mediaType, maxCount)"
            />
          </form>
        </div>
        <div class="box" v-if="showBox(itemList, index)"></div>
      </div>
      <div
        class="upload_image_outter"
        v-if="
          mediaList.length % rowMaxCount === 0 && mediaList.length !== maxCount
        "
      >
        <div class="box">
          <form>
            <label
              :for="btnName"
              class="upload_btn"
              :class="appendPlusBoxClass"
            >
              <div class="icon">
                <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
              </div>
            </label>
            <input
              :id="btnName"
              type="file"
              :accept="acceptKind"
              multiple
              @change="onFileChanged($event, sourceType, mediaType, maxCount)"
            />
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import CdnImage from "./CdnImage.vue";
import { MediaType, SourceType } from "@/enums";
import { MediaCenter, Tools } from "@/mixins";
import { ImageItemModel, MediaModel, VideoItemModel } from "@/models";
import { defineComponent } from "vue";
import AssetImage from "./AssetImage.vue";
import AssetVideo from "./AssetVideo.vue";

export default defineComponent({
  props: {
    title: String,
    source: Number,
    media: Number,
    max: Number,
    plusBoxClass: String,
    showMediaControl: Boolean,
    annotation: String,
  },
  components: { AssetImage, AssetVideo, CdnImage },
  mixins: [MediaCenter, Tools],
  emits: ["show"],
  data() {
    return {
      rowMaxCount: 3,
      MediaType,
    };
  },
  methods: {
    showImageZoom(imageModel: MediaModel) {
      this.$emit("show", imageModel);
    },
    showButton(itemList: MediaModel[]): Boolean {
      return (
        itemList.length !== this.rowMaxCount &&
        this.mediaList.length < this.maxCount
      );
    },
    showBox(itemList: MediaModel[], index: number) {
      return (
        this.rowMaxCount - 1 > itemList.length ||
        (this.mediaList.length === this.maxCount &&
          this.outterImageList.length - 1 === index &&
          itemList.length % this.rowMaxCount !== 0)
      );
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
        class: "box_image",
        src: info.bytes,
        alt: "",
        isCloud: info.isCloud || false,
        coverUrl: "",
      };
      return item;
    },
  },
  computed: {
    btnName() {
      return this.mediaType === MediaType.Image
        ? "upload_btn"
        : "upload_video_btn";
    },
    acceptKind() {
      return this.mediaType === MediaType.Image
        ? "image/*"
        : "video/mp4,video/x-m4v,video/*";
    },
    appendPlusBoxClass() {
      return this.plusBoxClass || "";
    },
    showAnnotation(){
      return this.annotation || "";
    },
    extendsClass() {
      return this.sourceType === SourceType.Report ? "highlight highlight_color" : "";
    },
    maxCount() {
      return this.max || 0;
    },
    titleName() {
      return `${this.title}（${this.mediaList.length} / ${this.maxCount}）`;
    },
    mediaList() {
      return this.$store.state.mediaSelectList.filter(
        (item) => item.mediaType === this.mediaType
      );
    },
    outterImageList(): MediaModel[][] {
      const outterArray: MediaModel[][] = [];
      const foreachCount = Math.ceil(this.mediaList.length / this.rowMaxCount);
      for (let i = 0; i < foreachCount; i++) {
        const startIndex = i * this.rowMaxCount;
        const newArray = this.mediaList.slice(
          startIndex,
          startIndex + this.rowMaxCount
        );
        outterArray.push(newArray);
      }
      return outterArray;
    },
    sourceType() {
      return this.source as SourceType;
    },
    mediaType() {
      return this.media as MediaType;
    },
  },
  beforeUnmount() {
    this.cleanMediaState();
  },
});
</script>
<style scoped>
.video-js {
  width: 105px;
  height: 105px;
}
</style>
