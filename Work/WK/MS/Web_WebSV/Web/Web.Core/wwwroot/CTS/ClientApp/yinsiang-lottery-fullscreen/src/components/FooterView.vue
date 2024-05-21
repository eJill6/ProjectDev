<template>
  <footer :class="{ android: setAndroidStyle() }">
    <div class="footer_nav">
      <div
        class="footer_item"
        :class="{ focus: setFocus('Bet') }"
        @click="navigateToPath('Bet')"
      >
        <div class="ic_ft">
          <AssetImage
            :src="`@/assets/images/footer/ic_home_${setIcon('Bet')}`"
            alt=""
          />
        </div>
        <p>首页</p>
      </div>
      <div
        class="footer_item"
        :class="{ focus: setFocus('OrderHistory') }"
        @click="navigateToPath('OrderHistory')"
      >
        <div class="ic_ft">
          <AssetImage
            :src="`@/assets/images/footer/ic_betting_${setIcon(
              'OrderHistory'
            )}`"
            alt=""
          />
        </div>
        <p>投注记录</p>
      </div>
      <div
        class="footer_item"
        :class="{ focus: setFocus('IssueHistory') }"
        @click="navigateToPath('IssueHistory')"
      >
        <div class="ic_ft">
          <AssetImage
            :src="`@/assets/images/footer/ic_lottery_${setIcon(
              'IssueHistory'
            )}`"
            alt=""
          />
        </div>
        <p>开奖记录</p>
      </div>
      <div
        class="footer_item"
        :class="{ focus: setFocus('Description') }"
        @click="navigateToPath('Description')"
      >
        <div class="ic_ft">
          <AssetImage
            :src="`@/assets/images/footer/ic_illustrate_${setIcon(
              'Description'
            )}`"
            alt=""
          />
        </div>
        <p>玩法说明</p>
      </div>
      <div
        class="footer_item"
        :class="{ focus: setFocus('FollowPlan') }"
        @click="navigateToPath('FollowPlan')"
      >
        <div class="ic_ft">
          <AssetImage
            :src="`@/assets/images/footer/ic_plan_${setIcon('FollowPlan')}`"
            alt=""
          />
        </div>
        <p>跟单计划</p>
      </div>
    </div>
  </footer>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "./shared/AssetImage.vue";
import { isAndroid } from "@/gameConfig";

export default defineComponent({
  components: { AssetImage },
  methods: {
    setAndroidStyle() {
      return isAndroid;
    },
    setIcon(name: string) {
      return this.pathName === name ? "active.png" : "default.svg";
    },
    setFocus(name: string) {
      return this.pathName === name;
    },
    navigateToPath(name: string) {
      let rootContainer = document.getElementById(
        "client-app-lottery"
      ) as HTMLElement;
      rootContainer.className =
        name === "Bet" || name === "Home"
          ? "main_container bg_main"
          : "main_container bg_second";

      this.$router.push({ name: name });
    },
  },
  computed: {
    pathName() {
      return this.$router.currentRoute.value.name;
    },
  },
});
</script>
