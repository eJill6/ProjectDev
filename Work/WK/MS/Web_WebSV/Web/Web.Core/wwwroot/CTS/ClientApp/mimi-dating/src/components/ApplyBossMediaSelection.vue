<template>
    <div class="sheet_main full_width">
        <div class="title title_full_width">   {{ coverPhotoTitleName }}</div>
        <div class="content_full">
            <div class="upload_image_outter"   v-for="(itemList, index) in outterCoverPhotoImageList"> 
              <div class="box spacing" v-for="item in itemList" @click="showImageZoom(item)">
                    <AssetImage :item="setImageItem(item)" v-if="mediaType === MediaType.Image" />
                    <AssetVideo :item="setVideoItem(item)" v-else></AssetVideo>
              </div>
              <div class="box spacing"  v-if="showCoverPhotoButton(itemList)" >
                  <form>
                    <label :for="uploadCoverPhotoBtnName" class="upload_btn spacing" >
                      <div class="icon">
                        <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" /> 
                      </div>
                    </label>
                    <input  :id="uploadCoverPhotoBtnName" type="file" :accept="acceptKind" multiple sourceType="4"  @change="onFileChanged($event, coverPhotoSourceType, mediaType, coverPhotoMaxCount)"/>
                </form>
              </div>
              
              <div class="box spacing" v-if="showCoverPhotoBox(itemList, index)"></div>
            </div>
                                                       
            <div class="upload_image_outter" v-if="coverPhotoMediaList.length % coverPhotoRowMaxCount === 0 && coverPhotoMediaList.length < coverPhotoMaxCount">
                <div class="box spacing">
                    <form>
                        <label :for="uploadCoverPhotoBtnName" class="upload_btn spacing"  >
                        <div class="icon">
                            <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
                        </div>
                        </label>
                        <input :id="uploadCoverPhotoBtnName" type="file" :accept="acceptKind" sourceType="4"  multiple @change="onFileChanged($event, coverPhotoSourceType, mediaType, coverPhotoMaxCount)"/>
                    </form>
                </div>
            </div>

        </div>
    </div>
    <div class="sheet_main full_width">
        <div class="title title_full_width">{{titleName}}</div>
        <div class="content_full">
            <div class="upload_image_outter" v-for="(itemList, index) in outterImageList">
                <div class="box spacing" v-for="item in itemList" @click="showImageZoom(item)">
                
                    <div class="icon">
                      <AssetImage :item="setImageItem(item)" v-if="mediaType === MediaType.Image" />
                      <AssetVideo :item="setVideoItem(item)" v-else></AssetVideo>
                    </div>
                </div>
                <div class="box spacing"  v-for="(item,index) in maxCount-itemList.length" >
                  <form>
                        <label :for="uploadShopPhotoBtnName" class="upload_btn spacing"  :class="appendPlusBoxClass">
                        <div class="icon">
                            <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
                        </div>
                        </label>
                        <input :id="uploadShopPhotoBtnName" type="file"  :accept="acceptKind"  sourceType="6"  multiple @change="onFileChanged($event, shopPhotoSourceType, mediaType, maxCount)"/>
                  </form>
                </div>


            </div>
            <div class="upload_image_outter">
              

               <div class="box spacing"  v-if="mediaList.length % rowMaxCount === 0 && mediaList.length < rowMaxCount" >
                  <form>
                        <label :for="uploadShopPhotoBtnName" class="upload_btn spacing"  :class="appendPlusBoxClass">
                        <div class="icon">
                            <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
                        </div>
                        </label>
                        <input :id="uploadShopPhotoBtnName" type="file"  :accept="acceptKind"  sourceType="6"  multiple @change="onFileChanged($event, shopPhotoSourceType, mediaType, maxCount)"/>
                  </form>
                </div>

         

                <div class="box spacing"   v-if="mediaList.length % rowMaxCount === 0 && mediaList.length < rowMaxCount" >
                  <form>
                        <label :for="uploadShopPhotoBtnName" class="upload_btn spacing"  :class="appendPlusBoxClass">
                        <div class="icon">
                            <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
                        </div>
                        </label>
                        <input :id="uploadShopPhotoBtnName" type="file"  :accept="acceptKind" sourceType="6"  multiple @change="onFileChanged($event, shopPhotoSourceType, mediaType, maxCount)"/>
                  </form>
                </div>

                <div class="box spacing"    v-if="mediaList.length % rowMaxCount === 0 && mediaList.length < rowMaxCount" >
                  <form>
                        <label  :for="uploadShopPhotoBtnName" class="upload_btn spacing"  :class="appendPlusBoxClass">
                        <div class="icon">
                            <CdnImage src="@/assets/images/element/ic_upload.svg" alt="" />
                        </div>
                        </label>
                        <input :id="uploadShopPhotoBtnName" type="file" :accept="acceptKind" sourceType="6" multiple @change="onFileChanged($event, shopPhotoSourceType, mediaType, maxCount)"/>
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
    coverPhotoTitle:String,
    coverPhotoMax:Number,
    coverPhotoSource: Number,
    shopPhotoSource:Number,
    media: Number,
    max: Number,
    plusBoxClass: String,
    showMediaControl: Boolean,
  },
  components: { AssetImage, AssetVideo, CdnImage },
  mixins: [MediaCenter, Tools],
  emits: ["show"],
  data() {
    return {
      rowMaxCount: 3,
      coverPhotoRowMaxCount:1,
      MediaType
    };
  },
  methods: {
    showImageZoom(imageModel: MediaModel) {
      this.$store.state.isImageZoomMode=true;
      this.$emit("show", imageModel);
    },
    showButton(itemList: MediaModel[]): Boolean {
      return (
        itemList.length !== this.rowMaxCount &&
        this.mediaList.length < this.maxCount
      );
    },
    showCoverPhotoButton(itemList: MediaModel[]): Boolean {
      return (
        itemList.length !== this.coverPhotoRowMaxCount &&
        this.coverPhotoMediaList.length < this.coverPhotoMaxCount
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
    showCoverPhotoBox(itemList: MediaModel[], index: number){
      return (
        this.coverPhotoRowMaxCount - 1 > itemList.length ||
        (this.coverPhotoMediaList.length === this.coverPhotoMaxCount &&
          this.outterCoverPhotoImageList.length - 1 === index &&
          itemList.length % this.coverPhotoRowMaxCount !== 0)
      );
    },
    setImageItem(info: MediaModel) {
      let item: ImageItemModel = {
        id: info.id,
        subId: info.bytes,
        class: "box_image spacing",
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
  created() {

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
    maxCount() {
      return this.max || 0;
    },
    coverPhotoMaxCount(){
        return this.coverPhotoMax || 0;
    },
    titleName() {
      return `${this.title}（${this.mediaList.length} / ${this.maxCount}）`;
    },
    coverPhotoTitleName(){
        return `${this.coverPhotoTitle}（${this.coverPhotoMediaList.length} / ${this.coverPhotoMaxCount}）`;
    },
    mediaList() {

      
      return this.$store.state.mediaSelectList.filter(
        (item) => item.mediaType === this.mediaType && item.sourceType === this.shopPhotoSourceType
      );
    },
    coverPhotoMediaList(){
  
        return this.$store.state.mediaSelectList.filter(
        (item) => item.mediaType === this.mediaType && item.sourceType === this.coverPhotoSourceType
      );
    },
    uploadCoverPhotoBtnName(){
        return "uploadCoverPhoto_upload_btn";
    },
    uploadShopPhotoBtnName(){
      return "uploadShopPhoto_upload_btn";
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
    outterCoverPhotoImageList():MediaModel[][]{
        const outterArray: MediaModel[][] = [];
        const foreachCount = Math.ceil(this.coverPhotoMediaList.length / this.coverPhotoRowMaxCount);
        for (let i = 0; i < foreachCount; i++) {
            const startIndex = i * this.coverPhotoRowMaxCount;
            const newArray = this.coverPhotoMediaList.slice(
            startIndex,
            startIndex + this.coverPhotoRowMaxCount
            );
            outterArray.push(newArray);
        }
        return outterArray;
    },
    coverPhotoSourceType() {
  
      return this.coverPhotoSource as SourceType;
    },
    shopPhotoSourceType(){

      return this.shopPhotoSource as SourceType;
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