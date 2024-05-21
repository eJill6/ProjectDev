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
          :options="getProvinceOptions"
          v-model="currentProvinceOption"
        />
        <VueScrollPicker
          :options="getCityOptions"
          v-model="currentCityOption"
        />
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { ChinaCityInfo, OptionItemModel, PopupModel } from "@/models";
import { defineComponent } from "vue";
import { VueScrollPicker } from "vue-scroll-picker";
import BaseView from "./BaseDialog";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  extends: BaseView,
  props: {
    propObject: {
      type: Object as () => PopupModel,
      required: true,
    },
  },
  components: { VueScrollPicker },
  data() {
    return {
      selectedValue: [] as string[],
      currentProvinceOption: "",
      currentCityOption: "",
    };
  },
  methods: {
    confirmEveit() {
      const item: OptionItemModel = {
        key: 0,
        value: this.getCityItem.code,
      };
      this.callbackEvent([item]);
    },
  },
  computed: {
    getProvinceOptions(): string[] {
      return provinceJson.map((item) => item.name);
    },
    getCityOptions(): string[] {
      const province = provinceJson.find(
        (item) => item.name === this.currentProvinceOption
      );
      if (!province) {
        return [];
      }

      const city = cityJson
        .filter((item) => Number(item.province) === Number(province?.province))
        .map((item) => item.name);

      return city.length ? city : [province.name];
    },
    getCityItem(): ChinaCityInfo {
      const province = provinceJson.find(
        (item) => item.name === this.currentProvinceOption
      );
      const city = cityJson.find(
        (item) =>
          Number(item.province) === Number(province?.province) &&
          item.name === this.currentCityOption
      );

      return city ? city : province as ChinaCityInfo;
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
