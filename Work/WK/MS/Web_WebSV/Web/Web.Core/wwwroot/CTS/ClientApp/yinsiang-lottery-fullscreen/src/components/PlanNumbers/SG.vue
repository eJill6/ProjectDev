<template>
  <div class="nuinui_inner">
    <div class="nuinui_content mb0">
      <!-- 獲勝的那方才顯示 -->
      <div class="flag_blue spacing" v-if="currentPoker.player.isWin"></div>
      <div class="flag_red spacing" v-if="currentPoker.banker.isWin"></div>
      <!-- 闲 -->
      <div class="blue baccarat plan">
        <div class="blue_inner ani spacing center">
          <div class="bg_result baccarat">
            <div
              :data-text="currentPoker.player.point + `点`"
              :class="playerResult() ? 'win' : 'lose'"
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
          <div class="title lg unset" data-text="闲">闲</div>
          <div class="poker_baccarat">
            <div class="piece">
              <AssetImage :src="getImageUrl(currentPoker.player.cards[0])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(currentPoker.player.cards[1])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(currentPoker.player.cards[2])" />
            </div>
          </div>
        </div>
      </div>
      <div class="versus">
        <AssetImage src="@/assets/images/game/img_versus.png" />
      </div>
      <!-- 庄 -->
      <div class="red baccarat plan">
        <div class="red_inner ani spacing center">
          <div class="bg_result baccarat">
            <div
              :class="bankerResult() ? 'win' : 'lose'"
              :data-text="currentPoker.banker.point + `点`"
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
          <div class="title lg unset" data-text="庄">庄</div>
          <div class="poker_baccarat">
            <div class="piece">
              <AssetImage :src="getImageUrl(currentPoker.banker.cards[0])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(currentPoker.banker.cards[1])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(currentPoker.banker.cards[2])" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";
import { JSSG, JSSgPoker } from "../../GameRules/JSSgRule";
import { PokerCard, CardSuit } from "../../GameRules/BasePokerRule";
import { IssueHistory } from "@/models/IssueHistory";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Recently],
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
    getImageUrl(card: PokerCard): string {
      if (card.originalNumber == "0") {
        return "@/assets/images/poker_baccarat/poker_baccarat_default.png";
      }
      let rule = new JSSG();
      let cardType = rule.getCardType(`${card.pokerNumber}`).toUpperCase();
      return `@/assets/images/poker_baccarat/${
        CardSuit[card.suit]
      }_baccarat_${cardType}.png`;
    },
  },
  computed: {
    hasDrawNumbers(): boolean {
      return !!this.$store.state.issueNo.lastDrawNumber;
    },
    allDrawNumbers() {
      return this.hasDrawNumbers
        ? this.currentDrawNumbers
        : this.lastDrawNumbers;
    },
    currentPoker(): ShowIssueHistory {
      let rule = new JSSG();

      let pokers = rule.confirmResult(this.allDrawNumbers);
      let result: ShowIssueHistory = {
        player: pokers[0],
        banker: pokers[1],
        issueNo: "",
        drawNumbers: this.allDrawNumbers,
      };
      return result;
    },
  },
});
export interface ShowIssueHistory extends IssueHistory {
  player: JSSgPoker;
  banker: JSSgPoker;
}
</script>
