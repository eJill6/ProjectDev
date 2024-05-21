<template>
  <ResultTemplate :drawNumbers="randomDrawNumbers"></ResultTemplate>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import ResultTemplate from "./ResultTemplate.vue";

export default defineComponent({
  components: { ResultTemplate },
  computed: {
    randomDrawNumbers(): string[] {
      return ["0", "0", "0", "0", "0", "0"];
    },
  },
});
</script>
