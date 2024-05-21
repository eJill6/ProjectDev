<template>
  <div class="nuinui_inner">
    <div class="nuinui_content mb0">
      <!-- 獲勝的那方才顯示 -->
      <div class="flag_blue spacing" v-if="gameData[0].isWin"></div>
      <div class="flag_red spacing" v-if="gameData[1].isWin"></div>
      <!-- 藍方 -->
      <div class="blue plan">
        <div class="blue_inner spacing">
          <div class="bg_result">
            <div class="result">
              <AssetImage
                :src="`@/assets/images/game/img_${
                  gameData[0].isWin ? 'win' : 'lose'
                }_nui_${gameData[0].imageType}.png`"
              />
            </div>
          </div>
          <div class="title" data-text="蓝方">蓝方</div>
          <div class="poker">
            <div class="piece" v-for="blueArea in gameData[0].cards">
              <AssetImage :src="getPokerImageSrc(blueArea)" />
            </div>
          </div>
        </div>
      </div>
      <div class="versus">
        <AssetImage src="@/assets/images/game/img_versus.png" />
      </div>
      <!-- 紅方 -->
      <div class="red plan">
        <div class="red_inner spacing">
          <div class="bg_result">
            <div class="result">
              <AssetImage
                :src="`@/assets/images/game/img_${
                  gameData[1].isWin ? 'win' : 'lose'
                }_nui_${gameData[1].imageType}.png`"
              />
            </div>
          </div>
          <div class="title" data-text="红方">红方</div>
          <div class="poker">
            <div class="piece" v-for="redArea in gameData[1].cards">
              <AssetImage :src="getPokerImageSrc(redArea)" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, reactive } from "vue";
import { WinNumbers } from "@/mixins";
import AssetImage from "../shared/AssetImage.vue";
import {
  NuiNui,
  CardSuit,
  PokerCard,
} from "@/GameRules/NuiNuiRule";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Recently],
  methods: {
    getPokerImageSrc(item: PokerCard) {
      return `@/assets/images/poker/${
        CardSuit[item.suit]
      }_${item.type.toLocaleUpperCase()}.png`;
    },
  },
  computed: {
    gameData() {
      const currentNumbers = this.hasDrawNumbers
        ? this.currentDrawNumbers
        : this.lastDrawNumbers;
      const gameData = new NuiNui();
      const result = gameData.confirmResult(currentNumbers);
      const blueArea = reactive(result[0]);
      const redArea = reactive(result[1]);

      return [blueArea, redArea];
    },
    hasDrawNumbers(): boolean {
      return !!this.$store.state.issueNo.lastDrawNumber;
    },
  },
});
</script>
