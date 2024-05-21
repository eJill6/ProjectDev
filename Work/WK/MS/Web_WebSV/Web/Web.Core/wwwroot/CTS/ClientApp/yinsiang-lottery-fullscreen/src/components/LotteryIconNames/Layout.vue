<template>
  <div :class="lotteryIconsClassName">
    <div class="header_game_title" :class="lotteryClassName">
      <component :is="lotteryIconsComponentName"></component>
    </div>
    <div class="header_game_arrow">
      <AssetImage src="@/assets/images/game/ic_game_arrow.png" alt="" />
    </div>
  </div>
</template>

<script lang="ts">
const context = require.context("", true, /Layout.vue$/);

const selfComponents = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string) => `icon_name_${gameTypeName}`;

context.keys().forEach((fileName) => {
  let config = context(fileName);
  let fileNameParts = fileName.split("/");
  let gameTypeName = fileNameParts[1];
  let componentName = getComponentName(gameTypeName);
  let options = config.default;

  selfComponents[componentName] = options;
});

import { defineComponent } from "vue";
import { AssetImage } from "../shared";

export default defineComponent({
  components: {
    AssetImage,
    ...selfComponents,
  },
  props: {
    gameTypeName: {
      type: String || undefined,
      required: false,
    },
    className: {
      type: String,
      required: true,
    },
  },
  computed: {
    lotteryIconsComponentName(): string {
      return getComponentName(
        this.gameTypeName ?? this.$store.state.lotteryInfo.gameTypeName
      );
    },
    lotteryIconsClassName(): string {
      return this.className;
    },
    lotteryClassName() {
      const gameTypeName = this.$store.state.lotteryInfo.gameTypeName;
      switch (gameTypeName) {
        case "SSC":
          return "omssc";
        case "LHC":
          return "omlhc";
        case "Baccarat":
          return "baccarat";
        case "YXX":
          return "jsyxx";
        case "SG":
          return "jssg";
        default:
          return "";
      }
    },
  },
});
</script>
