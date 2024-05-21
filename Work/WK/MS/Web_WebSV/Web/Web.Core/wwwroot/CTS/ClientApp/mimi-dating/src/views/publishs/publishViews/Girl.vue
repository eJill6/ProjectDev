<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post bg_post_c">
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_f_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_f_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn" @click="verifyIdentity">
          <div class="btn_default">申请成为觅女郎</div>
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
import { AdvertisingContentType, IdentityApplyStatusType, IdentityType, PostType } from "@/enums";
import { MessageDialogModel, WhatIsDataModel } from "@/models";
import toast from "@/toast";
import { MutationType } from "@/store";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, DialogControl, PlayGame],
  data() {
    return {
      messages: {} as WhatIsDataModel,
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
      } else if (this.userInfo.identity !== IdentityType.General){
        toast("您已有身份，无法再次申请");      
      } else {
        // 如果是後台改的話，可能會有異常變動，所以讓他再執行一次抓取申請狀態的api比較保險
        let identityApplyStatus = await api.getCertificationInfo();

        // 沒申請的時候
        if (this.userInfo.identity === IdentityType.General && 
          identityApplyStatus.applyStatus === IdentityApplyStatusType.NotYet) 
        {
          try {
            await api.postGirlIdentityApply();
            //更新申請狀態
            this.certificationStatus.applyIdentity = identityApplyStatus.applyIdentity;
            this.certificationStatus.applyStatus = identityApplyStatus.applyStatus;
            this.$store.commit(
              MutationType.SetCertificationStatus,
              this.certificationStatus
            );
            this.navigateToApply("觅女郎申请");
          } catch (e) {
            toast(e);
          }
        } else if (identityApplyStatus.applyStatus === IdentityApplyStatusType.Applying){
          toast("已有身份申请，请耐心等候审核");
        }
      }
    },  
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(
          PostType.Square,
          AdvertisingContentType.SeekGirl
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
