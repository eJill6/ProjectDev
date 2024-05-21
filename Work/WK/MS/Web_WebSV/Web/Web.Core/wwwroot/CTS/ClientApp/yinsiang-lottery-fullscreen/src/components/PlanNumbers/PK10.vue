<template>
  <div class="pk10_inner">
    <div class="pk10_content">
      <div class="pk10_corner"></div>
      <div class="pk10_section">
        <div class="pk10_item" v-for="n in hasDrawNumbers ? currentDrawNumbers : lastDrawNumbers">
          <AssetImage :src="`@/assets/images/game/pk10_num_lg_${parseInt(n)}.png`" alt=""/>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Recently],  
  computed: {
    hasDrawNumbers(): boolean {
      return !!this.$store.state.issueNo.lastDrawNumber;
    }
  }
});
</script>

