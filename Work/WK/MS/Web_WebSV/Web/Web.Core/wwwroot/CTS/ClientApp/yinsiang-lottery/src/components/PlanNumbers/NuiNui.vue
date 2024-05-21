<template>
  <div class="nuinui_result bet-nuinui-table" v-if="gameData.length">
    <div class="result_blue">
      <div class="poker_group d-flex justify-content-between">
        <div
          class="poker_card"
          v-for="item in gameData[0].cards"
          :class="getDiceClassName(item)"
        ></div>
      </div>
      <div
        v-if="gameData[0].imageType"
        :class="
          gameData[0].victoryConditions.weight !== NuiNuiWeight.noNui
            ? `blue_result`
            : `gary_result`
        "
      >
        <AssetImage
          :src="`@/assets/images/nuinui_sesult/game_result_${gameData[0].imageType}.png`"
        />
      </div>
      <div class="blue_win_tag" v-if="gameData[0].isWin">
        <AssetImage src="@/assets/images/nuinui_sesult/win_tag_left.png" />
      </div>
    </div>
    <div class="result_red">
      <div class="poker_group d-flex justify-content-between">
        <div
          class="poker_card"
          v-for="item in gameData[1].cards"
          :class="getDiceClassName(item)"
        ></div>
      </div>
      <div
        v-if="gameData[1].imageType"
        :class="
          gameData[1].victoryConditions.weight !== NuiNuiWeight.noNui
            ? `red_result`
            : `gary_result`
        "
      >
        <AssetImage
          :src="`@/assets/images/nuinui_sesult/game_result_${gameData[1].imageType}.png`"
        />
      </div>
      <div class="red_win_tag" v-if="gameData[1].isWin">
        <AssetImage src="@/assets/images/nuinui_sesult/win_tag_right.png" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, reactive } from "vue";
import { WinNumbers } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";
import {
  NuiNui,
  CardSuit,
  PokerCard,
  NuiNuiWeight,
} from "@/GameRules/NuiNuiRule";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Recently],
  data() {
    return {
      NuiNuiWeight: NuiNuiWeight,
    };
  },
  methods: {
    getDiceClassName(item: PokerCard) {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `poker_${CardSuit[item.suit]}_${item.type}`;
    },
  },
  computed: {
    gameData() {
      const currentNumbers = !this.currentDrawNumbers.length
        ? ["0", "0", "0", "0", "0", "0", "0", "0", "0", "0"]
        : this.currentDrawNumbers;
      const gameData = new NuiNui();
      const result = gameData.confirmResult(currentNumbers);
      const blueArea = reactive(result[0]);
      const redArea = reactive(result[1]);

      return [blueArea, redArea];
    },
  },
});
</script>
