<template>
  <div class="popup_main_content bottom bg2 action_sheet">
    <div class="head">
      <div class="btn align_left" @click="closeEvent"><div>取消</div></div>
      <div class="title">{{ popupInfo.title }}</div>
      <div class="btn align_right highlight" @click="confirmEveit">
        <div>完成</div>
      </div>
    </div>
    <div class="scroll_content overflow no_scrollbar">
      <div class="d-flex">
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
    </div>
  </div>
</template>
<script lang="ts">
import { PopupModel } from "@/models";
import { defineComponent } from "vue";
import { VueScrollPicker } from "vue-scroll-picker";
import dayjs from "dayjs";
import { OptionItemModel } from "@/models";
import BaseDialog from "./BaseDialog";
import { PlayGame } from "@/mixins";

export default defineComponent({
  extends: BaseDialog,
  mixins: [PlayGame],
  props: {
    propObject: {
      type: Object as () => PopupModel,
      required: true,
    },
  },
  components: { VueScrollPicker },
  data() {
    return {
      currentYearOption: 0,
      currentMonthOption: 0,
      currentDateOption: 0,
    };
  },
  methods: {
    getYearOptions() {
      return Object.keys(this.dateOptions);
    },
    getMonthOptions() {
      return Object.keys(this.dateOptions[this.currentYearOption] || []) || [];
    },
    getDateOptions() {
      return (
        (this.dateOptions[this.currentYearOption] || [])[
          this.currentMonthOption
        ] || []
      );
    },
    setCurrentDate() {
      const isExist =
        this.popupInfo &&
        this.popupInfo.content &&
        this.popupInfo.content.length > 0;
      if (isExist) {
        const list = this.popupInfo.content;
        this.setDateTime(list[0]);
      } else {
        const today = new Date();
        this.currentYearOption = today.getFullYear();
        this.currentMonthOption = today.getMonth() + 1;
        this.currentDateOption = today.getDate();
      }
    },
    setDateTime(dateItem: OptionItemModel) {
      const date = new Date(dateItem.value);
      this.currentYearOption = date.getUTCFullYear();
      this.currentMonthOption = date.getUTCMonth() + 1;
      this.currentDateOption = date.getUTCDate();
    },
    confirmEveit() {
      const month = this.prefixInteger(this.currentMonthOption);
      const day = this.prefixInteger(this.currentDateOption);
      const item: OptionItemModel = {
        key: 0,
        value: `${this.currentYearOption}-${month}-${day}`,
      };
      this.callbackEvent([item]);
    },
  },
  created() {
    this.setCurrentDate();
  },
  computed: {
    dateOptions() {
      const dayCount = 500;
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
    popupInfo() {
      return this.propObject;
    },
  },
});
</script>
<style src="vue-scroll-picker/lib/style.css"></style>
<style scoped>
.d-flex {
  display: flex;
}
</style>
