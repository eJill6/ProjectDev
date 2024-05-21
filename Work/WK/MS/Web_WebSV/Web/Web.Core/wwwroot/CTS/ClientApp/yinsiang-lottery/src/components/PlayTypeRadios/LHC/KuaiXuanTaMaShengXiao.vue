<template>
  <div class="pl-7 pr-7">
    <div
      class="d-flex justify-content-between"
      :class="fieldIndex === 0 ? 'mt-5' : 'mt-4'"
      v-for="(field, fieldIndex) in playTypeRadio"
    >
      <div
        class="d-flex flex-direction-column align-items-center"
        v-for="(number, numberIndex) in field"
      >
        <button
          class="d-flex align-items-center justify-content-center bet-btn-s"
          :class="getClass(fieldIndex, numberIndex)"
          @click="toggleSelectNumber(fieldIndex, numberIndex)"
        >
          <div class="bet-option_S">{{ number }}</div>
          <div class="bet-odds_S">{{ getNumberOdds(number) }}</div>
        </button>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";

export default defineComponent({
  mixins: [PlayTypeRadio],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      const number = fieldIndex === 1 ? 6 + numberIndex : numberIndex;
      const mappings = [
        { index: "1", numbers: [0, 1, 8, 9] },
        { index: "2", numbers: [2, 3, 10, 11] },
        { index: "3", numbers: [6, 7] },
        { index: "4", numbers: [4, 5] }
      ];
      const classNumber = mappings.filter(x => x.numbers.indexOf(number) > -1)[0];
      const buttonClassName = `bet-btns${classNumber.index}_S`;
      return `${buttonClassName}${isActive ? "_active" : ""}`;
    },
  },
});
</script>
