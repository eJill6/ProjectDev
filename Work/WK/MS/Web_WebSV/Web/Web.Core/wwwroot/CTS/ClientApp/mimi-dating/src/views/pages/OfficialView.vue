<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar" ref="scrollContainer" @scroll="onScroll">
      <!-- 官方店鋪搜尋商品 start -->
      <div class="officialstore_section">
        <div class="officialstore_bg_title">
          <div class="officialstore_title" data-text="官方店鋪">官方店鋪</div>
        </div>
        <div class="officialstore_search">
          <div class="search_sheet">
            <div class="officialstore_icon">
              <CdnImage
                src="@/assets/images/card/ic_officialstore_search.svg"
              />
            </div>
            <form class="search_form">
              <input
                class="search_input"
                v-model="searchModel.keyword"
                placeholder="搜索商家或商品"
              />
              <div class="search_finish_outer" @click="goToOfficialSearch()">
                <div class="search_finish_inner">搜索</div>
              </div>
            </form>
          </div>
        </div>
      </div>
      <!-- 官方店鋪搜尋商品 end -->
      <!-- slide banner -->
      <SlideBanner></SlideBanner>
      <!-- slide banner End -->

      <!-- 金牌店鋪 start -->
      <div class="goldstore_section">
        <div class="goldstore_block">
          <div class="goldstore_decorate">
            <CdnImage
              src="@/assets/images/card/img_greenstone_decorate_left.png"
            />
          </div>
          <div class="goldstore_bg_title">
            <div class="goldstore_title">
              <CdnImage
                src="@/assets/images/card/img_goldstore_text.png"
              />
            </div>
          </div>
          <div class="goldstore_decorate">
            <CdnImage
              src="@/assets/images/card/img_greenstone_decorate_right.png"
            />
          </div>
        </div>
      </div>
      <div class="goldstore_wrapper">
        <div
          class="goldstore_outer"
          v-for="(item, index) in officialGoldenShopList"
        >
          <div class="goldstore_ranking" :class="{ top: index == 1 }">
            <CdnImage :src="getImgSrc(index)" />
          </div>
          <div
            class="goldstore_border"
            @click="navigateToOfficialShopDetail(item.applyId)"
          >
            <div class="goldstore_avatar">
              <AssetImage :item="setImageItem(item)" />
            </div>
            <div class="goldstore_info_inner">
              <div class="goldstore_title" :data-text="item.shopName">
                {{ item.shopName }}
              </div>
              <div class="goldstore_btns">
                <div class="btn_outer">
                  <div class="btn_inner green">妹子{{ item.girls }}位</div>
                </div>
                <div class="btn_outer">
                  <div class="btn_inner orange">进入店铺</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="goldstore_decorate_section">
        <div class="goldstore_decorate_bird">
          <CdnImage src="@/assets/images/card/img_bird_decorate.png" />
        </div>
        <div class="goldstore_decorate_up">
          <CdnImage src="@/assets/images/card/img_stone_decorate_up.png" />
        </div>
        <div class="goldstore_decorate_down">
          <CdnImage src="@/assets/images/card/img_stone_decorate_bottom.png" />
        </div>
      </div>
      <!-- 金牌店鋪 end -->

      <!-- 销量最高、热门店铺 start -->
      <div class="popular_tabs">
        <div class="popular_tab crown" @click="sort(0)">
          <div class="popular_icon">
            <CdnImage src="@/assets/images/card/ic_store_crown.png" />
          </div>
          <div class="popular_text">销量最高</div>
        </div>
        <div class="popular_tab fire" @click="sort(1)">
          <div class="popular_icon">
            <CdnImage src="@/assets/images/card/ic_store_fire.png" />
          </div>
          <div class="popular_text">热门店铺</div>
        </div>
      </div>
      <!-- 销量最高、热门店铺 end -->

      <div class="padding_order">
        <div class="me_order me_order_second spacing" v-for="info in shopList">
          <div class="freeroad">
            <div class="freeroad_text">免路程费</div>
          </div>
          <div class="user_info user_flex">
            <div
              class="avatar_fourth avatar_fit"
              @click="navigateToOfficialShopDetail(info.applyId)"
            >
              <div class="total_view">
                <div class="icon"></div>
                <div class="text">{{ info.viewBaseCount + info.views }}</div>
              </div>
              <AssetImage :item="setImageItem(info)" alt="" />
            </div>
            <div class="info">
              <div class="first_co">
                <div class="basic">
                  <div class="name_info_inner spacing">
                    <div class="name_sixth" :data-text="info.shopName">
                      {{ info.shopName }}
                    </div>
                  </div>
                </div>
                <div
                  class="basic"
                  @click="navigateToOfficialShopDetail(info.applyId)"
                >
                  <div class="keywords_info_btn">
                    <div class="btn_one">店龄{{ info.shopYears }}年</div>
                    <div class="btn_two">妹子{{ info.girls }}位</div>
                    <div class="btn_three">{{ info.dealOrder }}已约过</div>
                    <div class="btn_four">评分{{ info.selfPopularity }}</div>
                  </div>
                </div>
                <div class="basic spacing overflow no_scrollbar">
                  <div class="info_photos" v-for="post in info.postList">
                    <div
                      class="item"
                      @click="navigateToOfficialDetail(post.postId)"
                    >
                      <div class="photo">
                        <AssetImage :item="setPostImageItem(post)" alt="" />
                      </div>
                      <div class="outer">
                        <div class="title">{{ post.title }}</div>
                        <div class="inner">
                          <div class="text">期待收入</div>
                          <div class="money">￥{{ post.lowPrice }}</div>
                        </div>
                      </div>
                    </div>
                  </div>
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
import { CdnImage, SlideBanner, AssetImage } from "@/components";
import { defineComponent } from "vue";
import api from "@/api";
import { NavigateRule, PageManager } from "@/mixins";
import { MutationType } from "@/store";
import {
  OfficialShopModel,
  OfficialShopListModel,
  OfficialShopListParamModel,
  BannerModel,
  PageParamModel,
  OfficialPostModel,
  ImageItemModel,
  ProductListModel,
} from "@/models";
import { MediaType, PostType } from "@/enums";
import toast from "@/toast";

