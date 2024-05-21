<template>
  <!-- header second start -->
  <header class="header_second_height header_second_bg">
    <div class="header_middle">
      <div class="header_second_title"><p>跟单计划</p></div>
    </div>
  </header>
  <!-- header second  end -->
  <PlanIssueNo></PlanIssueNo>
  <PlanNumbers></PlanNumbers>
  <div class="plan_bottom">
    <div class="plan_infos_group">
      <div class="info_group">
        <div class="info_img">
          <AssetImage src="@/assets/images/plan/ic_arrow_lighttaupe.png" />
        </div>
        <div class="info_text" v-if="currentLotteryPlan.issueNos">
          {{ `${combineIssueNos(currentLotteryPlan.issueNos)}期` }}
        </div>
        <div class="info_text white">{{ currentLotteryPlan.typePlan }}</div>
        <div class="info_text yellow">{{ currentLotteryPlan.lotteryBet }}</div>
      </div>
      <div class="info_group">
        <div class="info_img">
          <AssetImage src="@/assets/images/plan/ic_arrow_lighttaupe.png" />
        </div>
        <div class="info_text">
          {{ `${removeDate(currentLotteryPlan.issueNo)}期` }}
        </div>
        <div class="info_text yellow">
          {{ `${currentLotteryPlan.multi}倍` }}
        </div>
      </div>
    </div>
    <div class="plan_type_filter" @click="showFollowPlanOptionsVisible">
      <div class="type_filter_img">
        <AssetImage src="@/assets/images/record/ic_state.png" />
      </div>
      <div class="type_filter_text">{{ followOrderType[selectedType] }}</div>
    </div>
  </div>
  <div class="plan_list_header">
    <div class="plan_list_header_text column_1">期数</div>
    <div class="plan_list_header_text column_2">玩法</div>
    <div class="plan_list_header_text">投注项</div>
    <div class="plan_list_header_text">期号</div>
    <div class="plan_list_header_text">结果</div>
  </div>
  <!-- 滑動區塊 start-->
  <div class="flex_second_height">
    <div class="overflow no-scrollbar">
      <div class="second_adding_basic pd_0">
        <div class="plan_list_rows">
          <div class="plan_list_row" v-for="item in lotteryPlayInfos">
            <div class="plan_list_row_text column_1">
              {{ `${combineIssueNos(item.issueNos)}期` }}
            </div>
            <div class="plan_list_row_text white column_2">
              {{ item.typePlan }}
            </div>
            <div class="plan_list_row_text">
              <div
                class="win_type"
                :class="getBackgroundClassName(item.lotteryBet)"
              >
                {{ item.lotteryBet }}
              </div>
            </div>
            <div class="plan_list_row_text">
              {{ parseInt(removeDate(item.issueNo)) }}
            </div>
            <div class="plan_list_row_text white">{{ item.betResult }}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- 滑動區塊  end -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlanIssueNo, PlanNumbers, Dialogs } from "@/components";
import { AssetImage } from "@/components/shared";
import api from "@/api";
import {
  DialogSettingModel,
  event as eventModel,
  LotteryPlayInfo,
} from "@/models";
import { MqEvent } from "@/mixins";
import { followOrderTypeList } from "@/gameConfig";
import createDialog from "@/createDialog";

export default defineComponent({
  components: { PlanIssueNo, AssetImage, PlanNumbers },
  mixins: [MqEvent],
  data() {
    return {
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
      const model: DialogSettingModel = {
        selectedIndex: this.selectedType,
      };
      createDialog(Dialogs.FollowPlanSelection, model, (model) => {
        this.toggleSelectNumber(model.selectedIndex || 0);
      });
    },
    async toggleSelectNumber(type: number) {
      this.selectedType = type;
      await this.getLotteryPlanData();
    },
    isTypeSelected(index: number) {
      return this.selectedType === index;
    },
    onLoadData(arg: eventModel.LotteryDrawArg) {
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
      if (issueNo) {
        return issueNo.slice(8);
      }
      return ``;
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
        blue: ["双", "小", "蓝", "虎", "闲"],
        orange: ["单", "大", "龙"],
        red: ["红", "庄"],
        green: ["绿", "和"],
        black: ["黑"]
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
        YXX: "h-follow-hight",
      };
      return gameType[this.$store.state.lotteryInfo.gameTypeName] || "";
    },
  },
});
</script>
