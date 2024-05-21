<template>
  <div class="kuaisan_inner">
    <div class="pk10_content">
      <div class="pk10_corner"></div>
      <div class="pk10_light" v-if="!hasDrawNumbers"></div>
      <div class="pk10_section">
        <div v-for="n in currentDrawNumbers" class="pk10_item">
          <AssetImage :src="getDiceImageName(n)" alt="" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers, PK10 } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Layout, WinNumbers.Scroll, PK10],
  methods: {
    getDiceImageName(number: string) {
      return `@/assets/images/game/pk10_num_lg_${parseInt(number)}.png`;
    },
  },
  computed: {
    currentDrawNumbers(): string[] {
      if (!this.hasDrawNumbers) {
        return this.randomDrawNumbers;
      }
      let issueNo = this.$store.state.issueNo;

      return (
        (issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(",")) ||
        this.randomDrawNumbers
      );
    },
    $_gameTypeDrawNumbers(): string[] {
      return ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"];
    },
    $_gameTypeDrawNumberCount(): number {
      return 10;
    },
  },
});
</script>
