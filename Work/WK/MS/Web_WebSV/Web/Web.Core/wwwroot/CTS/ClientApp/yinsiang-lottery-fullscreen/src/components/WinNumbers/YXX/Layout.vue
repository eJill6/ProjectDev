<template>
  <div class="kuaisan_inner">
    <div class="full_yusiasieh" v-if="!hasDrawNumbers"></div>
    <div class="kuaisan_content" v-else>
      <div class="jsyxx_dice_lg" v-for="n in currentDrawNumbers">
        <AssetImage :src="getDiceImageName(n)" alt="" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers, YXX } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Layout, WinNumbers.Scroll, YXX],
  methods: {
    getDiceImageName(number: string) {
      return `@/assets/images/game/ic_dice_${this.getAliasName(number)}.png`;
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
      return ["1", "2", "3", "4", "5", "6"];
    },
    $_gameTypeDrawNumberCount(): number {
      return 3;
    },
  },
});
</script>
