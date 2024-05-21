<template>
  <div class="h-follow-title">
    <div class="list_group racing pt-4-5 pb-3">
      <li :class="`no_${parseInt(n)}_color`" v-for="n in currentDrawNumbers">{{ parseInt(n) }}</li>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from "@/mixins";

export default defineComponent({
  mixins: [WinNumbers.Recently],
});
</script>
