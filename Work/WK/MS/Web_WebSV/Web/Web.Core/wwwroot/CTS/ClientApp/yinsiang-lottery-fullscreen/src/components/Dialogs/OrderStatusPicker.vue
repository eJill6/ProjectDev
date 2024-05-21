<template>
  <div class="confirm_wrapper">
    <div class="confirm_outter bg_consecutive">
      <div class="confirm_close">
        <AssetImage
          src="@/assets/images/modal/ic_confirm_bet_close.png"
          @click="hideOrderStatusOptions"
        />
      </div>
      <div class="confirm_header">
        <div class="confirm_header_title">选择交易状态</div>
      </div>
      <div class="setting_wrapper">
        <template v-for="(option, index) in orderStatusOptions">
          <div
            class="setting_item"
            :class="{ active: isCurrentOrderStatusOption(option.value) }"
            @click="changeCurrentOrderStatusOption(option.value)"
          >
            <div class="setting_select">{{ option.text }}</div>
          </div>
          <div
            class="consecutive_line"
            v-if="index !== orderStatusOptions.length - 1"
          >
            <AssetImage src="@/assets/images/modal/bg_consecutive_line.png" />
          </div>
        </template>
      </div>
      <div class="menu_block" v-if="hasIOS"></div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { VueScrollPicker } from "vue-scroll-picker";
import { AssetImage } from "@/components/shared";
import BaseDialog from "./BaseDialog";
import { DialogSettingModel } from "@/models";
import { isIOS, orderStatusTypeList } from "@/gameConfig";

// todo:彩種清單
export default defineComponent({
  components: { VueScrollPicker, AssetImage },
  extends: BaseDialog,
  data() {
    return {
      currentOrderStatusOption: this.dialogSetting?.confirmString || "",
    };
  },
  methods: {
    hideOrderStatusOptions() {
      this.closeEvent();
    },
    isCurrentOrderStatusOption(value: string) {
      return this.currentOrderStatusOption === value;
    },
    changeCurrentOrderStatusOption(value: string) {
      const model: DialogSettingModel = {
        confirmString: value,
      };
      this.callbackEvent(model);
    },
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    orderStatusOptions() {
      return orderStatusTypeList;
    },
    hasIOS(){
      return isIOS;
    }
  },
});
</script>

<!-- <style src="vue-scroll-picker/lib/style.css"></style> -->
