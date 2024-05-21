<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post bg_post_d">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_d_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_d_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn">
          <div class="btn_default">立即发帖</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame } from "@/mixins";
import api from "@/api";
import { AdvertisingContentType, PostType } from "@/enums";
import { WhatIsDataModel } from "@/models";
import toast from "@/toast";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, DialogControl, PlayGame],
  data() {
    return {
      messages: {} as WhatIsDataModel,
    };
  },
  methods: {
    verifyIdentity() {},
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(
          PostType.Square,
          AdvertisingContentType.TalentScout
        );
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
