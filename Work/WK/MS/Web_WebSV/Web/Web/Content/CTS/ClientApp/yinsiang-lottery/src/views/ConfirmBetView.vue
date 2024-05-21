<template>
  <div class="h-100 second-content rounded-main">
    <div class="d-flex flex-direction-column justify-content-between h-100">
      <div class="h-confirm-list">
        <div class="position-relative">
          <div
            class="bg-orange text-white fw-bold rounded-main history_page_title"
          >
            确认投注
          </div>
          <div
            v-if="!isDocumentary"
            class="position-absolute backbtn"
            @click="navigateToBet"
          ></div>
        </div>
        <div class="bet_history_info border-bottom">
          <div class="d-flex justify-content-between align-items-center">
            <div class="d-flex align-items-center cusror-pointer">
              <div class="text-black followbet_title">{{ lotteryInfo.lotteryTypeName }}</div>
              <div class="followbet_issue d-flex">
                <div class="text-medium-gary pr-2">第</div>
                <div class="text-black pr-2">{{ issueNo.currentIssueNo }}</div>
                <div class="text-medium-gary">期</div>
              </div>
            </div>
            <div
              class="d-flex justify-content-between align-items-center countdown_second_pd"
            >
              <div class="d-flex align-items-end">
                <div class="d-flex align-items-center countdown_second">
                  <div class="mr-1 countdown_icon">
                    <AssetImage src="@/assets/images/ic_countdown.svg" alt="" />
                  </div>
                  <div class="text-medium-gary align-items-center d-flex">
                    下期倒计时
                  </div>
                  <div class="text-red countdown_second_bg font_number">
                    {{ formattedIssueNoCountdownMinute }}
                  </div>
                  <div class="text-black fw-bold">:</div>
                  <div class="text-red countdown_second_bg font_number">
                    {{ formattedIssueNoCountdownSecond }}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="h-confirm overflow-scroll-y no-scrollbar">
          <table class="w-100 fs-3 bet-confirm-table border-bottom">
            <tbody>
              <tr class="bet-table-title">
                <td>玩法</td>
                <td>赔率</td>
                <td>金额</td>
                <td>删除</td>
              </tr>
              <tr v-for="info in betInfo">
                <td class="text-medium-gary pr-2">
                  {{
                    info.playTypeRadioName
                  }}｜
                  <span class="text-bet-order">{{
                    info.selectedBetNumber
                  }}</span>
                </td>
                <td>
                  <span class="text-bet-odds">{{ info.odds }}</span>
                </td>
                <td>
                  <div class="bet_confirm_input">
                    {{ betBaseAmount }}
                  </div>
                </td>
                <td>
                  <div
                    class="d-flex justify-content-center align-items-center cusror-pointer"
                    @click="removeBetInfo(info.id)"
                  >
                    <AssetImage src="@/assets/images/ic_delet.svg" alt="" />
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div
          class="d-flex align-items-center justify-content-between pr-5-sm pl-5-sm border-top border-gray bet-confirm-double"
        >
          <button
            class="d-flex justify-content-center align-items-center bet-multiple fs-4 flex-fill mr-2"
            v-for="m in multiples"
            :class="{ active: isCurrentMultiple(m) }"
            @click="changeCurrentMultiple(m)"
          >
            {{ m }}倍
          </button>
        </div>
        <div
          class="d-flex justify-content-between align-items-center pl-7 pr-7 pb-5 pr-5-sm pl-5-sm mt-2"
        >
          <div>
            <div class="d-flex align-items-center">
              <div class="text-black fs-3">合计</div>
              <div class="text-orange-default fw-bold ml-2 fs-4">
                {{ betCount }}
              </div>
              <div class="text-black ml-2 fs-3">注</div>
              <div class="text-black ml-2 fs-3">金额</div>
              <div class="text-orange-default ml-2 fs-4 fw-bold">
                {{ totalAmount }}
              </div>
              <div class="text-black ml-2 fs-3">元</div>
            </div>
            <div class="d-flex align-items-center mt-2">
              <div class="text-black fs-3">余额</div>
              <div class="text-orange-default fw-bold ml-2 fs-4">
                {{ formattedBalance }}
              </div>
            </div>
          </div>
          <div
            class="d-flex justify-content-center align-items-center fs-6 fs-7-sm fw-bold text-white cusror-pointer bet-go-confirm"
            @click="confirmBetAsync"
          >
            确认投注
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Balance } from "@/mixins";
import toast from "@/toast";
import promptDialog from "@/promptDialog";
import { event as eventModel, MsSetting } from "@/models";
import { MutationType } from "@/store";
import event from "@/event";
import api from "@/api";
import { TimeRules } from "@/enums";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  props: {
    isDocumentary: {
      type: Object as () => boolean,
    },
  },
  mixins: [Balance],
  components: { AssetImage },
  data() {
    return {
      currentMultiple: 0,
      showPrompt: false,
    };
  },
  methods: {
    navigateToBet() {
      this.$store.commit(MutationType.SetPlayType, 0);
      this.$router.push({ name: "Bet" });
    },
    isCurrentMultiple(multiple: number) {
      return this.currentMultiple === multiple;
    },
    changeCurrentMultiple(multiple: number) {
      this.currentMultiple = multiple;
    },
    removeBetInfo(id: string) {
      if (this.betInfo.length === 1) {
        toast("必须至少保留一个投注，无法全部移除");
        return;
      }

      this.$store.commit(MutationType.RemoveCurrentBetInfoById, id);
    },
    async confirmBetAsync() {
      let totalAmount = parseFloat(this.totalAmount);

      if (!this.issueNo.currentIssueNo) {
        toast("本期封盘中，请等下期");
        return;
      }

      // if(totalAmount > parseInt(this.formattedBalance)) {
      //     this.showPrompt = true;
      //     return;
      // }
      try {
        let submitParams = {
          amount: this.betCount,
          currencyUnit: "1",
          currentIssueNo: this.issueNo.currentIssueNo,
          habitRebatePro: this.$store.getters.currentRebatePro?.value
            .rebate as number,
          lotteryId: this.lotteryInfo.lotteryId,
          playType: this.$store.getters.currentPlayType?.info
            .playTypeID as number,
          playTypeName: this.$store.getters.currentPlayType?.info
            .playTypeName as string,
          playTypeRadio: this.$store.getters.currentPlayTypeRadio?.info
            .playTypeRadioID as number,
          playTypeRadioName: this.$store.getters.currentPlayTypeRadio?.info
            .playTypeRadioName as string,
          price: this.betBaseAmount.toString(),
          ratio: 1,
          selectedNums: this.betInfo
            .map((x) => `${x.playTypeRadioName} ${x.selectedBetNumber}`)
            .join("|"),
        };
        this.$store.commit(MutationType.SetIsLoading, true);
        let result = await api.postOrderAsync(submitParams);
        toast("投注成功");
      } catch (error) {
        const message =
          error instanceof Error ? (error as Error).message : error;
        this.showPrompt = (message as string).indexOf('不足') >= 0;
        if (this.showPrompt)
          //弹框
          promptDialog(message as string, this.msSetting.depositUrl,this.msSetting.logonMode);
        else toast(message as string);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
        if (!this.showPrompt) {
          this.$store.commit(MutationType.SetCurrentBetInfo, []);
          this.$store.commit(MutationType.SetNumbers, null);
          this.navigateToBet();
        }
      }
    },
    onIssueNoChanged(arg: eventModel.IssueNoChangedArg) {
      let message = `${this.lotteryInfo.lotteryTypeName} 期號已变更为${arg.issueNo}`;
      toast(message);
    },
  },
  created() {
    this.currentMultiple = this.multiples[0];
    event.on("issueNoChanged", this.onIssueNoChanged);
  },
  beforeUnmount() {
    event.off("issueNoChanged", this.onIssueNoChanged);
  },
  mounted() {
    if (!this.betInfo.length) this.navigateToBet();
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    issueNo() {
      return this.$store.state.issueNo;
    },
    formattedIssueNoCountdownTime(): string {
      return this.$store.getters.formattedIssueNoCountdownTime;
    },
    formattedIssueNoCountdownMinute(): string {
      return this.$store.getters.formattedIssueNoCountdownTime.split(":")[0];
    },
    formattedIssueNoCountdownSecond(): string {
      return this.$store.getters.formattedIssueNoCountdownTime.split(":")[1];
    },
    formattedClosureCountdownTime(): string {
      return this.$store.getters.formattedClosureCountdownTime;
    },
    betInfo() {
      return this.$store.state.currnetBetInfo;
    },
    multiples() {
      return [1, 2, 5, 10, 20];
    },
    betCount() {
      return this.$store.getters.betCount;
    },
    baseAmount() {
      return this.$store.state.baseAmount;
    },
    betBaseAmount() {
      return this.baseAmount * this.currentMultiple;
    },
    totalAmount() {
      return (this.betBaseAmount * this.betCount).toFixed(2);
    },
    showIssueNo(): boolean {
      return (
        this.$store.getters.showTimeRuleStatus === TimeRules.issueNoCountdown
      );
    },
    closuring(): boolean {
      return (
        this.$store.getters.showTimeRuleStatus === TimeRules.closureCountdown
      );
    },
    msSetting(): MsSetting {
      return this.$store.state.msSetting;
    },
  },
});
</script>
