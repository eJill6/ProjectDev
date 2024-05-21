<template>
  <div class="game_up">
      <div class="tabs right">
        <div class="tab"  @click="navigateToRecentIssueHistory">
          <div class="tab_icon">
            <AssetImage src="@/assets/images/game/ic_record_lottery.png" />
          </div>
          <div class="tab_text" data-text="开奖记录">开奖记录</div>
        </div>
        <div class="line_wrapper">
          <div class="line champagne"></div>
          <div class="line black"></div>
        </div>
        <div class="tab"  @click="navigateToRecentOrderHistory">
          <div class="tab_icon">
            <AssetImage src="@/assets/images/game/ic_record_betting.png" />
          </div>
          <div class="tab_text" data-text="投注记录">投注记录</div>
        </div>
      </div>
      <div class="tabs left">
        <div class="tab" @click="navigateToLongDragonPlan">
          <div class="tab_icon"><AssetImage src="@/assets/images/game/ic_bell.png" /></div>
          <div class="tab_text" data-text="长龙提醒">长龙提醒</div>
        </div>
        <div class="line_wrapper">
          <div class="line champagne"></div>
          <div class="line black"></div>
        </div>
        <div class="tab" @click="navigateToFollowPlan">
          <div class="tab_icon"><AssetImage src="@/assets/images/game/ic_plan.png" /></div>
          <div class="tab_text" data-text="跟单计划">跟单计划</div>
        </div>
      </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  methods: {
    navigateToRecentOrderHistory() {
      this.$router.push({ name: "Bet_RecentOrderHistory" });
    },
    navigateToRecentIssueHistory() {
      this.$router.push({ name: "Bet_RecentIssueHistory" });
    },
    navigateToFollowPlan() {
      this.$router.push({ name: "FollowPlan" });
    },
    navigateToLongDragonPlan() {
      this.$router.push({ name: "LongDragonPlan" });
    },
  } 
});
</script>