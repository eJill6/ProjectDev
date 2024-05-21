<template>
  <template v-for="item in list">
    <div class="history_row roulette" :class="{ unset: isFull }">
      <PlanNumbers :item="item" :isFull="isFull"></PlanNumbers>
    </div>
    <div class="record_line" v-if="isFull"></div>
  </template>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistory } from "@/models";
import PlanNumbers from "../PlanNumbers";

export default defineComponent({
  components: { PlanNumbers },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
    isFull: Boolean,
  },
});
</script>
