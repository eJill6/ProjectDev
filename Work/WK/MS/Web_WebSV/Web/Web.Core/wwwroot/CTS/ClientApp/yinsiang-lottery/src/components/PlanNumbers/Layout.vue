<template>
  <component :is="winNumbersComponentName"></component>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MqEvent } from "@/mixins";

const context = require.context("", true, /\w+.vue$/);

const components = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string) => `Plan_${gameTypeName}`;

context.keys().forEach((fileName) => {
  let config = context(fileName);
  let fileNameParts = fileName.split("/");
  let gameTypeName = fileNameParts[1].replace(".vue", "");
  if (gameTypeName === "Layout") return;
  let componentName = getComponentName(gameTypeName);
  let options = config.default;
  components[componentName] = options;
});

export default defineComponent({
  components,  
  mixins:[MqEvent],
  computed: {
    winNumbersComponentName(): string {
      return getComponentName(this.$store.state.lotteryInfo.gameTypeName);
    },
    lotteryInfo(){
        return this.$store.state.lotteryInfo;
    }
  },
});
</script>
