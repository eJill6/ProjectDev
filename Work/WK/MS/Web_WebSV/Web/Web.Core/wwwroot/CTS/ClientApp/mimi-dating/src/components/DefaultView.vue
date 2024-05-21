<template>
  <!-- Header start -->
  <header class="header_height1">
    <div class="header_index">
      <div class="list">
        <ul>
          <router-link
            :to="{ name: 'Home' }"
            custom
            v-slot="{ isActive, navigate }"
          >
            <li
              :class="{ active: isActive }"
              @click="navigator(navigate, 'Home')"
            >
              <div class="text">首页</div>
              <div class="tab_bottom_line"></div>
            </li>
          </router-link>
          <router-link
            :to="{ name: 'Official' }"
            custom
            v-slot="{ isActive, navigate }"
          >
            <li
              :class="{ active: isActive }"
              @click="navigator(navigate, 'Official')"
            >
              <div class="text">官方</div>
              <div class="tab_bottom_line"></div>
            </li>
          </router-link>
          <router-link
            :to="{ name: 'Agency' }"
            custom
            v-slot="{ isActive, navigate }"
          >
            <li
              :class="{ active: isActive }"
              @click="navigator(navigate, 'Agency')"
            >
              <div class="text">寻芳阁</div>
              <div class="tab_bottom_line"></div>
            </li>
          </router-link>
          <router-link
            :to="{ name: 'Square' }"
            custom
            v-slot="{ isActive, navigate }"
          >
            <li
              :class="{ active: isActive }"
              @click="navigator(navigate, 'Square')"
            >
              <div class="text">广场</div>
              <div class="tab_bottom_line"></div>
            </li>
          </router-link>
        </ul>
      </div>
      <div class="filter" v-if="!isHome && !isOfficial">
        <div class="item" @click="showFilterInfoDialog">
          <div class="icon">
            <CdnImage
              src="@/assets/images/header/ic_header_filter.svg"
              alt=""
            />
          </div>
        </div>
        <div class="item" @click="showPositionDialog" v-if="isSquare">
          <div class="icon">
            <CdnImage
              src="@/assets/images/header/ic_header_location.svg"
              alt=""
            />
          </div>
          <div>
            {{ localInfo.name.length > 4 ? localInfo.name.slice(0, 4) + '...' : localInfo.name }}
          </div>
        </div>
      </div>
    </div>
  </header>
  <!-- Header end -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import CdnImage from "./CdnImage.vue";
import { DialogControl, VirtualScroll, ScrollManager, Tools } from "@/mixins";
import { ChinaCityInfo } from "@/models";

export default defineComponent({
  mixins: [DialogControl, VirtualScroll, ScrollManager, Tools],
  components: { CdnImage },
  methods: {
    navigator(nextNavigate: Function, routeName: string) {
      if (this.currentRouteName === routeName) return;
      this.initParameter();
      this.cleanFilterInfo();
      nextNavigate();
    },
  },
  computed: {
    localInfo(): ChinaCityInfo {
      return this.$store.state.city as ChinaCityInfo;
    },
    isHome() {
      return this.currentRouteName === "Home";
    },
    isOfficial(){
      return this.currentRouteName === "Official";
    },
    isSquare(){
      return this.currentRouteName === "Square";
    },
    currentRouteName() {
      return (this.$router.currentRoute.value.name as string) || "";
    },
  },
});
</script>
