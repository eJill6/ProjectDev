<template>
  <div class="game_wrapper">
    <div :class="isYXX ? 'play_rule1' : 'play_rule'" @click="navigateToDescription" v-if="!isDisable">
      <div class="icon">
        <AssetImage v-if="isYXX" src="@/assets/images/game/ic_play_rule1.png" />
        <AssetImage v-else src="@/assets/images/game/ic_play_rule.png" />        
      </div>
      <div class="rule" v-if="!isYXX">
        <div class="rule_title" data-text="玩法">玩法</div>
      </div>
    </div>
    <div class="top_line"></div>
    <router-view name="nav"></router-view>
    <template v-if="isDisable">
      <div class="line_record_lottery" v-if="isIssueView">
        <AssetImage src="@/assets/images/record/record_line_lottery.png" />
      </div>
      <div class="line_record_betting" v-else>
        <AssetImage src="@/assets/images/record/record_line_betting.png" />
      </div>      
      <router-view name="navContent"></router-view>
      <div class="line_record">
        <AssetImage src="@/assets/images/record/record_line.png" />
      </div>
    </template>
    <WinNumbers v-else></WinNumbers>

    <div class="game_down overflow no-scrollbar">
      <div class="betting_tabs">
        <PlayTypes></PlayTypes>
      </div>
      <PlayTypeRadios></PlayTypeRadios>
    </div>
    <div class="betting_outter">
      <div class="betting_inner">
        <div class="betting_content">
          <div class="betting_content_inner">
            <div class="betting_money">
              <AssetImage src="@/assets/images/game/ic_gold_spade.png" />
            </div>
            <div class="betting_item">
              <div class="betting_text">{{ formattedBalance }}</div>
              <div class="betting_recharge" @click="goDepositUrl">
                <AssetImage src="@/assets/images/game/ic_recharge.png" />
              </div>
            </div>
          </div>
        </div>
        <div class="betting_refresh" @click="refreshBalance">
          <AssetImage src="@/assets/images/game/ic_refresh.png" />
        </div>
      </div>
      <div class="betting_inner">
        <div class="betting_chips" @click="navigateToBaseAmountSelection">
          <div class="chip" :class="baseAmountIconClassName">
            <div class="num" :data-text="baseAmount">{{ baseAmount }}</div>
          </div>
          <div class="betting_arrow">
            <AssetImage src="@/assets/images/game/ic_arrow_chip.png" />
          </div>
        </div>
        <div class="bettingbtn" @click="navigateToConfirmBet">
          <AssetImage src="@/assets/images/game/btn_betting.png" />
        </div>
      </div>
    </div>
    <router-view name="dialog"></router-view>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import {
  WinNumbers,
  PlayTypeRadios,
  PlayTypes,
  ChangLongFollow,
} from "@/components";
import { MsSetting, LotteryMenuInfo } from "@/models";
import { BaseAmount, Balance } from "@/mixins";
import toast from "@/toast";
import AssetImage from "@/components/shared/AssetImage.vue";
import mesage from "@/message";
import { defaultLotteryInfo } from "@/gameConfig";

export default defineComponent({
  components: {
    WinNumbers,
    PlayTypes,
    PlayTypeRadios,
    AssetImage,
    ChangLongFollow,
  },
  data() {
    return {
      isSelectMode: false,
      menuIconCount: 4,
    };
  },
  mixins: [BaseAmount, Balance],
  watch: {},
  methods: {
    navigateToDescription() {
      this.$router.push({ name: "Description" });
    },
    navigateToBaseAmountSelection() {
      this.$router.push({ name: "Bet_BaseAmountSelection" });
    },
    navigateToConfirmBet() {
      if (!this.betCount) {
        toast("当前没有投注型态，请先下注");
        return;
      }

      this.$router.push({ name: "ConfirmBet" });
    },
    refreshBalance() {
      this.refreshBalanceAsync();
      toast(`余额刷新成功`);
    },
    goDepositUrl() {
      mesage.openUrl(this.msSetting.logonMode, this.msSetting.depositUrl, null);
    },
    selectGameView(switchMode: boolean = false) {
      this.isSelectMode = switchMode;
    },
  },
  computed: {
    isYXX() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return typeName.toLocaleLowerCase() === "yxx";    
    },
    betCount() {
      return this.$store.getters.betCount;
    },
    msSetting(): MsSetting {
      return this.$store.state.msSetting;
    },
    LotteryMenu(): Array<LotteryMenuInfo> {
      let list = this.$store.state.lotteryMenuInfo;
      list = list.filter((x) => x.lotteryID < 999000);
      const count = list.length;
      for (let index = 0; index < this.menuIconCount - count; index++) {
        list.push(defaultLotteryInfo);
      }
      return list;
    },
    isIssueView(): boolean {
      let routeName = (this.$router.currentRoute?.value?.name as string) ?? "";
      return routeName === "Bet_RecentIssueHistory";
    },
    isDisable(): boolean {
      let routeName = (this.$router.currentRoute?.value?.name as string) ?? "";      
      return (
        routeName === "Bet_RecentOrderHistory" ||
        routeName === "Bet_RecentIssueHistory"
      );
    },
  },
});
</script>
