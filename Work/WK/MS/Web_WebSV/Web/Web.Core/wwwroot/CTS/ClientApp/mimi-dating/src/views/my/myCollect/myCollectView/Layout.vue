<template>
    <component :is="publishComponentName"></component>
  </template>
  
  <script lang="ts">

  import { defineComponent } from "vue";


  const context = require.context("", true, /\w+.vue$/);
  
  const components = {} as { [k: string]: any };
  
  const getComponentName = (gameTypeName: string) => `MyCollect_${gameTypeName}`;
  
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
    computed: {
      publishComponentName(): string {
        const publishName =
          this.$store.state.myCollectViewTypeStatus;
        return getComponentName(publishName);
      },
    },
  });
  </script>
  