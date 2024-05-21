<template>
  <div class="d-flex">
    <div class="lottery_ball ml-2" v-for="n in drawNumbers">
      {{ parseInt(n) }}
    </div>
  </div>
  <div class="d-flex justify-content-end mt-2">
    <div
      class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 win-type"
    >
      {{ sum }}
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
      class="d-flex justify-content-center align-items-center rounded-1 fs-2 ml-2 win-type text-white" :class="getLongHuHuoBackgroundClassName(drawNumbers)"
    >
      {{ longHuHuo }}
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SSC } from "@/mixins";

export default defineComponent({
  mixins: [SSC],
  props: {
    drawNumbers: {
      type: Object as () => string[],
      required: true,
    },
  },
  computed: {
    sum() {
      return this.getSum(this.drawNumbers);
    },
    daXiaoText() {
      return this.getDaXiaoText(this.drawNumbers);
    },
    danShuang() {
      return this.getDanShuang(this.drawNumbers);
    },
    longHuHuo() {
      return this.getLongHuHuo(this.drawNumbers);
    },
    daXiaoClassName() {
      return this.getDaXiaoBackgroundClassName(this.drawNumbers);
    },
    danShuangClassName() {
      return this.getDanShuangBackgroundClassName(this.drawNumbers);
    }
  },
});
</script>
