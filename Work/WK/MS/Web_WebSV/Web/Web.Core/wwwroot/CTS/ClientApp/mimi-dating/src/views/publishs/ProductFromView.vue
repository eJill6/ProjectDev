<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color solid_line">
        <div class="header_back" @click="backButtonEvent">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">发布</div>
        <!-- <div class="header_btn align_right" @click="navigateToRule">
          <div>
            <div class="style_a">发帖规则</div>
          </div>
        </div> -->
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height bg_personal">
        <ImageZoom
          :image="imageZoomItem"
          :isEdit="true"
          v-if="imageZoomSwitch"
          @deleteImage="deleteImageItem"
        ></ImageZoom>
        <div class="overflow no_scrollbar post">
          <!-- 你的切版放這裡 -->
          <div class="padding_basic_2 pt_0 pb_0">
            <div id="messageId" v-if="isSquare" class="sheet_main full_width" :class="{ alert: checkFailure(productInfo.messageId) }">
              <div class="title title_full_width">信息类型</div>
              <div class="content_full">
                <div class="tag">
                  <div class="tag_item" v-for="n in options.messageType" :class="{ active: isSeletedMessageType(n) }"
                    @click="selectedMessageType(n)">
                    {{ n.value }}
                  </div>
                </div>
              </div>
            </div>
            <div class="sheet_main">
              <div class="title title_center">解锁价格</div>
              <div class="content content_right">
                <div class="text highlight_text spacing_second">{{ unlockAmount }}钻石</div>
              </div>
            </div>
            <div class="sheet_main main_none" @click="applyAdjustPricePicker">
              <div class="title title_center">申请调价（选填）</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showApplyAmount }}
                  </div>
                  <!--註：有箭頭的選項，選擇後後文字會變白色 把class default_text換成normal_text即可-->
                  <div class="arrow">
                    <CdnImage src="@/assets/images/element/ic_sheet_choose.svg" alt="" />
                  </div>
                </div>
              </div>
            </div>
            <div class="sheet_main main_none">
              <div class="title_default title_center">备注</div>
              <div class="content content_right">
                <div class="text default_text spacing_second">
                  审核员决定是否通过，未通过仍为基础价
                </div>
              </div>
            </div>
          </div>

          <div class="text_notice">
            用户每解锁一次，你都会获得收益，可进行提现
          </div>
          <div class="padding_basic_2 pt_0 pb_0">
            <div id="postTitle" class="sheet_main" :class="{ alert: checkFailure(productInfo.title) }">
              <div class="title_default title_center">帖子标题</div>
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style" placeholder="请输入帖子标题" v-model="productInfo.title" maxlength="10" />
                  <input hidden />
                </form>
              </div>
            </div>
            <div id="areaCode" class="sheet_main" @click="showCityPicker"
              :class="{ alert: checkFailure(productInfo.areaCode) }">
              <div class="title_default title_center">所在地区</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showCityName }}
                  </div>
                  <div class="arrow">
                    <CdnImage src="@/assets/images/element/ic_sheet_choose.svg" alt="" />
                  </div>
                </div>
              </div>
            </div>
            <div id="quantity" class="sheet_main" :class="{ alert: checkFailure(productInfo.quantity) }">
              <div class="title title_center">数量</div>
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style" placeholder="例如：3-7人" v-model="productInfo.quantity" maxlength="10" />
                </form>
              </div>
            </div>
            <!-- <div id="label" class="sheet_main" @click="labelPicker">
              <div class="title_default title_center">标签</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">请选择标签</div>
                  <div class="arrow">
                    <CdnImage src="@/assets/images/element/ic_sheet_choose.svg" alt="" />
                  </div>
                </div>
              </div>
            </div> -->
            <div id="age" class="sheet_main" @click="agePicker" :class="{ alert: checkFailure(productInfo.age) }">
              <div class="title title_center">年龄（岁）</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showAge }}
                  </div>
                  <div class="arrow">
                    <CdnImage src="@/assets/images/element/ic_sheet_choose.svg" alt="" />
                  </div>
                </div>
              </div>
            </div>
            <div id="postHeight" class="sheet_main" @click="bodyHeightPicker"
              :class="{ alert: checkFailure(productInfo.height) }">
              <div class="title_default title_center">身高（cm）</div>
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
                <!-- <form class="form_full spacing_second">
                  <input class="input_style" placeholder="请输入身高" v-model="productInfo.height">
                </form> -->
              </div>
            </div>
            <div id="cup" class="sheet_main" @click="cupPicker" :class="{ alert: checkFailure(productInfo.cup) }">
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
                <!-- <form class="form_full spacing_second">
                  <input class="input_style_second" placeholder="请选择罩杯尺寸" v-model="productInfo.cup" />
                </form> -->
              </div>
            </div>

            <div id="businessHours" class="sheet_main" :class="{ alert: checkFailure(productInfo.businessHours) }">
              <div class="title title_center">营业时间</div>
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style" placeholder="例如：全天" v-model="productInfo.businessHours"
                    maxlength="15" />
                </form>
              </div>
            </div>
            
            <div id="serviceIds" class="sheet_main" @click="servicePicker"
              :class="{ alert: checkFailure(productInfo.serviceIds) }">
              <div class="title title_center">服务项目</div>
              <div class="content content_right">
                <div class="flex_wrap cursor">
                  <div class="text default_text flex_full spacing">
                    {{ showService }}
                  </div>
                  <div class="arrow">
                    <CdnImage src="@/assets/images/element/ic_sheet_choose.svg" alt="" />
                  </div>
                </div>
              </div>
            </div>
            <div id="postPrice" class="sheet_main" :class="{
              alert: checkLowAndHighPriceFailure(
                productInfo.lowPrice,
                productInfo.highPrice
              ),
            }">    
              <div class="title title_center">服务价格</div>
              <div class="content content_right">
                <div class="flex_wrap spacing_second">
                  <form class="form_w100 spacing">
                    <input type="number" class="input_style" placeholder="最低价格" v-model="productInfo.lowPrice"
                      maxlength="10" />
                  </form>
                  <p class="dash spacing">-</p>
                  <form class="form_w100">
                    <input type="number" class="input_style" placeholder="最高价格" v-model="productInfo.highPrice"
                      maxlength="10" />
                  </form>
                </div>
              </div>
            </div>
            <div id="address" class="sheet_main" :class="{ alert: checkFailure(productInfo.address) }">
              <div class="title title_center">详细地址</div>
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style" placeholder="请输入详细地址" v-model="productInfo.address" maxlength="20" />
                </form>
              </div>
            </div>
            <div id="contactInfos" class="sheet_main" :class="{ alert: checkFailure(productInfo.contactInfos) }">
              <div class="title title_center">联系方式</div>
              <div class="title title_center">
                <span class="attention">（请填写至少一种联系方式）</span>
              </div>
            </div>
            <div class="sheet_main">
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style_second" placeholder="微信号" v-model="contacts.weixin" maxlength="50" />
                </form>
              </div>
            </div>
            <div class="sheet_main">
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style_second" placeholder="QQ号" v-model="contacts.qQ" maxlength="50" />
                </form>
              </div>
            </div>
            <div class="sheet_main">
              <div class="title title_center"></div>
              <div class="content content_right">
                <form class="form_full spacing_second">
                  <input class="input_style_second" placeholder="手机号" v-model="contacts.phone" maxlength="50" />
                </form>
              </div>
            </div>

            <div id="serviceDescribe" class="sheet_main full_width main_none"
              :class="{ alert: checkFailure(productInfo.serviceDescribe) }">
              <div class="title title_full_width">服务描述</div>
              <div class="content_full">
                <form class="textarea_full_width_normal bg_describe">
                  <textarea v-model="productInfo.serviceDescribe" class="no_scrollbar" name=""
                    placeholder="可以详细描述妹子的优势特征，服务环境和服务技术等..." maxlength="100"></textarea>
                  <div class="describe_text">{{ textLength }}/100</div>
                </form>
              </div>
            </div>
            <MediaSelection :title="selectTitle" :annotation="annotation" :source="source" :media="MediaType.Image" :max="maxCount"
              @show="showImageZoom"></MediaSelection>
            <MediaSelection :title="videoSelectTitle" :source="source" :media="MediaType.Video" :max="videoMaxCount"
              @show="showImageZoom"></MediaSelection>
          </div>
          <div class="sheet_btn">
            <div class="btn_default" @click="publish">{{ buttonTitle }}</div>
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
  ProductModel,
  PostTypeOptionsModel,
  OptionItemModel,
  ChinaCityInfo,
} from "@/models";
import { MutationType } from "@/store";
import { MediaSelection, ImageZoom, CdnImage } from "@/components";
import { SourceType, MediaType, PostType, ContactType } from "@/enums";
import api from "@/api";
import toast from "@/toast";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: { MediaSelection, ImageZoom, CdnImage },
  mixins: [NavigateRule, DialogControl, MediaCenter, PlayGame, ScrollManager],
  data() {
    return {
      productInfo: {} as ProductModel,
      cityPickerVisible: false,
      currentProvinceOption: "",
      currentCityOption: "",
      selectTitle: "照片（第一张照片将作为封面图展示",
      annotation: "",
      maxCount: 5,
      source: SourceType.Post,
      videoSelectTitle: "视频（选填，添加视频会获得更多关注哦）",
      videoMaxCount: 1,
      MediaType,
      imageZoomSwitch: false,
      options: {} as PostTypeOptionsModel,
      contacts: {
        phone: "",
        qQ: "",
        weixin: "",
      },
      isCheckFailure: false,
      unlockAmount: 100,
      isSquare: false
    };
  },
  methods: {
    applyAdjustPricePicker() {
      const content = this.options.price;
      const infoModel: PopupModel = {
        title: "请选择调整价格",
        content: content,
        isMultiple: false,
      };

      this.showPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        this.productInfo.applyAmount = Number(list[0].value);
      });
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
    isSeletedMessageType(item: OptionItemModel) {
      return this.productInfo.messageId === item.key;
    },
    selectedMessageType(item: OptionItemModel) {
      this.productInfo.messageId = item.key;
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
    labelPicker() {
      const content = this.options.label;
      const infoModel: PopupModel = {
        title: "标签",
        content: content,
        isMultiple: true,
      };

      this.showPickerDialog(infoModel, (selectedArray) => {
        const list = selectedArray as OptionItemModel[];
        const data = list.map((l) => l.key);
        //this.productInfo.label = data;
      });
    },
    async publish() {
      this.$store.commit(MutationType.SetIsLoading, true);
      this.sortContactInfo(this.productInfo);

      const tagString = this.checkProperties(this.productInfo);
      
      this.isCheckFailure = !!tagString;
      if (this.isCheckFailure) {
        if (!this.productInfo.messageId && this.isSquare) {
          toast("请选择信息类型");
        } else if (this.productInfo.lowPrice > this.productInfo.highPrice) {
          toast("服务价格最低价不得高于最高价");
        } else if (!this.productInfo.contactInfos ||
          !this.productInfo.contactInfos.length ||
          this.productInfo.contactInfos.every(x => x.contact == "")
        ) {
          toast("请填写至少一种联系方式");
        } else if (!this.productInfo.serviceDescribe || !this.productInfo.serviceDescribe.length) {
          toast("请填写服务描述");
        }

        const top = document.getElementById(tagString)?.scrollIntoView(); //Getting Y of target element
        window.scrollTo(0, top || 0);

        this.$store.commit(MutationType.SetIsLoading, false);
        return;
      }

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
      this.productInfo.photoIds = imageKeys;

      if (this.videoSelect.length > 0) {
        const videos = await this.uploadVideos();
        if (!videos.length) {
          this.$store.commit(MutationType.SetIsLoading, false);
          toast("影片上传失败");
          return;
        }
        const videoKeys = videos.map((image) => image.id);
        this.productInfo.videoIds = videoKeys;
      } else {
        this.productInfo.videoIds = [];
      }

      if (!this.productInfo.photoIds.length) {
        this.$store.commit(MutationType.SetIsLoading, false);
        return;
      }
      try {
        if (this.postId) {
          await api.editPost(this.postId as string, this.productInfo);
        } else {
          this.productInfo.postType = this.queryPostType;
          await api.createPost(this.productInfo);
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
    checkProperties(obj: ProductModel): string {
      if (!obj.messageId && this.isSquare) {
        return `messageId`;
      }
      if (!obj.title) {
        return `postTitle`;
      }
      if (!obj.areaCode) {
        return `areaCode`;
      }
      if (!obj.quantity) {
        return `quantity`;
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
        !obj.lowPrice ||
        !obj.highPrice ||
        obj.lowPrice < 100 ||
        obj.highPrice > 99999 ||
        obj.lowPrice > obj.highPrice
      ) {
        return `postPrice`;
      }
      if (!obj.address) {
        return `address`;
      }
      if (!obj.contactInfos || !obj.contactInfos.length || obj.contactInfos.every(x => x.contact == "")) {
        return `contactInfos`;
      }
      if (!obj.serviceDescribe) {
        return `serviceDescribe`;
      }
      return "";
    },
    sortContactInfo(obj: ProductModel) {
      obj.contactInfos = [];

      obj.contactInfos.push({
        contactType: ContactType.Phone,
        contact: this.contacts.phone,
      });
      obj.contactInfos.push({
        contactType: ContactType.QQ,
        contact: this.contacts.qQ,
      });
      obj.contactInfos.push({
        contactType: ContactType.Weixin,
        contact: this.contacts.weixin,
      });
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
    async getUnlockAmountByType() {
      try {
        const result = await api.getBaseUnlockAmountByType(this.queryPostType);
        this.unlockAmount = result.unlockAmount;
      } catch (e) {
        toast(e);
      }
    },
    async getEditData() {
      if (!this.postId) return;
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        this.productInfo = await api.getPostEditData(this.postId);
        const phoneItem = this.productInfo.contactInfos.find(
          (item) => item.contactType === ContactType.Phone
        );
        this.contacts.phone = (phoneItem?.contact as string) || "";

        const qqItem = this.productInfo.contactInfos.find(
          (item) => item.contactType === ContactType.QQ
        );
        this.contacts.qQ = (qqItem?.contact as string) || "";

        const weixinItem = this.productInfo.contactInfos.find(
          (item) => item.contactType === ContactType.Weixin
        );
        this.contacts.weixin = (weixinItem?.contact as string) || "";
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
    checkFailure(item: number | string | any[]) {
      return (
        ((Array.isArray(item) && !item.length) || !item) && this.isCheckFailure
      );
    },
    checkLowAndHighPriceFailure(lowPrice: number, highPrice: number) {
      return (
        lowPrice < 100 ||
        highPrice > 99999 ||
        !lowPrice ||
        !highPrice ||
        lowPrice > highPrice
      );
    },
  },
  async created() {
    await this.setUserInfo();
    await this.getOptionsByPostType();
    await this.getUnlockAmountByType();
    await this.getEditData();
  },
  mounted() {
    let body = document.getElementsByTagName("body")[0];
    body.style.fontSize = "22px";
  },
  computed: {
    showApplyAmount() {
      return this.productInfo.applyAmount || "选择调整价格";
    },
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
        "请选择年龄"
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
      const postType = (this.$route.query.postType as unknown as PostType) || PostType.Square
      this.isSquare = postType == PostType.Square;
      return postType;
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
