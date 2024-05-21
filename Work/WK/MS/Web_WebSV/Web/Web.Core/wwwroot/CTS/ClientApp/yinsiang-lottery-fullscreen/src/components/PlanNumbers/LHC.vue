<template>
  <div class="omlhc_inner">
      <div class="omlhc_content">
          <div class="omlhc_item" v-for="n in drawNumbers">
              <div class="omlhc_lg_ball" :class="getSeBoBackgroundClassName(n)">
                <div class="num">{{n}}</div>
              </div>
          </div>
          <div class="omlhc_lg_plus">
              <AssetImage v-if="drawNumbers.length > 0" src="@/assets/images/game/omlsc_lg_plus.png" />
          </div>
          <div class="omlhc_item">
              <div class="omlhc_lg_ball" :class="getSeBoBackgroundClassName(specialDrawNumber)">
                <div class="num">{{ specialDrawNumber }}</div>
              </div>
          </div>
      </div>
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
    allDrawNumbers(){
      return this.hasDrawNumbers ? this.currentDrawNumbers : this.lastDrawNumbers;
    },
    drawNumbers() {
      return this.allDrawNumbers.slice(0, 6);
    },
    specialDrawNumber() {
      return this.allDrawNumbers.slice(-1)[0];
    },
    hasDrawNumbers(): boolean {
      return !!this.$store.state.issueNo.lastDrawNumber;
    },
  },
});
</script>
