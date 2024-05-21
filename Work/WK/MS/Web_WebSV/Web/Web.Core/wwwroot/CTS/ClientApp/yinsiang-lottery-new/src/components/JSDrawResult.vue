<template> 
  <DrawResults></DrawResults>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MqEvent } from "@/mixins";
import DrawResults from "./DrawResults";

export default defineComponent({
  mixins: [MqEvent],
  components: { DrawResults }, 
});
</script>