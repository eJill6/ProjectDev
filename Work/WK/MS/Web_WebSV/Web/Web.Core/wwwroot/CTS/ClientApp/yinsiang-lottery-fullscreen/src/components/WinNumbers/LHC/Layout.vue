<template>
    <div class="kuaisan_inner">
        <div class="omlhc_ani" v-if="!hasDrawNumbers"></div>
        <div class="omlhc_content">
            <div class="omlhc_section">
                <div class="omlhc_item" v-for="n in currentDrawNumbers">
                    <div class="omlhc_lg_ball" :class="getSeBoBackgroundClassName(n)">
                        <div class="num">
                            {{ n }}
                        </div>
                    </div>
                </div>
                <div class="omlhc_lg_plus">
                    <AssetImage src="@/assets/images/game/omlsc_lg_plus.png" />
                </div>
                <div class="omlhc_item">
                    <div class="omlhc_lg_ball" :class="getSeBoBackgroundClassName(specialDrawNumber)">
                        <div class="num">
                            {{ specialDrawNumber }}
                        </div>
                    </div>
                </div >
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { WinNumbers } from '@/mixins';
import { LHC } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  mixins: [LHC, WinNumbers.Layout, WinNumbers.Recently],
  components: { AssetImage },
  computed: {
    currentDrawNumbers() {
      return this.drawNumbers.slice(0, 6);
    },
    drawNumbers(): string[] {
      let issueNo = this.$store.state.issueNo;

      return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
    },
    lastDrawNumbers(): string[] {
        let issueNo = this.$store.state.lastIssueNo;

        return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
    },
    specialDrawNumber() {
      return this.drawNumbers.slice(-1)[0];
    },
    // daXiaoText() {
    //   return this.getDaXiaoText(this.specialDrawNumber);
    // },
    // danShuang() {
    //   return this.getDanShuang(this.specialDrawNumber);
    // },
    // daXiaoClassName() {
    //   return this.getDaXiaoBackgroundClassName(this.specialDrawNumber);
    // },
    // danShuangClassName() {
    //   return this.getDanShuangBackgroundClassName(this.specialDrawNumber);
    // },
    // seBoText(){
    //   return this.getSeBoText(this.specialDrawNumber);
    // },
    // shengXiaoText(){
    //   return this.getShengXiao(this.specialDrawNumber);
    // },
  },
});
</script>
