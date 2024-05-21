<template>
  <EmptyView :message="`暂无数据～`" v-if="isEmpty"></EmptyView>
  <div class="flex_height bg_personal">
    <div class="overflow no_scrollbar" @scroll="onScroll" ref="scrollContainer">
      <div
        class="padding_order pt_0"
        :style="{
          'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
          'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
        }"
      >
        <div class="product_view">
          <ul class="full_cards">
            <li
              class="full_card"
              v-for="dataItem in postList"
              @click="postDetails(dataItem.postId)"
            >
              <div class="location_tag">
                <div>
                  <CdnImage
                    src="@/assets/images/card/ic_card_location.svg"
                    alt=""
                  />{{ cityName(dataItem.areaCode) }}
                </div>
              </div>
              <div class="card_text">
                <h1>{{ showTitle(dataItem.title) }}</h1>
                <div class="info_co">
                  <div class="info">
                    <div class="icon">
                      <CdnImage
                        src="@/assets/images/card/ic_card_body_info.svg"
                        alt=""
                      />
                    </div>
                    <div>{{ dataItem.height }}cm</div>
                    <div class="dot"></div>
                    <div>{{ dataItem.age }}岁</div>
                    <div class="dot"></div>
                    <div>{{ dataItem.cup }}杯</div>
                  </div>
                </div>
                <div class="info_co">
                  <div class="info hobby">
                    <div class="icon">
                      <CdnImage
                        src="@/assets/images/card/ic_card_hobby.svg"
                        alt=""
                      />
                    </div>
                    <div
                      v-for="n in favoriteInfo(dataItem.serviceItem)"
                      :class="{ dot: !n }"
                    >
                      {{ n }}
                    </div>
                  </div>
                </div>
                <p>职业：{{ dataItem.job }}</p>
                <p>期望收入：¥{{ dataItem.lowPrice }}</p>
                <div class="user_view">
                  <div class="num_total">
                    <ul>
                      <li>
                        <CdnImage
                          src="@/assets/images/card/ic_card_num_lock.svg"
                          alt=""
                        />{{ dataItem.unlockCount }}
                      </li>
                      <li>
                        <CdnImage
                          src="@/assets/images/card/ic_card_num_watch.svg"
                          alt=""
                        />{{ dataItem.views }}
                      </li>
                      <li @click="userCanCelFavorite(dataItem.favoriteId)">
                        <CdnImage
                          src="@/assets/images/card/ic_card_num_collect_red.svg"
                          alt=""
                        />
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
              <div class="circle_outer">
                <div class="circle_border featured"></div>
                <div class="circle_photo">
                  <AssetImage :item="setImageItem(dataItem)" />
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {
  NavigateRule,
  DialogControl,
  PlayGame,
  Tools,
  ImageCacheManager,
  VirtualScroll,
  ScrollManager,
} from "@/mixins";
import toast from "@/toast";
import { MutationType } from "@/store";
import { EmptyView, AssetImage, CdnImage } from "@/components";
import api from "@/api";
import {
  MyFavoritePostQueryParamModel,
  MyFavoritePostModel,
  ImageItemModel,
} from "@/models";

export default defineComponent({
  mixins: [
    NavigateRule,
    DialogControl,
    PlayGame,
    Tools,
    ImageCacheManager,
    VirtualScroll,
    ScrollManager,
  ],
  components: {
    EmptyView,
    AssetImage,
    CdnImage,
  },
  data() {
    return {
      isEmpty: false,
    };
  },
  methods: {
    postDetails(postId: string) {
      this.navigateToProductDetail(postId);
    },
    async loadAsync() {
      if (
        this.isLoading ||
        this.scrollStatus.totalPage === this.pageInfo.pageNo ||
        this.isPromising
      )
        return;

      this.$store.commit(MutationType.SetIsLoading, true);

      try {
        const pageSize = 10;
        const nextPage = this.pageInfo.pageNo + 1;
        const search: MyFavoritePostQueryParamModel = {
          pageNo: nextPage,
          pageSize: pageSize,
          ts: "",
          postType: 1,
        };
        const result = await api.getMyFavoritePost(search);
        this.scrollStatus.totalPage = result.totalPage;
        this.myFavoritePostDownload(result.data);
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.isEmpty = !this.scrollStatus.list.length;
        this.pageInfo.pageNo = result.pageNo;
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },

    async userCanCelFavorite(favoriteId: string) {
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        var result = await api.canCelFavorite(favoriteId);
      } catch (e) {
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
      this.postList.splice(
        this.postList.findIndex((c) => c.favoriteId === favoriteId),
        1
      );
      this.isEmpty = !this.postList.length;
    },
    showTitle(title: string) {
      if (title !== null && title !== undefined) {
        return title.length > 12 ? title.substring(0, 12) + "..." : title;
      } else {
        return "";
      }
    },
    async reload() {
      this.resetPageInfo();
      this.resetScroll();
      await this.loadAsync();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    setImageItem(info: MyFavoritePostModel) {
      let item: ImageItemModel = {
        id: info.favoriteId,
        subId: info.postId,
        class: "",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
  },
  async created() {
    await this.reload();
  },
  computed: {
    currentOrderStatus() {
      return this.$store.state.myCollectViewTypeStatus;
    },
    $_virtualScrollItemElemHeight() {
      return 155;
    },
    postList() {
      return this.scrollStatus.virtualScroll.list as MyFavoritePostModel[];
    },
  },
});
</script>
