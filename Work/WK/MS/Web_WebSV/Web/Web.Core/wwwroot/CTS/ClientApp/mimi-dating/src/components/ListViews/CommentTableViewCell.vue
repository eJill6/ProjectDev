<template>
  <div class="item">
    <div class="user">
      <div class="avatar avatar_fit">
        <img :src="info.avatarUrl" alt="" />
      </div>
      <div class="text">
        <p>{{ info.nickname }}</p>
        <p>{{ info.publishTime }} 发布</p>
      </div>
    </div>
    <div class="comment_text">
      <p>【时间】：{{ info.spentTime }}</p>
      <p>【所在位置】：{{ cityName(info.areaCode) }}</p>
      <p>【服务详情】：{{ info.comment }}</p>
    </div>
    <div class="photos">
      <div
        class="outter"
        v-for="photos in outterImageList(info.photoUrls)"
        v-if="info.photoUrls && info.photoUrls.length"
      >
        <div class="box" v-for="photo in photos">
          <AssetImage
            :item="setImageItem(photo)"
            @click="showImageZoom(photo)"
          />
        </div>
        <div
          class="box"
          v-for="count in rowMaxCount - photos.length"
          v-if="rowMaxCount - photos.length > 0"
        ></div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import CommentBaseCell from "./CommentBaseCell"
import { Tools } from "@/mixins";
import { defineComponent } from "vue";

export default defineComponent({
  mixins: [Tools],
  extends: CommentBaseCell
});
</script>
