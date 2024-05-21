<template>
  <div class="kuaisan_inner height">
    <div class="nuinui_content">
      <!-- 獲勝的那方才顯示 -->
      <div
        class="flag_blue"
        v-if="currentPoker.player.isWin && stages === CardStages.show"
      ></div>
      <div
        class="flag_red"
        v-if="currentPoker.banker.isWin && stages === CardStages.show"
      ></div>
      <template
        v-if="
          currentPoker.player.cards.length && currentPoker.banker.cards.length
        "
      >
        <!-- 闲 -->
        <div class="blue baccarat">
          <div class="blue_inner ani">
            <div class="bg_result baccarat" v-if="stages === CardStages.show">
              <div
                :class="playerResult() ? 'win' : 'lose'"
                :data-text="currentPoker.player.point + '点'"
              >
                {{ currentPoker.player.point }}点
              </div>
              <div
                class="win"
                :data-text="playerVictoryString()"
                v-if="playerVictoryString()"
              >
                {{ playerVictoryString() }}
              </div>
            </div>
            <div class="title lg" data-text="闲">闲</div>

            <!--發牌動畫 左邊兩張開始-->
            <div
              class="piece_ani"
              v-if="stages === CardStages.deal"
              v-for="n in [1, 2]"
            >
              <div :class="`piece_left_0${n}`">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--發牌動畫 左邊兩張結束-->

            <!--發牌動畫 左邊補牌 開始-->
            <div
              class="piece_ani"
              v-if="
                currentPoker.player.cards[2].originalNumber !== `0` &&
                stages === CardStages.leftDraw
              "
            >
              <div class="piece_left_03">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--發牌動畫 左邊補牌 結束-->

            <!--翻牌動畫 開始-->
            <div class="piece_ani" v-if="stages >= CardStages.flop">
              <div
                class="piece_left_result_01"
                :style="stages === CardStages.show ? resultPokerStopCss : {}"
              >
                <AssetImage :src="getImageUrl(currentPoker.player.cards[0])" />
              </div>
              <div
                class="piece_left_back_01"
                :style="stages === CardStages.show ? backPokerStopCss : {}"
              >
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>

              <div
                class="piece_left_result_02"
                :style="stages === CardStages.show ? resultPokerStopCss : {}"
              >
                <AssetImage :src="getImageUrl(currentPoker.player.cards[1])" />
              </div>
              <div
                class="piece_left_back_02"
                :style="stages === CardStages.show ? backPokerStopCss : {}"
              >
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--翻牌動畫 結束-->

            <!--翻牌補牌動畫 開始-->
            <div
              class="piece_ani"
              v-if="
                stages >= CardStages.leftFlopDraw &&
                currentPoker.player.cards[2].originalNumber !== `0`
              "
            >
              <div
                class="piece_left_result_03"
                :style="stages === CardStages.show ? resultPokerStopCss : {}"
              >
                <AssetImage :src="getImageUrl(currentPoker.player.cards[2])" />
              </div>
              <div
                class="piece_left_back_03"
                :style="stages === CardStages.show ? backPokerStopCss : {}"
              >
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--翻牌補牌動畫 結束-->

            <div class="poker_baccarat">
              <div class="piece left">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_default.png"
                />
              </div>
              <div class="piece">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_default.png"
                />
              </div>
              <div class="piece">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_default.png"
                />
              </div>
            </div>
          </div>
        </div>
        <div class="versus">
          <AssetImage src="@/assets/images/game/img_versus.png" />
        </div>
        <!-- 庄 -->
        <div class="red baccarat">
          <div class="red_inner ani">
            <div class="bg_result baccarat" v-if="stages === CardStages.show">
              <div
                :class="bankerResult() ? 'win' : 'lose'"
                :data-text="currentPoker.banker.point + '点'"
              >
                {{ currentPoker.banker.point }}点
              </div>
              <div
                class="win"
                :data-text="bankerVictoryString()"
                v-if="bankerVictoryString()"
              >
                {{ bankerVictoryString() }}
              </div>
            </div>
            <div class="title lg" data-text="庄">庄</div>

            <!--發牌動畫 右邊兩張開始-->
            <div
              class="piece_ani"
              v-if="stages == CardStages.deal"
              v-for="n in [1, 2]"
            >
              <div :class="`piece_right_0${n}`">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--發牌動畫 右邊兩張結束-->

            <!--發牌動畫 右邊補牌 開始-->
            <div
              class="piece_ani"
              v-if="
                currentPoker.banker.cards[2].originalNumber !== `0` &&
                stages == CardStages.rightDraw
              "
            >
              <div class="piece_right_03">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--發牌動畫 右邊補牌 結束-->

            <!--翻牌動畫 開始-->
            <div class="piece_ani" v-if="stages >= CardStages.flop">
              <div
                class="piece_right_result_01"
                :style="stages === CardStages.show ? resultPokerStopCss : {}"
              >
                <AssetImage :src="getImageUrl(currentPoker.banker.cards[0])" />
              </div>
              <div
                class="piece_right_back_01"
                :style="stages === CardStages.show ? backPokerStopCss : {}"
              >
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>

              <div
                class="piece_right_result_02"
                :style="stages === CardStages.show ? resultPokerStopCss : {}"
              >
                <AssetImage :src="getImageUrl(currentPoker.banker.cards[1])" />
              </div>
              <div
                class="piece_right_back_02"
                :style="stages === CardStages.show ? backPokerStopCss : {}"
              >
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--翻牌動畫 結束-->

            <!--翻牌補牌動畫 開始-->
            <div
              class="piece_ani"
              v-if="
                currentPoker.banker.cards[2].originalNumber !== `0` &&
                stages >= CardStages.rightFlopDraw
              "
            >
              <div
                class="piece_right_result_03"
                :style="stages === CardStages.show ? resultPokerStopCss : {}"
              >
                <AssetImage :src="getImageUrl(currentPoker.banker.cards[2])" />
              </div>
              <div
                class="piece_right_back_03"
                :style="stages === CardStages.show ? backPokerStopCss : {}"
              >
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_back.png"
                />
              </div>
            </div>
            <!--翻牌補牌動畫 結束-->

            <div class="poker_baccarat">
              <div class="piece">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_default.png"
                />
              </div>
              <div class="piece">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_default.png"
                />
              </div>
              <div class="piece right">
                <AssetImage
                  src="@/assets/images/poker_baccarat/poker_baccarat_default.png"
                />
              </div>
            </div>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { WinNumbers } from "@/mixins";
