<template>
  <div class="h-100 second-content rounded-main">
    <div class="h-100">
      <div class="position-relative">
        <div
          class="bg-orange text-white fw-bold rounded-main history_page_title"
        >
          跟单计划
        </div>
        <div class="position-absolute backbtn" @click="navigateToBet"></div>
      </div>
      <div class="lottery_follow border-bottom" v-if="!isBigWinNumber">
        <PlanIssueNo></PlanIssueNo>
        <PlanNumbers></PlanNumbers>
        <div>
          <div
            class="d-flex cusror-pointer lottery_follow_pd flex-direction-column"
          >
            <div class="followbet_issue d-flex justify-content-between test">
              <div class="text-black" v-if="currentLotteryPlan.issueNos">
                {{ `${combineIssueNos(currentLotteryPlan.issueNos)}期` }}
                <span class="text-medium-gary">{{
                  currentLotteryPlan.typePlan
                }}</span>
                <span class="text-red"
                  >&nbsp;{{ currentLotteryPlan.lotteryBet }}</span
                >
                <div class="text-black lottery_font_pd">
                  {{ `${removeDate(currentLotteryPlan.issueNo)}期` }}
                  <span class="text-red">{{
                    `${currentLotteryPlan.multi}倍`
                  }}</span>
                </div>
              </div>
              <div
                class="d-flex follow_filter align-items-center justify-content-center"
                @click="showFollowPlanOptionsVisible"
              >
                <div class="d-flex text-black cusror-pointer">
                  {{ followOrderType[selectedType] }}
                  <AssetImage
                    class="ml-2"
                    src="@/assets/images/ic_history_arrow.svg"
                    alt=""
                  ></AssetImage>
                </div>
              </div>
            </div>
          </div>
          <div></div>
        </div>
      </div>

      <div class="lottery_follow" v-if="isBigWinNumber">
        <PlanIssueNo></PlanIssueNo>
      </div>
      <PlanNumbers v-if="isBigWinNumber"></PlanNumbers>
      <div
        class="d-flex cusror-pointer flex-direction-column border-bottom"
        v-if="isBigWinNumber"
      >
        <div
          class="followbet_issue d-flex justify-content-between test pr-7 pl-7 pt-3-5 pb-5"
        >
          <div class="text-black" v-if="currentLotteryPlan.issueNos">
            {{ `${combineIssueNos(currentLotteryPlan.issueNos)}期` }}
            <span class="text-medium-gary">{{
              currentLotteryPlan.typePlan
            }}</span>
            <span class="text-red">{{ currentLotteryPlan.lotteryBet }}</span>
            <div class="text-black lottery_font_pd">
              {{ `${removeDate(currentLotteryPlan.issueNo)}期` }}
              <span class="text-red">{{
                `${currentLotteryPlan.multi}倍`
              }}</span>
            </div>
          </div>
          <div
            class="d-flex follow_filter align-items-center justify-content-center"
            @click="showFollowPlanOptionsVisible"
          >
            <div class="d-flex text-black cusror-pointer">
              {{ followOrderType[selectedType] }}
              <AssetImage
                class="ml-2"
                src="@/assets/images/ic_history_arrow.svg"
                alt=""
              />
            </div>
          </div>
        </div>
      </div>

      <div class="overflow-scroll-y no-scrollbar" :class="gameTypeClass">
        <table class="w-100 fs-3 bet-table border-bottom">
          <tbody>
            <tr class="bet-table-title">
              <td>期数</td>
              <td>玩法</td>
              <td>投注项</td>
              <td>期号</td>
              <td>结果</td>
            </tr>
            <tr v-for="item in lotteryPlayInfos">
              <td>{{ `${combineIssueNos(item.issueNos)}期` }}</td>
              <td>{{ lotteryInfo.lotteryTypeName }}</td>
              <td>
                <div class="d-flex justify-content-center">
                  <div
                    class="d-flex justify-content-center align-items-center rounded-1 fs-2 ml-2 win-type-history"
                    :class="getBackgroundClassName(item.lotteryBet)"
                  >
                    {{ item.lotteryBet }}
                  </div>
                </div>
              </td>
              <td>{{ parseInt(removeDate(item.issueNo)) }}</td>
              <td>{{ item.betResult }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div
      class="popup-cover"
      v-if="followPlanOptionsVisible"
      style="position: absolute; right: 0; bottom: 0; top: 0; left: 0"
      @click.self="hideFollowPlanOptionsVisible"
    >
      <div class="position-fixed w-100 rounded-main bottom-0 bg-pop">
        <div class="position-relative">
          <div
            class="d-flex justify-content-center align-items-center text-black fw-bold list_title_text"
          >
            选择跟单计划类型
          </div>
          <div
            class="position-absolute list_closebtn"
            @click="hideFollowPlanOptionsVisible"
          >
            <div class="cusror-pointer"></div>
          </div>
        </div>
        <div class="list_content">
          <div class="d-flex flex-wrap-wrap">
            <div
              class="w-50 pl-2 pr-2"
              v-for="(name, index) in followOrderType"
            >
              <div
                class="d-flex justify-content-center align-items-center pt-6 pb-6 mt-4 pl-1 pr-1 cusror-pointer list-btn fw-bold"
                :class="{ active: isTypeSelected(index) }"
                @click="toggleSelectNumber(index)"
              >
                {{ name }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlanIssueNo, PlanNumbers } from "@/components";
import AssetImage from "@/components/shared/AssetImage.vue";
import api from "@/api";
import { event as eventModel, LotteryPlayInfo } from "@/models";
import { MqEvent } from "@/mixins";
import { bigWinNumberList, followOrderTypeList } from "@/gameConfig";

export default defineComponent({
  components: { PlanIssueNo, AssetImage, PlanNumbers },
  mixins: [MqEvent],
  data() {
    return {
      followPlanOptionsVisible: false,
      selectedType: 0,
      currentLotteryPlan: {} as LotteryPlayInfo,
      lotteryPlayInfos: [] as LotteryPlayInfo[],
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    showFollowPlanOptionsVisible() {
      this.followPlanOptionsVisible = true;
    },
    hideFollowPlanOptionsVisible() {
      this.followPlanOptionsVisible = false;
    },
    async toggleSelectNumber(type: number) {
      this.selectedType = type;
      this.hideFollowPlanOptionsVisible();
      await this.getLotteryPlanData();
    },
    isTypeSelected(index: number) {
      return this.selectedType === index;
    },
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.getLotteryPlanData();
    },
    async getLotteryPlanData() {
      const selected = this.selectedType + 1;
      const result = await api.getLotteryPlanData(
        this.lotteryInfo.lotteryId,
        selected
      );
      this.currentLotteryPlan = result.currentLotteryPlan;
      this.lotteryPlayInfos = result.lotteryPlayInfos;
    },
    combineIssueNos(issueNos: string[]) {
      if (issueNos.length !== 2) return "";
      const startDate = this.removeDate(issueNos[0] as string);
      const endDate = this.removeDate(issueNos[1] as string);
      return `${parseInt(startDate)}-${parseInt(endDate)}`;
    },
    removeDate(issueNo: string): string {
      return issueNo.slice(8);
    },
    getBackgroundClassName(lotteryBet: string) {
      if (
        this.$store.state.lotteryInfo.gameTypeName === "LHC" &&
        this.selectedType === 2
      ) {
        return "";
      }
      let colorName = Object.keys(this.className).find((key) =>
        this.className[key].includes(lotteryBet)
      );
      return colorName;
    },
  },
  created() {
    this.getLotteryPlanData();
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    followOrderType() {
      return (
        followOrderTypeList[this.$store.state.lotteryInfo.gameTypeName] || []
      );
    },
    className(): { [key: string]: string[] } {
      return {
        "bg-cyan text-white": ["双", "小", "蓝", "虎"],
        "bg-orange text-white": ["单", "大", "龙"],        
        "bg-red text-white": ["红"],
        "bg-green text-white": ["绿", "和"],
      };
    },
    gameTypeClass() {
      const gameType: {
        [key: string]: string;
      } = {
        K3: "h-follow-hight",
        PK10: "h-follow-pk-hight",
        SSC: "h-follow-hight",
        LHC: "h-follow-hight",
        NuiNui: "h-follow-nuinui-hight",
      };
      return gameType[this.$store.state.lotteryInfo.gameTypeName] || "";
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
