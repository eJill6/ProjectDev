<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color solid_line">
        <div class="header_back" @click="backButtonEvent">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">发布</div>
        <!-- <div class="header_btn align_right">
                        <div>
                            <div class="style_a">发帖规则</div>
                        </div>
                    </div> -->
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height bg_personal" ref="officialFrom">
        <!-- 圖片放大 -->
        <ImageZoom
          :image="imageZoomItem"
          :isEdit="true"
          v-if="imageZoomSwitch"
          @deleteImage="deleteImageItem"
        ></ImageZoom>
        <div class="overflow no_scrollbar post">
          <!-- 此錯誤提示非常駐，出現幾秒後就會消失 -->
          <!-- <div class="head_prompt_notice alert">请选择举报原因</div> -->
          <!-- 滑動區域 -->
          <div class="padding_basic_2 pt_0 pb_0">
            <div
              id="title"
              class="sheet_main full_width"
              :class="{ alert: checkFailure(productInfo.title) }"
            >
              <div class="title_default title_center">帖子标题</div>
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input
                    class="input_style"
                    placeholder="请输入帖子标题"
                    v-model="productInfo.title"
                    maxlength="20"
                  />
                </form>
              </div>
            </div>
            <div
              id="areaCode"
              class="sheet_main"
              @click="showCityPicker"
              :class="{ alert: checkFailure(productInfo.areaCode) }"
            >
              <div class="title_default title_center">所在区域</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showCityName }}
                  </div>
                  <div class="arrow">
                    <CdnImage
                      src="@/assets/images/element/ic_sheet_choose.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <!-- <div
              id="address"
              class="sheet_main"
              :class="{ alert: checkFailure(productInfo.address) }"
            >
              <div class="title title_center">详细地址</div>
              <div class="content content_right">
                <form class="form_full">
                  <input
                    class="input_style"
                    placeholder="请输入详细地址"
                    v-model="productInfo.address"
                    maxlength="12"
                  />
                </form>
              </div>
            </div> -->
            <div
              id="age"
              class="sheet_main"
              @click="agePicker"
              :class="{ alert: checkFailure(productInfo.age) }"
            >
              <div class="title title_center">年龄（岁）</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showAge }}
                  </div>
                  <div class="arrow">
                    <CdnImage
                      src="@/assets/images/element/ic_sheet_choose.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <div
              id="postHeight"
              class="sheet_main"
              @click="bodyHeightPicker"
              :class="{ alert: checkFailure(productInfo.height) }"
            >
              <div class="title title_center">身高（cm）</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showBodyHeight }}
                  </div>
                  <div class="arrow">
                    <CdnImage
                      src="@/assets/images/element/ic_sheet_choose.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <div
              id="cup"
              class="sheet_main"
              @click="cupPicker"
              :class="{ alert: checkFailure(productInfo.cup) }"
            >
              <div class="title title_center">罩杯</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showCup }}
                  </div>
                  <div class="arrow">
                    <CdnImage
                      src="@/assets/images/element/ic_sheet_choose.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <div
              id="businessHours"
              class="sheet_main"
              :class="{ alert: checkFailure(productInfo.businessHours) }"
            >
              <div class="title title_center">营业时间</div>
              <div class="content content_right">
                <form class="form_full">
                  <input
                    class="input_style"
                    placeholder="例如：下午1点到晚上11点"
                    v-model="productInfo.businessHours"
                    maxlength="15"
                  />
                </form>
              </div>
            </div>
            <div
              id="serviceIds"
              class="sheet_main"
              @click="servicePicker"
              :class="{ alert: checkFailure(productInfo.serviceIds) }"
            >
              <div class="title title_center">服务项目</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showService }}
                  </div>
                  <div class="arrow">
                    <CdnImage
                      src="@/assets/images/element/ic_sheet_choose.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <div
              id="combo"
              class="sheet_main full_width"
              :class="{ alert: checkComboFailure() }"
            >
              <div class="title title_full_width">服务套餐（至少填写一个套餐）</div>
              <div class="content_full content_grid">
                <div
                  class="grid_layout"
                  v-for="(item, index) in productInfo.combo"
                >
                  <form
                    :id="`combo_field1${index}`"
                    class="form_outline field1"
                    :class="{
                      field_error: checkComboNameFeildFailure(item, index),
                    }"
                  >
                    <input
                      class="input_style"
                      :placeholder="`套餐${getChineseChar(index)}名称`"
                      maxlength="8"
                      v-model="item.comboName"
                    />
                  </form>
                  <form
                    :id="`combo_field2${index}`"
                    class="form_outline field2"
                    :class="{
                      field_error: checkComboPriceFeildFailure(item, index),
                    }"
                  >
                    <input
                      class="input_style"
                      :placeholder="`套餐${getChineseChar(index)}价格`"
                      v-model="item.comboPrice"
                      type="tel"
                      onkeydown="javascript: return  (event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode<= 105) || event.keyCode !== 8 || event.keyCode !== 46 ? true: false"
                      @input="checkInputIsNumber($event, index)"
                    />
                  </form>
                  <form
                    :id="`combo_field3${index}`"
                    class="form_outline field3"
                    :class="{
                      field_error: checkComboServiceFeildFailure(item, index),
                    }"
                  >
                    <input
                      class="input_style"
                      placeholder="服务时间、次数或包含的项目"
                      maxlength="8"
                      v-model="item.service"
                    />
                  </form>
                </div>
              </div>
            </div>
            <div
              id="serviceDescribe"
              class="sheet_main full_width"
              :class="{ alert: checkFailure(productInfo.serviceDescribe) }"
            >
              <div class="title title_full_width">服务描述</div>
              <div class="content_full">
                <form class="textarea_full_width_style">
                  <textarea
                    v-model="productInfo.serviceDescribe"
                    class="no_scrollbar"
                    name=""
                    placeholder="可以详细描述妹子的优势特征，服务环境和服务技术等..."
                    maxlength="100"
                  ></textarea>
                  <div class="notice_text">{{ textLength }}/100</div>
                </form>
              </div>
            </div>
            <MediaSelection
              :title="selectTitle"
              :source="source"
              :media="MediaType.Image"
              :max="maxCount"
              @show="showImageZoom"
            ></MediaSelection>
            <MediaSelection
              :title="videoSelectTitle"
              :source="source"
              :media="MediaType.Video"
              :max="videoMaxCount"
              @show="showImageZoom"
            ></MediaSelection>
          </div>
          <div class="sheet_btn" @click="publish">
            <div class="btn_default">发布</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, MediaCenter, PlayGame, ScrollManager } from "@/mixins";
