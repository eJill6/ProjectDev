<template>
  <div class="kuaisan_inner">
    <div class="omssc_content">
      <div class="omssc_result" v-if="!hasDrawNumbers"></div>
      <div class="omssc_frame" v-if="showAnimation"></div>
      <div class="omssc_section">
        <div v-for="n in currentDrawNumbers" class="omssc_item">
          <AssetImage :src="getDiceImageName(n)" alt="" />
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
  mixins: [WinNumbers.Layout, WinNumbers.Scroll],
  data() {
    return {
      showAnimation: false,
    };
  },
  watch: {
    hasDrawNumbers: {
      handler(newValue) {
        if (newValue) {
          this.animationEffect();
        }
      },
      deep: true,
    },
  },
  methods: {
    getDiceImageName(number: string) {
      return `@/assets/images/game/omssc_ball_lg_${parseInt(number)}.png`;
    },
    animationEffect() {
      this.showAnimation = true;
      setTimeout(() => {
        this.showAnimation = false;
      }, 2000);
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
      return ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    },
    $_gameTypeDrawNumberCount(): number {
      return 5;
    },
  },
});
</script>
