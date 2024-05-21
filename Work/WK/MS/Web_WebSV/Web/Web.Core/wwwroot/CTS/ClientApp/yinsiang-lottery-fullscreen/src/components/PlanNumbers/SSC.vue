<template>
  <div class="omssc_inner">
    <div class="omssc_content">
      <div class="omssc_section">
        <div
          class="omssc_item"
          v-for="n in hasDrawNumbers ? currentDrawNumbers : lastDrawNumbers"
        >
          <AssetImage :src="`@/assets/images/game/omssc_ball_lg_${n}.png`" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Recently],
  computed: {
    hasDrawNumbers(): boolean {
      return !!this.$store.state.issueNo.lastDrawNumber;
    },
  },
});
</script>
