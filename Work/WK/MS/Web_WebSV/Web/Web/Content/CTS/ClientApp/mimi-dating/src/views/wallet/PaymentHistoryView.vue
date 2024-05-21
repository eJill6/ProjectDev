<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">消费记录</div>
        <div class="header_btn align_left"></div>
      </header>
      <!-- Header end -->
      <div class="expenses_record_section bg_subpage">
        <div class="expenses_record_box">
          <div class="expenses_record_title">今日消费{{ sumAmount }}</div>
          <!-- <div class="expenses_record_options">今日</div> -->
          <div class="expenses_record_date" @click="showDatePicker">
            <div>
              <p>{{ spentTime }}</p>
            </div>
            <div>
              <img src="@/assets/images/wallet/ic_wallet_arrow_down.svg" />
            </div>
          </div>
        </div>
      </div>
      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div
          class="overflow no_scrollbar"
          @scroll="onScroll"
          ref="scrollContainer"
        >
          <div
            class="padding_basic pr_0"
            :style="{
              'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
              'padding-bottom': scrollStatus.virtualScroll.paddingBottom + 'px',
            }"
          >
            <!-- 你的切版放這裡 -->
            <div class="expenses_record_list_section" v-for="item in orderList">
              <div class="expenses_record_list_item">
                <div class="expenses_record_list_box">
                  <div>
                    <img :src="welletDetailImage(item.category)" />
                  </div>
                  <div class="expenses_record_list_rows">
                    <div class="expenses_record_list_title">
                      {{ item.title }}
                    </div>
                    <div class="expenses_record_list_content">
                      <div class="expenses_record_list_text"></div>
                      <span></span>
                    </div>
                    <div class="expenses_record_list_content">
                      <div class="expenses_record_list_subtext spanishgray">
                        {{ item.transactionTime }}
                      </div>
                      <span></span>
                    </div>
                  </div>
                </div>
                <div class="expenses_record_text">-{{ item.amount }}钻石</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import dayjs from "dayjs";
import { defineComponent } from "vue";
import {
  NavigateRule,
  VirtualScroll,
  PlayGame,
  DialogControl,
  ScrollManager,
} from "@/mixins";
import api from "@/api";
import { MutationType } from "@/store";
import {
  ExpenseInfoModel,
  OptionItemModel,
  PopupModel,
} from "@/models";

export default defineComponent({
  mixins: [NavigateRule, VirtualScroll, PlayGame, DialogControl, ScrollManager],
  data() {
    return {
      totalPage: 1,
      date: "",
      sumAmount: 0,
    };
  },
  methods: {
    showDatePicker() {
      const dateItem: OptionItemModel = {
        key: 0,
        value: this.date,
      };
      const content = this.date ? [dateItem] : [];
      const infoModel: PopupModel = {
        title: "请选择时间",
        content: content,
        isMultiple: false,
      };
      this.showDatePickerDialog(infoModel, (selectedModel) => {
        const list = selectedModel as OptionItemModel[];
        this.date = list[0].value;
        this.reload();
      });
    },
    async reload() {
      this.scrollStatus.list = [];
      this.totalPage = 1;
      this.pageInfo.pageNo = 0;
      await this.loadAsync();
    },
    async loadAsync() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        const nextPage = this.pageInfo.pageNo + 1;
        const result = await api.getExpenseInfo(nextPage, this.date);

        this.totalPage = result.totalPage;
        this.sumAmount = result.totalAmount;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.pageInfo.pageNo = result.pageNo;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    $_onScrollReload() {
      this.reload();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    initDate() {
      this.date = dayjs().format("YYYY-MM-DD");
    },
  },
  async created() {
    this.resetScroll();
    this.initDate();
    await this.reload();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 63;
    },
    spentTime() {
      const dateArray = this.date.split("-");
      if (dateArray.length < 3) return "";
      return `${dateArray[0]}年${dateArray[1]}月${dateArray[2]}日`;
    },
    orderList() {
      return this.scrollStatus.virtualScroll.list as ExpenseInfoModel[];
    },
  },
});
</script>
