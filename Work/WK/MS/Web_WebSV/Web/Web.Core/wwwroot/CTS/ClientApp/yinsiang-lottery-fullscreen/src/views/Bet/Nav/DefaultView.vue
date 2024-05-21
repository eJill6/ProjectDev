<template>
  <div class="lottery_main">
    <!-- Tab start -->
    <div class="lottery_section">
      <div class="lottery_tabs">
        <div
          class="lottery_tab"
          :class="{
            active: navConentStatus === NavContentType.RecentIssueHistory,
          }"
          @click="navigateToRecentIssueHistory"
        >
          <div class="lottery_tab_img">
            <AssetImage
              :src="`@/assets/images/game/ic_sheet_lottery_${
                navConentStatus === NavContentType.RecentIssueHistory
                  ? 'active'
                  : 'default'
              }.png`"
              alt=""
            />
          </div>
          <p class="lottery_tab_text">开奖记录</p>
        </div>
        <div
          class="lottery_tab tab_position"
          :class="{
            active: navConentStatus === NavContentType.RecentOrderHistory,
          }"
          @click="navigateToRecentOrderHistory"
        >
          <div class="lottery_tab_img">
            <AssetImage
              :src="`@/assets/images/game/ic_sheet_bet_${
                navConentStatus === NavContentType.RecentOrderHistory
                  ? 'active'
                  : 'default'
              }.png`"
              alt=""
            />
          </div>
          <p class="lottery_tab_text">投注记录</p>
        </div>
      </div>
    </div>
    <!-- Tab end -->
    <!-- Betting History start -->
    <div class="history_list history_height overflow no-scrollbar">
      <NavContent></NavContent>
    </div>
    <!-- Betting History end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { AssetImage } from "@/components/shared";
import NavContent from "../NavContent";
import { NavContentType } from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  components: { AssetImage, NavContent },
  data() {
    return {
      NavContentType,
    };
  },
  methods: {
    navigateToRecentOrderHistory() {
      this.$store.commit(
        MutationType.SetNavContent,
        NavContentType.RecentOrderHistory
      );
    },
    navigateToRecentIssueHistory() {
      this.$store.commit(
        MutationType.SetNavContent,
        NavContentType.RecentIssueHistory
      );
    },
  },
  computed: {
    navConentStatus() {
      return this.$store.state.navContentName;
    },
  },
});
</script>
