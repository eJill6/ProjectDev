<template>
  <!-- 總和 start 4-17號碼 -->
  <div class="betting_table fourteen">
    <div class="table fourteen">
      <AssetImage src="@/assets/images/game/table_border_fourteen.png" />
    </div>
    <div class="fourteen_outer">
      <template v-for="(number, numberIndex) in newPlayTypeRadio">
        <div :class="getClass(fieldIndex, numberIndex)" @click="toggleSelectNumber(fieldIndex, numberIndex)">
          <div class="focus"></div>
          <div class="text yellow spacing" :data-text="number">
            {{ number }}
          </div>
          <div class="num sm fixed3">{{ getNumberOdds(number) }}</div>
        </div>
      </template>
    </div>
  </div>
  <!-- 總和 end 4-17號碼 -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

const NumClassDefined: any = {
  corner_tl: [0],
  corner_tr: [4],
  corner_br: [9, 13],
  corner_none: [1, 2, 3, 5, 6, 7, 8, 11, 12],
  corner_bl: [10]
};

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  data() {
    return {
      rowNumber: 5,
      fieldIndex: 0,
    };
  },
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      let isBlue = !(Math.floor(numberIndex / this.rowNumber) % 2);
      let keyName = Object.keys(NumClassDefined).find(
        (key) => NumClassDefined[key].indexOf(numberIndex) > -1
      );

      return `item ${isBlue ? "ceruleanblue" : "winered"} block ${keyName} ${isActive ? "active" : ""}`;
    },
  },
  computed: {
    newPlayTypeRadio() {      
      return this.playTypeRadio.flatMap((x: any) => x);
    },
  },
});
</script>
