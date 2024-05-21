<template>
  <div class="kuaisan_content">
      <div class="kuaisan_text_lg win_type">{{specialDrawNumber}}</div>
      <div class="kuaisan_text_lg win_type" :class="getDaXiaoBackgroundClassName(specialDrawNumber)">{{ daXiaoText }}</div>
      <div class="kuaisan_text_lg win_type" :class="getDanShuangBackgroundClassName(specialDrawNumber)">{{ danShuang }}</div>
      <div class="kuaisan_text_lg win_type" :class="getSeBoBackgroundClassName(specialDrawNumber)">{{ seBoText }}</div>
      <div class="kuaisan_text_lg win_type">{{ shengXiaoText }}</div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { LHC } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [LHC],
  computed: {
    drawNumbers(): string[] {
      let issueNo = this.$store.state.issueNo;

      return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
    },
    currentDrawNumbers(): string {
      let issueNo = this.$store.state.issueNo;

      return issueNo.lastDrawNumber || '';
    },
    specialDrawNumber() {
      return this.drawNumbers.slice(-1)[0];
    },
    daXiaoText() {
      return this.currentDrawNumbers.length
        ? this.getDaXiaoText(this.specialDrawNumber)
        : "-";
    },
    danShuang() {
      return this.currentDrawNumbers.length
        ? this.getDanShuang(this.specialDrawNumber)
        : "-";
    },
    daXiaoClassName() {
      return this.getDaXiaoBackgroundClassName(this.specialDrawNumber);
    },
    danShuangClassName() {
      return this.getDanShuangBackgroundClassName(this.specialDrawNumber);
    },
    seBoText(){
      return this.getSeBoText(this.specialDrawNumber);
    },
    shengXiaoText(){
      return this.getShengXiao(this.specialDrawNumber);
    },
  },
});
</script>
