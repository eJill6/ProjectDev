<template>
  <div class="betting_table thirtyseven">
    <div class="table thirtyseven">
      <AssetImage src="@/assets/images/game/table_border_betting.png" />
    </div>
    <div class="thirtyseven_outer">
      <div
        class="item block"
        v-for="number in newPlayTypeRadio"
        :class="getClass(number)"
        @click="selectedNubmer(number)"
      >
        <div class="focus"></div>
        <div class="text white" :data-text="number">{{ number }}</div>
        <div class="num">{{ getNumberOdds(number) }}</div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "../../shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  data() {
    return {
      rowLimit: 5,
    };
  },
  methods: {
    getClass(selectedNumber: number) {
      const fieldIndex = this.getFieldIndex(selectedNumber);
      const numberIndex = this.getNumberIndex(selectedNumber);
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
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
  },
  computed: {
    newPlayTypeRadio() {
      const radio = this.playTypeRadio.flatMap((x: any) => x);
      return radio;
    },
  },
});
</script>
