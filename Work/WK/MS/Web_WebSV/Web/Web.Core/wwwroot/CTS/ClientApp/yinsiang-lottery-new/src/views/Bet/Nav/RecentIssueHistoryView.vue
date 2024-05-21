<template>
  <div class="game_up end">
    <div class="header_second" :class="{ end: isAddEndClass}">
      <div class="left">
        <div class="logo_second" :class="logoType">
          <AssetImage :src="`@/assets/images/record/logo_${logoType}_second.png`" />
        </div>
        <IssueNo :isSM="true"></IssueNo>
      </div>
      <div class="right">
        <div class="tab active">
          <div class="tab_icon">
            <AssetImage src="@/assets/images/game/ic_record_lottery.png" />
          </div>
          <div class="tab_text" data-text="开奖记录">开奖记录</div>
        </div>
        <div class="tab default" @click="navigateToRecentOrderHistory">
          <div class="tab_icon">
            <AssetImage src="@/assets/images/game/ic_record_betting.png" />
          </div>
          <div class="tab_text" data-text="投注记录">投注记录</div>
        </div>
        <div class="btn" @click="navigateToIssueHistory">
          <AssetImage src="@/assets/images/record/ic_more_lottery.png" />
        </div>
        <div class="btn" @click="navigateToBet">
          <AssetImage src="@/assets/images/record/ic_close_record.png" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "@/components/shared/AssetImage.vue";
import { IssueNo } from "@/components";

export default defineComponent({
  components: { AssetImage, IssueNo },
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
  computed: {
    logoType() {
      let typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      let iconImg = "";
      if (typeName){
        iconImg = typeName.toLocaleLowerCase();
        switch(iconImg){
          case "lp":
            iconImg = "roulette";
            break;           
          case "yxx":
            iconImg = "yusiasieh";
            break;      
        }
      }
      return iconImg;
    },
    isAddEndClass(): boolean{
      let typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return typeName.toLocaleLowerCase() !== 'baccarat';
    }
  },
});
</script>
