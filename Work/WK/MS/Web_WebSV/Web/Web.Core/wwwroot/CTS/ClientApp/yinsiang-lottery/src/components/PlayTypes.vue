<template>
  <div class="bet-nav-outter" v-if="currentPlayName">
    <button
      :class="[
        { active: isCurrentPlayType(numberIndex) },
        isBigWinNumber ? setBigWinNumberClass(playTypeName.length) : 'bet-nav',
      ]"
      v-for="(playTypeName, numberIndex) in currentPlayName"
      @click="changePlayType(numberIndex)"
    >
      {{ playTypeName }}
    </button>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayMode } from "@/mixins";
import { MutationType } from "@/store";
import { bigWinNumberList } from "@/gameConfig";

export default defineComponent({
  mixins: [PlayMode],
  methods: {
    isCurrentPlayType(index: number) {
      return this.currentPlayTypeSelected === index;
    },
    changePlayType(index: number) {
      this.$store.commit(MutationType.SetPlayType, index);
    },
    setBigWinNumberClass(length: number) {
      const isLong = length > 3;
      return isLong ? "nuinui-bet-nav nuinui-large" : "nuinui-bet-nav";
    },
  },
  computed: {
    currentPlayName() {
      const playConfig = this.$store.getters.playConfig;
      return Object.keys(playConfig);
    },
    currentPlayType() {
      return this.$store.getters.currentPlayType;
    },
    currentPlayTypeSelected() {
      return this.$store.getters.playTypeSelected.selected;
    },
    isBigWinNumber() {
      return (
        bigWinNumberList.indexOf(this.$store.state.lotteryInfo.gameTypeName) >
        -1
      );
    },
  },
});
</script>
