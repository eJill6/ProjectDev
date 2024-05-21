<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post bg_post_b">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_b_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_b_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn" @click="verifyIdentity">
          <div class="btn_default">立即发帖</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame } from "@/mixins";
import api from "@/api";
import { AdvertisingContentType, IdentityType, PostType } from "@/enums";
import { WhatIsDataModel } from "@/models";
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
      if (
        this.certificationStatus.applyIdentity !== IdentityType.Agent &&
        this.certificationStatus.applyIdentity !== IdentityType.Boss
      ) {
        toast("仅觅经纪/觅老板才可发帖");
        return;
      }
      if (this.userInfo.quantity.remainingSend <= 0) {
        toast("发帖剩余次数不足");
        return;
      }
      this.navigateToFrom(PostType.Agency);
    },
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(
          PostType.Square,
          AdvertisingContentType.Agency
        );
      } catch (error) {
        toast(error);
      }
    },
  },
  async created() {
    await this.getWhatIs();
  },
  computed:{
    certificationStatus() {
      return this.$store.state.certificationStatus;
    },
  }
});
</script>
