<template>
  <div class="kuaisan_content">
    <div class="kuaisan_text_lg win_type">{{ sum }}</div>
    <div class="kuaisan_text_lg win_type" :class="daXiaoClassName">
      {{ daXiaoText }}
    </div>
    <div class="kuaisan_text_lg win_type" :class="danShuangClassName">
      {{ danShuang }}
    </div>
    <div class="kuaisan_text_lg win_type" :class="longHuHuoClassName">
      {{ longHuHuo }}
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SSC } from "../../../mixins";
import { AssetImage } from "../../../components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [SSC],
  computed: {
    sum() {
      return this.currentDrawNumbers.length
        ? this.getSum(this.currentDrawNumbers)
        : "-";
    },
    daXiaoText() {
      return this.currentDrawNumbers.length
        ? this.getDaXiaoText(this.currentDrawNumbers)
        : "-";
    },
    danShuang() {
      return this.currentDrawNumbers.length
        ? this.getDanShuang(this.currentDrawNumbers)
        : "-";
    },
    longHuHuo() {
      return this.currentDrawNumbers.length
        ? this.getLongHuHuo(this.currentDrawNumbers)
        : "-";
    },
    daXiaoClassName() {
      return this.getDaXiaoBackgroundClassName(this.currentDrawNumbers);
    },
    danShuangClassName() {
      return this.getDanShuangBackgroundClassName(this.currentDrawNumbers);
    },
    longHuHuoClassName() {
      return this.getLongHuHuoBackgroundClassName(this.currentDrawNumbers);
    },
    currentDrawNumbers(): string[] {
      let issueNo = this.$store.state.issueNo;

      return (
        (issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(",")) || []
      );
    },
  },
});
</script>
