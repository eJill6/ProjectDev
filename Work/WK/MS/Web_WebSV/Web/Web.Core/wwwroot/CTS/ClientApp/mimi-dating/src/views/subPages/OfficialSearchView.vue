<template>
  <div class="main_container bg_main_index index" v-if="!hasResult">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="officialstore_section spacing">
          <div class="officialstore_bg_title">
            <div class="officialstore_title" data-text="官方店鋪">官方店鋪</div>
          </div>
          <div class="officialstore_search">
            <div class="search_sheet">
              <div class="officialstore_icon">
                <CdnImage src="@/assets/images/card/ic_officialstore_search.svg" alt="" />
              </div>
              <form class="search_form">
                <input class="search_input" v-model="searchModel.keyword" placeholder="搜索商家或商品">
                <div class="search_finish_outer" @click="search">
                  <div class="search_finish_inner">搜索</div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class=" flex_height">
        <div class="overflow no_scrollbar">
          <div class="empty_state_content">
            <div class="empty_state">
              <div class="icon">
                <CdnImage src="@/assets/images/public/pic_none.png" alt="" />
              </div>
              <div class="nodata">暂无符合条件的内容</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="main_container bg_main_index index" v-else>
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="officialstore_section spacing">
          <div class="officialstore_bg_title">
            <div class="officialstore_title" data-text="官方店鋪">官方店鋪</div>
          </div>
          <div class="officialstore_search">
            <div class="search_sheet">
              <div class="officialstore_icon">
                <CdnImage src="@/assets/images/card/ic_officialstore_search.svg" alt="" />
              </div>
              <form class="search_form">
                <input class="search_input" v-model="searchModel.keyword" placeholder="搜索商家或商品">
                <div class="search_finish_outer" @click="search">
                  <div class="search_finish_inner">搜索</div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class=" flex_height">
        <div class="overflow no_scrollbar" ref="scrollContainer" @scroll="onScroll">
          <div class="padding_order">
            <div class="me_order me_order_second spacing" v-for="info in shopList"
              @click="navigateToOfficialShopDetail(info.applyId)">
              <div class="freeroad">
                <div class="freeroad_text">免路程费</div>
              </div>
              <div class="user_info user_flex">
                <div class="avatar_fourth avatar_fit">
                  <div class="total_view">
                    <div class="icon"></div>
                    <div class="text">{{ info.viewBaseCount + info.views }}</div>
                  </div>
                  <AssetImage :item="setOfficialImageItem(info)" alt="" />
                </div>
                <div class="info">
                  <div class="first_co">
                    <div class="basic">
                      <div class="name_info_inner spacing">
                        <div class="name_sixth" :data-text="info.shopName">{{ info.shopName }}</div>
                      </div>
                    </div>
                    <div class="basic">
                      <div class="keywords_info_btn">
                        <div class="btn_one">店龄{{ info.shopYears }}年</div>
                        <div class="btn_two">妹子{{ info.girls }}位</div>
                        <div class="btn_three">{{ info.dealOrder }}已约过</div>
                        <div class="btn_four">评分{{ info.selfPopularity }}</div>
                      </div>
                    </div>
                    <div class="basic spacing overflow no_scrollbar">
                      <div class="info_photos" v-for="post in info.postList">
                        <div class="item" @click.stop="navigateToOfficialDetail(post.postId)">
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
    </div>
  </div>
</template>
<script lang="ts">
import { AssetImage, CdnImage } from "@/components";
import { defineComponent } from "vue";
import api from "@/api";
import { PageManager } from "@/mixins";
import { MutationType } from "@/store";
import {
  OfficialShopListModel,
  OfficialShopListParamModel,
  OfficialPostModel,
  ImageItemModel,
  PageParamModel,
  BaseInfoModel
} from "@/models";
import { PostType } from "@/enums";

export default defineComponent({
  data() {
    return {
      totalPage: 1,
      hasResult: true,
      searchModel: {
        keyword: "",
        sortType: 0,
        pageSize: 5,
        pageNo: 0
      } as OfficialShopListParamModel,
    }
  },
  components: {
    CdnImage,
    AssetImage
  },
  mixins: [PageManager],
  async created() {
    this.scrollStatus.list = [];
    this.searchModel.keyword = this.keyword;
    await this.loadAsync();
  },
  methods: {
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        this.searchModel.pageNo = this.pageInfo.pageNo + 1;
        const result = await api.getOfficialShopList(this.searchModel);
        this.hasResult = result.totalCount > 0;

        this.officialDownload(result.data);

        const postList = result.data.map((item) => item.postList);
        const medias = postList.flat().map((model) => <BaseInfoModel>{
          postId: model.postId,
          subId: model.coverUrl,
          coverUrl: model.coverUrl,
          postType: PostType.Official
        })
        this.baseImageInfoDownload(medias);
        this.totalPage = result.totalPage;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.pageInfo.pageNo = result.pageNo;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
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
    setOfficialImageItem(info: OfficialShopListModel) {
      let item: ImageItemModel = {
        id: info.applyId,
        subId: info.shopAvatarSource,
        class: "",
        src: info.shopAvatarSource,
        alt: "",
      };
      return item;
    },
    async search() {
      this.$route.query.keyword = this.searchModel.keyword;
      await this.reload();
    },
    async reload() {
      this.totalPage = 1;
      this.scrollStatus.list = [];
      this.pageInfo.pageNo = 0;
      await this.loadAsync();
    },
    async $_onScrollToBottom() {
      await this.loadAsync();
    },
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 200;
    },
    shopList() {
      return this.scrollStatus.list as OfficialShopListModel[] || [];
    },
    keyword() {
      return (this.$route.query.keyword as unknown as string) || "";
    },
    pageInfo() {
      return {
        pageNo: 0,
        pageSize: 5,
      } as PageParamModel;
    },
  }
});
</script>
