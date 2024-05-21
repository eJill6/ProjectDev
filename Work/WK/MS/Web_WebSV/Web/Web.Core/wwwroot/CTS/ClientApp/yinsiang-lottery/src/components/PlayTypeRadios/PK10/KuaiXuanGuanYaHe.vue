<template>
  <div class="pl-7 pr-7">
    <div
      class="d-flex justify-content-between mt-5"
      v-for="(field, fieldIndex) in sortPlayTypeRadio"
    >
      <div
        class="d-flex flex-direction-column align-items-center"
        v-for="(number, numberIndex) in field"
      >
        <button v-if="number"
          class="d-flex align-items-center justify-content-center bet-btn bet-btn-s "
          :class="{                    
            'bet-btns1_S': fieldIndex % 2 === 0, 
            'bet-btns1_S_active': fieldIndex % 2 === 0 && isNumberSelected(fieldIndex, numberIndex),
            'bet-btns2_S': fieldIndex % 2 !== 0,
            'bet-btns2_S_active': fieldIndex % 2 !== 0 && isNumberSelected(fieldIndex, numberIndex)
            }"
          @click="toggleSelectNumber(fieldIndex, numberIndex)"
        >
          <div class="bet-option_S">{{ number }}</div>
          <div class="bet-odds_S">
            {{ getNumberOdds(number) }}
          </div>
        </button>
        <button v-else
          class="d-flex align-items-center justify-content-center bet-btn-s"
        ></button>
      </div>
    </div>
  </div> 
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";

export default defineComponent({
  mixins: [PlayTypeRadio],
  computed:{
    sortPlayTypeRadio(){
      let list = this.playTypeRadio;
      const total = list.length;
      if (total > 1) {
        const count = list[0].length;
        const lastArrray =list[total - 1];
        const lastArrrayCount =list[total - 1].length;
        for(let index = 0; index < (count - lastArrrayCount); index ++) {                    
          lastArrray.push('');
        }
      }
      return list;
    }
  }
});
</script>
