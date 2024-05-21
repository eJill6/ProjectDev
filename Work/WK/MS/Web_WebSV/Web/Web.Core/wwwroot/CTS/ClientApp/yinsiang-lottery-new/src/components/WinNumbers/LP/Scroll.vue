<template>
  <ResultTemplate :drawNumbers="randomDrawNumbers"></ResultTemplate>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import ResultTemplate from "./ResultTemplate.vue";
import { Roulette } from "@/GameRules/RouletteRule";
import { MutationType } from "@/store";

export default defineComponent({
  components: { ResultTemplate },
  data() {
    return {
      timerId: null as unknown as number,
      randomDrawNumbers: [] as string[],
    };
  },
  methods: {
    startRandom() {
      this.stopRandom();
      const roulette = new Roulette();
      let numberMaxCount = roulette.rouletteNumberArray.length;
      this.timerId = setInterval(() => {
        let newRouletteIndex = this.rouletteIndex + 1;
        if (numberMaxCount === newRouletteIndex) {
          newRouletteIndex = 0;
        }
        this.$store.commit(MutationType.SetRouletteIndex, newRouletteIndex);
        this.randomDrawNumbers = [
          roulette.rouletteNumberArray[newRouletteIndex].toString(),
        ];
      }, 30);
    },
    stopRandom() {
      clearInterval(this.timerId);
    },
  },
  created() {
    this.startRandom();
  },
  beforeUnmount() {
    this.stopRandom();
  },
  computed: {
    rouletteIndex() {
      return this.$store.state.rouletteIndex;
    },
  },
});
</script>
