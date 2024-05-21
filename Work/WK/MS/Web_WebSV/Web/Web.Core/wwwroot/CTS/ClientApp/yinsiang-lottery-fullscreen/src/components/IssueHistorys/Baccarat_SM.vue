<template>
  <template v-for="item in getAllPokers()">
    <div class="nuinui_row">
      <!-- 闲 -->
      <div class="blue baccarat">
        <div class="blue_inner">
          <div class="item">
            <div class="title" data-text="闲">闲</div>
            <div class="bg_result">
              <div
                :data-text="item.player.point + `点`"
                :class="playerResult(item) ? 'win' : 'lose'"
              >
                {{ item.player.point }}点
              </div>
              <div
                class="win"
                :data-text="playerVictoryString(item)"
                v-if="playerVictoryString(item)"
              >
                {{ playerVictoryString(item) }}
              </div>
            </div>
          </div>
          <div class="poker_baccarat">
            <div class="piece left">
              <AssetImage :src="getImageUrl(item.player.cards[2])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(item.player.cards[0])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(item.player.cards[1])" />
            </div>
          </div>
        </div>
      </div>
      <div class="issue">{{ item.issueNo }}</div>
      <div class="versus">
        <AssetImage src="@/assets/images/game/img_versus.png" />
      </div>
      <!-- 庄 -->
      <div class="red baccarat">
        <div class="red_inner">
          <div class="poker_baccarat">
            <div class="piece">
              <AssetImage :src="getImageUrl(item.banker.cards[0])" />
            </div>
            <div class="piece">
              <AssetImage :src="getImageUrl(item.banker.cards[1])" />
            </div>
            <div class="piece right">
              <AssetImage :src="getImageUrl(item.banker.cards[2])" />
            </div>
          </div>
          <div class="item">
            <div class="title" data-text="庄">庄</div>
            <div class="bg_result">
              <div
                :class="bankerResult(item) ? 'win' : 'lose'"
                :data-text="item.banker.point + `点`"
              >
                {{ item.banker.point }}点
              </div>
              <div
                class="win"
                :data-text="bankerVictoryString(item)"
                v-if="bankerVictoryString(item)"
              >
                {{ bankerVictoryString(item) }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="history_line"></div>
  </template>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";
import { Baccarat, JSBaccaratPoker } from "../../GameRules/JSBaccaratRule";
import { PokerCard, CardSuit } from "../../GameRules/BasePokerRule";

export default defineComponent({
  components: { AssetImage },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
  methods: {
    playerResult(item: ShowIssueHistory) {
      return item.player.isWin || (!item.player.isWin && !item.banker.isWin);
    },
    bankerResult(item: ShowIssueHistory) {
      return item.banker.isWin || (!item.player.isWin && !item.banker.isWin);
    },
    playerVictoryString(item: ShowIssueHistory) {
      if (item.player.isWin) {
        return "闲赢";
      } else if (!item.player.isWin && !item.banker.isWin) {
        return "和";
      }
      return "";
    },
    bankerVictoryString(item: ShowIssueHistory) {
      if (item.banker.isWin) {
        return "庄赢";
      } else if (!item.player.isWin && !item.banker.isWin) {
        return "和";
      }
      return "";
    },
    getAllPokers(): ShowIssueHistory[] {
      return this.list.map((x) => {
        let rule = new Baccarat();
        let pokers = rule.confirmResult(x.drawNumbers);
        let result: ShowIssueHistory = {
          player: pokers[0],
          banker: pokers[1],
          issueNo: x.issueNo,
          drawNumbers: x.drawNumbers,
        };
        return result;
      });
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
  },
});
export interface ShowIssueHistory extends IssueHistory {
  player: JSBaccaratPoker;
  banker: JSBaccaratPoker;
}
</script>
