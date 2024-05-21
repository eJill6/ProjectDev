<template>
  <div class="loading-content" v-if="isLoading">
    <div>
      <span class="loader"></span>
    </div>
  </div>
  <router-view v-if="isInit" v-slot="{ Component }">
    <transition name="fade" mode="out-in" @before-enter="onBeforeFadeEnter" @after-enter="onAfterFadeEnter">
      <component :is="Component" />
    </transition>
  </router-view>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import api from '@/api';
import { ActionType, MutationType } from '@/store';
import { PlayMode } from '@/mixins';
import event from "./event";
import toast from "./toast";
import { TimeRules} from "@/enums";
import { RouterView } from "vue-router";
import { AppData } from "./models";

export default defineComponent({
  mixins: [PlayMode],
  data() {
    return {
      isInit: false,
      issueNoCountdownTimerId: null as unknown as number,
      closureCountdownTimerId: null as unknown as number
    };
  },
  watch:{
    gameType:{
      handler(value) {
        this.changeGame(value);
      },
      deep: true
    },
    orderNo:{
      handler(value) {
        if(value) {
          this.navigateToReConfirmBet();
        }
      },
      deep: true
    }
  },
  methods: {    
    navigateToReConfirmBet() {
      this.$router.push({ name: "ReConfirmBet" });
    },
    refreshBalanceAsync() {
      this.$store.dispatch(ActionType.RefreshBalanceAsync);
    },
    async refreshIssueNoAsync() {
      let result = await api.getNextIssueNoAsync(this.lotteryInfo.lotteryId);        
      this.$store.commit(MutationType.SetIssueNo, result);      
      // todo: retry
      if (result.currentIssueNo) {        
        this.countdownIssueNo();
        event.emit('issueNoChanged', { issueNo: result.currentIssueNo });
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
        
        let currentSecondsLeft = this.$store.state.issueNoSecondLeft;
        if(this.$store.state.closureSecondLeft === 0) {
          this.$store.commit(MutationType.SetTimeRuleStatus, TimeRules.issueNoCountdown);
        }

        if(currentSecondsLeft === 1) {
          this.countdownTimeClosure();
        }

        if (currentSecondsLeft > 0) {
          this.$store.commit(MutationType.SetIssueNoSecondLeft, --currentSecondsLeft);
        } else {          
          clearInterval(this.issueNoCountdownTimerId);
          this.refreshIssueNoAsync();
        }
      }, 1000);
    },
    countdownTimeClosure(){
      //預設封盤時間倒數5到0秒
      this.$store.commit(MutationType.SetTimeRuleStatus, TimeRules.closureCountdown);
      this.$store.commit(MutationType.SetClosureSecondLeft, 5);  

      clearInterval(this.closureCountdownTimerId);
      this.closureCountdownTimerId = setInterval(() => {
        let currentSecondsLeft = this.$store.state.closureSecondLeft;
        if (currentSecondsLeft >= 0) {
          this.$store.commit(MutationType.SetClosureSecondLeft, --currentSecondsLeft);
        } else {
          this.$store.commit(MutationType.SetTimeRuleStatus, TimeRules.issueNoCountdown);
          clearInterval(this.closureCountdownTimerId);          
        }
      }, 1000);
    }, 
    onBeforeFadeEnter() {
      document.body.style.overflow = 'hidden';
    },
    onAfterFadeEnter() {
      document.body.style.overflow = '';
    },
    initData(){
      this.closeKj();
      clearInterval(this.issueNoCountdownTimerId);
      this.isInit = false;
      this.$store.commit(MutationType.InitData);
    },
    async changeGame(typeURL:string){
      this.$store.commit(MutationType.SetIsLoading, true);  
      this.initData();
      const result = await api.getViewModel(typeURL)      
      await this.refreshAppData(result);
      this.$store.commit(MutationType.SetIsLoading, false);
    },
    async refreshAppData(appData:AppData){
      this.$store.commit(MutationType.SetLottery, appData);
      let defaultPlayMode = appData.playConfigs[0];
      await this.changePlayModeAsync(defaultPlayMode);
      await this.getAllLotteryInfo();
      this.refreshIssueNoAsync();
      this.isInit = true;

      event.on('kj', this.refreshBalanceAsync);
    },
    closeKj(){
      clearInterval(this.issueNoCountdownTimerId);
      event.off('kj', this.refreshBalanceAsync);
    }
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
    gameType(){
      return this.$store.state.gameType;
    },
    orderNo(){
      return this.$store.state.msSetting.orderNo;
    }
  }
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