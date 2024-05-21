<template>
  <div class="sheet_main full_width">
    <div class="title title_full_width" :class="extendsClass">
      {{ titleName }}
    </div>
    <div class="content_full">
      <div
        class="upload_image_outter"
        v-for="(itemList, index) in outterImageList"
      >
        <div class="box" v-for="item in itemList" @click="showImageZoom(item)">
          <AssetImage :item="setImageItem(item)" />
        </div>
        <div class="box" v-if="showButton(itemList)">
          <form>
            <label
              for="upload_btn"
              class="upload_btn"
              :class="appendPlusBoxClass"
            >
              <div class="icon">
                <img src="@/assets/images/element/ic_upload.svg" alt="" />
              </div>
            </label>
            <input
              id="upload_btn"
              type="file"
              accept="image/*"
              @change="onFileChanged($event, sourceType, mediaType)"
            />
          </form>
        </div>
        <div class="box" v-if="showBox(itemList, index)"></div>
      </div>
      <div
        class="upload_image_outter"
        v-if="
          imageList.length % rowMaxCount === 0 && imageList.length !== maxCount
        "
      >
        <div class="box">
          <form>
            <label
              for="upload_btn"
              class="upload_btn"
              :class="appendPlusBoxClass"
            >
              <div class="icon">
                <img src="@/assets/images/element/ic_upload.svg" alt="" />
              </div>
            </label>
            <input
              id="upload_btn"
              type="file"
              accept="image/*"
              @change="onFileChanged($event, sourceType, mediaType)"
            />
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { DecryptoSourceType, MediaType, SourceType } from "@/enums";
import { MediaCenter, DecryptoManager } from "@/mixins";
import { ImageItemModel, MediaModel } from "@/models";
import { defineComponent } from "vue";
import AssetImage from "./AssetImage.vue";

export default defineComponent({
  props: {
    title: String,
    source: Number,
    media: Number,
    max: Number,
    plusBoxClass: String,
  },
  components: { AssetImage },
  mixins: [MediaCenter, DecryptoManager],
  data() {
    return {
      rowMaxCount: 3,
    };
  },
  methods: {
    async showImageZoom(imageModel: MediaModel) {
      if (imageModel.bytes.indexOf("http") === 0) {
        imageModel.bytes = await this.fetchSingleDownload(imageModel.bytes);
      }
      this.$emit("show", imageModel);
    },
    showButton(itemList: MediaModel[]): Boolean {
      return (
        itemList.length !== this.rowMaxCount &&
        this.imageList.length < this.maxCount
      );
    },
    showBox(itemList: MediaModel[], index: number) {
      return (
        this.rowMaxCount - 1 > itemList.length ||
        (this.imageList.length === this.maxCount &&
          this.outterImageList.length - 1 === index &&
          itemList.length % this.rowMaxCount !== 0)
      );
    },
    setImageItem(info: MediaModel) {
      let item: ImageItemModel = {
        id: "",
        sourceType: DecryptoSourceType.ImageSelection,
        class: "box_image",
        src: info.bytes,
        alt: "",
      };
      return item;
    },
  },
  computed: {
    appendPlusBoxClass() {
      return this.plusBoxClass || "";
    },
    extendsClass() {
      return this.sourceType === SourceType.Report ? "highlight" : "";
    },
    maxCount() {
      return this.max || 0;
    },
    titleName() {
      return `${this.title}（${this.imageList.length} / ${this.maxCount}）`;
    },
    imageList() {
      return this.$store.state.imageSelectList;
    },
    outterImageList(): MediaModel[][] {
      const outterArray: MediaModel[][] = [];
      const copyArray = JSON.parse(JSON.stringify(this.imageList));
      const imageCount = copyArray.length;
      const total = Math.ceil(imageCount / this.rowMaxCount);
      for (let i = 0; i < total; i++) {
        const offsetCondition = this.rowMaxCount - imageCount >= 0;
        let offset = offsetCondition ? imageCount : this.rowMaxCount;
        const splitArray = copyArray.splice(0, offset);
        outterArray.push(splitArray);
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
    this.cleanImagesState();
  },
});
</script>
