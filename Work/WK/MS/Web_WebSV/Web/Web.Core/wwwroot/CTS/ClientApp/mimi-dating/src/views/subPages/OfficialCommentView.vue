<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="backButtonEvent">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">评价</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar post" v-if="!isSubmit">
          <div class="head_prompt_notice alert" v-if="errorMessage">
            {{ errorMessage }}
          </div>
          <!-- 此錯誤提示非常駐，出現幾秒後就會消失 -->
          <!-- 滑動區域 -->
          <div class="text_notice normal">
            与觅友评论分享你的真实体验吧！每次都可参与空降抽奖
          </div>
          <div class="padding_basic_2 pt_0 pb_0">
            <div class="sheet_main">
              <div class="title title_full_width display_flex">
                妹子颜值：
                <div class="evaluate_item">
                  <ul role="img">
                    <li
                      class="evaluate_star_icon"
                      :class="{ active: n <= commentInfo.facialScore }"
                      v-for="n in startMaxCount"
                      @click="commentInfo.facialScore = n"
                    ></li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="sheet_main">
              <div class="title title_full_width display_flex">
                服务质量：
                <div class="evaluate_item">
                  <ul role="img">
                    <li
                      class="evaluate_star_icon"
                      :class="{ active: n <= commentInfo.serviceQuality }"
                      v-for="n in startMaxCount"
                      @click="commentInfo.serviceQuality = n"
                    ></li>
                  </ul>
                </div>
              </div>
            </div>

            <div class="sheet_main full_width">
              <div class="title title_full_width">服务详情（必填）</div>
              <div class="content_full">
                <form class="textarea_full_width_style">
                  <textarea
                    v-model="commentInfo.comment"
                    class="no_scrollbar"
                    name=""
                    placeholder="具体写整个服务过程，感受（有何特点）"
                    maxlength="100"
                  ></textarea>
                  <div class="notice_text">{{ textLength }}/100</div>
                </form>
              </div>
            </div>
          </div>
          <div class="sheet_btn" @click="confirmEvent">
            <div class="btn_default">确认提交</div>
          </div>
        </div>
        <div class="overflow no_scrollbar" v-else>
          <div class="padding_basic_2 full_height finish_page">
            <div class="content">
              <div class="icon">
                <CdnImage
                  src="@/assets/images/post/ic_finish_check.svg"
                  alt=""
                />
              </div>
              <p>已提交评价</p>
            </div>
            <div class="bottom_btn" @click="navigateToPrevious">
              <div class="btn_default">我知道了</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import api from "@/api";
import { MediaSelection, ImageZoom, CdnImage } from "@/components";
import { MediaType, PostType, SourceType } from "@/enums";
import { NavigateRule, DialogControl, MediaCenter, PlayGame, ScrollManager } from "@/mixins";
import { ProductDetailModel, OfficialCommentData } from "@/models";
import { MutationType } from "@/store";
import toast from "@/toast";
import { defineComponent } from "vue";

export default defineComponent({
  mixins: [NavigateRule, DialogControl, MediaCenter, PlayGame, ScrollManager],
  components: { MediaSelection, ImageZoom, CdnImage },
  data() {
    return {
      isSubmit: false,
      commentInfo: {} as OfficialCommentData,
      errorMessage: "",
      selectTitle: "上传照片",
      maxCount: 6,
      source: SourceType.Comment,
      media: MediaType.Image,
      imageZoomSwitch: false,
      PostType,
      startMaxCount: 5,
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
    backButtonEvent() {
      if (this.imageZoomSwitch) {
        this.imageZoomSwitch = false;
      } else {
        this.navigateToPrevious();
      }
    },
    async confirmEvent() {
      if (!this.commentInfo.comment) {
        this.errorMessage = "请填写您的服务详情";
        return;
      }

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        if (this.commentId) {
          await api.editOfficialComment(
            this.commentId as string,
            this.commentInfo
          );
        } else {
          this.commentInfo.bookingId = this.bookingId;
          await api.createOfficialComment(this.commentInfo);
        }
        this.resetPageInfo();
        this.resetScroll();
        this.isSubmit = true;
      } catch (error) {
        const message =
          error instanceof Error ? (error as Error).message : (error as string);
        const errorMessage = `上传失败(${message})`;
        toast(errorMessage);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    async getEditData() {
      if (!this.commentId) return;
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        const result = await api.getOfficialCommentEditData(this.commentId);
        this.commentInfo.comment = result.comment;
        this.commentInfo.facialScore = result.facialScore;
        this.commentInfo.serviceQuality = result.serviceQuality;
        this.commentInfo.bookingId = this.bookingId;
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
  },
  async created() {
    this.commentInfo.facialScore = 3;
    this.commentInfo.serviceQuality = 3;
    await this.getEditData();
  },
  beforeUnmount() {
    this.$store.commit(MutationType.SetProductDetail, {} as ProductDetailModel);
  },
  computed: {
    textLength() {
      if (Object.keys(this.commentInfo).length === 0) {
        return 0;
      } else {
        return this.commentInfo.comment ? this.commentInfo.comment.length : 0;
      }
    },
    commentId() {
      const id = this.$route.query.commentId as string;
      return id.replace(/\s/g, "");
    },
    bookingId() {
      const id = this.$route.query.bookingId as string;
      return id.replace(/\s/g, "");
    },
    postId() {
      return this.$store.state.officialDetail.postId;
    },
  },
});
</script>
