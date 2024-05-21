<template>
  <div class="player">
    <div class="inner">
      <div class="wrapper">
        <div class="title" data-text="闲">闲</div>
        <div class="result">
          <div
            :class="playerResult() ? 'win' : 'lose'"
            :data-text="`${item.player.point}点`"
          >
            {{ item.player.point }}点
          </div>
          <div
            class="win"
            v-if="playerVictoryString()"
            :data-text="playerVictoryString()"
          >
            {{ playerVictoryString() }}
          </div>
        </div>
      </div>
      <div class="poker left">
        <div
          class="vertical turnleft"
          v-if="isPokerDraw(item.player.cards[2].originalNumber)"
        >
          <AssetImage
            :src="`@/assets/images/poker_card/${getDiceClassName(
              item.player.cards[2]
            )}.png`"
          />
        </div>
        <div class="vertical">
          <AssetImage
            :src="`@/assets/images/poker_card/${getDiceClassName(
              item.player.cards[1]
            )}.png`"
          />
        </div>
        <div class="vertical">
          <AssetImage
            :src="`@/assets/images/poker_card/${getDiceClassName(
              item.player.cards[0]
            )}.png`"
          />
        </div>
      </div>
    </div>
  </div>
  <div class="issue">{{ item.issueNo }}</div>
  <div class="versus">
    <AssetImage src="@/assets/images/record/img_versus.png" />
  </div>
  <div class="bank">
    <div class="inner">
      <div class="poker right">
        <div class="vertical">
          <AssetImage
            :src="`@/assets/images/poker_card/${getDiceClassName(
              item.banker.cards[0]
            )}.png`"
          />
        </div>
        <div class="vertical">
          <AssetImage
            :src="`@/assets/images/poker_card/${getDiceClassName(
              item.banker.cards[1]
            )}.png`"
          />
        </div>
        <div
          class="vertical turnright"
          v-if="isPokerDraw(item.banker.cards[2].originalNumber)"
        >
          <AssetImage
            :src="`@/assets/images/poker_card/${getDiceClassName(
              item.banker.cards[2]
            )}.png`"
          />
        </div>
      </div>
      <div class="wrapper">
        <div class="title" data-text="庄">庄</div>
        <div class="result">
          <div
            :class="bankerResult() ? 'win' : 'lose'"
            :data-text="`${item.banker.point}点`"
          >
            {{ item.banker.point }}点
          </div>
          <div
            class="win"
            v-if="bankerVictoryString()"
            :data-text="bankerVictoryString()"
          >
            {{ bankerVictoryString() }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "../shared/AssetImage.vue";
import { BaccaratIssueModel } from "@/models";
import { CardSuit, PokerCard } from "@/GameRules/BasePokerRule";

export default defineComponent({
  components: { AssetImage },
  props: {
    item: {
      type: Object as () => BaccaratIssueModel,
      required: true,
    },
  },
  methods: {
    getDiceClassName(item: PokerCard) {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `${CardSuit[item.suit]}_${item.type.toUpperCase()}`;
    },
    isPokerDraw(pokerNumber: string): boolean {
      return pokerNumber !== "0";
    },
    playerResult() {
      return (
        this.item.player.isWin ||
        (!this.item.player.isWin && !this.item.banker.isWin)
      );
    },
    bankerResult() {
      return (
        this.item.banker.isWin ||
        (!this.item.player.isWin && !this.item.banker.isWin)
      );
    },
    playerVictoryString() {
      if (this.item.player.isWin) {
        return "闲赢";
      } else if (!this.item.player.isWin && !this.item.banker.isWin) {
        return "和";
      }
      return "";
    },
    bankerVictoryString() {
      if (this.item.banker.isWin) {
        return "庄赢";
      } else if (!this.item.player.isWin && !this.item.banker.isWin) {
        return "和";
      }
      return "";
    },
  },
});
</script>
