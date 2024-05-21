<template>
  <div class="confirm_main">
    <!-- 跟單計畫 start -->
    <div class="confirm_wrapper">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToBet()">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="跟单计划">跟单计划</div>
        </div>
        <div class="setting_wrapper pd_0 flex_second_height spacing">
          <JSIssueCountdown></JSIssueCountdown>
          <JSDrawResult></JSDrawResult>
          <div class="plan_info_group flex">
            <div>
              <template v-if="currentLotteryPlan.issueNos">
                <div class="plan_info">
                  <AssetImage
                    src="@/assets/images/plan/ic_arrow_lighttaupe.png"
                  />
                  <div class="text">
                    {{ combineIssueNos(currentLotteryPlan.issueNos) }}期
                  </div>
                  <div class="text white">
                    {{ currentLotteryPlan.typePlan }}
                  </div>
                  <div class="text yellow">
                    {{ currentLotteryPlan.lotteryBet }}
                  </div>
                </div>
                <div class="plan_info">
                  <AssetImage
                    src="@/assets/images/plan/ic_arrow_lighttaupe.png"
                  />
                  <div class="text">
                    {{ removeDate(currentLotteryPlan.issueNo) }}期
                  </div>
                  <div class="text yellow">
                    {{ currentLotteryPlan.multi }}倍
                  </div>
                </div>
              </template>
            </div>

            <div
              class="filter_menu"
              v-if="followOrderType.length > 1"
              @click="showFollowPlanOptionsVisible"
            >
              <div class="inner">
                <div class="icon">
                  <AssetImage src="@/assets/images/record/ic_state.png" />
                </div>
                <div class="text">{{ followOrderType[selectedType] }}</div>
              </div>
            </div>
          </div>
          <div class="list_header_type mx">
            <div class="list_header_title style1">期数</div>
            <div class="list_header_title" :class="betItemColumnClass">玩法</div>
            <div class="list_header_title">投注项</div>
            <div class="list_header_title">期号</div>
            <div class="list_header_title">结果</div>
          </div>
          <div class="confirm_middle flex_second_height">
            <div class="overflow no-scrollbar">
              <div class="type_list_inner">
                <div class="type_row" v-for="item in lotteryPlayInfos">
                  <div class="type_item style1">
                    <div class="type_text pearl">
                      {{ combineIssueNos(item.issueNos) }}期
                    </div>
                  </div>
                  <div class="type_item" :class="betItemColumnClass">
                    <div class="type_text">{{ item.typePlan }}</div>
                  </div>
                  <div class="type_item">
                    <div
                      class="type_text type"
                      :class="playOptionClass(item.lotteryBet)"
                    >
                      {{ item.lotteryBet }}
                    </div>
                  </div>
                  <div class="type_item">
                    <div class="type_text pearl">
                      {{ parseInt(removeDate(item.issueNo)) }}
                    </div>
                  </div>
                  <div class="type_item">
                    <div class="type_text">{{ item.betResult }}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 跟單計畫 end -->

    <div
      class="confirm_main"
      v-if="followPlanOptionsVisible"
      @click.self="hideFollowPlanOptionsVisible"
    >
      <div class="confirm_wrapper auto_height">
        <div class="confirm_outter">
          <div class="confirm_close" @click="hideFollowPlanOptionsVisible">
            <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
          </div>
          <div class="confirm_header">
            <div class="confirm_header_title" data-text="选择跟单计划类型">
              选择跟单计划类型
            </div>
          </div>
          <div class="setting_wrapper">
            <div class="deal_btns">
              <div class="inner">
                <div
                  class="default"
                  :class="{ active: isTypeSelected(index) }"
                  @click="toggleSelectNumber(index)"
                  v-for="(option, index) in followOrderType"
                >
                  <div class="text" :data-text="option">
                    {{ option }}
                  </div>
                </div>
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
import { JSDrawResult, JSIssueCountdown } from "@/components";
import AssetImage from "@/components/shared/AssetImage.vue";
import api from "@/api";
import { event as eventModel, LotteryPlayInfo } from "@/models";
import { MqEvent } from "@/mixins";
import { followOrderTypeList } from "@/gameConfig";

const playOptionMappingClass: {
  [key: string]: string;
} = {
  庄: "red",
  闲: "blue",
  大: "orange",
  小: "blue",
  单: "orange",
  双: "blue",
  红: "red",
  黑: "black",
};

export default defineComponent({
  components: { AssetImage, JSDrawResult, JSIssueCountdown },
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

      this.currentLotteryPlan = result?.currentLotteryPlan || {};
      this.lotteryPlayInfos = result?.lotteryPlayInfos || {};
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
    playOptionClass(option: string) {
      return option && option.trim() !== ""
        ? playOptionMappingClass[option]
        : "";
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
    isYXX() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return typeName.toLocaleLowerCase() === "yxx";
    },
    betItemColumnClass(){
      return this.isYXX ? "style1" : "style4"
    }
  },
});
</script>
