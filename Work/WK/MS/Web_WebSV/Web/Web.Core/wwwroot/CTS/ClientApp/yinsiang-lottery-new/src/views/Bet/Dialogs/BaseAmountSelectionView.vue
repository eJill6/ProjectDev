<template>
  <div
    class="confirm_main"
    style="position: absolute; right: 0; bottom: 0; top: 0; left: 0"
  >
    <div class="chips_wrapper">
      <div class="setting_wrapper flex_setting">
        <div class="setting_chips" v-for="unit in baseAmountUnits" :class="{ active: isSelectedBaseAmount(unit) }" @click="changeSelectedBaseAmount(unit)">
          <div class="setting_num" :class="baseAmountUnitColors[unit]">
            <div class="setting_option" :data-text="unit">{{ unit }}</div>
          </div>
        </div>
        <div class="setting_chips" @click="navigateToBaseAmountCustom">
          <div class="setting_num black">
            <div class="setting_option" data-text="自定义">自定义</div>
          </div>
        </div>
      </div>
      <div class="setting_btns" @click="confirmBaseAmount">
        <div class="btn_default basis_40 confirm">确定</div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaseAmount } from "@/mixins";
import { MutationType } from "@/store";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [BaseAmount],
  data() {
    return {
      selectedBaseAmount: 0,
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    navigateToBaseAmountCustom() {
      this.$router.push({ name: "Bet_BaseAmountCustom" });
    },
    getClassName(amount: number) {
      return `chips-${amount}`;
    },
    isSelectedBaseAmount(amount: number) {
      return this.selectedBaseAmount === amount;
    },
    changeSelectedBaseAmount(amount: number) {
      this.selectedBaseAmount = amount;
    },
    confirmBaseAmount() {
      this.$store.commit(MutationType.SetBaseAmount, this.selectedBaseAmount);
      this.navigateToBet();
    },
  },
  created() {
    this.selectedBaseAmount = this.baseAmount;
  },
  computed: {
    groupedBaseAmountUnits() {
      let countPerGroup = 4;
      let groupCount = Math.ceil(this.baseAmountUnits.length / countPerGroup);
      let result = [] as number[][];

      for (let i = 0; i < groupCount; i++) {
        let offest = i * countPerGroup;
        result[i] = this.baseAmountUnits.slice(offest, offest + countPerGroup);
      }
      return result;
    },
  },
});
</script>
