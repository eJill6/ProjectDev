<template>
  <!-- 確認投注 start -->
  <div class="confirm_wrapper" :class="{ iphone: hasIOS }">
    <div class="confirm_outter">
      <div class="confirm_close" @click="navigateToBet">
        <AssetImage src="@/assets/images/modal/ic_confirm_bet_close.png" />
      </div>
      <div class="confirm_header">
        <div class="confirm_header_title">确认投注</div>
      </div>
      <div class="setting_wrapper pd_0">
        <div class="confirm_up">
          <div class="confirm_infos">
            <LotteryIcons
              className="confirm_gameimg"
              :gameTypeName="betGameTypeName()"
              :gameTypeId="betGameTypeId()"
            ></LotteryIcons>
            <div class="confirm_info">
              <div class="confirm_title">{{ betLotteryTypeName() }}</div>
              <div class="confirm_num">第 {{ issueNo.currentIssueNo }} 期</div>
            </div>
          </div>
          <ClockView :clockClass="clockClass"></ClockView>
        </div>
        <div class="confirm_middle">
          <div class="list_outter">
            <div class="list_inner">
              <div class="list_header_outter">
                <div class="list_header_title">玩法</div>
                <div class="list_header_title">赔率</div>
                <div class="list_header_title">金额</div>
                <div class="list_header_title">删除</div>
              </div>
            </div>
          </div>
          <div class="confirm_list_outter">
            <div class="confirm_list_inner overflow no-scrollbar">
              <div class="confirm_row" v-for="info in betInfo">
                <div class="confirm_item">
                  <div class="confirm_text">
                    {{ info.playTypeRadioName }}｜<span>{{
                      info.selectedBetNumber
                    }}</span>
                  </div>
                </div>
                <div class="confirm_item">
                  <div class="confirm_text white">{{ info.odds }}</div>
                </div>
                <div class="confirm_item">
                  <div class="confirm_text">{{ showBetAmount(info) }}</div>
                </div>
                <div class="confirm_item">
                  <div class="confirm_icon" @click="removeBetInfo(info.id)">
                    <AssetImage src="@/assets/images/modal/ic_trash.png" />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="confirm_bottom">
          <div
            class="multiple_item"
            :class="{ active: isCurrentMultiple(m) }"
            @click="changeCurrentMultiple(m)"
            v-for="m in multiples"
          >
            {{ m }}倍
          </div>
        </div>
        <div class="confirm_betting_outter" :class="{ iphone: hasIOS }">
          <div class="confirm_betting_inner spacing">
            <div class="confirm_betting_content">
              <div class="confirm_betting_money">
                <AssetImage src="@/assets/images/modal/ic_betting_money.png" />
              </div>
              <div class="confirm_betting_section">
                <div class="confirm_betting_item">
                  <div class="confirm_betting_text">
                    合計<span>{{ betCount }}</span
                    >注
                  </div>
                  <div class="confirm_betting_text">
                    金额<span>{{ totalAmount }}</span
                    >元
                  </div>
                </div>
                <div class="confirm_betting_item">
                  <div class="confirm_betting_text">
                    余额<span>{{ formattedBalance }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="confirm_betting_inner" @click="confirmBetAsync">
            <div class="confirm_betting_betbtn"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="confirm_block" v-if="hasIOS"></div>
  <!-- 確認投注 end -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Balance } from "@/mixins";
import toast from "@/toast";
import promptDialog from "@/promptDialog";
import betSuccessDialog from "@/betSuccessDialog";
import {
  MsSetting,
  ChangLongSelectedItem,
  ClockViewModel,
  BetInfo,
} from "@/models";
import { MutationType } from "@/store";
import api from "@/api";
import { AssetImage } from "../shared";
import BaseDialog from "./BaseDialog";
import ClockView from "../shared/ClockView.vue";
import { isIOS } from "@/gameConfig";
import LotteryIcons from "../LotteryIcons";

