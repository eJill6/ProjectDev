<template>
  <div>
    <button
      @click="testhtml"
      style="width: 200px; height: 200px; background-color: azure"
    >
      測試
    </button>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  methods: {
    testhtml() {
      location.href = "https://www.youtube.com/";
    },
  },
});
</script>
