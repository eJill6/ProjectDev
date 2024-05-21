<template>
  <div class="kuaisan_inner">
    <div class="kuaisan_content" v-if="hasDrawNumbers">
      <div v-for="n in currentDrawNumbers" class="kuaisan_dice_lg">
        <AssetImage :src="getDiceImageName(n)" alt="" />
      </div>
    </div>
    <div class="animate_dice" v-else>
      <div class="dice_a"></div>
      <div class="dice_b"></div>
      <div class="dice_c"></div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers, K3 } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Layout, WinNumbers.Scroll, K3],
  methods: {
    getDiceImageName(number: string) {
      return `@/assets/images/game/img_lg_dice${number}.png`;
    },
  },
  computed: {
    currentDrawNumbers(): string[] {
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
