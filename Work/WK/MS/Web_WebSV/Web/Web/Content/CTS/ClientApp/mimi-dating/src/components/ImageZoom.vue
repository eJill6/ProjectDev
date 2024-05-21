<template>
  <!-- 圖片放大 -->
  <div class="pic_zoomin_outter">
    <div class="pic_zoomin">
      <img :src="itemInfo.bytes" />
    </div>
    <div class="pic_delet" v-if="showDeleteButton" @click="deleteEvent">
      <img src="@/assets/images/element/ic_pic_delet.svg" />
      <p>删除</p>
    </div>
  </div>
</template>
<script lang="ts">
import { MediaModel } from "@/models";
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    image: {
      type: Object as () => MediaModel,
      required: true,
    },
    edit: Boolean,
  },
  methods:{
    deleteEvent(){
      this.$emit("deleteImage",this.itemInfo);
    }
  },
  computed: {
    itemInfo() {
      return this.image;
    },
    showDeleteButton() {
      return this.edit;
    },
  },
});
</script>
