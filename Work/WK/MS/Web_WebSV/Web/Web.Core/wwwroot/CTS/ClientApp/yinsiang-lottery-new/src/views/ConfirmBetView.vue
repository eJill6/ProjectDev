<template>
  <div class="confirm_main">
    <!-- 確認投注 start -->
    <div class="confirm_wrapper">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToBet">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="确认投注">确认投注</div>
        </div>
        <div class="setting_wrapper pd_0 flex_second_height spacing">
          <div class="confirm_up">
            <div class="confirm_infos">
              <div class="confirm_gameimg" :class="logoClassName">
                <AssetImage :src="logoUrl" />
              </div>
              <!-- <div class="confirm_gameimg roulette">
                <AssetImage src="@/assets/images/modal/logo_roulette_h.png" />
              </div> -->
              <div class="confirm_num">第 {{ issueNo.currentIssueNo }} 期</div>
            </div>
            <CountdownView></CountdownView>
          </div>
          <div class="confirm_middle flex_second_height col">
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
            <div class="confirm_list_outter flex_second_height">
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
                    <div class="confirm_text">{{ betBaseAmount }}</div>
                  </div>
                  <div class="confirm_item" @click="removeBetInfo(info.id)">
                    <div class="confirm_icon">
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
        </div>
        <div class="confirm_betting_outter">
          <div class="confirm_betting_inner">
            <div class="confirm_betting_content">
              <div class="confirm_betting_money">
                <AssetImage src="@/assets/images/modal/ic_betting_money.png" />
              </div>
              <div class="confirm_betting_section">
                <div class="confirm_betting_item">
                  <div class="confirm_betting_text">
                    合计<span>{{ betCount }}</span
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
    <!-- 確認投注 end -->
  </div>
  <div class="popup_main" v-if="showPrompt">
    <!-- 前往充值 start -->
    <div class="popup_wrapper">
      <div class="popup_outter">
        <div class="popup_headerimg">
          <AssetImage src="@/assets/images/modal/img_popup_recharge.png" />
        </div>
        <div class="close" @click="showPrompt = false">
          <AssetImage src="@/assets/images/modal/ic_modal_close.png" alt="" />
        </div>
        <div class="popup_inner">
          <p class="popup_title">提示</p>
          <p class="popup_text">{{ promptMessage }}</p>
        </div>
        <div class="popup_btns">
          <div class="btn_default basis_50" @click="showPrompt = false">
            取消
          </div>
          <div class="btn_default basis_50 confirm" @click="goDepositUrl">
            确定
          </div>
        </div>
      </div>
    </div>
    <!-- 前往充值 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Balance, MqEvent } from "@/mixins";
import DialogControl from "@/mixins/DialogControl";
import toast from "@/toast";
import { event as eventModel, MsSetting } from "@/models";
import { MutationType } from "@/store";
import event from "@/event";
import api from "@/api";
import AssetImage from "@/components/shared/AssetImage.vue";
import { CountdownView } from "@/components";
import messageURL from "@/message";

export default defineComponent({
  props: {
    isDocumentary: {
      type: Object as () => boolean,
    },
  },
  mixins: [Balance, MqEvent, DialogControl],
  components: { AssetImage, CountdownView },
  data() {
    return {
      currentMultiple: 0,
      showPrompt: false,
      betting: false, //修正連續點擊會多送出投注的問題
      promptMessage: "",
    };
  },
  methods: {
    openUrl() {},
    navigateToBet() {
      // this.$store.commit(MutationType.SetPlayType, 0);
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
      this.removeBetViewSelectedItem(id);
      this.$store.commit(MutationType.RemoveCurrentBetInfoById, id);
    },
    removeBetViewSelectedItem(id: string) {
      const info = this.$store.state.currnetBetInfo.find(
        (item) => item.id === id
      );
      if (!info || !info.playTypeRadioName) {
        return;
      }
      this.$store.state.selectedNumbers[info.playTypeRadioName].forEach(
        (playTypeRadio, index) => {
          const selectedIndex = playTypeRadio.findIndex(
            (selectedName) => selectedName === info.selectedBetNumber
          );
          if (selectedIndex > -1) {
            this.$store.state.selectedNumbers[info.playTypeRadioName][
              index
            ].splice(selectedIndex, 1, "");
          }
        }
      );
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
      const isEmptyRoomId = !Number(this.$store.state.msSetting.roomId);
      const roomId = isEmptyRoomId ? "1" : this.$store.state.msSetting.roomId;
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
          roomId: roomId,
        };
        this.$store.commit(MutationType.SetIsLoading, true);
        let result = await api.postOrderAsync(submitParams);

        this.showSuccessfulView();
      } catch (error) {
        const message =
          error instanceof Error ? (error as Error).message : error;
        this.showPrompt = (message as string).indexOf("不足") >= 0;
        if (!this.showPrompt) {
          toast(message as string);
        } else {
          this.promptMessage = message as string;
        }
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
        if (!this.showPrompt) {
          this.$store.commit(MutationType.SetCurrentBetInfo, []);
          this.$store.commit(MutationType.SetNumbers, null);
          this.navigateToBet();
        } else {
          this.betting = false;
        }
      }
    },
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.rouletteHandle(arg);
    },
    onIssueNoChanged(arg: eventModel.IssueNoChangedArg) {
      let message = `${this.lotteryInfo.lotteryTypeName} 期號已变更为${arg.issueNo}`;
      toast(message);
    },
    goDepositUrl() {
      messageURL.openUrl(
        this.msSetting.logonMode,
        this.msSetting.depositUrl,
        null
      );
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
    msSetting(): MsSetting {
      return this.$store.state.msSetting;
    },
    logoClassName() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      switch (typeName.toLocaleLowerCase()) {
        case "lp":
          return "roulette";
        case "yxx":
          return "yusiasieh";
        default:
          return "";
      }
    },
    logoUrl() {
      let typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      let iconImg = "";
      if (typeName) {
        iconImg = typeName.toLocaleLowerCase();
        switch (iconImg) {
          case "lp":
            iconImg = "roulette";
            break;
          case "yxx":
            iconImg = "yusiasieh";
            break;
        }
        iconImg = `@/assets/images/modal/logo_${iconImg.toLocaleLowerCase()}_h.png`;
      }
      return iconImg;
    },
  },
});
</script>
