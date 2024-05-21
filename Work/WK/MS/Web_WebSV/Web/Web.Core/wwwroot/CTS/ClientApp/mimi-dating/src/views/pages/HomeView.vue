<template>
  <!-- 共同滑動區塊 -->
  <div class="flex_height">
    <div class="overflow no_scrollbar">
      <div class="main_shortcut">
        <div class="shortcut_outter" v-for="items in sortedShortcutList">
          <div
            class="shortcut_item"
            v-for="item in items"
            @click="goUrl(item.redirectUrl, item.linkType)"
          >
            <div class="shortcut_img">
              <AssetImage :item="setShortcutImageItem(item)" />
            </div>
            <div class="shortcut_text">{{ item.title }}</div>
            <div
              v-if="
                item.redirectUrl.toLocaleLowerCase() === 'private' &&
                hasUnreadMessage
              "
              class="dot"
            ></div>
          </div>
        </div>
      </div>
      <div class="loudspeaker_outer" v-if="hasNotifycation">
        <div class="loudspeaker_img">
          <CdnImage src="@/assets/images/index/ic_loudspeaker.png" alt="" />
        </div>
        <div class="loudspeaker_inner">
          <div style="padding-left: 100%" class="marquee_animation">
            <div class="loudspeaker_text">
              {{ announcementContent }}
            </div>
          </div>
        </div>
      </div>
      <MemberControl></MemberControl>
      <SlideBanner></SlideBanner>
      <RecommendProducts></RecommendProducts>
      <!-- 广场交友推荐 -->
      <div>
        <div class="index_title card_title_pd">
          <div class="index_outer">
            <div class="index_title_img">
              <CdnImage src="@/assets/images/index/ic_square_friend.png" />
            </div>
            <h1>广场交友推荐</h1>
          </div>
          <div class="btn" @click="goPath(PostType.Square)">
            更多
            <CdnImage src="@/assets/images/index/ic_go_arrow.svg" alt="" />
          </div>
        </div>
        <div class="reminder_outer">
          <div class="reminder_gif">
            <CdnImage src="@/assets/images/index/gif_newcomer.gif" />
          </div>
          <div class="reminder_text">
            广场区发布者没有向平台支付寻芳阁金,新手消费请先看防骗指南，谨记
            ‘先服务后付钱’ ，线下被骗平台概不负责！
          </div>
        </div>
        <div class="card_title_pd">
          <ul
            class="full_cards"
            :style="{
              'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
              'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
            }"
          >
            <MainTableViewCell
              :info="info"
              v-for="info in postList"
              @click="navigateToProductDetail(info.postId)"
            >
            </MainTableViewCell>
          </ul>
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
  RecommendProducts,
  MainTableViewCell,
  MainCollectionViewCell,
} from "@/components";
import api from "@/api";
import { MutationType } from "@/store";
import { CdnImage, AssetImage } from "@/components";
import {
  DialogControl,
  PlayGame,
  VirtualScroll,
  NavigateRule,
  ScrollManager,
  Tools,
  ImageCacheManager,
  MqEvent,
} from "@/mixins";
import {
  ImageItemModel,
  BannerModel,
  ProductListModel,
  OfficialShopModel,
  HomeStatusInfoModel,
  event as eventModel,
  LabelFilterModel,
  SearchModel,
  PaginationModel,
  PostPaginationModel,
} from "@/models";
import toast from "@/toast";
import {
  PostType,
  LinkType,
  IntroductionType,
  MqChatNotificationType,
} from "@/enums";
import { Swiper, SwiperSlide } from "swiper/vue";
import global from "@/global";

// Import Swiper styles
import "swiper/css";

