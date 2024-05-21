<template>
  <div class="popup_main_content right side_filter">
    <div class="overflow no_scrollbar filter_hight">
      <div class="filter_padding_basic">
        <div class="filter_tag_title">
          <div class="filter_title">筛选条件</div>
          <div class="filter_title">年龄</div>
          <div class="filter_item">
            <div
              class="filter_btn"
              v-for="n in ageInfo"
              :class="[{ active: isAgeSelected(n) }]"
              @click="selectedAge(n)"
            >
              <p>{{ n }}</p>
            </div>
          </div>
        </div>
        <div>
          <div class="filter_title">价格</div>
          <div class="filter_item">
            <div
              class="filter_btn"
              v-for="n in priceInfo"
              :class="[{ active: isPriceSelected(n) }]"
              @click="selectedPrice(n)"
            >
              <p>{{ n }}</p>
            </div>
          </div>
        </div>
        <div>
          <div class="filter_title">身高</div>
          <div class="filter_item">
            <div
              class="filter_btn"
              v-for="n in heightInfo"
              :class="[{ active: isHeightSelected(n) }]"
              @click="selectedHeight(n)"
            >
              <p>{{ n }}</p>
            </div>
          </div>
        </div>
        <div>
          <div class="filter_title">罩杯</div>
          <div class="filter_item">
            <div
              class="filter_btn"
              v-for="n in filterItem.cup"
              :class="[{ active: isCupSelected(n) > -1 }]"
              @click="selectedCup(n)"
            >
              <p>{{ n.value }}</p>
            </div>
          </div>
        </div>
        <div>
          <div class="filter_title">服务项目</div>
          <div class="filter_item">
            <div
              class="filter_btn"
              v-for="n in filterItem.service"
              :class="[{ active: isServiceSelected(n) > -1 }]"
              @click="selectedService(n)"
            >
              <p>{{ n.value }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="filter_confirm">
      <div class="filter_confirmbtn_reset" @click="resetEvent">
        <p>重置</p>
      </div>
      <div class="confirm_btn" @click="confirmEvent">
        <p>确定</p>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import api from "@/api";
import { MutationType } from "@/store";
import {
  OptionItemModel,
  PostFilterOptionsModel,
  PriceLowAndHighModel,
} from "@/models";
import { PostType } from "@/enums";
import BaseDialog from "./BaseDialog";
import toast from "@/toast";

export default defineComponent({
  extends: BaseDialog,
  data() {
    return {
      filterItem: {} as PostFilterOptionsModel,
    };
  },
  methods: {
    isAgeSelected(name: string) {
      if (this.postFilterInfo && this.postFilterInfo.age) {
        return this.postFilterInfo.age[name] || false;
      }
      return false;
    },
    selectedAge(name: string) {
      if (!this.postFilterInfo.age) {
        this.postFilterInfo.age = {} as { [name: string]: number[] };
      }
      const isSelected = this.isAgeSelected(name);
      if (isSelected) {
        delete this.postFilterInfo.age[name];
      } else {
        const item = this.filterItem.age[name];
        this.postFilterInfo.age[name] = item;
      }
      this.$store.commit(MutationType.SetPostFilterInfo, this.postFilterInfo);
    },
    isPriceSelected(name: string) {
      if (this.postFilterInfo && this.postFilterInfo.price) {
        return this.postFilterInfo.price[name];
      }
      return false;
    },
    selectedPrice(name: string) {
      if (!this.postFilterInfo.price) {
        this.postFilterInfo.price = {} as {
          [name: string]: PriceLowAndHighModel;
        };
      }
      const isSelected = this.isPriceSelected(name);
      if (isSelected) {
        delete this.postFilterInfo.price[name];
      } else {
        const item: PriceLowAndHighModel = this.filterItem.price[name];
        this.postFilterInfo.price[name] = item;
      }
      this.$store.commit(MutationType.SetPostFilterInfo, this.postFilterInfo);
    },
    isHeightSelected(name: string) {
      if (this.postFilterInfo && this.postFilterInfo.height) {
        return this.postFilterInfo.height[name];
      }
      return false;
    },
    selectedHeight(name: string) {
      if (!this.postFilterInfo.height) {
        this.postFilterInfo.height = {} as { [name: string]: number[] };
      }
      const isSelected = this.isHeightSelected(name);
      if (isSelected) {
        delete this.postFilterInfo.height[name];
      } else {
        const item = this.filterItem.height[name];
        this.postFilterInfo.height[name] = item;
      }
      this.$store.commit(MutationType.SetPostFilterInfo, this.postFilterInfo);
    },
    isCupSelected(info: OptionItemModel) {
      if (this.postFilterInfo && this.postFilterInfo.cup) {
        return this.postFilterInfo.cup.map((e) => e.key).indexOf(info.key);
      }
      return -1;
    },
    selectedCup(info: OptionItemModel) {
      if (!this.postFilterInfo.cup) {
        this.postFilterInfo.cup = [] as OptionItemModel[];
      }
      const index = this.isCupSelected(info);
      if (index > -1) {
        this.postFilterInfo.cup.splice(index, 1);
      } else {
        this.postFilterInfo.cup.push(info);
      }
      this.$store.commit(MutationType.SetPostFilterInfo, this.postFilterInfo);
    },
    isServiceSelected(info: OptionItemModel) {
      if (this.postFilterInfo && this.postFilterInfo.service) {
        return this.postFilterInfo.service.map((e) => e.key).indexOf(info.key);
      }
      return -1;
    },
    selectedService(info: OptionItemModel) {
      if (!this.postFilterInfo.service) {
        this.postFilterInfo.service = [] as OptionItemModel[];
      }
      const index = this.isServiceSelected(info);
      if (index > -1) {
        this.postFilterInfo.service.splice(index, 1);
      } else {
        this.postFilterInfo.service.push(info);
      }
      this.$store.commit(MutationType.SetPostFilterInfo, this.postFilterInfo);
    },
    async getOptions() {
      try {
        this.filterItem = await api.getPostFilterOptions(PostType.Square);
      } catch (e) {
        toast(e);
      }
    },
    confirmEvent() {
      this.$store.commit(MutationType.SetSearchStatus, true);
      this.closeEvent();
    },
    resetEvent() {
      this.$store.commit(
        MutationType.SetPostFilterInfo,
        {} as PostFilterOptionsModel
      );
    },
  },
  async created() {
    await this.getOptions();
  },
  computed: {
    paddingCss() {
      const isIOS = !this.$store.state.logonMode;
      return isIOS ? "50px 16px 0 16px" : "0px 16px";
    },
    postFilterInfo() {
      return this.$store.state.postFilterInfo;
    },
    ageInfo(): string[] {
      return this.filterItem.age ? Object.keys(this.filterItem.age) : [];
    },
    priceInfo(): string[] {
      return this.filterItem.price ? Object.keys(this.filterItem.price) : [];
    },
    heightInfo(): string[] {
      return this.filterItem.height ? Object.keys(this.filterItem.height) : [];
    },
  },
});
</script>
