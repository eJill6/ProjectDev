<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <div class="play_inner leopard">
      <template v-for="(field, fieldIndex) in playTypeRadio">
        <template v-for="(number, numberIndex) in field">
          <div
            class="play_betting md"
            :class="getClickClass(fieldIndex, numberIndex)"
            @click="toggleSelectNumber(fieldIndex, numberIndex)"
          >
            <div class="play_item" :class="getClass(fieldIndex, numberIndex)">
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
        </template>
      </template>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      let row = Math.floor((fieldIndex * 6 + numberIndex) / 4);
      return {
        blue: row % 2 === 0,
        orange: row % 2 === 1,
      };
    },
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
});
</script>