import {
  MediaModel,
  PopupModel,
  PostTypeOptionsModel,
  OptionItemModel,
  ChinaCityInfo,
  OfficialProductModel,
  OfficialComboModel,
} from "@/models";
import { MutationType } from "@/store";
import { MediaSelection, ImageZoom, CdnImage } from "@/components";
import { SourceType, MediaType, PostType } from "@/enums";
import api from "@/api";
import toast from "@/toast";
import { officialComboKeyValue } from "@/defaultConfig";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: { MediaSelection, ImageZoom, CdnImage },
  mixins: [NavigateRule, DialogControl, MediaCenter, PlayGame, ScrollManager],
  data() {
    return {
      productInfo: {} as OfficialProductModel,
      cityPickerVisible: false,
      currentProvinceOption: "",
      currentCityOption: "",
      selectTitle: "照片（第一张照片将作为封面图展示",
      maxCount: 5,
      videoSelectTitle: "视频（选填，添加视频会获得更多关注哦）",
      videoMaxCount: 1,
      source: SourceType.Post,
      imageZoomSwitch: false,
      options: {} as PostTypeOptionsModel,
      isCheckFailure: false,
      comboCount: 3,
      basePrice: 100,
      MediaType,
    };
  },
  methods: {
    checkInputIsNumber(event: Event, index: number) {
      const target = <HTMLInputElement>event.target;
      const endChat = target.value.slice(-1);

      if (isNaN(Number(endChat))) {
        const newString = target.value.slice(0, -1);
        const newPrice = isNaN(Number(newString)) ? 0 : Number(newString);
        this.productInfo.combo[index].comboPrice = newPrice;
      }

      if (target.value.length > 5) {
        const value = target.value.slice(0, 5);
        this.productInfo.combo[index].comboPrice = parseInt(value);
      }
    },
    getChineseChar(index: number) {
      const result = officialComboKeyValue.find((item) => item.key === index);
      return result ? result.value : "";
    },
    showCityPicker() {
      const infoModel: PopupModel = {
        title: "请选择地区",
        content: [],
        isMultiple: false,
      };
      this.showCityPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        this.productInfo.areaCode = list[0].value;
      });
    },
    servicePicker() {
      const content = this.options.service;
      const infoModel: PopupModel = {
        title: "请选择服务项目",
        content: content,
        isMultiple: true,
      };
      this.showPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        const data = list.map((l) => l.key);
        this.productInfo.serviceIds = data;
      });
    },
    agePicker() {
      const content = this.options.age;
      const infoModel: PopupModel = {
        title: "年龄",
        content: content,
        isMultiple: false,
      };

      this.showPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        this.productInfo.age = Number(list[0].key);
      });
    },
    bodyHeightPicker() {
      const content = this.options.bodyHeight;
      const infoModel: PopupModel = {
        title: "身高",
        content: content,
        isMultiple: false,
      };

      this.showPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        this.productInfo.height = Number(list[0].key);
      });
    },
    cupPicker() {
      const content = this.options.cup;
      const infoModel: PopupModel = {
        title: "罩杯",
        content: content,
        isMultiple: false,
      };

      this.showPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        this.productInfo.cup = Number(list[0].key);
      });
    },
    async publish() {
      this.$store.commit(MutationType.SetIsLoading, true);
      let tagString = this.checkProperties(this.productInfo);
      this.isCheckFailure = !!tagString;
      if (this.isCheckFailure) {
        const top = document.getElementById(tagString)?.scrollIntoView(); //Getting Y of target element
        window.scrollTo(0, top || 0);

        this.$store.commit(MutationType.SetIsLoading, false);
        return;
      }
      let newParams = JSON.parse(
        JSON.stringify(this.productInfo)
      ) as OfficialProductModel;
      newParams.combo = this.productInfo.combo.filter(
        (item) =>
          Object.keys(item).length === this.comboCount &&
          item.comboName &&
          item.comboPrice >= this.basePrice &&
          item.service
      );

      if (!this.imageSelect.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        toast("请添加照片");
        return;
      }

      const images = await this.uploadImages();
      if (!images.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        toast("照片上传失败");
        return;
      }

      const imageKeys = images.map((image) => image.id);
      newParams.photoIds = imageKeys;


      if (this.videoSelect.length > 0) {
        const videos = await this.uploadVideos();
        if (!videos.length) {
          this.$store.commit(MutationType.SetIsLoading, false);
          toast("影片上传失败");
          return;
        }
        const videoKeys = videos.map((image) => image.id);
        newParams.videoIds = videoKeys;
      } else {
        newParams.videoIds = [];
      }

      if (!newParams.photoIds.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        return;
      }

      try {
        if (this.postId) {
          await api.EditOfficialPost(this.postId as string, newParams);
        } else {
          await api.createOfficialPost(newParams);
          this.resetScroll();
          this.resetPageInfo();
        }
        await this.setUserInfo();
        toast(`发布成功`);
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
    checkProperties(obj: OfficialProductModel): string {
      if (!obj.title) {
        return `title`;
      }
      if (!obj.areaCode) {
        return `areaCode`;
      }
      if (!obj.age) {
        return `age`;
      }
      if (!obj.height) {
        return `postHeight`;
      }
      if (!obj.cup) {
        return `cup`;
      }
      if (!obj.businessHours) {
        return `businessHours`;
      }
      if (!obj.serviceIds || !obj.serviceIds.length) {
        return `serviceIds`;
      }
      if (
        !this.productInfo.combo.some(
          (item) =>
            Object.keys(item).length === this.comboCount &&
            item.comboName &&
            item.comboPrice >= this.basePrice &&
            item.service
        )
      ) {
        return `combo`;
      }
      if (!obj.serviceDescribe) {
        return `serviceDescribe`;
      }

      return "";
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
    async getOptionsByPostType() {
      try {
        this.options = await api.getOptionsByPostType(this.queryPostType);
      } catch (error) {
        toast(error);
      }
    },
    setComboList(count: number) {
      for (let i = count; i < this.comboCount; i++) {
        let item = {} as OfficialComboModel;
        this.productInfo.combo.push(item);
      }
    },
    async getEditData() {
      if (!this.postId) {
        this.productInfo.combo = [];
        this.setComboList(0);
        return;
      }
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        this.productInfo = await api.getOfficialPostEditData(this.postId);
        this.setComboList(this.productInfo.combo.length);
        if (Object.keys(this.productInfo.videoSource).length > 0) {
          await this.setVideoList(
            this.postId,
            this.productInfo.videoSource,
            SourceType.Post
          );
        }
        await this.setImageList(
          this.postId,
          this.productInfo.photoSource,
          SourceType.Post
        );
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    checkComboFailure() {
      const result = this.productInfo.combo || [];
      const incomplete = result.some((item) => Object.keys(item).length > 0);
      return !incomplete && this.isCheckFailure;
    },
    checkComboNameFeildFailure(item: OfficialComboModel, index: number) {
      const combo = this.productInfo.combo || [];
      const isUnfilled = Object.keys(combo[index]).length === 0;
      const incomplete = item.comboName;
      const isClear = this.isClearComboItemAlert(index);
      return isUnfilled || isClear ? false : !incomplete && this.isCheckFailure;
    },
    checkComboPriceFeildFailure(item: OfficialComboModel, index: number) {
      const combo = this.productInfo.combo || [];
      const isUnfilled = Object.keys(combo[index]).length === 0;
      const incomplete = item.comboPrice >= this.basePrice;
      const isClear = this.isClearComboItemAlert(index);
      return isUnfilled || isClear ? false : !incomplete && this.isCheckFailure;
    },
    checkComboServiceFeildFailure(item: OfficialComboModel, index: number) {
      const combo = this.productInfo.combo || [];
      const isUnfilled = Object.keys(combo[index]).length === 0;
      const incomplete = item.service;
      const isClear = this.isClearComboItemAlert(index);
      return isUnfilled || isClear ? false : !incomplete && this.isCheckFailure;
    },
    isClearComboItemAlert(index: number) {
      const comboList = this.productInfo.combo || [];
      if (Object.keys(comboList[index]).length === this.comboCount) {
        const result = comboList[index];
        if (!result.comboName && !result.comboPrice && !result.service) {
          let comboField1 = document.getElementById(
            `combo_field1${index}`
          ) as HTMLFormElement;
          comboField1.classList.remove("field_error");
          let comboField2 = document.getElementById(
            `combo_field2${index}`
          ) as HTMLFormElement;
          comboField2.classList.remove("field_error");
          let comboField3 = document.getElementById(
            `combo_field3${index}`
          ) as HTMLFormElement;
          comboField3.classList.remove("field_error");
          return true;
        }
      }
      return false;
    },
    checkFailure(item: number | string | any[]) {
      return (
        ((Array.isArray(item) && !item.length) || !item) && this.isCheckFailure
      );
    },
  },
  async created() {
    await this.setUserInfo();
    await this.getOptionsByPostType();
    await this.getEditData();
  },
  computed: {
    showCityName() {
      const provinceInfo = provinceJson.find(
        (item) => item.code === this.productInfo.areaCode
      );
      const cityInfo = cityJson.find(
        (item) => item.code === this.productInfo.areaCode
      );
      const item = cityInfo || provinceInfo;
      return item ? item.name : "选择所在城市";
    },
    showService() {
      if (this.productInfo.serviceIds && this.productInfo.serviceIds.length) {
        const results = this.options.service.filter((item) => {
          return this.productInfo.serviceIds.indexOf(item.key) > -1;
        });
        return results.length
          ? results.map((x) => x.value).join(",")
          : "请选择服务项目";
      } else {
        return "请选择服务项目";
      }
    },
    showAge() {
      return (
        this.options?.age?.find((x) => x.key === this.productInfo.age)?.value ||
        "选择年龄"
      );
    },
    showBodyHeight() {
      return (
        this.options?.bodyHeight?.find((x) => x.key === this.productInfo.height)
          ?.value || "选择身高"
      );
    },
    showCup() {
      return (
        this.options?.cup?.find((x) => x.key === this.productInfo.cup)?.value ||
        "选择罩杯"
      );
    },
    postId() {
      const id = (this.$route.query.postId as unknown as string) || "";
      return id.replace(/\s/g, "");
    },
    buttonTitle() {
      return !this.postId ? "发布" : "提交审核";
    },
    queryPostType() {
      return (
        (this.$route.query.postType as unknown as PostType) || PostType.Square
      );
    },
    textLength() {
      if (Object.keys(this.productInfo).length === 0) {
        return 0;
      } else {
        return this.productInfo.serviceDescribe
          ? this.productInfo.serviceDescribe.length
          : 0;
      }
    },
  },
});
</script>
