<template>
  <!-- slide banner -->
  <div class="slide_banner">
    <div class="slide_img" :class="{ official: isOfficial }">
      <div v-if="isHome || isOfficial">
        <div class="img_cover" style="z-index: 2"></div>
        <swiper
          :modules="modules"
          :loop="true"
          :autoplay="{
            delay: bannerDelay,
            disableOnInteraction: false,
          }"
          :reverseDirection="reverseDirection"
          @slideChange="onSlideChange"
          @reach-beginning="onReachBeginning"
          @reach-end="onReachEnd"
        >
          <swiper-slide v-for="n in bannerList" @click="clickEvent(n)">
            <BannerImage
              :item="setImageItem(n)"
              :isHome="isHome || isOfficial"
            ></BannerImage>
          </swiper-slide>
        </swiper>
      </div>
      <swiper
        v-else
        :modules="modules"
        :autoplay="{
          delay: bannerDelay,
          disableOnInteraction: false,
        }"
        @slideChange="onSlideChange"
      >
        <swiper-slide v-for="n in bannerList" @click="clickEvent(n)">
          <BannerImage
            :item="setImageItem(n)"
            :isHome="isHome"
            v-if="n.mediaType === MediaType.Image"
          ></BannerImage>
          <div class="video_container" v-else>
            <AssetVideo
              :item="setVideoItem(n)"
              :showMediaControl="true"
              :bottomHight="33"
            ></AssetVideo>
          </div>
        </swiper-slide>
      </swiper>
      <div class="img_cover" style="z-index: 2" v-if="!isHome"></div>
    </div>

    <div class="banner_pagedot" style="z-index: 2">
      <ul>
        <li
          v-for="(n, index) in bannerList"
          :class="{ active: index === activeIndex }"
        ></li>
      </ul>
    </div>
  </div>
  <!-- slide banner End -->
</template>
<script lang="ts">
import { defineComponent } from "vue";

// Import Swiper Vue.js components
import { Swiper, SwiperSlide } from "swiper/vue";
// Import Swiper styles
import "swiper/css";
// import required modules
import { Autoplay, Navigation } from "swiper";
import { BannerModel, ImageItemModel, VideoItemModel } from "@/models";
import global from "@/global";
import { IntroductionType, LinkType, MediaType } from "@/enums";
import { PlayGame, Tools, NavigateRule, GaReport } from "@/mixins";
import BannerImage from "./BannerImage.vue";
import videojs from "video.js";
import { hlsKey } from "@/defaultConfig";
import AssetVideo from "./AssetVideo.vue";
import { MutationType } from "@/store/mutations";

export default defineComponent({
  components: {
    Swiper,
    SwiperSlide,
    BannerImage,
    AssetVideo,
  },
  mixins: [PlayGame, Tools, NavigateRule, GaReport],
  data() {
    return {
      activeIndex: 0,
      MediaType,
      reverseDirection: false,
    };
  },
  setup() {
    return {
      modules: [Autoplay, Navigation],
    };
  },
  methods: {
    setImageItem(info: BannerModel) {
      let item: ImageItemModel = {
        id: info.id,
        class: "",
        src: info.fullMediaUrl,
        alt: "",
        subId: info.fullMediaUrl,
      };
      return item;
    },
    setVideoItem(info: BannerModel) {
      let item: VideoItemModel = {
        id: info.id,
        class: "video_w_100",
        src: info.fullMediaUrl,
        alt: "",
        isCloud: true,
        coverUrl: info.coverUrl || "",
      };
      return item;
    },
    clickEvent(item: BannerModel) {
      if (!item.redirectUrl) return;
      const imageId = this.getImageID(item.fullMediaUrl);
      if (this.isHome) {
        this.setGaEventName(`home_banner_click_${imageId}`);
      } else if (this.isOfficial) {
        this.setGaEventName(`store_banner_click_${imageId}`);
      }

      if (item.linkType === LinkType.RedirectUrl) {
        global.openUrlBlank(item.redirectUrl);
      } else if (item.linkType === LinkType.ClientWebPageVaule) {
        if (item.redirectUrl.toLocaleLowerCase() === "deposit") {
          this.goDepositUrl();
        } else if (item.redirectUrl.toLocaleLowerCase() === "withdraw") {
          this.goWithdrawUrl();
        } else if (item.redirectUrl.toLocaleLowerCase() === "exchange") {
          this.goExchangeUrl();
        } else if (item.redirectUrl.toLocaleLowerCase() === "exchangerecord") {
          this.goExchangerecordUrl();
        } else if (item.redirectUrl.toLocaleLowerCase() === "dwReport") {
          this.goDwReportUrl();
        } else if (item.redirectUrl.toLocaleLowerCase() === "bindPhone") {
          this.goBindPhoneUrl();
        } else if (item.query) {
          this.$router.push({ name: item.redirectUrl, query: item.query });
        } else {
          if (item.redirectUrl.toLocaleLowerCase() === "bossapply") {
            this.$store.commit(
              MutationType.SetPublishName,
              IntroductionType.Boss
            );
            this.navigateToIntroduction();
          } else if (item.redirectUrl.toLocaleLowerCase() === "agentapply") {
            this.$store.commit(
              MutationType.SetPublishName,
              IntroductionType.Agent
            );
            this.navigateToIntroduction();
          } else {
            this.$router.push({ name: item.redirectUrl });
          }
        }
      }
    },
    onSlideChange(swiper: { realIndex: number }) {
      this.activeIndex = swiper.realIndex;
    },
    onReachEnd() {
      this.reverseDirection = true;
    },
    onReachBeginning() {
      this.reverseDirection = false;
    },
    setVideoHeader() {
      videojs.Vhs.xhr.beforeRequest = function (options: {
        headers: { [x: string]: string };
      }) {
        options.headers = options.headers || {};
        options.headers["x-token"] = hlsKey;
        return options;
      };
    },
  },
  computed: {
    isHome() {
      const currentRouteName = this.$router.currentRoute.value.name;
      return currentRouteName === "Home";
    },
    isOfficial() {
      const currentRouteName = this.$router.currentRoute.value.name;
      return currentRouteName === "Official";
    },
    bannerList() {
      return this.isHome
        ? this.$store.state.homeBannerList
        : this.$store.state.bannerList;
    },
    logonMode() {
      return this.$store.state.logonMode;
    },
    bannerDelay() {
      return this.isHome || this.isOfficial ? 5000 : 3600000;
    },
  },
});
</script>