export default defineComponent({
  extends: BaseDialog,
  props: {
    isDocumentary: {
      type: Object as () => boolean,
    },
  },
  mixins: [Balance],
  components: { AssetImage, ClockView, LotteryIcons },
  data() {
    return {
      currentMultiple: 0,
      showPrompt: false,
      betting: false, //修正連續點擊會多送出投注的問題
      clockClass: {
        contentClassName: "confirm_content",
        tendigitsClassName: "confirm_digits",
        digitsClassName: "confirm_digits",
        textClassName: "digits_text",
      } as ClockViewModel,
    };
  },
  methods: {
    showBetAmount(item: BetInfo) {
      return item.betAmount || this.betBaseAmount;
    },
    navigateToBet() {
      this.closeEvent();
    },
    isCurrentMultiple(multiple: number) {
      return this.currentMultiple === multiple;
    },
    changeCurrentMultiple(multiple: number) {
      this.currentMultiple = multiple;
      this.showPrompt = false;
    },
    removeBetInfo(id: string) {
      if (this.betInfo.length === 1) {
        toast("必须至少保留一个投注，无法全部移除");
        return;
      }

      this.$store.commit(MutationType.RemoveCurrentBetInfoById, id);
    },
    async confirmBetAsync() {
      if (!this.issueNo.currentIssueNo) {
        toast("本期封盘中，请等下期");
        return;
      }
      if (this.betting) {
        return;
      }
      this.betting = true;
      try {
        let playTypeData = this.betLotteryPlayTypeData();
        let submitParams = {
          amount: this.betCount,
          currencyUnit: "1",
          currentIssueNo: this.issueNo.currentIssueNo,
          habitRebatePro: this.$store.getters.currentRebatePro?.value
            .rebate as number,
          lotteryId: this.betLotteryId,
          playType: playTypeData.playTypeId,
          playTypeName: playTypeData.playTypeName,
          playTypeRadio: playTypeData.playTypeRadioId,
          playTypeRadioName: playTypeData.playTypeRadioName,
          price: this.betBaseAmount.toString(),
          ratio: 1,
          selectedNums: this.betInfo
            .map((x) => `${x.playTypeRadioName} ${x.selectedBetNumber}`)
            .join("|"),
          roomId: "0",
        };

        this.$store.commit(MutationType.SetIsLoading, true);
        await api.postOrderAsync(submitParams);
        this.setBetsLog();
        betSuccessDialog();
      } catch (error) {
        const message =
          error instanceof Error ? (error as Error).message : error;
        this.showPrompt = (message as string).indexOf("不足") >= 0;
        if (this.showPrompt)
          //弹框
          promptDialog(
            message as string,
            this.msSetting.depositUrl,
            this.msSetting.logonMode
          );
        else toast(message as string);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
        if (!this.showPrompt) {
          this.$store.commit(MutationType.SetCurrentBetInfo, []);
          this.$store.commit(MutationType.SetNumbers, null);
          this.$store.commit(
            MutationType.SetChangLongNumbers,
            {} as ChangLongSelectedItem
          );
          this.$store.commit(MutationType.ReloadBetHistory, true);
          this.navigateToBet();
          this.refreshBalanceAsync();
        } else {
          this.betting = false;
        }
      }
    },
    betLotteryTypeName() {
      let currentBetInfo = this.betInfo;
      let lotteryTypeName = this.$store.state.lotteryInfo.lotteryTypeName;
      if (currentBetInfo && currentBetInfo.length > 0) {
        lotteryTypeName = currentBetInfo[0].lotteryTypeName ?? lotteryTypeName;
      }
      return lotteryTypeName;
    },
    betGameTypeName() {
      let currentBetInfo = this.betInfo;
      let gameTypeName = this.$store.state.lotteryInfo.gameTypeName;
      if (currentBetInfo && currentBetInfo.length > 0) {
        gameTypeName = currentBetInfo[0].gameTypeName ?? gameTypeName;
      }
      return gameTypeName;
    },
    betGameTypeId() {
      let currentBetInfo = this.betInfo;
      let gameTypeId = this.$store.state.lotteryInfo.gameTypeId;
      if (currentBetInfo && currentBetInfo.length > 0) {
        gameTypeId = currentBetInfo[0].gameTypeId ?? gameTypeId;
      }
      return gameTypeId;
    },
    betLotteryPlayTypeData() {
      let playTypeId = this.$store.getters.currentPlayType?.info
        .playTypeID as number;
      let playTypeName = this.$store.getters.currentPlayType?.info
        .playTypeName as string;
      let playTypeRadioId = this.$store.getters.currentPlayTypeRadio?.info
        .playTypeRadioID as number;
      let playTypeRadioName = this.$store.getters.currentPlayTypeRadio?.info
        .playTypeRadioName as string;

      let currnetBetInfo = this.betInfo;
      if (currnetBetInfo.length > 0) {
        let betLottery = currnetBetInfo[0];
        if (betLottery.lotteryId) {
          let allRebatePros = this.$store.getters.allRebatePros;
          let playType = allRebatePros[betLottery.lotteryId].find((x) => {
            return x.lotteryId === betLottery.lotteryId;
          });
          if (playType) {
            playTypeId = playType?.playTypeId as number;
            playTypeName = "";
            playTypeRadioId = playType?.playTypeRadioId as number;
            playTypeRadioName = "";
          }
        }
      }

      return {
        playTypeId: playTypeId,
        playTypeName: playTypeName,
        playTypeRadioId: playTypeRadioId,
        playTypeRadioName: playTypeRadioName,
      };
    },
    setBetsLog() {
      let betsLog = this.$store.state.betsLog;
      for (let item of this.betInfo) {
        let numberOdds = betsLog[0].numberOdds;
        let logsDetail = numberOdds.find(
          (x) => x.category === item.playTypeRadioName
        );
        if (logsDetail) {
          logsDetail.count++;
          let valueDetail = logsDetail.values.find(
            (x) => x.value === item.selectedBetNumber
          );
          if (valueDetail) {
            const numbers = Number(valueDetail.totalAmount);
            valueDetail.totalAmount = `${numbers + this.betBaseAmount}`;
          }
        }
      }
      this.$store.commit(MutationType.SetBetsLog, betsLog);
    },
  },
  created() {
    this.currentMultiple = this.multiples[0];
  },
  mounted() {
    if (!this.betInfo.length) this.navigateToBet();
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    betLotteryId() {
      let currnetBetInfo = this.betInfo;
      let betLotteryId = currnetBetInfo[0].lotteryId;
      return betLotteryId
        ? betLotteryId
        : this.$store.state.lotteryInfo.lotteryId;
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
    betInfo() {
      return this.$store.state.currnetBetInfo.filter((x) => x);
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
    msSetting(): MsSetting {
      return this.$store.state.msSetting;
    },
    hasIOS() {
      return isIOS;
    },
  },
});
</script>
