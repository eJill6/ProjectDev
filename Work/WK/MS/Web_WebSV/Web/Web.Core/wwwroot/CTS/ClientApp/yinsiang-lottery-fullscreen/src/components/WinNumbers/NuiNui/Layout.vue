<template>
  <div class="kuaisan_inner height">
    <div class="nuinui_content">
      <!-- 獲勝的那方才顯示 -->
      <div class="flag_blue" v-if="currentDrawNumbers[0].isWin"></div>
      <div class="flag_red" v-if="currentDrawNumbers[1].isWin"></div>
      <!-- 藍方 -->
      <div class="blue">
        <div class="blue_inner ani">
          <div class="bg_result bottom" v-if="currentDrawNumbers[0].imageType">
            <div class="result">
              <AssetImage
                :src="`@/assets/images/game/img_${
                  currentDrawNumbers[0].isWin ? 'win' : 'lose'
                }_nui_${currentDrawNumbers[0].imageType}.png`"
              />
            </div>
          </div>
          <div class="title ani" data-text="蓝方">蓝方</div>
          <!-- 發牌動畫 -->
          <div
            class="poker ani"
            v-if="isShowBackImage(currentDrawNumbers[0].cards)"
          >
            <div
              :class="`piece left_0${index + 1}`"
              v-for="(item, index) in currentDrawNumbers[0].cards"
            >
              <AssetImage :src="getPokerImageSrc(item)" />
            </div>
          </div>
          <!-- 翻牌、結果動畫 -->
          <div class="poker ani" v-else>
            <template v-for="(item, index) in currentDrawNumbers[0].cards">
              <div :class="[`piece left_result_0${index + 1}`, stopAnimation]">
                <AssetImage :src="getPokerImageSrc(item)" />
              </div>
              <div
                :class="[
                  `piece left_result_back_0${index + 1}`,
                  stopBackAnimation,
                ]"
              >
                <AssetImage src="@/assets/images/poker/poker_back.png" />
              </div>
            </template>
          </div>
        </div>
      </div>
      <div class="versus">
        <AssetImage src="@/assets/images/game/img_versus.png" />
      </div>
      <!-- 紅方 -->
      <div class="red">
        <div class="red_inner ani">
          <div class="bg_result bottom" v-if="currentDrawNumbers[1].imageType">
            <div class="result">
              <AssetImage
                :src="`@/assets/images/game/img_${
                  currentDrawNumbers[1].isWin ? 'win' : 'lose'
                }_nui_${currentDrawNumbers[1].imageType}.png`"
              />
            </div>
          </div>
          <div class="title ani" data-text="红方">红方</div>
          <!-- 發牌動畫 -->
          <div
            class="poker ani"
            v-if="isShowBackImage(currentDrawNumbers[1].cards)"
          >
            <div
              :class="`piece right_0${index + 1}`"
              v-for="(item, index) in currentDrawNumbers[1].cards"
            >
              <AssetImage :src="getPokerImageSrc(item)" />
            </div>
          </div>
          <!-- 翻牌、結果動畫 -->
          <div class="poker ani" v-else>
            <template v-for="(item, index) in currentDrawNumbers[1].cards">
              <div :class="[`piece right_result_0${index + 1}`, stopAnimation]">
                <AssetImage :src="getPokerImageSrc(item)" />
              </div>
              <div
                :class="[
                  `piece right_result_back_0${index + 1}`,
                  stopBackAnimation,
                ]"
              >
                <AssetImage src="@/assets/images/poker/poker_back.png" />
              </div>
            </template>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { NuiNui, CardSuit, PokerCard } from "@/GameRules/NuiNuiRule";
import { WinNumbers } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";
import { defineComponent, reactive } from "vue";
// import { useStore } from "vuex";
// // import anime from "animejs";
export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Layout, WinNumbers.Scroll],
  methods: {
    getPokerImageSrc(item: PokerCard) {
      if (item.originalNumber === "0") {
        return "@/assets/images/poker/poker_back.png";
      }
      return `@/assets/images/poker/${
        CardSuit[item.suit]
      }_${item.type.toLocaleUpperCase()}.png`;
    },
    isShowBackImage(cards: PokerCard[]) {
      return cards.some((item) => item.originalNumber === "0");
    },
  },
  computed: {
    stopAnimation() {
      return this.$store.state.isStopAnimation ? "ani_unset" : "";
    },
    stopBackAnimation() {
      return this.$store.state.isStopAnimation ? "back_ani_unset" : "";
    },
    currentDrawNumbers() {
      let issueNo = this.$store.state.issueNo;

      let currentNumbers =
        (issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(",")) ||
        this.randomDrawNumbers;
      currentNumbers =
        currentNumbers.length !== this.$_gameTypeDrawNumbers.length
          ? this.$_gameTypeDrawNumbers
          : currentNumbers;
      if (!this.hasDrawNumbers) {
        currentNumbers = this.$_gameTypeDrawNumbers;
      }
      const gameData = new NuiNui();
      const result = gameData.confirmResult(currentNumbers);
      const blueArea = reactive(result[0]);
      const redArea = reactive(result[1]);

      return [blueArea, redArea];
    },
    $_gameTypeDrawNumbers(): string[] {
      return ["0", "0", "0", "0", "0", "0", "0", "0", "0", "0"];
    },
    $_gameTypeDrawNumberCount(): number {
      return 10;
    },
  },
});
</script>
<style>
.piece.left_result_back_01.back_ani_unset,
.piece.left_result_back_02.back_ani_unset,
.piece.left_result_back_03.back_ani_unset,
.piece.left_result_back_04.back_ani_unset,
.piece.left_result_back_05.back_ani_unset,
.piece.right_result_back_01.back_ani_unset,
.piece.right_result_back_02.back_ani_unset,
.piece.right_result_back_03.back_ani_unset,
.piece.right_result_back_04.back_ani_unset,
.piece.right_result_back_05.back_ani_unset {
  animation: unset;
  opacity: 0%;
}
</style>
