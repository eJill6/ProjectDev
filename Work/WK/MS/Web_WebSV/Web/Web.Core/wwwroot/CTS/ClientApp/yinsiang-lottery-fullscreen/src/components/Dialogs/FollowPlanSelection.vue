<template>
  <!-- 选择跟单计划类型 start -->
  <div class="confirm_wrapper">
    <div class="confirm_outter bg_consecutive">
      <div class="confirm_close" @click="closeDialog">
        <AssetImage src="@/assets/images/modal/ic_confirm_bet_close.png" />
      </div>
      <div class="confirm_header">
        <div class="confirm_header_title">选择跟单计划类型</div>
      </div>
      <div class="setting_wrapper">
        <div
          class="setting_item"
          :class="{ active: isTypeSelected(index) }"
          v-for="(name, index) in followOrderType"
          @click="toggleSelectNumber(index)"
        >
          <div class="setting_select">{{ name }}</div>
        </div>
        <div class="consecutive_line">
          <AssetImage src="@/assets/images/modal/bg_consecutive_line.png" />
        </div>
      </div>
      <div class="menu_block" v-if="hasIOS"></div>
    </div>
  </div>
  <!-- 选择跟单计划类型 end -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "@/components/shared/AssetImage.vue";
import { DialogSettingModel, LotteryPlayInfo } from "@/models";
import { followOrderTypeList, isIOS } from "@/gameConfig";
import BaseDialog from "./BaseDialog";

export default defineComponent({
  extends: BaseDialog,
  components: { AssetImage },
  data() {
    return {
      selectedType: this.dialogSetting?.selectedIndex || 0,
      lotteryPlayInfos: [] as LotteryPlayInfo[],
    };
  },
  methods: {
    toggleSelectNumber(type: number) {
      this.selectedType = type;
      let model: DialogSettingModel = {
        selectedIndex: this.selectedType,
      };
      this.callbackEvent(model);
    },
    closeDialog() {
      let model: DialogSettingModel = {
        selectedIndex: this.selectedType,
      };
      this.callbackEvent(model);
    },
    isTypeSelected(index: number) {
      return this.selectedType === index;
    },
  },
  computed: {
    followOrderType() {
      return (
        followOrderTypeList[this.$store.state.lotteryInfo.gameTypeName] || []
      );
    },
    hasIOS() {
      return isIOS;
    },
  },
});
</script>
