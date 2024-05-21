<template>
  <div class="game_outter" v-if="currentPlayName">
    <div
      class="game_inner"
      :class="gameTypeClass(numberIndex)"
      v-for="(playTypeName, numberIndex) in currentPlayName"
      @click="changePlayType(numberIndex)"
      :data-text="playTypeName"
    >
      {{ playTypeName }}
      <div class="game_tab" v-if="betsLogInfoCount(playTypeName)">
        <div class="game_tab_text" :data-text="betsLogInfoCount(playTypeName)">
          {{ betsLogInfoCount(playTypeName) }}
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayMode } from "@/mixins";
import { MutationType } from "@/store";
import { changLongContext } from "@/gameConfig";
import { BetsLog, BetsLogDetail } from "@/models";

export default defineComponent({
  mixins: [PlayMode],
  methods: {
    changePlayType(index: number) {
      this.$store.commit(MutationType.SetPlayType, index);
    },
    betsLogInfoCount(playTypeName: string) {
      const info = this.currentNumberOdds.find(
        (x) => x.category === playTypeName
      );
      return info ? info.count : 0;
    },
    gameTypeClass(index: number) {
      let pClass = {} as any;
      const gameTypeName = this.$store.state.lotteryInfo.gameTypeName;
      if (gameTypeName === "PK10") {
        switch (index) {
          case 0:
            pClass.play_5 = true;
            break;
          case 1:
            pClass.play_3 = true;
            break;
          case 2:
            pClass.play_4 = true;
            break;
          default:
            pClass.play_2 = true;
            break;
        }
      } else if (gameTypeName === "SSC") {
        switch (index) {
          case 0:
            pClass.play_4 = true;
            break;
          case 1:
            pClass.play_6 = true;
            break;
          case 2:
            pClass.play_4 = true;
            break;
          default:
            pClass.play_2 = true;
            break;
        }
      }  else if (gameTypeName === "NuiNui") {
        switch (index) {
          case 1:
            pClass.play_nuinui_5 = true;
            break;
          case 2:
          case 3:
            pClass.play_nuinui_3 = true;
            break;
          default:
            pClass.play_nuinui_2 = true;
            break;
        }
      } else if (gameTypeName === "LP") {
        pClass.play_4 = true;
      } else if (gameTypeName === "Baccarat") {
        pClass.play_baccarat_2 = true;
      } else if (gameTypeName === "YXX") {
        if (index === 3) {
          pClass.play_4 = true;
        } else {
          pClass.play_2 = true;
        }
      } else {
        pClass.play_2 = true;
      }
      pClass.active = this.currentPlayTypeSelected === index;

      return pClass;
    },
  },
  computed: {
    currentPlayName() {
      let playConfig = this.$store.getters.playConfig;
      playConfig[changLongContext] = [[]];
      return Object.keys(playConfig);
    },
    currentPlayType() {
      return this.$store.getters.currentPlayType;
    },
    currentPlayTypeSelected() {
      return this.$store.getters.playTypeSelected.selected;
    },
    currentNumberOdds() {
      return this.$store.state.betsLog.length
        ? this.$store.state.betsLog[0].numberOdds
        : ([] as BetsLogDetail[]);
    },
  },
});
</script>
