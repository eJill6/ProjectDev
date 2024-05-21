<template>
  <!-- Header start -->
  <header class="header_height">
    <div class="header_index">
      <div class="header_game">
        <LotteryIcons className="header_gameimg"></LotteryIcons>
        <LotteryIconNames
          className="header_game_item"
          @click="navigateToSelectGame"
        ></LotteryIconNames>
      </div>
    </div>
  </header>
  <!-- Header end -->
  <!-- Game start -->
  <!-- <div class="game_container flex_height overflow no-scrollbar"> -->
  <!-- 开奖 & 投注记录 -->
  <DefaultView></DefaultView>
  <!-- 遊戲介面 & 倒數 -->
  <div class="game_main">
    <div :class="`night${n}`" v-for="(n, index) in nightAnimationArray">
      <div :class="`shooting_star${index === 0 ? '' : index}`"></div>
    </div>
    <!-- 本期倒數 start -->
    <div class="game_wrapper">
      <div class="reciprocal_outter" :class="outterGameTypeClass">
        <IssueNo></IssueNo>
        <WinNumbers></WinNumbers>
      </div>
    </div>
    <!-- 本期倒數 end -->
  </div>
  <!-- 遊戲介面 & 倒數 -->
  <!-- 遊戲玩法 start -->
  <div class="gameplay_main">
    <div class="game_wrapper">
      <PlayTypes></PlayTypes>
    </div>
  </div>

  <PlayTypeRadios></PlayTypeRadios>
  <!-- 投注區 start -->
  <div class="betting_outter" :class="{ android: setAndroidStyle() }">
    <div class="betting_inner spacing">
      <div class="betting_content">
        <div class="betting_money">
          <AssetImage src="@/assets/images/game/ic_money.png" alt="" />
        </div>
        <div class="betting_item">
          <div class="betting_text">{{ formattedBalance }}</div>
          <div class="betting_recharge">
            <AssetImage
              src="@/assets/images/game/btn_recharge.png"
              alt=""
              @click="goDepositUrl"
            />
          </div>
        </div>
      </div>
      <div class="betting_refresh">
        <AssetImage
          src="@/assets/images/game/ic_refresh.png"
          alt=""
          @click="refreshBalance"
        />
      </div>
    </div>
    <div class="betting_inner">
      <div class="betting_chips" @click="navigateToBaseAmountSelection">
        <div class="betting_num" :class="amountClass">{{ baseAmount }}</div>
        <div class="betting_arrow">
          <AssetImage src="@/assets/images/game/ic_chip_arrow.png" alt="" />
        </div>
      </div>
      <div class="betting_betbtn" @click="navigateToConfirmBet"></div>
    </div>
  </div>
  <!-- 投注區 end -->

  <!-- 遊戲玩法 end -->
  <!-- Game end -->
  <!-- <router-view name="dialog"></router-view> -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import {
  WinNumbers,
  PlayTypeRadios,
  IssueNo,
  PlayTypes,
  Dialogs,
} from "@/components";

import LotteryIcons from "../../components/LotteryIcons";
import LotteryIconNames from "../../components/LotteryIconNames";
import { event as eventModel, MsSetting, LotteryMenuInfo } from "@/models";
import { BaseAmount, Balance, MqEvent } from "@/mixins";
import toast from "@/toast";
import { AssetImage } from "@/components/shared";
import mesage from "@/message";
import DefaultView from "./Nav/DefaultView.vue";
import createDialog from "@/createDialog";
import { isAndroid } from "@/gameConfig";
import { GameType } from "@/enums";

export default defineComponent({
  components: {
    WinNumbers,
    IssueNo,
    PlayTypes,
    PlayTypeRadios,
    AssetImage,
    DefaultView,
    LotteryIcons,
    LotteryIconNames,
  },
  data() {
    return {
      isSelectMode: false,
      menuIconCount: 4,
      nightAnimationArray: ["", "", "1", "1", ""],
    };
  },
  mixins: [MqEvent, BaseAmount, Balance],
  methods: {
    setAndroidStyle() {
      return isAndroid;
    },
    navigateToBaseAmountSelection() {
      createDialog(Dialogs.AmountSelection);
    },
    navigateToConfirmBet() {
      if (!this.betCount) {
        toast("当前没有投注型态，请先下注");
        return;
      }
      createDialog(Dialogs.ConfirmBet);
    },
    navigateToSelectGame() {
      createDialog(Dialogs.SelectLottery);
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
    betLotteryTypeName() {
      let currentBetInfo = this.betInfo;
      let lotteryTypeName = this.$store.state.lotteryInfo.lotteryTypeName;
      if (currentBetInfo && currentBetInfo.length > 0) {
        lotteryTypeName = currentBetInfo[0].lotteryTypeName ?? lotteryTypeName;
      }
      return lotteryTypeName;
    },
    onLoadData(arg: eventModel.IssueNoChangedArg) {
      let message = `${this.betLotteryTypeName()} 期號已变更为${
        this.issueNo.currentIssueNo
      }`;
      toast(message);
    },
  },
  computed: {
    outterGameTypeClass() {
      let lotteryCode = this.$store.state.lotteryInfo.lotteryCode as string;
      let gameType = this.$store.state.lotteryInfo.gameTypeId as GameType;
      if(gameType === GameType.K3){
        return ""
      }
      if (gameType == GameType.PK10) {
        return "bg_car";
      }
      return `bg_${lotteryCode.toLocaleLowerCase()}`;
    },
    betInfo() {
      return this.$store.state.currnetBetInfo.filter((x) => x);
    },
    issueNo() {
      let currentIssueNo = this.$store.state.issueNo;
      let currnetBetInfo = this.betInfo;
      if (currnetBetInfo.length > 0) {
        currentIssueNo =
          this.$store.getters.allLotteryIssueNo.find((x) => {
            return x.lotteryId === currnetBetInfo[0].lotteryId;
          }) ?? currentIssueNo;
      }
      return currentIssueNo;
    },
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
          isMaintaining: false,
        };
        list.push(info);
      }
      return list;
    },
    isDisable(): boolean {
      let routeName = (this.$router.currentRoute?.value?.name as string) ?? "";
      return (
        routeName === "Bet_RecentOrderHistory" ||
        routeName === "Bet_RecentIssueHistory"
      );
    },
    logoUrl() {
      const lotteryID = this.$store.state.lotteryInfo.lotteryId;
      return !!lotteryID ? `@/assets/images/ic_lottery_${lotteryID}.png` : "";
    },
  },
});
</script>
