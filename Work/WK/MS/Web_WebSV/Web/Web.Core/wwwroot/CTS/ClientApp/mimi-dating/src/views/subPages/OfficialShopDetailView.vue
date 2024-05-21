<template>
  <div class="main_container bg_main_index">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title"></div>
      </header>
      <!-- Header end -->
      <!-- 店舖資訊 start -->
      <div class="officialstore_container">
        <div class="bg_circlemask_top"></div>
        <div class="officialstore_wrapper">
          <div class="officialstore_outer">
            <div class="collect_section" :class="{ active: shopDetail.isFollow }" @click.stop="setFollow"></div>
            <div class="officialstore_top spacing">
              <div class="officialstore_avatar">
                <AssetImage :item="setOfficialImageItem(shopDetail)" alt="" />
              </div>
              <div class="officialstore_inner">
                <div class="officialstore_inner_top">
                  <div class="officialstore_sheet type">
                    <div class="official_title_name" :data-text="shopDetail.shopName">{{ shopDetail.shopName }}</div>
                  </div>
                </div>
                <div class="officialstore_inner_bottom">
                  <div class="officialstore_flex">
                    <div class="officialstore_label blue">店龄{{ shopDetail.shopYears }}年
                    </div>
                    <div class="officialstore_label purple">妹子{{ shopDetail.girls }}位</div>
                  </div>
                  <div class="officialstore_flex">
                    <div class="officialstore_label copper">{{ shopDetail.dealOrder }}已约过
                    </div>
                    <div class="officialstore_label green">评分{{
                      shopDetail.selfPopularity }}</div>
                  </div>
                </div>
              </div>
            </div>
            <div class="officialstore_bottom">
              <div class="officialstore_londspeaker">
                <CdnImage src="@/assets/images/index/ic_loudspeaker.png" alt="" /><span>介绍：</span>
                <div class="londspeaker_text">{{ shopDetail.introduction }}</div>
              </div>
            </div>
          </div>
        </div>

      </div>
      <!-- 店舖資訊 end -->
      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="bg_circlemask">
          <div class="officialstore_tabs">
            <div class="officialstore_tab" :class="{ active: isPostPage }" @click="resetPage(1)">商品</div>
            <div class="officialstore_tab" :class="{ active: !isPostPage }" @click="resetPage(2)">商家信息</div>
          </div>
          <div class="officialstore_infos_wrapper" v-show="isPostPage">
            <div class="officialstore_infos_outer">
              <div class="overflow no_scrollbar">
                <div class="officialstore_cities">
                  <div class="city_outer" :class="{ active: areaCode == '' }">
                    <div class="city_inner" @click="resetAreaCode('')">
                      <div class="city_inner_name">全部</div>
                    </div>
                  </div>
                  <div class="city_outer" :class="{ active: areaCode == province.province }" v-for="province in showProvinceList">
                    <div class="city_inner" @click="resetAreaCode(province.province)">
                      <div class="city_inner_name">{{ province.name }}</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="officialstore_cards_outer">
              <div class="overflow no_scrollbar w_100" @scroll="onScroll" ref="scrollContainer">
                <div class="officialstore_cards" v-for="postList in sortedPostList">
                  <div class="card_outer" v-for="post in postList">
                    <div class="card_inner" @click="navigateToOfficialDetail(post.postId)">
                      <div class="card_top">
                        <div class="card_top_img">
                          <AssetImage :item="setPostImageItem(post)" alt="" />
                        </div>
                        <div class="total_view">
                          <div class="icon"></div>
                          <div class="text">{{ post.viewBaseCount + post.views }}</div>
                        </div>  
                      </div>
                      <div class="card_bottom">    
                        <div class="official_title">
                          <div class="official_name">{{ post.title }}</div>
                        </div>
                        <div class="info_co">
                          <div class="info">
                            <div class="icon">
                              <CdnImage src="@/assets/images/card/ic_card_body_info.svg" alt="" />
                            </div>
                            <div>{{ post.height }}cm</div>
                            <div class="dot"></div>
                            <div>{{ post.age }}岁</div>
                            <div class="dot"></div>
                            <div>{{ post.cup }}杯</div>
                          </div>
                        </div>
                        <p>期望收入：¥{{ post.lowPrice }}</p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="officialstore_height overflow no_scrollbar" v-show="!isPostPage">
            <div class="activate">
              <div class="icon">
                <CdnImage src="@/assets/images/post/ic_time.svg" alt="" />
              </div>
              <div class="text">
                <div class="business_title">营业时间</div>
                <div class="business_outer">
                  <div class="business_icon">
                    <CdnImage src="@/assets/images/post/ic_business_week.svg" alt="" />
                  </div>
                  <div class="business_text">营业时段：{{ shopDetail.businessDate }}</div>
                </div>
                <div class="business_outer">
                  <div class="business_icon">
                    <CdnImage src="@/assets/images/post/ic_business_time.svg" alt="" />
                  </div>
                  <div class="business_text">营业时间：{{ shopDetail.businessHour }}</div>
                </div>
              </div>
            </div>
            <div class="list_title">
              <div class="line"></div>
              <div class="text">商家照片</div>
            </div>
            <div class="store_photo_wrapper">
              <div class="store_photo" v-for="index in maxOwnerImgCount">
                <template v-if="index <= ownerImgCount">
                  <AssetImage :item="setBusinessPhotoImageItem(shopDetail.businessPhotoSource[index - 1])" alt="" />
                </template>
                <template v-else>
                  <CdnImage src="@/assets/images/card/avatar_store_default.png" />
                </template>
              </div>
            </div>
            <div class="list_title">
              <div class="line"></div>
              <div class="text">商家承诺</div>
            </div>
            <div class="service_info">
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_assure.svg" alt="" />
                </div>
                <div class="text">
                  <p>从业保障：</p>
                  <p>多年口碑经营 可验证信息</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_safty.svg" alt="" />
                </div>
                <div class="text">
                  <p>安全保障：</p>
                  <p>商家入驻已缴纳保证金</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_serviceitem.svg" alt="" />
                </div>
                <div class="text">
                  <p>服务项目：</p>
                  <p>售前咨询 售后服务</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {
  NavigateRule,
  VirtualScroll,
  DialogControl,
  PlayGame,
  Tools,
  ImageCacheManager,
  ScrollManager,
} from "@/mixins";
import api from "@/api";
import { MutationType } from "@/store";
import {
  OfficialShopDetailModel,
  ChinaCityInfo,
  PageParamModel,
  MediaModel,
  OfficialPostModel,
  OfficialShopModel,
  OfficialPostListParamModel,
  OfficialShopFollowParamModel,
  ImageItemModel,
  BaseInfoModel
} from "@/models";
import { SlideBanner, ImageZoom, AssetImage, CdnImage } from "@/components";
import toast from "@/toast";
import { PostType } from "@/enums";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: {
    SlideBanner,
    ImageZoom,
    AssetImage,
    CdnImage
  },
  mixins: [
    NavigateRule,
    VirtualScroll,
    DialogControl,
    PlayGame,
    Tools,
    ImageCacheManager,
    ScrollManager
  ],
  data() {
    return {
      totalPage: 1,
      shopDetail: {} as OfficialShopDetailModel,
      imageZoomSwitch: false,
      imageZoomItem: {} as MediaModel,
      maxOwnerImgCount: 3,
      ownerImgCount: 0,
      areaCode: "",
      paramterAreaCode:[] as  string [],
      isPostPage: true,
      officialPostList: [] as OfficialPostModel[],
      officialPostListParam: {} as OfficialPostListParamModel,
      showProvinceList:[] as ChinaCityInfo[],
      provinceList: require("@/assets/json/province.json") as ChinaCityInfo[],
      cityList:require("@/assets/json/city.json") as ChinaCityInfo[],
    };
  },
  methods: {
    async loadDetail() {
      try {
        this.shopDetail = await api.getOfficialShopDetail(this.applyId);
        const medias = [{
          applyId: this.applyId,
          shopAvatarSource: this.shopDetail.shopAvatarSource
        }] as OfficialShopModel[];

        this.selectProvinceList(this.shopDetail.areaCodes);
        this.ownerImgCount = this.shopDetail.businessPhotoSource.length;
        this.handleBussinessPhoto();
        this.officialDownload(medias);
        const newMedias = this.shopDetail.businessPhotoSource.map((src) => <BaseInfoModel>{
          postId: src,
          coverUrl: src
        });
        this.baseImageInfoDownload(newMedias);
        this.$store.commit(MutationType.SetOfficialShopDetail, this.shopDetail);
      } catch (e) {
        // toast("加载店铺详情失败");
        this.navigateToPrevious();
      }
    },
    async loadPost() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        this.officialPostListParam.areaCode = this.paramterAreaCode;
        this.officialPostListParam.pageNo = this.pageInfo.pageNo + 1;
        this.officialPostListParam.applyId = this.applyId;
        const result = await api.getOfficialPostList(this.officialPostListParam);

        if (result.data && result.data.length > 0) {
          this.baseImageInfoDownload(result.data);
          this.totalPage = result.totalPage;
          this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
          this.pageInfo.pageNo = result.pageNo;
        }
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
      this.calculateVirtualScroll();
    },
    selectProvinceList(selectProvince: string[]) {
      const provinces = this.cityList.filter(c => selectProvince.includes(c.code)).map(c => c.province);
      const uniqueProvinces = provinces.filter((value, index, self) => self.indexOf(value) === index);
      this.showProvinceList = this.provinceList.filter(c => uniqueProvinces.includes(c.province));
    },
    resetAreaCode(areaCode: string) {
      if (this.areaCode == areaCode) {
        return;
      }
      this.areaCode = areaCode;
      let cityInfo = this.cityList.filter(c => c.province === areaCode);

      if (cityInfo === undefined) {
        toast("找不到所属城市!");
        return;
      }
      this.paramterAreaCode = cityInfo.map(c => c.code);
      this.reload();
    },
    resetPage(page: number) {
      if((this.isPostPage && page == 2) || (!this.isPostPage && page == 1)) {
        this.isPostPage = !this.isPostPage;
      }
    },
    async reload() {
      this.totalPage = 1;
      this.scrollStatus.list = [];
      this.pageInfo.pageNo = 0;
      await this.loadPost();
    },
    async setFollow() {
      this.shopDetail.isFollow = !this.shopDetail.isFollow;
      let model = {} as OfficialShopFollowParamModel;
      model.applyId = this.shopDetail.applyId
      await api.officialShopFollow(model.applyId);
    },
    async $_onScrollToBottom() {
      await this.loadPost();
    },
    setPostImageItem(info: OfficialPostModel) {
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.coverUrl,
        class: "",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
    setOfficialImageItem(info: OfficialShopDetailModel) {
      let item: ImageItemModel = {
        id: info.applyId,
        subId: info.shopAvatarSource,
        class: "",
        src: info.shopAvatarSource,
        alt: "",
      };
      return item;
    },
    handleBussinessPhoto() {
      if (this.shopDetail.businessPhotoSource) {
        if (this.shopDetail.businessPhotoSource.length >= this.maxOwnerImgCount) {
          this.shopDetail.businessPhotoSource = this.shopDetail.businessPhotoSource.slice(0, 3);
        }
        else {
          do {
            this.shopDetail.businessPhotoSource.push(require("@/assets/images/card/avatar_store_default.png"));
          }
          while (this.shopDetail.businessPhotoSource.length < 3)
          return this.shopDetail.businessPhotoSource;
        }
      }
    },
    setBusinessPhotoImageItem(src: string) {
      let item: ImageItemModel = {
        id: src,
        subId: src,
        class: "",
        src: src,
        alt: "",
      };
      return item;
    },
  },
  async created() {
    this.scrollStatus.list = [];
    await this.loadDetail();
    await this.loadPost();
    this.$store.commit(MutationType.SetPostType, PostType.Official);
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 200;
    },
    applyId() {
      return (this.$route.query.applyId as unknown as string) || "";
    },
    postList() {
      return this.scrollStatus.list as OfficialPostModel[] || [];
    },
    sortedPostList() {
      let presentPostList = [];
      let length = this.postList.length % 2 === 0 ? this.postList.length / 2 : Math.floor(this.postList.length / 2 + 1);
      for (let i = 0; i < length; i++) {
        let temp = this.postList.slice(i * 2, i * 2 + 2);
        presentPostList.push(temp);
      }
      return presentPostList;
    },
    pageInfo() {
      return {
        pageNo: 0,
        pageSize: 10,
      } as PageParamModel;
    },
  },
});
</script>
