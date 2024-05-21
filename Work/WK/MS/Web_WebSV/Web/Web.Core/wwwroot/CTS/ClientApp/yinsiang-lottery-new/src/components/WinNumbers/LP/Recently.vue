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

export default defineComponent({
  components: { ResultTemplate },
  mixins: [WinNumbers.Recently],
  setup() {
    const countdownTime =
      Number(store.getters.formattedIssueNoCountdownTime?.split(":")[1]) | 0;
    const hotCodeSec = 21;
    const roulette = new Roulette();
    if (countdownTime - roulette.animationSec > hotCodeSec) {
      store.commit(MutationType.SetIncreaseDegree, true);
    }
  },
});
</script>
