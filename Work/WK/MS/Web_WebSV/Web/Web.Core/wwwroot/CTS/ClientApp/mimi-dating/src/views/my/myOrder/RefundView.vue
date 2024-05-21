<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color solid_line">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">申请退款</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height bg_personal">
        <div class="overflow no_scrollbar post">
          <div class="head_prompt_notice alert" v-if="errorMessage">
            {{ errorMessage }}
          </div>
          <!-- 此錯誤提示非常駐，出現幾秒後就會消失 -->
          <!-- 滑動區域 -->
          <div class="padding_basic_2 pt_0 pb_0">
            <div class="sheet_main full_width2 main_none">
              <div class="title title_full_width highlight">请选择退款理由 <span class="title_red">*</span></div>
              <div class="content_full">
                <div class="radiobutton complaint">
                  <ul>
                    <li class="border_line" @click="selected(RefundReasonType.Fraud)">
                      <label>
                        <div class="text">存在欺骗</div>
                        <input
                          type="radio"
                          name="radio"
                          :checked="isSelected(RefundReasonType.Fraud)"
                        />
                        <span class="icon"
                          ><i><div class="dot"></div></i
                        ></span>
                      </label>
                    </li>
                    <li class="border_line" @click="selected(RefundReasonType.Fake)">
                      <label>
                        <div class="text">货不对版</div>
                        <input
                          type="radio"
                          name="radio"
                          :checked="isSelected(RefundReasonType.Fake)"
                        />
                        <span class="icon"
                          ><i><div class="dot"></div></i
                        ></span>
                      </label>
                    </li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="sheet_main full_width2 main_none">
              <div class="title title_full_width highlight">详情描述<span class="title_red">*</span></div>
              <div class="content_full">
                <form class="textarea_full_width_style bg_raisinblack">
                  <textarea
                    class="no_scrollbar"
                    name=""
                    placeholder="请详细描述您所遇到的问题"
                    v-model="info.describe"
                    maxlength="150"
                  ></textarea>
                  <div class="notice_text">{{ textLength }}/150</div>
                </form>
              </div>
            </div>

            <MediaSelection
              :title="selectTitle"
              :source="source"
              :media="media"
              :max="maxCount"
              :annotation="annotation"
              @show="showImageZoom"
            ></MediaSelection>
          </div>
          <div class="sheet_btn" @click="publish">
            <div class="btn_default">提交</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MediaSelection, CdnImage } from "@/components";
import { SourceType, MediaType, RefundReasonType } from "@/enums";
import { MediaModel, ApplyRefundModel } from "@/models";
import { MediaCenter, NavigateRule, ScrollManager } from "@/mixins";
import api from "@/api";
import { MutationType } from "@/store";
import toast from "@/toast";

export default defineComponent({
  components: { MediaSelection, CdnImage },
  mixins: [MediaCenter, NavigateRule, ScrollManager],
  data() {
    return {
      selectTitle: "截图证据",
      maxCount: 9,
      source: SourceType.Refund,
      media: MediaType.Image,
      imageZoomSwitch: false,
      info: {} as ApplyRefundModel,
      errorMessage: "",
      RefundReasonType,
      annotation: "",
    };
  },
  watch: {
    errorMessage(content: String) {
      if (content) {
        this.showAlert();
      }
    },
  },
  methods: {
    showAlert() {
      setTimeout(() => {
        this.errorMessage = "";
      }, 1000);
    },
    showImageZoom(imageModel: MediaModel) {
      this.imageZoomSwitch = true;
      this.imageZoomItem = imageModel;
    },
    selected(type: RefundReasonType) {
      this.info.reasonType = type;
    },
    isSelected(type: RefundReasonType) {
      return this.info.reasonType === type;
    },
    async publish() {
      this.$store.commit(MutationType.SetIsLoading, true);

      if (!this.info.describe) {
        this.$store.commit(MutationType.SetIsLoading, false);
        this.errorMessage = "请填写详情描述";
        return;
      }

      if (!this.imageSelect.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        this.errorMessage = "请添加照片";
        return;
      }

      const images = await this.uploadImages();
      if (!images.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        this.errorMessage = "照片上传失败";
        return;
      }

      const imageKeys = images.map((image) => image.id);
      this.info.photoIds = imageKeys;

      if (!this.info.photoIds.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        return;
      }
      try {
        this.info.bookingId = this.bookingId;
        const result = await api.postApplyRefund(this.info);
        toast(`提交成功`);
        this.resetPageInfo();
        this.resetScroll();
        this.navigateToPrevious();
      } catch (error) {
        const message =
          error instanceof Error ? (error as Error).message : (error as string);
        const errorMessage = `上传失败(${message})` as string;
        toast(errorMessage);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
  },
  created() {
    this.info.reasonType = RefundReasonType.Fraud;
  },
  computed: {
    textLength() {
      if (Object.keys(this.info).length === 0) {
        return 0;
      } else {
        return this.info.describe ? this.info.describe.length : 0;
      }
    },
    bookingId() {
      return (this.$route.query.bookingId as unknown as string) || "";
    },
  },
});
</script>
