<template>
  <!-- 共同滑動區塊 -->
  <div class="flex_height">
    <div class="overflow no_scrollbar">
      ,
      <div class="index_padding_basic pt_0">
        <SlideBanner></SlideBanner>
        <MemberControl></MemberControl>
      </div>
      <div class="product_content">
        <div class="match_card">
          <div class="index_title card_title_pd">
            <h1>担保信息推荐</h1>
            <div class="btn" @click="goPath(PostType.Agency)">
              更多<img src="@/assets/images/index/ic_go_arrow.svg" alt="" />
            </div>
          </div>
          <div>
            <ul class="full_cards scroll-x no_scrollbar">
              <swiper @slideChange="onSlideChange" style="flex: 1">
                <swiper-slide
                  v-for="info in agencyList"
                  class="match_card_info"
                  @click="navigateToProductDetail(info.postId)"
                >
                  <li class="full_card">
                    <div class="tag">
                      <!-- <img src="@/assets/images/card/tag_premium.svg" alt="" /> -->
                    </div>
                    <div class="location_tag">
                      <div>
                        <img
                          src="@/assets/images/card/ic_card_location.svg"
                          alt=""
                        />{{ cityName(info.areaCode) }}
                      </div>
                    </div>
                    <div class="card_text">
                      <h1>{{ info.title }}</h1>
                      <div class="info_co">
                        <div class="info">
                          <div class="icon">
                            <img
                              src="@/assets/images/card/ic_card_body_info.svg"
                              alt=""
                            />
                          </div>
                          <div>{{ info.height }}cm</div>
                          <div class="dot"></div>
                          <div>{{ info.age }}岁</div>
                          <div class="dot"></div>
                          <div>{{ info.cup }}杯</div>
                        </div>
                      </div>
                      <div class="info_co">
                        <div class="info hobby">
                          <div class="icon">
                            <img
                              src="@/assets/images/card/ic_card_hobby.svg"
                              alt=""
                            />
                          </div>
                          <div
                            v-for="n in favoriteInfo(info.serviceItem)"
                            :class="{ dot: !n }"
                          >
                            {{ n }}
                          </div>
                        </div>
                      </div>
                      <p>职业：{{ info.job }}</p>
                      <p>期望收入：¥{{ info.lowPrice }}</p>
                      <div class="user_view">
                        <div class="num_total">
                          <ul>
                            <li>
                              <img
                                src="@/assets/images/card/ic_card_num_like.svg"
                                alt=""
                              />{{ info.favorites }}
                            </li>
                            <li>
                              <img
                                src="@/assets/images/card/ic_card_num_chat.svg"
                                alt=""
                              />{{ info.comments }}
                            </li>
                            <li>
                              <img
                                src="@/assets/images/card/ic_card_num_watch.svg"
                                alt=""
                              />{{ info.views }}
                            </li>
                          </ul>
                        </div>
                        <div class="num_time">{{ info.updateTime }}</div>
                      </div>
                    </div>
                    <div class="photo">
                      <AssetImage :item="setImageItem(info)" />
                      <div class="tag_ensure">
                        <img src="@/assets/images/card/tag_ensure.svg" alt="" />
                      </div>
                    </div>
                  </li>
                </swiper-slide>
              </swiper>
            </ul>
            <div class="banner_pagedot">
              <ul>
                <li
                  v-for="(n, index) in agencyList"
                  :class="{ active: index === activeIndex }"
                ></li>
              </ul>
            </div>
          </div>
        </div>

        <div>
          <div class="index_title card_title_pd">
            <h1>广场最新推荐</h1>
            <div class="btn" @click="goPath(PostType.Square)">
              更多<img src="@/assets/images/index/ic_go_arrow.svg" alt="" />
            </div>
          </div>
          <div class="card_title_pd">
            <ul
              class="full_cards"
              :style="{
                'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
                'padding-bottom':
                  scrollStatus.virtualScroll.paddingBottom + 'px',
              }"
            >
              <MainTableViewCell
                :info="info"
                :isRecommend="true"
                v-for="info in orderList"
                @click="navigateToProductDetail(info.postId)"
              ></MainTableViewCell>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {
  SlideBanner,
  MemberControl,
  MainTableViewCell,
  AssetImage,
} from "@/components";
import api from "@/api";
import { MutationType } from "@/store";
import {
  DialogControl,
  PlayGame,
  VirtualScroll,
  NavigateRule,
  ScrollManager,
  Tools,
  DecryptoManager,
} from "@/mixins";
import { ImageItemModel, ProductListModel, SearchModel } from "@/models";
import { defaultCityArea } from "@/defaultConfig";
import toast from "@/toast";
import { DecryptoSourceType, PostType } from "@/enums";
import { Swiper, SwiperSlide } from "swiper/vue";
// Import Swiper styles
import "swiper/css";

