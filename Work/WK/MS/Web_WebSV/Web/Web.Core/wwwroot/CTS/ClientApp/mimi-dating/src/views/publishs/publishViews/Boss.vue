<template>
  <div class="flex_height">
    <ImageZoom :image="imageZoomItem" :isEdit="true" v-if="imageZoomSwitch" @deleteImage="deleteImageItem"></ImageZoom>
    <div class="overflow no_scrollbar post bg_post_b">
      <div class="head_prompt_notice alert" v-if="errorMessage">
        {{ errorMessage }}
      </div>
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_h_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_text mt_16">
          <div class="padding_basic_2 pt_0 pb_0 px_0">
            <div class="sheet_main_title">填写资料</div>
            <div class="sheet_main grayline">
              <div class="title title_center">店铺名称</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入最多7个字符" maxlength="7" v-model="appointmentInfo.shopName" />
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">店铺介绍</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="一句话概述你的店铺，最多17个字" maxlength="17"
                    v-model="appointmentInfo.shopIntroduce">
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">妹子数量</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入数字1-99999" maxlength="5" v-model="appointmentInfo.girls" />
                </form>
              </div>
            </div>
            <div class="sheet_main alert grayline">
              <div class="title title_center">联系软件</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="例如微信、QQ、电话、potato" maxlength="20" v-model="appointmentInfo.contactApp">
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">QQ号码</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请填写正确的QQ号码" maxlength="20" v-model="appointmentInfo.contact">
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">成交订单</div>
              <div class="content content_right">
                <form class="form_full">
                  <!-- <input class="input_style" placeholder="请输入数字1—99999" maxlength="5" v-model="appointmentInfo.dealOrder"> -->
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">自评人气</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入数字1—99999" maxlength="5" v-model="appointmentInfo.rating">
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">店龄</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入数字1—99"  maxlength="2" v-model="appointmentInfo.shopAge">
                </form>
              </div>
            </div>

            <MediaSelection :title="selectTitle" :source="source" :media="media" :max="maxCount"
              :plusBoxClass="appendBoxClass" :annotation="annotation" @show="showImageZoom"></MediaSelection>
          </div>
        </div>
        <div class="introduce_title">
          <CdnImage src="@/assets/images/post/pic_title_h_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>
        <div class="introduce_btn" @click="verifyIdentity">
          <div class="btn_default">申请成为觅老板</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame, MediaCenter } from "@/mixins";
import api from "@/api";
import {
  AdvertisingContentType,
  PostType,
  SourceType,
  MediaType,
  IdentityType,
  IdentityApplyStatusType,
} from "@/enums";
import {
  WhatIsDataModel,
  BossIdentityApplyModel,
  MessageDialogModel,
  MediaModel,
} from "@/models";
import toast from "@/toast";
import { MediaSelection, ImageZoom, CdnImage } from "@/components";
import { MutationType } from "@/store";

export default defineComponent({
  components: { MediaSelection, ImageZoom, CdnImage },
  mixins: [NavigateRule, DialogControl, PlayGame, MediaCenter],
  data() {
    return {
      messages: {} as WhatIsDataModel,
      selectTitle: "照片（将作为店铺头像展示）",
      maxCount: 1,
      source: SourceType.BossApply,
      media: MediaType.Image,
      appendBoxClass: "spacing gray",
      appointmentInfo: {} as BossIdentityApplyModel,
      errorMessage: "",
      annotation: ""
    };
  },
  methods: {
    showImageZoom(imageModel: MediaModel) {
      this.$store.commit(MutationType.SetIntroductionImageMode, true);
      this.imageZoomItem = imageModel;
    },
    deleteImageItem() {
      this.deleteImage(this.imageZoomItem);
      this.$store.commit(MutationType.SetIntroductionImageMode, false);
    },
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
      } else if (this.userInfo.identity !== IdentityType.General) {
        toast("您已有身份，无法再次申请");
      } else if (!this.appointmentInfo.shopName) {
        this.errorMessage = "请输入店铺名称";
      } else if (!this.appointmentInfo.girls) {
        this.errorMessage = "请填写妹子数量";
      } else if (!this.appointmentInfo.contact) {
        this.errorMessage = "请填写联络方式";
      } else if (!this.imageSelect.length) {
        this.errorMessage = "请上传照片";
      } else {
        // 如果是後台改的話，可能會有異常變動，所以讓他再執行一次抓取申請狀態的api比較保險
        let identityApplyStatus = await api.getCertificationInfo();

        // 沒申請的時候
        if (
          this.userInfo.identity === IdentityType.General &&
          identityApplyStatus.applyStatus === IdentityApplyStatusType.NotYet
        ) {
          try {
            this.$store.commit(MutationType.SetIsLoading, true);

            const images = await this.uploadImages();
            if (!images.length) {
              this.$store.commit(MutationType.SetIsLoading, false);
              this.errorMessage = "照片上传失败";
              return;
            }

            const imageKeys = images.map((image) => image.id);
            this.appointmentInfo.photoIds = imageKeys;

            await api.postBossIdentityApply(this.appointmentInfo);
            //更新申請狀態
            this.certificationStatus.applyIdentity =
              identityApplyStatus.applyIdentity;
            this.certificationStatus.applyStatus =
              identityApplyStatus.applyStatus;
            this.$store.commit(
              MutationType.SetCertificationStatus,
              this.certificationStatus
            );
            this.navigateToApply("觅老板申请");
          } catch (e) {
            toast(e);
          } finally {
            this.$store.commit(MutationType.SetIsLoading, false);
          }
        } else if (
          identityApplyStatus.applyStatus === IdentityApplyStatusType.Applying
        ) {
          toast("已有身份申请，请耐心等候审核");
        }
      }
    },
    showAlert() {
      setTimeout(() => {
        this.errorMessage = "";
      }, 1000);
    },
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(
          PostType.Square,
          AdvertisingContentType.SeekBoss
        );
      } catch (error) {
        toast(error);
      }
    },
  },
  watch: {
    errorMessage(content: String) {
      if (content) {
        this.showAlert();
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
    imageZoomSwitch() {
      return this.$store.state.isImageZoomMode || false;
    },
  },
});
</script>
