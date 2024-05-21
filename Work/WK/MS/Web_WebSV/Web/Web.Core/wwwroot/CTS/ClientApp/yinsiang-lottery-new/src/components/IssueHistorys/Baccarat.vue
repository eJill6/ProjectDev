<template>
  <template v-for="item in itemList">
    <div class="history_row">
      <PlanNumbers :item="item"></PlanNumbers>
    </div>
    <div class="history_line" :class="{ mb2: isFull }"></div>
  </template>
</template>
<script lang="ts">
import { defineComponent, reactive } from "vue";
import AssetImage from "../shared/AssetImage.vue";
import { BaccaratIssueModel, IssueHistory } from "@/models";
import { Baccarat } from "@/GameRules/JSBaccaratRule";
import { CardSuit, PokerCard } from "@/GameRules/BasePokerRule";
import PlanNumbers from "../PlanNumbers";

const gameData = new Baccarat();

export default defineComponent({
  components: { AssetImage, PlanNumbers },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
    isFull: Boolean,
  },
  methods: {
    sortIssue(info: IssueHistory): BaccaratIssueModel {
      const result = gameData.confirmResult(info.drawNumbers);
      return {
        issueNo: info.issueNo,
        player: reactive(result[0]),
        banker: reactive(result[1]),
      };
    },
    getDiceClassName(item: PokerCard) {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `${CardSuit[item.suit]}_${item.type.toUpperCase()}`;
    },
  },
  computed: {
    itemList() {
      return this.list.map((item) => this.sortIssue(item));
    },
  },
});
</script>
