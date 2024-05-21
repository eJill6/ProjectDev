<template>
  <div class="pl-14-5 pr-14-5">
    <div
      class="d-flex justify-content-between mt-5"
      v-for="(field, fieldIndex) in playTypeRadio"
    >
      <div
        class="d-flex flex-direction-column align-items-center"
        v-for="(number, numberIndex) in field"
      >
        <button
          class="d-flex align-items-center justify-content-center bet-btn bet-btn-s"
          :class="getClass(fieldIndex, numberIndex)"
          @click="toggleSelectNumber(fieldIndex, numberIndex)"
        >
          <div class="bet-option_S">{{ number }}</div>
          <div class="bet-odds_S">
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
        "bet-btns1_S": fieldIndex === 0 && !isActive,
        "bet-btns1_S_active": fieldIndex === 0 && isActive,
        "bet-btns2_S": fieldIndex !== 0 && !isActive,
        "bet-btns2_S_active": fieldIndex !== 0 && isActive,
      };
    },
  },
});
</script>
