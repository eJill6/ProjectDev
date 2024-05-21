<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 輪盤 組合 start -->
    <div class="play_inner jslp">
      <div
        class="play_betting jslp"
        v-for="(number, index) in newPlayTypeRadio"
        :class="getClickClass(fieldIndex, index)"
        @click="toggleSelectNumber(fieldIndex, index)"
      >
        <div class="play_item green">
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
          <div class="bet_option xl" :data-text="number">{{ number }}</div>
          <div class="bet_num">{{ getNumberOdds(number) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>
    </div>
    <div class="play_block"></div>
    <!-- 輪盤 組合 end -->
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  data() {
    return {
      fieldIndex: 0,
    };
  },
  methods: {
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return [{ active: isActive }];
    },
  },
  computed: {
    newPlayTypeRadio() {
      return this.playTypeRadio.flatMap((x: any) => x);
    },
  },
});
</script>
