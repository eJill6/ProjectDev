<template>
  <div class="d-flex align-items-center lottery_follow_pd">
    <li
      :class="[getSeBoBackgroundClassName(n),
        index !== 0 ? 'ml-1-5' : '',
      ]"
      v-for="(n, index) in drawNumbers"
    >
      {{ n }}
    </li>
    <AssetImage v-if="drawNumbers.length > 0" class="ml-1-5" src="@/assets/images/ic_bet_plus_b.svg" />
    <li
      class="ml-1-5"
      :class="getSeBoBackgroundClassName(
        specialDrawNumber
      )"
    >
      {{ specialDrawNumber }}
    </li>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers, LHC } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
    components:{AssetImage},
  mixins: [WinNumbers.Recently, LHC],
  computed: {
    drawNumbers() {
      return this.currentDrawNumbers.slice(0, 6);
    },
    specialDrawNumber() {
      return this.currentDrawNumbers.slice(-1)[0];
    },
  },
});
</script>
