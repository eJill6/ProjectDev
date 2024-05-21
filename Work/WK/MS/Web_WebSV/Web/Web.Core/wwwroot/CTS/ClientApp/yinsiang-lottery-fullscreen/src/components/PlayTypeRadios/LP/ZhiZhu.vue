<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 輪盤 直注 start -->
    <div class="play_inner sum">
      <div
        class="play_betting sm"
        :class="getClickClass(number)"
        v-for="number in newPlayTypeRadio"
        @click="selectedNubmer(number)"
      >
        <div class="play_item" :class="getClass(number)">
          <div class="placebet animate_bet" v-if="showTotalAmount(number)">
            <div class="placebet_icon">
              <AssetImage src="@/assets/images/game/ic_placebet.png" />
            </div>
            <div class="bg_coin">
              <div class="coin_text" :data-text="showTotalAmount(number)">
                {{ showTotalAmount(number) }}
              </div>
            </div>
          </div>
          <div class="bet_option" :data-text="number">{{ number }}</div>
          <div class="bet_num">{{ getNumberOdds(number) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>
    </div>
    <div class="play_block"></div>
    <!-- 輪盤 直注 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import { Roulette } from "@/GameRules/RouletteRule";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  data() {
    return {
      roulette: new Roulette(),
      rowLimit: 5,
    };
  },
  methods: {
    getClass(selectedNumber: number) {
      const numberColor = this.roulette.getColor(selectedNumber);
      return [numberColor];
    },
    selectedNubmer(selectedNumber: number) {
      const fieldIndex = this.getFieldIndex(selectedNumber);
      const numberIndex = this.getNumberIndex(selectedNumber);
      this.toggleSelectNumber(fieldIndex, numberIndex);
    },
    getFieldIndex(selectedNumber: number) {
      return Math.floor(selectedNumber / this.rowLimit);
    },
    getNumberIndex(selectedNumber: number) {
      return selectedNumber % this.rowLimit;
    },
    getClickClass(selectedNumber: number) {
      const fieldIndex = this.getFieldIndex(selectedNumber);
      const numberIndex = this.getNumberIndex(selectedNumber);
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
  computed: {
    newPlayTypeRadio() {
      let playTypes = this.playTypeRadio.flatMap((x: any) => x);
      playTypes = playTypes.map(Number);
      return playTypes;
    },
  },
});
</script>
