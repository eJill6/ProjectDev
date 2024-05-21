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
        <div class="confirm_header_title">选择彩种</div>
      </div>
      <div class="setting_wrapper">
        <template v-for="(option, index) in lotteryOptions">
          <div
            class="setting_item"
            :class="{ active: isCurrentOrderStatusOption(option.lotteryID) }"
            @click="changeCurrentOrderStatusOption(option.lotteryID)"
          >
            <div class="setting_select">{{ option.lotteryType }}</div>
          </div>
          <div
            class="consecutive_line"
            v-if="index !== lotteryOptions.length - 1"
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
import { DialogSettingModel, LotteryMenuInfo } from "@/models";
import { defaultLotteryInfo, isIOS } from "@/gameConfig";

// todo:彩種清單
export default defineComponent({
  components: { VueScrollPicker, AssetImage },
  extends: BaseDialog,
  data() {
    return {
      currentLotteryId: this.dialogSetting?.selectedIndex || 0,
    };
  },
  methods: {
    hideOrderStatusOptions() {
      this.closeEvent();
    },
    isCurrentOrderStatusOption(value: number) {
      return this.currentLotteryId === value;
    },
    changeCurrentOrderStatusOption(value: number) {
      const model: DialogSettingModel = {
        selectedIndex: value,
      };
      this.callbackEvent(model);
    },
  },
  computed: {
    lotteryOptions(): Array<LotteryMenuInfo> {
      let list: LotteryMenuInfo[] = JSON.parse(
        JSON.stringify(this.$store.state.lotteryMenuInfo)
      );
      list.unshift(defaultLotteryInfo);
      return list;
    },
    hasIOS() {
      return isIOS;
    },
  },
});
</script>
