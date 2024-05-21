<template>
  <!-- 指定三色 start -->
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_ten.png" />
    </div>

    <template v-if="havePlayTypeRadioData">
      <div class="middle" v-for="(row, rowIndex) in playTypeRadio[0].length">
        <div
          v-for="(col, colIndex) in playTypeRadio.length"
          :class="getClass(colIndex, rowIndex)"
          @click="toggleSelectNumber(colIndex, rowIndex)"
        >
          <div class="focus sm"></div>
          <div class="square">
            <AssetImage :src="getIcon(playTypeRadio[colIndex][rowIndex])" />
          </div>
          <div class="num sm">{{ getNumberOdds(playTypeRadio[colIndex][rowIndex]) }}</div>
        </div>
      </div>
    </template>
  </div>
  <!-- 指定三色 end -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

const NumClassDefined: any = {
  corner_tl: [{x: 0, y: 0}],
  corner_bl: [{x: 1, y: 0}],
  corner_none: [{x: 0, y: 1}, {x: 0, y: 2}, {x: 0, y: 3}, {x: 1, y: 1}, {x: 1, y: 2}, {x: 1, y: 3}],
  corner_tr: [{x: 0, y: 4}],
  corner_br: [{x: 1, y: 4}],
};

const BlockColor: any = {
  ceruleanblue: [{x: 0, y: 2}, {x: 1, y: 0}, {x: 1, y: 3}],
  versegreen: [{x: 0, y: 1}, {x: 0, y: 4}, {x: 1, y: 2}],
  winered: [{x: 0, y: 0} , {x: 0, y: 3} , {x: 1, y: 1}],
  chineseblack: [{x: 1, y: 4}]
}

const BetItemMappingName: any = {
  "红": "singlered",
  "绿": "singlegreen",
  "蓝": "singleblue",
  "红对": "twored",
  "绿对": "twogreen",
  "蓝对": "twoblue",
  "红豹": "threered",
  "绿豹": "threegreen",
  "蓝豹": "threeblue",
  "红绿蓝任意豹子": "threecolors"
}

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {      
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      let keyName = Object.keys(NumClassDefined).find(
        (key) => NumClassDefined[key].find((geo: any) => geo.x === fieldIndex && geo.y === numberIndex)
      ); 
      let colorName = Object.keys(BlockColor).find(
        (key) => BlockColor[key].find((geo: any) => geo.x === fieldIndex && geo.y === numberIndex)
      ); 
      return `${colorName} block ${keyName} ${isActive ? "active" : ""}`;
    },
    getIcon(name: string){
      return `@/assets/images/game/img_${BetItemMappingName[name]}.png`;
    }
  },
  computed: {
    havePlayTypeRadioData() {
      return (this.playTypeRadio.length ?? []) > 0;
    },
  },
});
</script>
