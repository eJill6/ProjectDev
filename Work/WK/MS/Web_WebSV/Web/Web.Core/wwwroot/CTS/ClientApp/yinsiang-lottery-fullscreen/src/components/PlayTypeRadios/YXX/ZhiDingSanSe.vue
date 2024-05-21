<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 指定三色 start -->
    <div class="play_inner leopard">
      <template v-for="(col, colIndex) in playTypeRadio.length">
        <div
          class="play_betting md"
          v-for="(row, rowIndex) in playTypeRadio[0].length"
          :class="getClickClass(colIndex, rowIndex)"
          @click="toggleSelectNumber(colIndex, rowIndex)"
        >
          <div class="play_item" :class="getClass(colIndex, rowIndex)">
            <div
              class="placebet animate_bet"
              v-if="showTotalAmount(playTypeRadio[colIndex][rowIndex])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="
                    showTotalAmount(playTypeRadio[colIndex][rowIndex])
                  "
                >
                  {{ showTotalAmount(playTypeRadio[colIndex][rowIndex]) }}
                </div>
              </div>
            </div>
            <div
              class="bet_img_option"
              :class="getSquareClass(playTypeRadio[colIndex][rowIndex])"
            >
              <AssetImage :src="getIcon(playTypeRadio[colIndex][rowIndex])" />
            </div>
            <div class="bet_num">
              {{ getNumberOdds(playTypeRadio[colIndex][rowIndex]) }}
            </div>
          </div>
          <div class="shadow_bet"></div>
        </div>
      </template>
    </div>
    <div class="play_block"></div>
    <!-- 指定三色 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

const BlockColor: any = {
  red: [
    { x: 0, y: 0 },
    { x: 0, y: 3 },
    { x: 1, y: 1 },
  ],
  green: [
    { x: 0, y: 1 },
    { x: 0, y: 4 },
    { x: 1, y: 2 },
  ],
  blue: [
    { x: 0, y: 2 },
    { x: 1, y: 0 },
    { x: 1, y: 3 },
  ],
  black: [{ x: 1, y: 4 }],
};

const BetItemMappingName: any = {
  红: "singlered",
  绿: "singlegreen",
  蓝: "singleblue",
  红对: "twored",
  绿对: "twogreen",
  蓝对: "twoblue",
  红豹: "threered",
  绿豹: "threegreen",
  蓝豹: "threeblue",
  红绿蓝任意豹子: "threecolors",
};

const BetItemCssMappingName: any = {
  single: ["红", "绿", "蓝"],
  two: ["红对", "绿对", "蓝对"],
  three: ["红豹", "绿豹", "蓝豹"],
  threecolors: ["红绿蓝任意豹子"],
};

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      let colorName = Object.keys(BlockColor).find((key) =>
        BlockColor[key].find(
          (geo: any) => geo.x === fieldIndex && geo.y === numberIndex
        )
      );
      return `${colorName}`;
    },
    getSquareClass(name: string) {
      return Object.keys(BetItemCssMappingName).find((key) =>
        BetItemCssMappingName[key].some((x: string) => x === name)
      );
    },
    getIcon(name: string) {
      return `@/assets/images/game/img_${BetItemMappingName[name]}.png`;
    },
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
});
</script>
