<template>
  <div class="loading_row" v-if="isLoading">
    <div class="loading_cell">
      <div class="circle flip"></div>
      <div class="loading_light spin"></div>
    </div>
  </div>

  <router-view v-if="isInit" v-slot="{ Component }">
    <!-- <transition
      name="fade"
      mode="out-in"
      @before-enter="onBeforeFadeEnter"
      @after-enter="onAfterFadeEnter"
    > -->
    <div class="main_container_flex">
      <component :is="Component" />

      <!-- Footer start -->
      <FooterView></FooterView>
      <!-- Footer start -->
    </div>
    <!-- </transition> -->
  </router-view>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import api from "@/api";
import { ActionType, MutationType } from "@/store";
import { PlayMode } from "@/mixins";
import event from "./event";
import toast from "./toast";
import { TimeRules } from "@/enums";
import { RouterView } from "vue-router";
import {
  AppData,
  BetsLog,
  BetsLogDetail,
  ChangLongDateTimeModel,
  IssueNo,
} from "./models";
import { KJArg } from "./models/event";
import { FooterView } from "@/components";
import winnerDialog from "./winnerDialog";
import { thirtySeccondsInLotteryID } from "./gameConfig";

export default defineComponent({
  components: { FooterView },
  mixins: [PlayMode],
  data() {
    return {
      isInit: false,
      issueNoCountdownTimerId: null as unknown as number,
      closureCountdownTimerId: null as unknown as number,
    };
  },
  watch: {
    gameType: {
      handler(value) {
        this.changeGame(value);
      },
      deep: true,
    },
    isClosuring: {
      handler(value) {
        if (value) {
          this.resetBetsLog();
        }
      },
      deep: true,
    },
  },
  methods: {
    handleKJMessage(message: KJArg) {
      if (
        message.Summary.indexOf(this.$store.state.lotteryInfo.lotteryTypeName) <
        0
      ) {
        return;
      }

      if (message.AvailableScore > 0) {
        winnerDialog(`+${message.AvailableScore}`);
      }

      this.refreshBalanceAsync();
    },
    resetBetsLog() {
      let playConfig = this.$store.getters.playConfig;
      const playNames = Object.keys(playConfig);

      let betLog: BetsLog = {
        numberOdds: [],
      };
      for (let playName of playNames) {
        const items = playConfig[playName].flatMap((x: any) => x);
        if (items.length) {
          const log: BetsLogDetail = {
            category: playName,
            count: 0,
            values: [],
          };
          for (let item of items) {
            const valueItem = {
              value: item,
              totalAmount: "0",
            };
            log.values.push(valueItem);
          }
          betLog.numberOdds.push(log);
        }
      }
      this.$store.commit(MutationType.SetBetsLog, [betLog]);
    },
    refreshBalanceAsync() {
      this.$store.dispatch(ActionType.RefreshBalanceAsync);
    },
    async refreshIssueNoAsync() {
      let result: IssueNo | undefined;
      try {
        let allLotteryIssueNo = await api.getNextIssueNosAsync();
        result = allLotteryIssueNo.find((x) => {
          return x.lotteryId === this.lotteryInfo.lotteryId;
        });

        this.$store.commit(
          MutationType.SetAllLotteryIssueNo,
          allLotteryIssueNo
        );
        this.createChangeLongGameDateTime(allLotteryIssueNo);
        this.$store.commit(MutationType.SetIssueNo, result);
      } catch (e: any) {
        console.error(e);
      }
      if (result?.currentIssueNo) {
        if (!result?.lastDrawNumber) {
          // 開獎時會沒有上期期號，等5秒後再重新索取
          // 或者目前沒有索取到上期期號，等五秒後檢查如果還沒有取到就重新索取
          setTimeout(() => {
            // 五秒後發現MQ沒有推送就繼續索取
            if (!this.$store.state.issueNo.lastDrawNumber) {
              this.refreshIssueNoAsync();
            }
          }, 5000);
        }
        this.countdownIssueNo();
        event.emit("issueNoChanged", { issueNo: result?.currentIssueNo });
      } else {
        this.$store.commit(MutationType.SetTimeRuleStatus, TimeRules.unknown);
        setTimeout(() => {
          this.refreshIssueNoAsync();
        }, 500);
      }
    },
    countdownIssueNo() {
      clearInterval(this.issueNoCountdownTimerId);

      let endTimeValue = new Date(this.issueNo.endTime).getTime();
      let startTimeValue = new Date(this.issueNo.currentTime).getTime();
      let secondsLeft = Math.abs((endTimeValue - startTimeValue) / 1000);

      this.$store.commit(MutationType.SetIssueNoSecondLeft, secondsLeft);

      this.issueNoCountdownTimerId = setInterval(() => {
        this.updateChangeLongGameDateTime();
        let currentSecondsLeft = this.$store.state.issueNoSecondLeft;
        if (this.$store.state.closureSecondLeft === 0) {
          this.$store.commit(
            MutationType.SetTimeRuleStatus,
            TimeRules.issueNoCountdown
          );
          this.$store.commit(MutationType.SetStopAnimation, false);
        }

        const gameTimeSec =
          thirtySeccondsInLotteryID.indexOf(this.lotteryInfo.lotteryId) > -1
            ? 25
            : 55;
        //增加動畫效果3秒緩衝，以利切換頁時，動畫不會再重撥。
        const hotCodeSec = 3;
        const isStopAnimation = currentSecondsLeft <= gameTimeSec - hotCodeSec;
        this.$store.commit(MutationType.SetStopAnimation, isStopAnimation);

        if (currentSecondsLeft === 1) {
          this.countdownTimeClosure();
        }

        if (currentSecondsLeft > 0) {
          this.$store.commit(
            MutationType.SetIssueNoSecondLeft,
            --currentSecondsLeft
          );
        } else {
          clearInterval(this.issueNoCountdownTimerId);
          this.refreshIssueNoAsync();
        }
      }, 1000);
    },
    countdownTimeClosure() {
      //預設封盤時間倒數5到0秒
      this.$store.commit(
        MutationType.SetTimeRuleStatus,
        TimeRules.closureCountdown
      );
      this.$store.commit(MutationType.SetClosureSecondLeft, 5);

      clearInterval(this.closureCountdownTimerId);
      this.closureCountdownTimerId = setInterval(() => {
        let currentSecondsLeft = this.$store.state.closureSecondLeft;
        if (currentSecondsLeft >= 0) {
          this.$store.commit(
            MutationType.SetClosureSecondLeft,
            --currentSecondsLeft
          );
        } else {
          this.$store.commit(
            MutationType.SetTimeRuleStatus,
            TimeRules.issueNoCountdown
          );
          clearInterval(this.closureCountdownTimerId);
        }
      }, 1000);
    },
    updateChangeLongGameDateTime() {
      const allChangLongInfo: ChangLongDateTimeModel[] = [];
      this.$store.state.changLongLotteryTimeInfo.forEach((item) => {
        item.secondsTotal = item.secondsTotal - 1;
        item.secondsTenDigits = `${Math.floor(item.secondsTotal / 10)}`;
        item.secondsDigits = `${item.secondsTotal % 10}`;
        if (item.secondsTotal >= 0) {
          allChangLongInfo.push(item);
        }
      });
      this.$store.commit(
        MutationType.SetChangLongDateTimeInfo,
        allChangLongInfo
      );
    },
    onBeforeFadeEnter() {
      document.body.style.overflow = "hidden";
    },
    onAfterFadeEnter() {
      document.body.style.overflow = "";
    },
    initData() {
      this.closeKj();
      clearInterval(this.issueNoCountdownTimerId);
      this.isInit = false;
      this.$store.commit(MutationType.InitData);
    },
    async changeGame(typeURL: string) {
      this.$store.commit(MutationType.SetIsLoading, true);
      this.initData();
      const result = await api.getViewModel(typeURL);
      await this.refreshAppData(result);
      this.$store.commit(MutationType.SetIsLoading, false);
    },
    async refreshAppData(appData: AppData) {
      this.$store.commit(MutationType.SetLottery, appData);
      let defaultPlayMode = appData.playConfigs[0];
      await this.changePlayModeAsync(defaultPlayMode);
      await this.getAllLotteryInfo();

      await this.refreshIssueNoAsync();

      let submitParams = {
        lotteryId: this.lotteryInfo.lotteryId,
        currentIssueNo: this.issueNo.currentIssueNo || "",
        roomId: "0",
      };
      const result = await api.getUnawardedSummary(submitParams);
      this.$store.commit(MutationType.SetBetsLog, result);
      this.isInit = true;

      event.on("kj", this.handleKJMessage);
    },
    closeKj() {
      clearInterval(this.issueNoCountdownTimerId);
      event.off("kj", this.refreshBalanceAsync);
    },
  },
  async created() {
    let appData = api.getAppData();
    await this.refreshAppData(appData);
  },
  errorCaptured(err: any) {
    console.error(err);
    toast(err.message || err);
  },
  beforeUnmount() {
    this.closeKj();
  },
  computed: {
    isLoading() {
      return this.$store.state.isLoading;
    },
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    issueNo() {
      return this.$store.state.issueNo;
    },
    gameType() {
      return this.$store.state.gameType;
    },
    isClosuring(): boolean {
      return (
        this.$store.getters.countdownTime.timeRule ===
        TimeRules.closureCountdown
      );
    },
  },
});
</script>

<style>
.fade-enter-active,
.fade-leave-active {
  transition: all 0.3s ease;
}

.fade-enter-from {
  opacity: 0;
  transform: translateY(30px);
}

.fade-leave-to {
  opacity: 0;
  transform: translateY(-30px);
}
</style>
