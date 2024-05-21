<template>
  <!-- 單骰 start -->
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_singledice.png" />
    </div>
    <template v-if="havePlayTypeRadioData">
      <div class="middle" v-for="(row, rowIndex) in playTypeRadio[0].length">
        <!-- 魚 1、蝦 2、葫蘆 3、銅錢 4、螃蟹 5、雞 6 -->
        <div
          v-for="(col, colIndex) in playTypeRadio.length"
          :class="getClass(colIndex, rowIndex)"
          @click="toggleSelectNumber(colIndex, rowIndex)"
        >
          <div class="focus sm"></div>
          <div class="single_icon">
            <AssetImage :src="`@/assets/images/game/img_singledice_${getAliasName(convertCNameToNumber(playTypeRadio[colIndex][rowIndex]))}.png`" />
          </div>
          <div class="v_alignment">
            <div class="text yellow spacing sm" :data-text="convertCNameToNumber(playTypeRadio[colIndex][rowIndex])">
              {{ convertCNameToNumber(playTypeRadio[colIndex][rowIndex]) }}
            </div>
            <div class="num sm fixed">
              {{ getNumberOdds(playTypeRadio[colIndex][rowIndex]) }}
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
  <!-- 單骰 end -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio, YXX } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

const NumClassDefined: any = {
  corner_tl: [{x: 0, y: 0}],
  corner_bl: [{x: 1, y: 0}],
  corner_none: [{x: 0, y: 1}, {x: 1, y: 1}],
  corner_tr: [{x: 0, y: 2}],
  corner_br: [{x: 1, y: 2}],
};

const BlockColor: any = {
  ceruleanblue: [{x: 0, y: 2}, {x: 1, y: 0}],
  versegreen: [{x: 0, y: 1}, {x: 1, y: 1}],
  winered: [{x: 0, y: 0}, {x: 1, y: 2}],
}

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio, YXX],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      let keyName = Object.keys(NumClassDefined).find(
        (key) => NumClassDefined[key].find((geo: any) => geo.x === fieldIndex && geo.y === numberIndex)
      ); 
      let colorName = Object.keys(BlockColor).find(
        (key) => BlockColor[key].find((geo: any) => geo.x === fieldIndex && geo.y === numberIndex)
      ); 
      return `${colorName} block row ${keyName} ${isActive ? "active" : ""}`;
    },
  },
  computed: {
    havePlayTypeRadioData() {      
      return (this.playTypeRadio.length ?? []) > 0;
    },
  },
});
</script>