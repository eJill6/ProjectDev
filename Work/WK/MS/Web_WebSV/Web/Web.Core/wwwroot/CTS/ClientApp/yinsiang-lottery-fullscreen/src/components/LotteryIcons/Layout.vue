<template>
  <div :class="lotteryIconsClassName">
    <component :is="lotteryIconsComponentName"></component>
  </div>
</template>

<script lang="ts">
const context = require.context("", true, /Layout.vue$/);

const components = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string) => `icon_${gameTypeName}`;

context.keys().forEach((fileName) => {
  let config = context(fileName);
  let fileNameParts = fileName.split("/");
  let gameTypeName = fileNameParts[1];
  let componentName = getComponentName(gameTypeName);
  let options = config.default;

  components[componentName] = options;
});

import { defineComponent } from "vue";

export default defineComponent({
  components,
  props: {
    gameTypeName: {
      type: String || undefined,
      required: false,
    },
    className: {
      type: String,
      required: true,
    },
    gameTypeId: {
      type: Number || undefined,
      required: false,
    },
  },
  computed: {
    lotteryIconsComponentName(): string {
      return getComponentName(
        this.gameTypeName ?? this.$store.state.lotteryInfo.gameTypeName
      );
    },
    lotteryIconsClassName(): string {
      let result = this.className;
      let gameTypeName =
        this.gameTypeName ??
        (this.$store.state.lotteryInfo.gameTypeName as string);
      if (gameTypeName.toLocaleLowerCase() === "k3") {
        return `${result}`;
      } else if (gameTypeName.toLocaleLowerCase() === "ssc") {
        return `${result} omssc`;
      } else if (gameTypeName.toLocaleLowerCase() === "nuinui") {
        return `${result} nuinui`;
      } else if (gameTypeName.toLocaleLowerCase() === "baccarat") {
        return `${result} baccarat`;
      } else if (gameTypeName.toLocaleLowerCase() === "lp") {
        return `${result} jslp`;
      } else if (gameTypeName.toLocaleLowerCase() === "lhc") {
        return `${result} omlhc`;
      } else if (gameTypeName.toLocaleLowerCase() === "yxx") {
        return `${result} jsyxx`;
      } else if (gameTypeName.toLocaleLowerCase() === "sg") {
        return `${result} jssg`;
      }

      return `${result} pk10_car`;
    },
  },
});
</script>
