<template>
  <!-- slide banner -->
  <div class="slide_banner">
    <div class="slide_img">
      <swiper
        :modules="modules"
        :autoplay="{
          delay: bannerDelay,
          disableOnInteraction: false,
        }"
        @slideChange="onSlideChange"
      >
        <swiper-slide v-for="n in bannerList" @click="clickEvent(n)">
          <div class="cover_pic_outter">
            <img class="cover_pic" :src="n.fullMediaUrl" alt="" />
          </div>
        </swiper-slide>
      </swiper>
    </div>
    <div class="img_cover"></div>
    <div class="banner_pagedot">
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
import { BannerModel } from "@/models";
import global from "@/global";
import { LinkType } from "@/enums";
import { PlayGame } from "@/mixins";

export default defineComponent({
  components: {
    Swiper,
    SwiperSlide,
  },
  mixins: [PlayGame],
  data() {
    return {
      activeIndex: 0,
      bannerDelay: 5000,
    };
  },
  setup() {
    return {
      modules: [Autoplay, Navigation],
    };
  },
  methods: {
    clickEvent(item: BannerModel) {
      if (!item.redirectUrl) return;
      if (item.linkType === LinkType.RedirectUrl) {
        global.openUrl(this.logonMode, item.redirectUrl, null);
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
        } else {
          this.$router.push({ name: item.redirectUrl });
        }
      }
    },
    onSlideChange(swiper: { activeIndex: number }) {
      this.activeIndex = swiper.activeIndex;
    },
  },
  computed: {
    bannerList() {
      const currentRouteName = this.$router.currentRoute.value.name;
      return currentRouteName === "Home"
        ? this.$store.state.homeBannerList
        : this.$store.state.bannerList;
    },
    logonMode() {
      return this.$store.state.logonMode;
    },
  },
});
</script>
