<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_e_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_e_2.png" alt="" />
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
import { AdvertisingContentType, PostType, TipType, IdentityType } from "@/enums";
import { TipInfo, WhatIsDataModel } from "@/models";
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
      //身份为觅老板
      if (this.userInfo?.identity === IdentityType.Boss || this.userInfo?.identity === IdentityType.SuperBoss) {
        toast("身份不符，无法在此发帖");
      }
      //无身份/无会员卡
      else if (this.userInfo?.vips?.length <= 0 && this.userInfo?.identity === IdentityType.General) {
        if (this.userInfo.quantity.remainingSend > 0) {
          this.navigateToFrom(PostType.Square);
        } else {
          toast("您没有发帖次数，请联系管理员");
        }
      }
      //无身份/有会员卡
      else if (this.userInfo?.vips?.length > 0 && this.userInfo?.identity === IdentityType.General) {
        if (this.userInfo.quantity.remainingSend > 0) {
          this.navigateToFrom(PostType.Square);
        } else {
          if (this.userInfo.quantity.remainingSend <= 0) {
            toast("您今日的发帖次数已用完");
          } else {
            this.navigateToFrom(PostType.Square);
          }
        }
      }
      //觅经纪/无会员卡
      else if (this.userInfo?.identity === IdentityType.Agent) {
        if (this.userInfo.quantity.remainingSend > 0) {
          this.navigateToFrom(PostType.Square);
        } else {
          toast("您没有发帖次数，请联系管理员");
        }
      } else {
        const info: TipInfo = {
          content: "发布广场贴需开通会员",
          tipType: TipType.NonActivated,
          buttonTitle: "去开通",
        };
        this.showTipDialog(info);
      }
    },
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(PostType.Square, AdvertisingContentType.Square);
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