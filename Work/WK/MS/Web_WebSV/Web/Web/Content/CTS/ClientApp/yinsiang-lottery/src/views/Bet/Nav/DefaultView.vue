<template>
  <div class="d-flex align-items-center ">
    <div class="text-white fs-2 d-flex bet-menu-listbtn mr-4" @click="navigateToRecentIssueHistory">
      <div>
        <AssetImage src="@/assets/images/ic_menu_openlist.svg" alt="" />
      </div>
      开奖记录
    </div>
    <div class="text-white d-flex bet-menu-listbtn mr-4" @click="navigateToRecentOrderHistory">
      <div><AssetImage src="@/assets/images/ic_menu_betlist.svg" alt="" /></div>
      投注记录
    </div>
    <div class="text-white d-flex bet-menu-listbtn" @click="navigateToDescription">
      <div>
        <AssetImage src="@/assets/images/ic_menu_tutorial.svg" alt="" />
      </div>
      玩法
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  methods: {
    navigateToDescription() {
      this.$router.push({ name: "Description" });
    },
    navigateToRecentOrderHistory() {
      this.$router.push({ name: "Bet_RecentOrderHistory" });
    },
    navigateToRecentIssueHistory() {
      this.$router.push({ name: "Bet_RecentIssueHistory" });
    },
  } 
});
</script>