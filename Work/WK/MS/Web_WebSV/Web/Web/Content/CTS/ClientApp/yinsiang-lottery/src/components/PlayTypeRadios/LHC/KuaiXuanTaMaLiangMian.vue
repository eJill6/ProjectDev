<template>
  <div class="pl-7 pr-7">
    <div
      class="d-flex justify-content-between mt-5"
      v-for="(field, fieldIndex) in playTypeRadio"
    >
      <div class="d-flex flex-direction-column align-items-center" v-for="(number, numberIndex) in field">
        <button
          class="d-flex align-items-center justify-content-center bet-btn-xxl"
          :class="getClass(fieldIndex, numberIndex)"
          @click="toggleSelectNumber(fieldIndex, numberIndex)"
        >
          <div class="bet-option_XXL">{{ number }}</div>
          <div class="bet-odds_XXL">{{ getNumberOdds(number) }}</div>
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
        "bet-btns1_XXL": numberIndex < 2 && !isActive,
        "bet-btns1_XXL_active": numberIndex < 2 && isActive,
        "bet-btn bet-btns2_XXL": numberIndex >= 2 && !isActive,
        "bet-btn bet-btns2_XXL_active": numberIndex >= 2 && isActive,
      };
    },
  },
});
</script>
