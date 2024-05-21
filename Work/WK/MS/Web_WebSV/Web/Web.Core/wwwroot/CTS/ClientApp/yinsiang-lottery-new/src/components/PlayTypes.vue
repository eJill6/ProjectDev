<template>
  <div class="outer">
    <template v-for="(playTypeName, numberIndex) in currentPlayName">
      <div
        class="tab"
        :class="outerFrame(numberIndex)"
        @click="changePlayType(numberIndex)"
      >
        <div class="inner">
          <div class="text" :data-text="playTypeName">
            {{ playTypeName }}
          </div>
        </div>
      </div>
      <div
        class="tab_line"
        v-if="currentPlayName.length - 1 !== numberIndex"
      ></div>
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayMode } from "@/mixins";
import { MutationType } from "@/store";
import AssetImage from "./shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayMode],
  methods: {
    outerFrame(index: number) {
      const isSelected = this.isCurrentPlayType(index);
      if (isSelected) {
        return index === 1 ? "right" : "left";
      }
      return "";
    },
    isCurrentPlayType(index: number) {
      return this.currentPlayTypeSelected === index;
    },
    changePlayType(index: number) {
      this.$store.commit(MutationType.SetPlayType, index);
    },
  },
  computed: {
    currentPlayName() {
      const playConfig = this.$store.getters.playConfig;
      return Object.keys(playConfig);
    },
    currentPlayTypeSelected() {
      return this.$store.getters.playTypeSelected.selected;
    },
  },
});
</script>
