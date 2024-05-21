<template>
  <div class="plan_num_board">
    <div class="plan_circle" v-for="(n, index) in hasDrawNumbers ? currentDrawNumbers : lastDrawNumbers">
      <div class="plan_num" :text="n">{{n}}</div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from '@/mixins';

export default defineComponent({
    mixins: [WinNumbers.Recently],
    computed: {
        hasDrawNumbers(): boolean {
            return !!this.$store.state.issueNo.lastDrawNumber;
        }
    }

});
</script>