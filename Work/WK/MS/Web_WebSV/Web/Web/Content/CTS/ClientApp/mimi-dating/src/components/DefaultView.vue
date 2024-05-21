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

          <router-link
            :to="{ name: 'Agency' }"
            custom
            v-slot="{ isActive, navigate }"
          >
            <li
              :class="{ active: isActive }"
              @click="navigator(navigate, 'Agency')" 
            >
              <div class="text">担保</div>
              <div class="tab_bottom_line"></div>
            </li>
          </router-link>

          <li @click="showComingSoon">
            <div class="text">官方</div>
            <div class="tab_bottom_line"></div>
          </li>
          <li @click="showComingSoon">
            <div class="text">体验</div>
            <div class="tab_bottom_line"></div>
          </li>
        </ul>
      </div>
      <div class="filter" v-if="!isHome">
        <div class="item" @click="showFilterInfoDialog">
          <div class="icon">
            <img src="@/assets/images/header/ic_header_filter.svg" alt="" />
          </div>
        </div>
        <div class="item" @click="showPositionDialog">
          <div class="icon">
            <img src="@/assets/images/header/ic_header_location.svg" alt="" />
          </div>
          <div>{{ localInfo.name }}</div>
        </div>
      </div>
    </div>
  </header>
  <!-- Header end -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { DialogControl, VirtualScroll, ScrollManager } from "@/mixins";
import { ChinaCityInfo } from "@/models";

export default defineComponent({
  mixins: [DialogControl, VirtualScroll, ScrollManager],
  
  methods: {
    navigator(nextNavigate: Function, routeName: string) {
      if (this.currentRouteName === routeName) return;
      this.initParameter();
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
    currentRouteName() {
      return (this.$router.currentRoute.value.name as string) || "";
    },
  },
});
</script>
