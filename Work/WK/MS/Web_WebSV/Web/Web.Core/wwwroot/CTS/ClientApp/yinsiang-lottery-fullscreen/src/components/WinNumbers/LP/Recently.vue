<template>
  <ResultTemplate
    :drawNumbers="currentDrawNumbers"
    :isDraw="true"
  ></ResultTemplate>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from "@/mixins";
import ResultTemplate from "./ResultTemplate.vue";
import { MutationType, store } from "@/store";
import { Roulette } from "@/GameRules/RouletteRule";
import { TimeRules } from "@/enums";

export default defineComponent({
  components: { ResultTemplate },
  mixins: [WinNumbers.Recently],
  setup() {
    const countdownTime = store.getters.countdownTime;
    const hotCodeSec = 21;
    const roulette = new Roulette();
    if (
      countdownTime.secondsTotal - roulette.animationSec > hotCodeSec ||
      (countdownTime.secondsTotal <= 1 &&
        countdownTime.timeRule === TimeRules.closureCountdown)
    ) {
      store.commit(MutationType.SetIncreaseDegree, true);
    }
  },
});
</script>
