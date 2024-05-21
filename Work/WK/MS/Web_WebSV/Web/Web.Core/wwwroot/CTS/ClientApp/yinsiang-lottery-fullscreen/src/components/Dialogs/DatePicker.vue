<template>
  <div class="confirm_wrapper" v-if="confirmedDate">
    <div class="confirm_outter bg_consecutive">
      <div class="confirm_close">
        <AssetImage
          src="@/assets/images/modal/ic_confirm_bet_close.png"
          @click="hideDatePicker"
        />
      </div>
      <div class="confirm_header">
        <div class="confirm_header_title">选择日期</div>
      </div>
      <div
        class="setting_wrapper pt-4 pb-10 pl-6 pr-6 pl-4-sm pr-4-sm"
        style="display: flex"
      >
        <div
          class="consecutive_line"
          :style="{
            top: hasIOS ? `calc(38% - 0.427rem)` : `calc(45% - 0.427rem)`,
            position: `absolute`,
            left: 0,
            right: 0,
            margin: `0 auto`,
          }"
        >
          <AssetImage src="@/assets/images/modal/bg_consecutive_line.png" />
        </div>
        <div
          class="consecutive_line"
          :style="{
            position: `absolute`,
            top: hasIOS ? `calc(53% - 0.427rem)` : `calc(60% - 0.427rem)`,
            left: 0,
            right: 0,
            margin: `0 auto`,
          }"
        >
          <AssetImage src="@/assets/images/modal/bg_consecutive_line.png" />
        </div>
        <VueScrollPicker
          :options="getYearOptions()"
          v-model="currentYearOption"
        />
        <VueScrollPicker
          :options="getMonthOptions()"
          v-model="currentMonthOption"
        />
        <VueScrollPicker
          :options="getDateOptions()"
          v-model="currentDateOption"
        />
      </div>
      <div class="setting_btns" @click="confirmDate">
        <div class="btn_default basis_40 confirm">确定</div>
      </div>
      <div class="menu_block" v-if="hasIOS"></div>
    </div>
  </div>
</template>

<script lang="ts">
import dayjs from "dayjs";
import { defineComponent } from "vue";
import { VueScrollPicker } from "vue-scroll-picker";
import { AssetImage } from "@/components/shared";
import BaseDialog from "./BaseDialog";
import { DialogSettingModel } from "@/models";
import { isIOS } from "@/gameConfig";

const queryDateFormat = "YYYY-MM-DD";

// todo:彩種清單
export default defineComponent({
  components: { VueScrollPicker, AssetImage },
  extends: BaseDialog,
  data() {
    return {
      currentYearOption: 0,
      currentMonthOption: 0,
      currentDateOption: 0,
      confirmedDate: this.dialogSetting?.confirmString || "",
    };
  },
  methods: {
    showDatePicker() {
      let confirmedDate = dayjs(this.confirmedDate);
      this.currentYearOption = confirmedDate.year();
      this.currentMonthOption = confirmedDate.month() + 1;
      this.currentDateOption = confirmedDate.date();
    },
    hideDatePicker() {
      this.closeEvent();
    },
    confirmDate() {
      let year = this.currentYearOption;
      let month = this.currentMonthOption - 1;
      let date = this.currentDateOption;
      this.confirmedDate = dayjs(new Date(year, month, date)).format(
        queryDateFormat
      );
      const model: DialogSettingModel = {
        confirmString: this.confirmedDate,
      };
      this.callbackEvent(model);
    },
    getYearOptions() {
      return Object.keys(this.dateOptions);
    },
    getMonthOptions() {
      return Object.keys(this.dateOptions[this.currentYearOption]) || [];
    },
    getDateOptions() {
      return (
        (this.dateOptions[this.currentYearOption] || [])[
          this.currentMonthOption
        ] || []
      );
    },
  },
  created() {
    this.showDatePicker();
  },
  computed: {
    dateOptions() {
      const dayCount = 35;
      let end = dayjs();
      let start = end.subtract(dayCount, "days");
      let result = {} as {
        [year: number]: {
          [month: number]: number[];
        };
      };

      for (let i = 0; i <= dayCount; i++) {
        let currentDay = start.add(i, "days");
        let year = currentDay.year();
        let month = currentDay.month() + 1;
        let day = currentDay.date();

        result[year] = result[year] || {};
        (result[year][month] = result[year][month] || []).push(day);
      }

      return result;
    },
    formattedConfirmedDateText(): string {
      return dayjs(this.confirmedDate).format("M月DD日");
    },
    hasIOS() {
      return isIOS;
    },
  },
});
</script>
<style src="vue-scroll-picker/lib/style.css"></style>
