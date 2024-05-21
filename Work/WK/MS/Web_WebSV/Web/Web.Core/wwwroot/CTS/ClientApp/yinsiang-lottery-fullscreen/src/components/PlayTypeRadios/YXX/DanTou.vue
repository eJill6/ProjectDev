<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 魚蝦蟹 單骰 start -->
    <div class="play_inner jsyxx">
      <template v-for="(col, colIndex) in playTypeRadio.length">
        <div
          class="play_betting jsyxx"
          v-for="(row, rowIndex) in playTypeRadio[0].length"
          :class="getClickClass(colIndex, rowIndex)"
          @click="toggleSelectNumber(colIndex, rowIndex)"
        >
          <div class="play_item_jsyxx" :class="getClass(colIndex, rowIndex)">
            <div
              class="placebet animate_bet"
              v-if="
                showTotalAmount(
                  convertCNameToNumber(playTypeRadio[colIndex][rowIndex])
                )
              "
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="
                    showTotalAmount(
                      convertCNameToNumber(playTypeRadio[colIndex][rowIndex])
                    )
                  "
                >
                  {{
                    showTotalAmount(
                      convertCNameToNumber(playTypeRadio[colIndex][rowIndex])
                    )
                  }}
                </div>
              </div>
            </div>
            <!-- 魚 1、蝦 2、葫蘆 3、銅錢 4、螃蟹 5、雞 6 -->
            <div class="jsyxx_md_img">
              <AssetImage
                :src="`@/assets/images/game/img_singledice_${getAliasName(
                  convertCNameToNumber(playTypeRadio[colIndex][rowIndex])
                )}.png`"
              />
            </div>
            <div class="content">
              <div
                class="bet_option"
                :data-text="
                  convertCNameToNumber(playTypeRadio[colIndex][rowIndex])
                "
              >
                {{ convertCNameToNumber(playTypeRadio[colIndex][rowIndex]) }}
              </div>
              <div class="bet_num">
                {{ getNumberOdds(playTypeRadio[colIndex][rowIndex]) }}
              </div>
            </div>
          </div>
          <div class="shadow_bet row"></div>
        </div>
      </template>
    </div>
    <div class="play_block"></div>
    <!-- 魚蝦蟹 單骰 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio, YXX } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio, YXX],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      const BlockColor: any = {
        red: [
          { x: 0, y: 0 },
          { x: 1, y: 2 },
        ],
        blue: [
          { x: 0, y: 2 },
          { x: 1, y: 0 },
        ],
        green: [
          { x: 0, y: 1 },
          { x: 1, y: 1 },
        ],
      };
      let colorName = Object.keys(BlockColor).find((key) =>
        BlockColor[key].find(
          (geo: any) => geo.x === fieldIndex && geo.y === numberIndex
        )
      );

      return `${colorName}_md_row`;
    },
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
});
</script>
