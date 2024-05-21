<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post bg_post_c">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_f_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_f_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn" @click="showComingSoon">
          <div class="btn_default">申请成为觅女郎</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame } from "@/mixins";
import api from "@/api";
import { AdvertisingContentType, PostType, TipType } from "@/enums";
import { TipInfo, WhatIsDataModel } from "@/models";
import toast from "@/toast";

export default defineComponent({
  mixins: [NavigateRule, DialogControl, PlayGame],
  data() {
    return {
      messages: {} as WhatIsDataModel,
    };
  },
  methods: {
    verifyIdentity() {
    },
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(PostType.Square, AdvertisingContentType.SeekGirl );
      } catch (error) {
        toast(error);
      }
    },
  },
  async created() {
    await this.getWhatIs();
  },
});
</script>
