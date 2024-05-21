<template>
  <!-- 大小 start -->
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_four.png" />
    </div>
    <template v-if="havePlayTypeRadioData">
      <div class="middle" v-for="(tmp, index) in playTypeRadio.length">
        <!-- 大 --><!-- 小 -->
        <div :class="getClass(0, index)" @click="toggleSelectNumber(0, index)">
          <div class="focus sm"></div>
          <div class="text yellow spacing" :data-text="playTypeRadio[0][index]">{{playTypeRadio[0][index]}}</div>
          <div class="num fixed">{{ getNumberOdds(playTypeRadio[0][index]) }}</div>
        </div>
        <!-- 单 --><!-- 双 -->
        <div :class="getClass(1, index)" @click="toggleSelectNumber(1, index)">
          <div class="focus sm"></div>
          <div class="text yellow spacing" :data-text="playTypeRadio[1][index]">{{playTypeRadio[1][index]}}</div>
          <div class="num fixed">{{ getNumberOdds(playTypeRadio[1][index]) }}</div>
        </div>
      </div>
    </template>
  </div>
  <!-- 大小 end -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

const NumClassDefined: any = {
  corner_tl: {x: 0, y: 0},
  corner_tr: {x: 0, y: 1},
  corner_bl: {x: 1, y: 0},
  corner_br: {x: 1, y: 1},
};

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio], 
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {      
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      let keyName = Object.keys(NumClassDefined).find(
        (key) => NumClassDefined[key].x === fieldIndex && NumClassDefined[key].y ===  numberIndex
      );      
      return `${numberIndex === 0 ? "ceruleanblue" : "winered"} block ${keyName} ${isActive ? "active" : ""}`;
    },
  },
  computed: {
    havePlayTypeRadioData(){
      return (this.playTypeRadio.length ?? []) > 0
    }
  },
});
</script>