export default defineComponent({
  components: {
    CdnImage,
    SlideBanner,
    AssetImage,
  },
  mixins: [NavigateRule, PageManager],
  data() {
    return {
      totalPage: 1,
      searchModel: {
        keyword: "",
        sortType: 0,
        pageNo: 1,
        pageSize: 5,
      } as OfficialShopListParamModel,
      officialGoldenShopList: [] as OfficialShopModel[],
    };
  },
  async created() {
    this.$store.commit(MutationType.SetPostType, PostType.Official);
    this.scrollStatus.list = [];
    await this.loadBanner();
    await this.loadGoldenShop();
    await this.loadAsync();
  },
  methods: {
    async loadBanner() {
      var bannerList = await api.getOfficialBanner();
      const medias = bannerList.map(
        (model) =>
          <BannerModel>{
            title: model.title,
            type: model.type,
            linkType: model.linkType,
            redirectUrl: model.redirectUrl,
            fullMediaUrl: model.fullMediaUrl,
            id: model.id,
            mediaType: MediaType.Image,
          }
      );
      this.bannerDownload(medias);
      this.$store.commit(MutationType.SetBanner, medias);
    },
    async loadGoldenShop() {
      this.officialGoldenShopList = await api.getOfficialGoldenShopList();
      if (this.officialGoldenShopList.length > 1) {
        [this.officialGoldenShopList[0], this.officialGoldenShopList[1]] = [
          this.officialGoldenShopList[1],
          this.officialGoldenShopList[0],
        ];
      }
      this.officialDownload(this.officialGoldenShopList);
    },
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        this.searchModel.pageNo = this.pageInfo.pageNo + 1;
        const result = await api.getOfficialShopList(this.searchModel);
        this.totalPage = result.totalPage;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.pageInfo.pageNo = result.pageNo;
        const medias = result.data.map(
          (model) =>
            <OfficialShopModel>{
              applyId: model.applyId,
              shopAvatarSource: model.shopAvatarSource,
              shopName: model.shopName,
            }
        );
        this.officialDownload(medias);
        let posts = [] as OfficialPostModel[];
        result.data.forEach((x) => {
          posts = posts.concat(x.postList);
        });
        const postMedias = posts.map(
          (model) =>
            <ProductListModel>{
              postId: model.postId,
              coverUrl: model.coverUrl,
            }
        );
        this.postDownload(postMedias);
      } catch (error) {
        toast(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
      this.calculateVirtualScroll();
    },
    goToOfficialSearch() {
      if (this.searchModel.keyword) {
        this.navigateToOfficialSearch(this.searchModel.keyword);
      } else {
        toast("请输入内容");
      }
    },
    setImageItem(info: OfficialShopModel) {
      let item: ImageItemModel = {
        id: info.applyId,
        subId: info.shopAvatarSource,
        class: "",
        src: info.shopAvatarSource,
        alt: "",
      };
      return item;
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
    async $_onScrollToBottom() {
      await this.loadAsync();
    },
    async reload() {
      this.totalPage = 1;
      this.scrollStatus.list = [];
      this.pageInfo.pageNo = 0;
      await this.loadAsync();
    },
    getImgSrc(index: number) {
      if (this.officialGoldenShopList.length == 1) {
        return "@/assets/images/card/img_gold_1.png";
      } else {
        switch (index) {
          case 0:
            return "@/assets/images/card/img_gold_2.png";
          case 1:
            return "@/assets/images/card/img_gold_1.png";
          case 2:
            return "@/assets/images/card/img_gold_3.png";
        }
      }
      return "";
    },
    sort(sortType: number) {
      this.searchModel.sortType = sortType;
      this.reload();
    },
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 200;
    },
    shopList() {
      return (this.scrollStatus.list as OfficialShopListModel[]) || [];
    },
    pageInfo() {
      return {
        pageNo: 0,
        pageSize: 5,
      } as PageParamModel;
    },
  },
});
</script>
