<template>
    <div class="d-flex align-items-end lottery_follow_pd">
    <div
      v-for="(n, index) in currentDrawNumbers"
      class="lottery_ball_follow"
      :class="index === 0 ? '' : ' ml-3'"
    >
      {{n}}
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from '@/mixins';

export default defineComponent({
    mixins: [WinNumbers.Recently]
});
</script>