import { AssetImage } from "@/components/shared";
import { Baccarat, JSBaccaratPoker } from "../../../GameRules/JSBaccaratRule";
import { PokerCard, CardSuit } from "../../../GameRules/BasePokerRule";
import { TimeRules } from "@/enums/TimeRules";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Layout, WinNumbers.Scroll],
  data() {
    let stages = ref(CardStages.deal);
    return {
      stages,
      CardStages,
      resultPokerStopCss: {
        animation: "unset",
        opacity: "100%",
      },
      backPokerStopCss: {
        animation: "unset",
        opacity: "0%",
      },
    };
  },
  watch: {
    drawNumbers: {
      handler() {
        this.start();
      },
      deep: true,
    },
  },
  methods: {
    playerResult() {
      return (
        this.currentPoker.player.isWin ||
        (!this.currentPoker.player.isWin && !this.currentPoker.banker.isWin)
      );
    },
    bankerResult() {
      return (
        this.currentPoker.banker.isWin ||
        (!this.currentPoker.player.isWin && !this.currentPoker.banker.isWin)
      );
    },
    playerVictoryString() {
      if (this.currentPoker.player.isWin) {
        return "闲赢";
      } else if (
        !this.currentPoker.player.isWin &&
        !this.currentPoker.banker.isWin
      ) {
        return "和";
      }
      return "";
    },
    bankerVictoryString() {
      if (this.currentPoker.banker.isWin) {
        return "庄赢";
      } else if (
        !this.currentPoker.player.isWin &&
        !this.currentPoker.banker.isWin
      ) {
        return "和";
      }
      return "";
    },
    start() {
      const isDealEvent = this.drawNumbers.every((value) => value === "0");
      const isFlopEvent = this.drawNumbers.some((value) => value !== "0");
      if (isDealEvent) {
        this.dealPoker();
      } else if (isFlopEvent) {
        this.flopCard();
      }
    },
    getImageUrl(card: PokerCard): string {
      if (card.originalNumber == "0") {
        return "@/assets/images/poker_baccarat/poker_baccarat_default.png";
      }
      let rule = new Baccarat();
      let cardType = rule.getCardType(`${card.pokerNumber}`).toUpperCase();
      return `@/assets/images/poker_baccarat/${
        CardSuit[card.suit]
      }_baccarat_${cardType}.png`;
    },
    getWinLose(iswin: boolean): string {
      return iswin ? "win" : "lose";
    },
    async delay(ms: number) {
      return new Promise((resolve) => setTimeout(resolve, ms));
    },
    async flopCard() {
      this.stages = CardStages.flop;
      await this.delay(600);
      if (
        this.currentPoker.banker.cards[2].originalNumber === "0" &&
        this.currentPoker.player.cards[2].originalNumber === "0"
      ) {
        this.flopDrawCard();
      } else {
        this.drawCard();
      }
    },
    async dealPoker() {
      this.stages = CardStages.deal;
    },
    async drawCard() {
      this.stages = CardStages.leftDraw;
      await this.delay(100);
      this.stages = CardStages.leftFlopDraw;
      await this.delay(300);
      this.stages = CardStages.rightDraw;
      await this.delay(300);
      this.stages = CardStages.rightFlopDraw;
      await this.delay(300);
      this.flopDrawCard();
    },
    async flopDrawCard() {
      this.stages = CardStages.show;
    },
  },
  mounted() {
    const isShowIssueNo =
      this.$store.getters.showTimeRuleStatus === TimeRules.issueNoCountdown;
    if (isShowIssueNo) {
      this.flopDrawCard();
    }
  },
  computed: {
    hasDrawNumbers(): boolean {
      if (this.currentDrawNumbers.length > 0) {
        return true;
      }
      return false;
    },
    drawNumbers() {
      return this.hasDrawNumbers
        ? this.currentDrawNumbers
        : this.$_gameTypeDrawNumbers;
    },
    currentPoker(): ShowPoker {
      let rule = new Baccarat();
      let pokers = rule.confirmResult(this.drawNumbers);
      return {
        player: pokers[0],
        banker: pokers[1],
      };
    },
    $_gameTypeDrawNumbers(): string[] {
      return ["0", "0", "0", "0", "0", "0"];
    },
    $_gameTypeDrawNumberCount(): number {
      return 6;
    },
  },
});
export interface ShowPoker {
  player: JSBaccaratPoker;
  banker: JSBaccaratPoker;
}
export enum CardStages {
  /**發牌 */
  deal = 0,
  /**翻牌 */
  flop,
  /**左邊補牌 */
  leftDraw,
  /**左邊翻牌 */
  leftFlopDraw,
  /**右邊補牌 */
  rightDraw,
  /**右邊翻牌 */
  rightFlopDraw,
  /**呈現結果 */
  show,
}
</script>
