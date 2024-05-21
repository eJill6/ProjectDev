<template>
  <!-- label filter -->
  <div class="label_filter">
    <div class="item_wrap">
      <div class="item_title">类型：</div>
      <div class="item_list">
        <div
          class="item"
          v-for="n in messageTypeList"
          :class="{ active: isMessageSelected(n) }"
          @click="selectedMessageValue(n)"
        >
          {{ n.value }}
        </div>
      </div>
    </div>
    <div class="item_wrap no_scrollbar">
      <div class="item_title">排序：</div>
      <div class="item_list">
        <div
          class="item"
          v-for="n in sortData"
          :class="{ active: isSortSelected(n) }"
          @click="selectedSortValue(n)"
        >
          {{ n.value }}
        </div>
      </div>
    </div>
    <div class="item_wrap no_scrollbar">
      <div class="item_title">状态：</div>
      <div class="item_list">
        <div
          class="item"
          v-for="n in statusStatus"
          :class="{ active: isStatusSelected(n) }"
          @click="selectedStatusValue(n)"
        >
          {{ n.value }}
        </div>
      </div>
    </div>
  </div>
  <!-- label filter End -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { defaultSort, defaultStatus } from "@/defaultConfig";
import { LabelFilterModel, OptionItemModel } from "@/models";
import api from "@/api";
import { PostType } from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  props: {
    postType: {
      type: Number as () => PostType,
      required: true,
    },
  },
  data() {
    return {
      filterModel: {} as LabelFilterModel,
      messageType: {
        key: 0,
        value: "全部",
      } as OptionItemModel,
      selectedStatus: defaultStatus[0],
      sortStatus: {} as OptionItemModel,
      messageTypeList: [] as OptionItemModel[],
    };
  },
  methods: {
    async setMessageType() {
      const defaultMessage = {
        key: 0,
        value: "全部",
      };
      this.messageTypeList.push(defaultMessage);
      const messages = await api.getMessageTypeOptions(this.postType);
      this.messageTypeList = this.messageTypeList.concat(messages);
      if (Object.keys(this.filter).length > 0) {
        if (this.filter.messageId) {
          this.messageType = this.messageTypeList.find(
            (item) => item.key === this.filter.messageId
          ) as OptionItemModel;
        }
      }
    },
    setSortType() {
      const result = this.sortData.find(
        (item) => item.key === this.filter.sortType
      ) as OptionItemModel;
      this.sortStatus = result ? result : this.sortData[0];
    },
    setStatusType() {
      if (Object.keys(this.filter).length > 0) {
        const key =
          this.filter.lockStatus === undefined ? -1 : this.filter.lockStatus;

        this.selectedStatus = this.statusStatus.find(
          (item) => item.key === key
        ) as OptionItemModel;
      }
    },
    isSortSelected(item: OptionItemModel) {
      return this.sortStatus.key === item.key;
    },
    selectedSortValue(item: OptionItemModel) {
      const isSelected = this.isSortSelected(item);
      this.sortStatus = isSelected ? ({} as OptionItemModel) : item;
      this.callback();
    },
    isMessageSelected(item: OptionItemModel): boolean {
      return this.messageType.key === item.key;
    },
    selectedMessageValue(item: OptionItemModel) {
      const isSelected = this.isMessageSelected(item);
      this.messageType = isSelected ? ({} as OptionItemModel) : item;
      this.callback();
    },
    isStatusSelected(item: OptionItemModel): boolean {
      return this.selectedStatus.key === item.key;
    },
    selectedStatusValue(item: OptionItemModel) {
      const isSelected = this.isStatusSelected(item);
      this.selectedStatus = isSelected ? ({} as OptionItemModel) : item;
      this.callback();
    },
    callback() {
      this.filterModel.messageId =
        this.messageType.key === 0 ? undefined : this.messageType.key;
      this.filterModel.lockStatus =
        this.selectedStatus.key < 0 ? undefined : this.selectedStatus.key;
      this.filterModel.sortType =
        Object.keys(this.sortStatus).length === 0
          ? undefined
          : this.sortStatus.key;
      this.$emit("filter", this.filterModel);
    },
  },
  created() {
    this.setMessageType();

    this.setStatusType();
  },
  mounted() {
    this.setSortType();
  },
  computed: {
    statusStatus() {
      return defaultStatus;
    },
    sortData() {
      return defaultSort;
    },
    filter() {
      return this.$store.state.filter;
    },
  },
});
</script>
