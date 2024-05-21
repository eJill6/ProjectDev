<template>
  <div class="lotteryrecord_content">
    <template v-for="item in getAllPokers()">
      <div class="nuinui_row">
        <!-- 闲 -->
        <div class="blue baccarat md">
          <div class="blue_inner">
            <div class="item">
              <div class="title md" data-text="闲">闲</div>
              <div class="bg_result md">
                <div
                  class="md"
                  :data-text="playerPointResult(item)"
                  :class="playerResult(item) ? 'win' : 'lose'"
                >
                  {{ playerPointResult(item) }}
                </div>
                <div
                  class="win md"
                  :data-text="playerVictoryString(item)"
                  v-if="playerVictoryString(item)"
                >
                  {{ playerVictoryString(item) }}
                </div>
              </div>
            </div>
            <div class="poker_baccarat md">
              <div class="piece md">
                <AssetImage :src="getImageUrl(item.player.cards[0])" />
              </div>
              <div class="piece md">
                <AssetImage :src="getImageUrl(item.player.cards[1])" />
              </div>
              <div class="piece md">
                <AssetImage :src="getImageUrl(item.player.cards[2])" />
              </div>
            </div>
          </div>
        </div>
        <div class="issue md">{{ item.issueNo }}</div>
        <div class="versus spacing">
          <AssetImage src="@/assets/images/game/img_versus.png" />
        </div>
        <!-- 庄 -->
        <div class="red baccarat md">
          <div class="red_inner">
            <div class="poker_baccarat md">
              <div class="piece md">
                <AssetImage :src="getImageUrl(item.banker.cards[0])" />
              </div>
              <div class="piece md">
                <AssetImage :src="getImageUrl(item.banker.cards[1])" />
              </div>
              <div class="piece md">
                <AssetImage :src="getImageUrl(item.banker.cards[2])" />
              </div>
            </div>
            <div class="item">
              <div class="title md" data-text="庄">庄</div>
              <div class="bg_result md">
                <div
                  class="md"
                  :data-text="bankerPointResult(item)"
                  :class="bankerResult(item) ? 'win' : 'lose'"
                >
                  {{ bankerPointResult(item) }}
                </div>
                <div
                  class="win md"
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

      <div class="history_line spacing"></div>
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";
import { JSSgPoker, JSSG } from "../../GameRules/JSSgRule";
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
    playerPointResult(item: ShowIssueHistory) {
      const items = item.player.cards.filter((x) => x.pokerNumber > 10);
      return items.length === 3 ? `三公` : `${item.player.point}点`;
    },
    bankerPointResult(item: ShowIssueHistory) {
      const items = item.banker.cards.filter((x) => x.pokerNumber > 10);
      return items.length === 3 ? `三公` : `${item.banker.point}点`;
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
        let rule = new JSSG();
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
      let rule = new JSSG();
      let cardType = rule.getCardType(`${card.pokerNumber}`).toUpperCase();
      return `@/assets/images/poker_baccarat/${
        CardSuit[card.suit]
      }_baccarat_${cardType}.png`;
    },
  },
});
export interface ShowIssueHistory extends IssueHistory {
  player: JSSgPoker;
  banker: JSSgPoker;
}
</script>
