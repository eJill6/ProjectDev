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
        <!-- 圖片放大 -->
        <ImageZoom
          :image="imageZoomItem"
          :isEdit="true"
          v-if="imageZoomSwitch"
          @deleteImage="deleteImageItem"
        ></ImageZoom>
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
            <div class="sheet_main" @click="showDatePicker">
              <div class="title title_center">时间</div>
              <div class="content content_right">
                <div class="flex_wrap">
                  <div class="text default_text cursor">
                    {{ showDate }}
                  </div>
                </div>
              </div>
            </div>
            <div class="sheet_main" @click="showCityPicker">
              <div class="title title_center">所在地区</div>
              <div class="content content_right">
                <div class="flex_wrap">
                  <div class="text default_text cursor">
                    {{ showCityName }}
                  </div>
                </div>
              </div>
            </div>

            <MediaSelection
              :title="selectTitle"
              :source="source"
              :media="media"
              :max="maxCount"
              @show="showImageZoom"
            ></MediaSelection>

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
import { NavigateRule, DialogControl, MediaCenter, PlayGame } from "@/mixins";
import {
  MediaModel,
  CommentData,
  PopupModel,
  OptionItemModel,
  ChinaCityInfo,
  ProductDetailModel,
} from "@/models";
import { MutationType } from "@/store";
import toast from "@/toast";
import { defineComponent } from "vue";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  mixins: [NavigateRule, DialogControl, MediaCenter, PlayGame],
  components: { MediaSelection, ImageZoom, CdnImage },
  data() {
    return {
      isSubmit: false,
      commentInfo: {} as CommentData,
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
    showDatePicker() {
      const dateItem: OptionItemModel = {
        key: 0,
        value: this.commentInfo.spentTime,
      };
      const content = this.commentInfo.spentTime ? [dateItem] : [];
      const infoModel: PopupModel = {
        title: "请选择时间",
        content: content,
        isMultiple: false,
      };
      this.showDatePickerDialog(infoModel, (selectedModel) => {
        const list = selectedModel as OptionItemModel[];
        this.commentInfo.spentTime = list[0].value;
      });
    },
    showAlert() {
      setTimeout(() => {
        this.errorMessage = "";
      }, 1000);
    },
    showImageZoom(imageModel: MediaModel) {
      this.imageZoomSwitch = true;
      this.imageZoomItem = imageModel;
    },
    deleteImageItem() {
      this.deleteImage(this.imageZoomItem);
      this.imageZoomSwitch = false;
    },
    backButtonEvent() {
      if (this.imageZoomSwitch) {
        this.imageZoomSwitch = false;
      } else {
        this.navigateToPrevious();
      }
    },
    showCityPicker() {
      const infoModel: PopupModel = {
        title: "请选择地区",
        content: [],
        isMultiple: false,
      };
      this.showCityPickerDialog(infoModel, (selectedModel) => {
        const list = selectedModel as OptionItemModel[];
        this.commentInfo.areaCode = list[0].value;
      });
    },
    async confirmEvent() {
      if (!this.commentInfo.spentTime) {
        this.errorMessage = "请选择您体验的时间";
        return;
      }
      if (!this.commentInfo.areaCode) {
        this.errorMessage = "请选择所在地区";
        return;
      }
      if (!this.commentInfo.comment) {
        this.errorMessage = "请填写您的服务详情";
        return;
      }
      if (this.imageSelect.length > 0) {
        this.$store.commit(MutationType.SetIsLoading, true);
        const images = await this.uploadImages();
        this.$store.commit(MutationType.SetIsLoading, false);
        const imageKeys = images.map((image) => image.id);
        this.commentInfo.photoIds = imageKeys;
      }

      try {
        if (!this.commentInfo.postId) {
          this.commentInfo.postId = this.postId as string;
        }
        if (!this.commentInfo.photoIds) {
          this.commentInfo.photoIds = [];
        }

        this.$store.commit(MutationType.SetIsLoading, true);
        if (this.commentId) {
          await api.editComment(this.commentId as string, this.commentInfo);
        } else {
          await api.createComment(this.commentInfo);
        }
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
        this.commentInfo = await api.getCommentEditData(this.commentId);
        await this.setImageList(
          this.commentInfo.postId,
          this.commentInfo.photoSource,
          SourceType.Comment
        );
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
  },
  async created() {
    await this.getEditData();
  },
  beforeUnmount() {
    this.$store.commit(MutationType.SetProductDetail, {} as ProductDetailModel);
  },
  computed: {
    showCityName() {
      const provinceInfo = provinceJson.find(
        (item) => item.code === this.commentInfo.areaCode
      );
      const cityInfo = cityJson.find(
        (item) => item.code === this.commentInfo.areaCode
      );
      const item = cityInfo || provinceInfo;
      return item?.name || "请输入您体验所在的位置";
    },
    showDate() {
      return this.commentInfo.spentTime
        ? this.commentInfo.spentTime
        : "请选择您体验的时间";
    },
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
    postId() {
      return this.$store.state.productDetail.postId;
    },
    postType() {
      return this.$store.state.productDetail.postType;
    },
  },
});
</script>
