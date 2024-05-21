<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="backButtonEvent">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">投诉</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <!-- 圖片放大 -->
        <ImageZoom
          :image="imageZoomItem"
          :edit="true"
          v-if="imageZoomSwitch"
          @deleteImage="deleteImageItem"
        ></ImageZoom>
        <div class="overflow no_scrollbar post" v-if="!isSubmit">
          <div class="head_prompt_notice alert" v-if="errorMessage">
            {{errorMessage}}
          </div>
          <!-- 此錯誤提示非常駐，出現幾秒後就會消失 -->
          <!-- 滑動區域 -->
          <div class="padding_basic_2 pt_0 pb_0">
            <div class="sheet_main full_width">
              <div class="title title_full_width highlight">举报原因</div>
              <div class="content_full">
                <div class="radiobutton complaint">
                  <ul>
                    <li v-for="(n, index) in complaintReason">
                      <label>
                        <div class="text">{{ n }}</div>
                        <input
                          type="radio"
                          name="radio"
                          :checked="isSelected(index)"
                          @click="selectReason(index)"
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
            <ImageSelection
              :title="selectTitle"
              :source="source"
              :media="media"
              :max="maxCount"
              @show="showImageZoom"
            ></ImageSelection>
            <div class="sheet_main full_width">
              <div class="title title_full_width highlight">详情描述</div>
              <div class="content_full">
                <form class="textarea_full_width_style">
                  <textarea
                    v-model="reportInfo.describe"
                    class="no_scrollbar"
                    name=""
                    placeholder="请详细描述您所遇到的问题"
                    maxlength="150"
                  ></textarea>
                  <div class="notice_text">{{textLength}}/150</div>
                </form>
              </div>
            </div>
            <div class="sheet_main">
              <div class="notice">
                *恶意投诉者，平台核实后将对其永久封号处理！加不上联系方式的话，可能对方已休息，如24小时后仍未加上再投诉，谢谢！
              </div>
            </div>
          </div>
          <div class="sheet_btn" @click="confirmEvent()">
            <div class="btn_default">提交审核</div>
          </div>
        </div>
        <div class="overflow no_scrollbar" v-else>
          <div class="padding_basic_2 full_height finish_page">
            <div class="content">
              <div class="icon">
                <img src="@/assets/images/post/ic_finish_check.svg" alt="" />
              </div>
              <p>已提交投诉</p>
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
import { defineComponent } from "vue";
import { MediaCenter, NavigateRule } from "@/mixins";
import { ImageSelection, ImageZoom } from "@/components";
import { MediaType, SourceType, ReportType } from "@/enums";
import { MediaModel, ReportDataModel } from "@/models";
import api from "@/api";
import { MutationType } from "@/store/mutations";
import toast from "@/toast";

export default defineComponent({
  mixins: [NavigateRule, MediaCenter],
  components: { ImageSelection, ImageZoom },
  data() {
    return {
      isSubmit: false,
      errorMessage: "",
      selectTitle: `截图证据`,
      maxCount: 3,
      source: SourceType.Report,
      media: MediaType.Image,
      imageZoomSwitch: false,
      reportInfo:{} as ReportDataModel,      
    };
  },
  watch:{
    errorMessage(content:String){
      if(content) {
        this.showAlert();
      }
    }
  },
  methods: {
    showAlert() {      
      setTimeout(() => {
        this.errorMessage = "";
      }, 1000);
    },
    async confirmEvent() {
      this.$store.commit(MutationType.SetIsLoading, true);
      const isSuccess = await this.checkData();
      if(!isSuccess){
        this.$store.commit(MutationType.SetIsLoading, false);
        return;
      }
      this.reportInfo.postId = this.postId ;
      try{
        await api.createReport(this.reportInfo);
        this.isSubmit = true;
      } catch(error) {
        const message =
          error instanceof Error ? (error as Error).message : (error as string);
        const errorMessage = `上传失败(${message})`;
        toast(errorMessage);        
      }
      finally{
        this.$store.commit(MutationType.SetIsLoading, false);
      }      
    },
    async checkData() {
      if(!this.reportInfo.describe) {
        this.errorMessage = "请填写详情描述";
        return false;
      }
      if (!this.imageSelect.length) {
        this.errorMessage = "请添加截图证据"; 
        return false;
      }
      const images = await this.uploadImages();

      if (!images.length) {
        this.errorMessage = "截图证据上传失败"; 
        return false;
      }
      const imageKeys = images.map((image) => image.id);
      this.reportInfo.photoIds = imageKeys;
      return true;
    },
    isSelected(index: number){
      return index === this.reportInfo.reportType
    },
    selectReason(index: number) {
      this.reportInfo.reportType = index;
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
  },
  created() {
    this.reportInfo.reportType = ReportType.Fraud
  },
  computed: {
    complaintReason() {
      return ["骗子", "广告骚扰", "货不对版", "无效联系方式"];
    },
    textLength(){
      if(Object.keys(this.reportInfo).length === 0) {
        return 0;
      } else {
        return this.reportInfo.describe ? this.reportInfo.describe.length : 0;
      }
    },
    postId() {
      return this.$route.params.postId as string;
    },
  },
});
</script>
