<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post bg_post_a">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_e_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_e_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn" @click="verifyIdentity">
          <div class="btn_default">申请成为觅经纪</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame } from "@/mixins";
import api from "@/api";
import {
  AdvertisingContentType,
  IdentityApplyStatusType,
  IdentityType,
  PostType,
} from "@/enums";
import { MessageDialogModel, WhatIsDataModel } from "@/models";
import toast from "@/toast";
import { MutationType } from "@/store";
import global from "@/global";

export default defineComponent({
  mixins: [NavigateRule, DialogControl, PlayGame],
  data() {
    return {
      messages: {} as WhatIsDataModel,
      isApplying: false,
    };
  },
  methods: {
    async verifyIdentity() {
      if (!this.userInfo.hasPhone) {
        const messageModel: MessageDialogModel = {
          message: "需要绑定手机号码才能申请觅经纪/觅老板/觅女郎",
          cancelTitle: "前往绑定",
          buttonTitle: "暂不绑定",
          cancelButtonEnable: true,
        };
        this.showMessageDialog(messageModel, async () => {
          this.goBindPhoneUrl();
        });
      } else if (
        this.certificationStatus.applyIdentity === IdentityType.General
      ) {
        try {
          await api.postAgentIdentityApply();
          this.certificationStatus.applyIdentity = IdentityType.Agent;
          this.$store.commit(
            MutationType.SetCertificationStatus,
            this.certificationStatus
          );
          this.navigateToApply("觅经纪申请");
        } catch (e) {
          toast(e);
        }
      } else if (
        this.certificationStatus.applyIdentity === IdentityType.Agent &&
        this.certificationStatus.applyStatus ===
          IdentityApplyStatusType.Applying
      ) {
        toast("已有身份申请，请耐心等候审核");
      } else {
        toast("您已有身份，无法再次申请");
      }
    },
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(
          PostType.Square,
          AdvertisingContentType.SeekAgent
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
  },
});
</script>
