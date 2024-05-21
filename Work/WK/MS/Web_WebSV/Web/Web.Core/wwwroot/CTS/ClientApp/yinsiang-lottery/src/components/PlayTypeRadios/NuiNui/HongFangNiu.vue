<template>
  <div class="pl-7 pr-7">
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
          <div
            class="nuinui-option_S"
            :class="number.length > 2 ? `fs-2` : `fs-6`"
          >
            {{ number }}
          </div>
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
      return isActive ? "bet-btns6_S_active" : "bet-btns6_S";
    },
  },
});
</script>
