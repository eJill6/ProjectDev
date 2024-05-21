<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_a_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_text mt_16">
          <div class="padding_basic_2 pt_0 pb_0 px_0">
            <div class="sheet_main_title">填写资料</div>
            <!-- <div class="sheet_main grayline" :class="{ alert: checkFailure(contactInfo.contactType) }">
              <div class="title title_center">联系软件</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="例如微信、QQ、电话、potato" v-model="contactInfo.contactType">
                </form>
              </div>
            </div> -->
            <div class="sheet_main grayline" :class="{ alert: checkFailure(contactInfo.contact) }">
              <div class="title title_center">QQ号码</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请填写正确的QQ号码" v-model="contactInfo.contact">
                </form>
              </div>
            </div>
          </div>
        </div>
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_a_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn" @click="verifyIdentity">
          <div class="btn_default no_shadow">申请成为觅经纪</div>
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
import {
  AdvertisingContentType,
  IdentityApplyStatusType,
  IdentityType,
  PostType
} from "@/enums";
import { MessageDialogModel, WhatIsDataModel, AgentContactInfoModel } from "@/models";
import toast from "@/toast";
import { MutationType } from "@/store";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, DialogControl, PlayGame],
  data() {
    return {
      messages: {} as WhatIsDataModel,
      isApplying: false,
      contactInfo: {
        contactType: "QQ"
      } as AgentContactInfoModel,
      isCheckFailure: false,
    };
  },
  methods: {
    async verifyIdentity() {
      if (!this.userInfo.hasPhone) {
        const messageModel: MessageDialogModel = {
          message: "需要绑定手机号码才能申请觅经纪/觅老板",
          cancelTitle: "前往绑定",
          buttonTitle: "暂不绑定",
          cancelButtonEnable: true,
        };
        this.showMessageDialog(messageModel, async () => {
          this.goBindPhoneUrl();
        });
        return;
      }

      const tagString = this.checkProperties(this.contactInfo);
      this.isCheckFailure = !!tagString;
      if (this.isCheckFailure) {
        return;
      }
      else if (this.userInfo.identity !== IdentityType.General) {
        toast("您已有身份，无法再次申请");
      } else {
        // 如果是後台改的話，可能會有異常變動，所以讓他再執行一次抓取申請狀態的api比較保險
        let identityApplyStatus = await api.getCertificationInfo();

        // 沒申請的時候
        if (this.userInfo.identity === IdentityType.General &&
          identityApplyStatus.applyStatus === IdentityApplyStatusType.NotYet) {
          try {
            await api.postAgentIdentityApply(this.contactInfo);
            //更新申請狀態
            this.certificationStatus.applyIdentity = identityApplyStatus.applyIdentity;
            this.certificationStatus.applyStatus = identityApplyStatus.applyStatus;
            this.$store.commit(
              MutationType.SetCertificationStatus,
              this.certificationStatus
            );
            this.navigateToApply("觅经纪申请");
          } catch (e) {
            toast(e);
          }
        } else if (identityApplyStatus.applyStatus === IdentityApplyStatusType.Applying) {
          toast("已有身份申请，请耐心等候审核");
        }
      }
    },
    checkFailure(item: number | string | any[]) {
      return (
        ((Array.isArray(item) && !item.length) || !item) && this.isCheckFailure
      );
    },
    checkProperties(obj: AgentContactInfoModel): string {
      if (!obj.contact) {
        return `contact`;
      }
      if (!obj.contactType) {
        return `contactType`;
      }
      return "";
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
