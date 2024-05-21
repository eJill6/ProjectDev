<template>
  <!-- label filter -->
  <div class="label_filter">
    <div class="item_wrap">
      <div class="item_title">看帖模式：</div>
      <div class="item_list">
        <!-- <div class="item active">小图模式</div> -->
        <div
          v-if="isReloadDiv"
          class="item"
          v-for="n in modeData"
          :class="{ active: isModeSelected(n) }"
          @click="selectedModeValue(n)"
        >
          {{ n.value }}
        </div>
      </div>
    </div>
    <div class="item_wrap no_scrollbar">
      <div class="item_title">排序选择：</div>
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
      <div class="item_title">筛选模式：</div>
      <div class="item_list">
        <div
          class="item"
          v-for="n in statusData"
          :class="{ active: isStatusSelected(n) }"
          @click="selectedStatusValue(n)"
        >
          {{ n.value }}
        </div>
      </div>
    </div>
    <div class="item_wrap no_scrollbar" v-if="!isAgency">
      <div class="item_title">职业类型：</div>
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
    <div class="item_wrap no_scrollbar" v-if="isAgency">
      <div class="item_title">地区选择：</div>
      <div class="item_list" @click="showPositionDialog">
        <div class="filter_wrapper">
          <div class="filiter_icon">
            <CdnImage
              src="@/assets/images/header/ic_location_header.svg"
              alt=""
            />
          </div>
          <div class="item red">
            {{ cityName == "全国" ? "全国（点击选择城市）" : cityName }}
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- label filter End -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import CdnImage from "./CdnImage.vue";
import {
  defaultSort,
  defaultStatus,
  officialDefaultMessage,
  officialDefaultSort,
  officialDefaultStatus,
  postMode,
} from "@/defaultConfig";
import { LabelFilterModel, OptionItemModel, ChinaCityInfo } from "@/models";
import { DialogControl } from "@/mixins";
import api from "@/api";
import { PostLockStatus, PostModeType, PostType, ReportType } from "@/enums";

export default defineComponent({
  components: {
    CdnImage,
  },
  props: {
    postType: {
      type: Number as () => PostType,
      required: true,
    },
  },
  mixins: [DialogControl],
  data() {
    return {
      filterModel: {} as LabelFilterModel,
      messageType: {
        key: 0,
        value: "全部",
      } as OptionItemModel,
      selectedStatus: {} as OptionItemModel,
      sortStatus: {} as OptionItemModel,
      modeStatus: {} as OptionItemModel,
      messageTypeList: [] as OptionItemModel[],
      PostType,
      isReloadDiv: true,
    };
  },
  methods: {
    async setMessageType() {
      if (this.postType === PostType.Official) {
        this.messageTypeList = officialDefaultMessage;
        return;
      }
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
    setModeType() {
      const result = this.modeData.find(
        (item) => item.key === this.filter.modeType
      ) as OptionItemModel;

      this.modeStatus = result ? result : this.modeData[0];
      if (Object.keys(this.filter).length > 0 && this.modeData.length > 1) {
        if (this.filter.modeType) {
          this.modeStatus = this.modeData.find(
            (item) => item.key === this.filter.modeType
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
      const result = this.statusData.find(
        (item) => item.key === this.filter.statusType
      ) as OptionItemModel;
      this.selectedStatus = result ? result : this.statusData[0];
    },
    isModeSelected(item: OptionItemModel) {
      return this.modeStatus.key === item.key;
    },
    selectedModeValue(item: OptionItemModel) {
      const isSelected = this.isModeSelected(item);
      if (!isSelected) {
        this.modeStatus = item;
        this.callback();
      }
    },
    isSortSelected(item: OptionItemModel) {
      return this.sortStatus.key === item.key;
    },
    selectedSortValue(item: OptionItemModel) {
      const isSelected = this.isSortSelected(item);
      if (!isSelected) {
        this.sortStatus = item;
        this.callback();
      }
      //this.sortStatus = isSelected ? ({} as OptionItemModel) : item;
    },
    isMessageSelected(item: OptionItemModel): boolean {
      return this.messageType.key === item.key;
    },
    selectedMessageValue(item: OptionItemModel) {
      const isSelected = this.isMessageSelected(item);
      if (!isSelected) {
        this.messageType = item;
        this.callback();
      }
      //this.messageType = isSelected ? ({} as OptionItemModel) : item;
    },
    isStatusSelected(item: OptionItemModel): boolean {
      return this.selectedStatus.key === item.key;
    },
    selectedStatusValue(item: OptionItemModel) {
      const isSelected = this.isStatusSelected(item);
      if (!isSelected) {
        this.selectedStatus = item;
        this.callback();
      }
      //this.selectedStatus = isSelected ? ({} as OptionItemModel) : item;
    },
    callback() {
      this.filterModel.modeType = this.modeStatus.key;
      this.filterModel.messageId =
        this.messageType.key === 0 ? undefined : this.messageType.key;
      this.filterModel.statusType =
        this.selectedStatus.key < 0 ? undefined : this.selectedStatus.key;
      this.filterModel.sortType =
        Object.keys(this.sortStatus).length === 0
          ? undefined
          : this.sortStatus.key;
      this.$emit("filter", this.filterModel);
    },
  },
  created() {},
  mounted() {
    this.setModeType();
    this.setMessageType();
    this.setSortType();
    this.setStatusType();
  },
  computed: {
    isAgency() {
      return this.$store.state.postType === PostType.Agency;
    },
    cityName() {
      var chinaCity = this.$store.state.city as ChinaCityInfo;
      return chinaCity.name.length > 4
        ? chinaCity.name.slice(0, 4) + "..."
        : chinaCity.name;
    },
    statusData() {
      if (this.postType === PostType.Official) {
        return officialDefaultStatus;
      } else if (this.postType === PostType.Agency) {
        return defaultStatus;
      } else {
        return defaultStatus.filter(
          (item) => item.key !== PostLockStatus.VideoCertified
        );
      }
    },
    sortData() {
      return this.postType === PostType.Official
        ? officialDefaultSort
        : defaultSort;
    },
    modeData() {
      return this.postType === PostType.Agency
        ? postMode
        : postMode.filter((item) => item.key === PostModeType.little);
    },
    filter() {
      return this.$store.state.filter;
    },
    pagePostType() {
      return this.$store.state.postType;
    },
  },
});
</script>
