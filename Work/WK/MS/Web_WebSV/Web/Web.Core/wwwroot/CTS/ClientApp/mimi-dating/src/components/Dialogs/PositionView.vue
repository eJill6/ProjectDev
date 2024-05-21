<template>
  <div class="popup_main_content right side_filter">
    <div class="filter_location_side">
      <div class="filter_location"
        :style="{
          padding: isIOS ? '50px 16px 0 16px' : '0px 16px',
          height: '100%',
          display: 'flex',
          'flex-flow': 'column nowrap',
        }"
      >
        <div class="filter_location_title">
          <div class="filter_title_wrapper">
            <div class="filter_title spacing active">热门城市</div>
            <!-- <div class="filter_title spacing">海外城市</div> -->
          </div>
          <div class="filter_item">
            <div
              class="filter_btn"
              v-for="n in popularCity"
              :class="[{ active: isSelectCity(n) }]"
              @click="selectCity(n)"
            >
              <p>{{ n.name }}</p>
            </div>
          </div>
        </div>
        <div class="location_title">
          <div class="location_title_area">
            <div class="location_select_title">
              <p>省</p>
            </div>
          </div>
          <div class="location_title_area">
            <div class="location_select_title">
              <p>市</p>
            </div>
          </div>
        </div>
        <div class="location_select">
          <div class="location_select_area overflow no_scrollbar">
            <div class="location_select_item">
              <div
                class="location_select_btn"
                v-for="province in provinceData"
                :class="[{ active: isSelect(province) }]"
                @click="selectProvice(province)"
              >
                <p>{{ province.name }}</p>
              </div>
            </div>
          </div>
          <div class="location_select_area overflow no_scrollbar">
            <div class="location_select_item">
              <div
                v-if="cityData"
                class="location_select_btn"
                v-for="city in cityData"
                :class="[{ active: isSelectCity(city) }]"
                @click="selectCity(city)"
              >
                <p>{{ city.name }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { ChinaCityInfo } from "@/models";

import { MutationType } from "@/store";
import { defaultCityArea } from "@/defaultConfig";
import BaseDialog from "./BaseDialog";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  extends: BaseDialog,
  data() {
    return {
      popular: [] as string[],
      selectedProvince: {} as ChinaCityInfo | undefined,
    };
  },
  methods: {
    selectProvice(item: ChinaCityInfo) {
      this.selectedProvince = item;
    },
    isSelect(item: ChinaCityInfo) {
      if (this.selectedProvince) {
        return this.selectedProvince.province === item.province;
      }
      return false;
    },
    selectCity(item: ChinaCityInfo) {
      this.$store.commit(MutationType.SetCity, item);
      this.$store.commit(MutationType.SetSearchStatus, true);
      this.closeEvent();
    },
    isSelectCity(item: ChinaCityInfo) {
      if (this.cityInfo) {
        return (
          this.cityInfo.code === item.code
        );
      }
      return false;
    },
    initEvent() {
      if (this.cityInfo) {
        this.selectedProvince = this.provinceData.find(
          (item) => item.province === this.cityInfo?.province
        );
      }
    },
  },
  created() {
    this.initEvent();
  },
  computed: {
    popularCity() {
      return defaultCityArea;
    },
    cityInfo() {
      return this.$store.state.city;
    },
    provinceData() {
      return provinceJson;
    },
    cityData() {
      if (this.selectedProvince) {
        const cityArray = cityJson.filter(
          (item) =>
            Number(item.province) === Number(this.selectedProvince?.province)
        );
        return cityArray.length ? cityArray : [this.selectedProvince];
      }
      return [];
    },
    isIOS() {
      return !this.$store.state.logonMode;
    },
  },
});
</script>