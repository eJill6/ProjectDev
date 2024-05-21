<template>
  <!-- 设置筹码 start -->
  <div class="confirm_wrapper">
    <div class="confirm_outter">
      <div class="confirm_close" @click="navigateToBet">
        <AssetImage src="@/assets/images/modal/ic_confirm_bet_close.png" />
      </div>
      <div class="confirm_header">
        <div class="confirm_header_title">设置筹码</div>
      </div>
      <div class="setting_wrapper flex_setting">
        <div
          class="setting_chips"
          :class="{ active: isSelectedBaseAmount(unit) }"
          v-for="unit in groupedBaseAmountUnits"
          @click="changeSelectedBaseAmount(unit)"
        >
          <div class="setting_num" :class="[getClassName(unit)]">
            <div class="setting_option">{{ unit }}</div>
          </div>
        </div>
        <div class="setting_chips" @click="navigateToBaseAmountCustom">
          <div class="setting_num black">
            <div class="setting_option">自定义</div>
          </div>
        </div>
      </div>
      <div class="setting_btns" @click="confirmBaseAmount">
        <div class="btn_default basis_40 confirm">确定</div>
      </div>
      <div class="menu_block" v-if="hasIOS"></div>
    </div>
  </div>
  <!-- 设置筹码 end -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaseAmount } from "@/mixins";
import { MutationType } from "@/store";
import { AssetImage } from "../shared";
import BaseDialog from "./BaseDialog";
import showAmountCustom from "@/showAmountCustom";
import { isIOS } from "@/gameConfig";

export default defineComponent({
  extends: BaseDialog,
  components: { AssetImage },
  mixins: [BaseAmount],
  data() {
    return {
      selectedBaseAmount: 0,
    };
  },
  methods: {
    navigateToBet() {
      this.closeEvent();
    },
    navigateToBaseAmountCustom() {
      showAmountCustom(() => {
        this.closeEvent();
      });
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
      return this.baseAmountUnits;
    },
    hasIOS() {
      return isIOS;
    },
  },
});
</script>
