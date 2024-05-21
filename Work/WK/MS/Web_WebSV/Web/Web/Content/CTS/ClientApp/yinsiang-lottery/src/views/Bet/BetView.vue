<template>
  <div class="d-flex flex-direction-column justify-content-between h-100"> 
    <div class="pt-6 pb-6">
      <div class="d-flex justify-content-between align-items-center pl-7 pr-7 pr-5-sm pl-5-sm">
        <div class="d-flex align-items-center cusror-pointer">
          <div class="lobby-menu">
            <AssetImage :src="logoUrl"/>
          </div>
          <IssueNo></IssueNo>
        </div>
        <div>
          <WinNumbers></WinNumbers>
        </div>
      </div>

      <div class="d-flex pl-7 pr-7 pr-5-sm pl-5-sm mt-4" :class="isDisable ? 'bet-menu-tab' : 'bet-menu-list'">
        <router-view v-if="!useSmallIcon" name="nav"></router-view>
        <ChangLongFollow v-if="!isDisable"></ChangLongFollow>
      </div>      
      <router-view name="navContent"></router-view>
      <div class="pl-5 pr-5">
        <PlayTypes></PlayTypes>
      </div> 
      <PlayTypeRadios></PlayTypeRadios>
    </div>
    <div class="d-flex justify-content-between pl-5 pr-5 pr-5-sm pl-5-sm pb-5">
      <div class="d-flex align-items-center">
        <div class="text-white fs-3">余额</div>
        <div class="text-yellow ml-2 fs-3">{{ formattedBalance }}</div>
        <div class="ml-2 cusror-pointer" @click="refreshBalance">
          <AssetImage src="@/assets/images/ic_amount_reload.svg" />
        </div>
        <div
          class="d-flex justify-content-center align-items-center fs-4 fw-bold text-white cusror-pointer bet-deposit"
          @click="goDepositUrl"
        >
          充值
        </div>
      </div>
      <div class="d-flex align-items-center" v-if="!isSelectMode">
        <div
          class="d-flex align-items-center mr-2"
          @click="navigateToBaseAmountSelection"
        >
          <div
            class="d-flex justify-content-center align-items-center text-white fs-3 fw-bold chips-small chips-small-5 cusror-pointer"
            :class="baseAmountIconClassName"
          >
            {{ baseAmount }}
          </div>
          <div>
            <AssetImage src="@/assets/images/ic_chips_arrow.svg" />
          </div>
        </div>
        <div
          class="d-flex justify-content-center align-items-center fs-8 fs-7-sm fw-bold text-white cusror-pointer bet-go"
          @click="navigateToConfirmBet"
        >
          投注
        </div>
      </div>
    </div>
    <router-view name="dialog"></router-view>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers, PlayTypeRadios, IssueNo, PlayTypes, ChangLongFollow } from "@/components";
import { LotteryInfo, MsSetting, LotteryMenuInfo } from "@/models";
import { BaseAmount, Balance } from "@/mixins";
import toast from "@/toast";
import AssetImage from "@/components/shared/AssetImage.vue";
import mesage from '@/message';


export default defineComponent({
  components: { WinNumbers, IssueNo, PlayTypes, PlayTypeRadios, AssetImage, ChangLongFollow },
  data() {
    return {
      isSelectMode: false,
      menuIconCount: 4,
    };
  },
  mixins: [BaseAmount, Balance],
  watch: {},
  methods: {
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
      mesage.openUrl(this.msSetting.logonMode,this.msSetting.depositUrl,null);
    },
    selectGameView(switchMode: boolean = false) {
      this.isSelectMode = switchMode;
    },
    
  },
  computed: {
    betCount() {
      return this.$store.getters.betCount;
    },
    useSmallIcon() {
      return !!this.$route.meta.useSmallIcon;
    },
    msSetting(): MsSetting {
      return this.$store.state.msSetting;
    },
    LotteryMenu(): Array<LotteryMenuInfo> {
      let list = this.$store.state.lotteryMenuInfo;
      list = list.filter((x) => x.lotteryID < 999000);
      const count = list.length;
      for (let index = 0; index < this.menuIconCount - count; index++) {
        const info: LotteryMenuInfo = {
          gameTypeID: 0,
          groupPriority: 0,
          hotNew: 0,
          lotteryID: 0,
          lotteryType: "",
          maxBonusMoney: 0,
          notice: "",
          numberTrendUrl: "",
          officialLotteryUrl: "",
          priority: 0,
          typeURL: "",
          userType: 0,
        };
        list.push(info);
      }
      return list;
    },
    isDisable(): boolean {
        let routeName = (this.$router.currentRoute?.value?.name as string) ?? "";
        return routeName === 'Bet_RecentOrderHistory' || routeName === 'Bet_RecentIssueHistory';
    },
    logoUrl() {
      const lotteryID = this.$store.state.lotteryInfo.lotteryId;
      return !!lotteryID ? `@/assets/images/ic_lottery_${lotteryID}.png` : "";
    },
  },
});
</script>