export default defineComponent({
  components: {
    SlideBanner,
    MemberControl,
    RecommendProducts,
    Swiper,
    SwiperSlide,
    MainTableViewCell,
    MainCollectionViewCell,
    CdnImage,
    AssetImage,
  },
  mixins: [
    DialogControl,
    PlayGame,
    VirtualScroll,
    NavigateRule,
    ScrollManager,
    Tools,
    ImageCacheManager,
    MqEvent,
  ],
  data() {
    return {
      PostType,
      rowMaxCount: 3,
      hasNotifycation: false,
      announcementContent: "",
      rowShortcutMaxCount: 4,
      hasUnreadMessage: false,
      shortcutList: [] as BannerModel[],
    };
  },
  methods: {
    goPath(postType: PostType) {
      this.initParameter();
      if (postType === PostType.Square) {
        this.navigateToSquare();
      } else if (postType === PostType.Agency) {
        this.navigateToAgency();
      } else if (postType === PostType.Official) {
        this.navigateToOfficial();
      }
    },
    async getBanners() {
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        const bannerList = (await api.getBanner()).filter(
          (x) => x.locationType == 2
        );
        bannerList.forEach((item) => (item.id = "banner"));
        this.bannerDownload(bannerList);
        this.$store.commit(MutationType.SetHomeBanner, bannerList);
      } catch (e) {
        toast(e);
      }
      this.$store.commit(MutationType.SetIsLoading, false);
    },
    async getContent() {
      try {
        const result = await api.getFrontsideAnnouncement();
        if (result) {
          this.hasNotifycation = true;
          this.announcementContent = result.homeContent;
        }
      } catch (e) {}
    },
    // 快捷入口
    async loadShortcutList() {
      // if(this.isPromising){
      //   return;
      // }
      this.shortcutList = await api.getShortcutList();
      if (
        this.shortcutList &&
        this.shortcutList.some(
          (x) => x.redirectUrl.toLocaleLowerCase() === "private"
        )
      ) {
        this.hasUnreadMessage = await api.hasUnreadMessage();
      }

      this.bannerDownload(this.shortcutList);
      this.homeStatusInfo.homeShortcutList = this.shortcutList;
      this.$store.commit(MutationType.SetHomeStatus, this.homeStatusInfo);
    },
    // 帖子
    async loadAsync() {
      // if(this.isPromising){
      //   return;
      // }
      const result = await api.getHomeRecommendPostList();
      result.forEach((x) => {
        if (x.serviceItem.length > 3) {
          x.serviceItem = x.serviceItem.slice(0, 3);
        }
        if (x.title.length > 12) {
          x.title = x.title.slice(0, 12) + "...";
        }
      });

      this.homeStatusInfo.homeAgencyList = result
        .filter((x) => x.postType === PostType.Agency)
        .sort((a, b) => b.weight - a.weight);
      this.homeStatusInfo.homeSquareList = result
        .filter((x) => x.postType === PostType.Square)
        .sort((a, b) => b.weight - a.weight);
      let favoritePosts = this.$store.state.favoritePosts;
      let posts = result.filter((x) => x.isFavorite);
      if (posts.length) {
        posts.forEach((x) => {
          if (favoritePosts.indexOf(x.postId) < 0) {
            favoritePosts.push(x.postId);
          }
        });
      }
      this.$store.commit(MutationType.SetFavoritePosts, favoritePosts);
      this.postDownload(result);
      this.$store.commit(MutationType.SetHomeStatus, this.homeStatusInfo);
      this.$store.commit(MutationType.SetIsLoading, false);
    },
    async loadOfficial() {
      // if(this.isPromising){
      //   return;
      // }
      const shopList = await api.getHomeOfficialShopList();
      this.officialDownload(shopList);
      this.homeStatusInfo.homeOfficialList = shopList;
      this.$store.commit(MutationType.SetHomeStatus, this.homeStatusInfo);
    },
    async showAnnouncement() {
      try {
        const result = await api.getGetHomeAnnouncement();
        if (result && result.length) {
          if (this.popupContent == "") {
            this.$store.commit(MutationType.SetAnnouncement, true);
          }
          this.$store.commit(
            MutationType.SetAnnouncementContent,
            result[0].homeContent
          );
          if (this.announcementStatus) {
            this.showAnnouncementDialog(() => {
              this.$store.commit(MutationType.SetAnnouncement, false);
            });
          }
        } else {
          this.$store.commit(MutationType.SetAnnouncement, false);
        }
      } catch (e) {}
    },
    goUrl(redirectUrl: string, linktype: LinkType) {
      if (linktype === LinkType.ClientWebPageVaule) {
        if (redirectUrl.toLocaleLowerCase() === "bossapply") {
          this.$store.commit(
            MutationType.SetPublishName,
            IntroductionType.Boss
          );
          this.navigateToIntroduction();
        } else if (redirectUrl.toLocaleLowerCase() === "agentapply") {
          this.$store.commit(
            MutationType.SetPublishName,
            IntroductionType.Agent
          );
          this.navigateToIntroduction();
        } else {
          this.$router.push(redirectUrl);
        }
      } else if (linktype === LinkType.RedirectUrl) {
        global.openUrl(this.logonMode, redirectUrl, null);
      } else {
        this.$router.push(redirectUrl);
      }
    },
    async adminContact() {
      const result = await this.getAdminContact();
      this.$store.commit(MutationType.SetAdminContact, result);
    },
    async onReceiveMsg(arg: eventModel.ReceiveMsgArg) {
      if (arg.ChatNotificationType === MqChatNotificationType.NewMessage) {
        this.hasUnreadMessage = true;
      }
    },
    async onDeleteRoom(arg: eventModel.DeleteRoomArg) {
      if (
        this.shortcutList &&
        arg.ChatNotificationType === MqChatNotificationType.DeleteChat &&
        this.shortcutList.some(
          (x) => x.redirectUrl.toLocaleLowerCase() === "private"
        )
      ) {
        this.hasUnreadMessage = await api.hasUnreadMessage();
      }
    },
    setImageItem(info: ProductListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.coverUrl,
        class: "",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
    setShortcutImageItem(info: BannerModel) {
      let item: ImageItemModel = {
        id: info.id,
        subId: info.fullMediaUrl ?? "",
        class: "",
        src: info.fullMediaUrl ?? "",
        alt: "",
      };
      return item;
    },
    searchConditionModel(
      nextPage: Number,
      filter: LabelFilterModel,
      ts: string,
      type: PostType,
      size: number
    ): SearchModel {
      return {
        pageNo: nextPage,
        pageSize: size,
        areaCode: this.localInfo?.code as string,
        labelIds: [],
        postType: type,
        messageId: filter.messageId,
        lockStatus: filter.statusType,
        sortType: filter.sortType,
        age: this.ageArray(),
        height: this.heightArray(),
        cup: this.cupArray(),
        price: this.priceArray(),
        serviceIds: this.serviceIdsArray(),
        ts: ts,
      } as SearchModel;
    },
    async loadPostCover(type: PostType) {
      let result = {} as PostPaginationModel<any>;
      const search = this.searchConditionModel(
        1,
        this.filter,
        this.pageInfo.ts,
        type,
        20
      );

      const productModel = await api.getProductList(search);
      result = productModel;
      this.baseImageInfoDownload(result.data);
      this.nextPagePostCoverDownload(result?.nextPagePost);
    },
  },
  async created() {
    this.$store.commit(MutationType.SetPostType, PostType.None);
    await this.showAnnouncement();
    await this.getBanners();
    await this.getContent();
    await this.loadShortcutList();
    await this.loadOfficial();
    await this.loadAsync();
    await this.setUserInfo();
    await this.loadPostCover(PostType.Agency);
    await this.loadPostCover(PostType.Square);
    this.adminContact();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 155;
    },
    announcementStatus() {
      return this.$store.state.showAnnouncement;
    },
    popupContent() {
      return this.$store.state.announcementContent;
    },
    homeStatusInfo() {
      const result =
        this.$store.state.homeStatusInfo ||
        ({
          scrollTop: 0,
          homeSquareList: [] as ProductListModel[],
          homeAgencyList: [] as ProductListModel[],
          homeOfficialList: [] as OfficialShopModel[],
          homeShortcutList: [] as BannerModel[],
        } as HomeStatusInfoModel);

      return result;
    },
    postList() {
      return this.homeStatusInfo.homeSquareList as ProductListModel[];
    },
    sortedShortcutList() {
      let outterArray: BannerModel[][] = [];
      const foreachCount = Math.ceil(
        this.shortcutList.length / this.rowShortcutMaxCount
      );

      for (let i = 0; i < foreachCount; i++) {
        const startIndex = i * this.rowShortcutMaxCount;
        const newArray = this.shortcutList.slice(
          startIndex,
          startIndex + this.rowShortcutMaxCount
        );
        outterArray.push(newArray);
      }
      return outterArray;
    },
  },
});
</script>
<style scoped>
.marquee_animation {
  -webkit-animation: marquee 12s linear infinite;
  -moz-animation: marquee 12s linear infinite;
  -o-animation: marquee 12s linear infinite;
  animation: marquee 12s linear infinite;
}
@keyframes marquee {
  0% {
    transform: translateX(0);
  }
  100% {
    transform: translateX(-100%);
  }
}
</style>
