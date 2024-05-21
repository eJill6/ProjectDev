<template>
    <div class="d-flex align-items-center">
      <div class="text-white  fs-2 d-flex  bet-menu-tabbtn-active">
        <div>
          <AssetImage src="@/assets/images/ic_menu_openlist.svg" alt="" />
        </div>
        开奖记录
      </div>
      <div class="text-white d-flex bet-menu-tabbtn " @click="navigateToRecentOrderHistory">
        <div>
          <AssetImage src="@/assets/images/ic_menu_betlist.svg" alt="" />
        </div>
        投注记录
      </div>
    </div>
    <div class="d-flex">
      <div class="text-white fs-2 d-flex bet-morelist-btn" @click="navigateToIssueHistory">
        <div>更多开奖</div>
        <AssetImage src="@/assets/images/ic_bet_more.svg" alt="" />
      </div>
      <div class="ml-2" @click="navigateToBet">
        <AssetImage src="@/assets/images/ic_bet_more_closebtn.svg" alt="" />
      </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    navigateToRecentOrderHistory() {
      this.$router.push({ name: "Bet_RecentOrderHistory" });
    },
    navigateToIssueHistory() {
      this.$router.push({ name: "IssueHistory" });
    },
  },
});
</script>