<template>
  <div class="d-flex">
    <div
      class="ml-2"
      :class="getSeBoBackgroundClassName(n)"
      v-for="n in currentDrawNumbers"
    >
      {{ n }}
    </div>
    <AssetImage class="ml-2" src="@/assets/images/ic_bet_plus.svg" />
    <div
      class="ml-2"
      :class="getSeBoBackgroundClassName(specialDrawNumber)"
    >
      {{ specialDrawNumber }}
    </div>
  </div>

  <div class="d-flex justify-content-end mt-2">
    <div
      class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 win-type"
    >
      {{ specialDrawNumber }}
    </div>
    <div
      class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
      :class="daXiaoClassName"
    >
      {{ daXiaoText }}
    </div>
    <div
      class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
      :class="danShuangClassName"
    >
      {{ danShuang }}
    </div>
    <div
      class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
      :class="getColorBackgroundClassName(specialDrawNumber)"
    >
      {{ seBoText }}
    </div>
    <div
      class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 ml-2 win-type"
    >
      {{ shengXiaoText }}
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { LHC } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  mixins: [LHC],
  components: { AssetImage },
  props: {
    drawNumbers: {
      type: Object as () => string[],
      required: true,
    },
  },
  computed: {
    currentDrawNumbers() {
      return this.drawNumbers.slice(0, 6);
    },
    specialDrawNumber() {
      return this.drawNumbers.slice(-1)[0];
    },
    daXiaoText() {
      return this.getDaXiaoText(this.specialDrawNumber);
    },
    danShuang() {
      return this.getDanShuang(this.specialDrawNumber);
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