export default defineComponent({
  components: {
    SlideBanner,
    MemberControl,
    Swiper,
    SwiperSlide,
    MainTableViewCell,
    AssetImage,
  },
  mixins: [
    DialogControl,
    PlayGame,
    VirtualScroll,
    NavigateRule,
    ScrollManager,
    Tools,
    DecryptoManager,
  ],
  data() {
    return {
      totalPage: 1,
      agencyList: [] as ProductListModel[],
      activeIndex: 0,
      PostType,
      DecryptoSourceType,
    };
  },
  methods: {
    goPath(postType: PostType) {
      this.initParameter();
      if (postType === PostType.Square) {
        this.navigateToSquare();
      } else if (postType === PostType.Agency) {
        this.navigateToAgency();
      }
    },
    onSlideChange(swiper: { activeIndex: number }) {
      this.activeIndex = swiper.activeIndex;
    },
    async getBanners() {
      const bannerList = await api.getBanner();
      for (let banner of bannerList) {
        banner.fullMediaUrl = await this.fetchSingleDownload(
          banner.fullMediaUrl
        );
      }
      this.$store.commit(MutationType.SetHomeBanner, bannerList);
    },
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;
      try {
        this.$store.commit(MutationType.SetIsLoading, true);

        const city = defaultCityArea[0];
        const nextPage = this.pageInfo.pageNo + 1;
        const search: SearchModel = {
          page: nextPage,
          postType: PostType.Square,
          areaCode: city.code,
          labelIds: [],
          isRecommend: true,
          age: [],
          height: [],
          cup: [],
          price: [],
          serviceIds: [],
          ts: this.pageInfo.ts,
        };
        const result = await api.getProductList(search);

        this.pageInfo.pageNo = result.pageNo;
        this.totalPage = result.totalPage;
        this.pageInfo.ts = result.ts;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
      } catch (error) {
        console.error(error);
        toast(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    async loadAgencyAsync() {
      const city = defaultCityArea[0];
      const search: SearchModel = {
        page: 1,
        pageSize: 5,
        postType: PostType.Agency,
        areaCode: city.code,
        labelIds: [],
        isRecommend: true,
        age: [],
        height: [],
        cup: [],
        price: [],
        serviceIds: [],
        ts: "",
      };
      const result = await api.getProductList(search);
      this.agencyList = result.data;
    },
    async reload() {
      this.pageInfo.pageNo = 0;
      this.totalPage = 1;
      this.pageInfo.ts = "";
      this.resetScroll();
      await this.loadAsync();
    },
    $_onScrollReload() {
      this.reload();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    showAnnouncement() {
      if (this.announcementStatus) {
        this.showAnnouncementDialog(() => {
          this.$store.commit(MutationType.SetAnnouncement, false);
        });
      }
    },
    async adminContact() {
      const result = await this.getAdminContact();
      this.$store.commit(MutationType.SetAdminContact, result);
    },
    setImageItem(info: ProductListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        sourceType: DecryptoSourceType.None,
        class: "loading-image",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
  },
  async created() {
    await this.getBanners();
    await this.adminContact();
    await this.setUserInfo();
    await this.reload();
    await this.loadAgencyAsync();
    this.showAnnouncement();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 155;
    },
    announcementStatus() {
      return this.$store.state.showAnnouncement;
    },
    orderList() {
      return this.scrollStatus.list as ProductListModel[];
    },
  },
});
</script>
