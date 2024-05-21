<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post">
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
        <div class="introduce_btn" @click="verifyIdentity">
          <div class="btn_default no_shadow">立即发帖</div>
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
import { AdvertisingContentType, IdentityType, PostType } from "@/enums";
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
    verifyIdentity() {
      if (this.userInfo.identity === IdentityType.General) {
        toast("身份不符，无法在此发帖");
      }
      else if (this.userInfo.identity === IdentityType.Boss || this.userInfo.identity === IdentityType.SuperBoss) {
        if (this.userInfo.quantity.remainingSend <= 0) {
          toast("您没有发帖次数，请联系管理员");
        } else {
          this.navigateToFrom(PostType.Agency);
        }
      }
      else if (this.userInfo.identity === IdentityType.Agent) {
        this.navigateToFrom(PostType.Agency);
      } 
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
  computed: {
    certificationStatus() {
      return this.$store.state.certificationStatus;
    },
    userInfo() {
      return this.$store.state.centerInfo;
    },
  }
});
</script>
