<template>
  <div class="pl-18 pr-18">
    <div
      class="d-flex justify-content-between mt-4"
      v-for="(field, fieldIndex) in playTypeRadio"
    >
      <div
        class="d-flex flex-direction-column align-items-center"
        v-for="(number, numberIndex) in field"
      >
        <button
          class="d-flex align-items-center justify-content-center bet-btn bet-btn-m"
          :class="getClass(fieldIndex, numberIndex)"
          @click="toggleSelectNumber(fieldIndex, numberIndex)"
        >
          <div :class="fieldIndex === 0 ? 'bet-option_M' : 'bet-option_M smallfont'">{{ number }}</div>
          <div class="bet-odds_M">
            {{ getNumberOdds(number) }}
          </div>
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
      return {
        "bet-btns1_M": fieldIndex === 0 && !isActive,
        "bet-btns1_M_active": fieldIndex === 0 && isActive,
        "bet-btns2_M": fieldIndex !== 0 && !isActive,
        "bet-btns2_M_active": fieldIndex !== 0 && isActive,
      };
    },
  },
});
</script>

