<template>
  <div class="h-100 second-content rounded-main">
    <div class="h-100">
      <div class="position-relative">
        <div
          class="bg-orange text-white fw-bold rounded-main history_page_title"
        >
          长龙提醒
        </div>
        <div class="position-absolute backbtn" @click="navigateToBet"></div>
      </div>
      <div
        class="lottery_changlong"
        :class="{ 'border-bottom': !isBigWinNumber }"
      >
        <PlanIssueNo></PlanIssueNo>
        <PlanNumbers v-if="!isBigWinNumber"></PlanNumbers>
        <div>
          <div></div>
        </div>
      </div>
      <PlanNumbers v-if="isBigWinNumber"></PlanNumbers>

      <div class="overflow-scroll-y no-scrollbar" :class="gameTypeClass()">
        <table class="w-100 fs-3 bet-table border-bottom">
          <tbody>
            <tr class="bet-table-title">
              <td class="w-33">类别</td>
              <td class="w-33">两面单双</td>
              <td class="w-33">已开期数</td>
            </tr>
            <tr v-for="item in longDragonInfo.longInfo">
              <td>{{ item.type }}</td>
              <td>{{ item.content }}</td>
              <td>{{ item.count }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlanIssueNo, PlanNumbers } from "@/components";
import api from "@/api";
import { event as eventModel, LongDragonInfo } from "@/models";
import { MutationType } from "@/store";
import { MqEvent } from "@/mixins";
import { bigWinNumberList } from "@/gameConfig";

export default defineComponent({
  components: { PlanNumbers, PlanIssueNo },
  mixins: [MqEvent],
  data() {
    return {
      longDragonInfo: {} as LongDragonInfo,
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    async getLongData() {
      this.longDragonInfo = await api.getLongData(this.lotteryInfo.lotteryId);
    },
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      setTimeout(() => {
        this.getLongData();
      }, 2000);
    },
    gameTypeClass() {
      const gameType: {
        [key: string]: string;
      } = {
        K3: "h_changlong_title",
        PK10: "h_changlong_pk_title",
        SSC: "h_changlong_title",
        LHC: "h_changlong_title",
        NuiNui: "h_nuinui_changlong_title",
      };
      return gameType[this.$store.state.lotteryInfo.gameTypeName] || "";
    },
  },
  created() {
    this.getLongData();
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    componentName(): string {
      return this.lotteryInfo.gameTypeName;
    },
    isBigWinNumber() {
      return (
        bigWinNumberList.indexOf(this.$store.state.lotteryInfo.gameTypeName) >
        -1
      );
    },
  },
});
</script>
