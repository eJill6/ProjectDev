<template>
  <div class="h-100 second-content">
    <div class="position-relative">
      <div class="bg-orange text-white fw-bold rounded-main history_page_title">
        投注记录
      </div>
      <div class="position-absolute backbtn" @click="navigateToBet"></div>
    </div>
    <div
      class="d-flex justify-content-between pt-2 pb-2 pl-7 pr-7 pr-5-sm pl-5-sm border-bottom border-gray"
    >
      <div
        class="d-flex align-items-center fs-3 text-black cusror-pointer pt-3 pb-3"
      >
        {{ lotteryInfo.lotteryTypeName }}
        <!-- <AssetImage class="ml-2" src="@/assets/images/ic_history_arrow.svg" alt=""/> -->
      </div>
      <div
        class="d-flex align-items-center fs-3 text-black cusror-pointer pt-3 pb-3"
        @click="showOrderStatusOptions"
      >
        {{ currentOrderStatusOptionText }}
        <AssetImage
          class="ml-2"
          src="@/assets/images/ic_history_arrow.svg"
          alt=""
        />
      </div>
      <div
        class="d-flex align-items-center fs-3 text-black cusror-pointer pt-3 pb-3"
        @click="showDatePicker"
      >
        {{ formattedConfirmedDateText }}
        <AssetImage
          class="ml-2"
          src="@/assets/images/ic_history_arrow.svg"
          alt=""
        />
      </div>
    </div>
    <div
      class="h-history-bet-list overflow-scroll-y no-scrollbar position-relative"
      @scroll="onScroll"
      ref="scrollContainer"
    >
      <div
        class="pt-1 pb-6 pl-7 pr-7 pr-5-sm pl-5-sm"
        v-if="orderHistoryList.length"
      >
        <div
          :style="{
            'padding-top': virtualScroll.paddingTop + 'px',
            'padding-bottom': virtualScroll.paddingBottom + 'px',
          }"
        >
          <div
            class="p-5 mt-4 bg-gary rounded-2 d-flex justify-content-between"
            v-for="item in orderHistoryList"
          >
            <div class="">
              <p class="text-black fs-5 fs-4-sm fw-bold mb-3">
                {{ item.lotteryType }}
              </p>
              <p class="text-medium-gary fs-2 mb-2">
                期号<span class="text-black">{{ item.issueNo }}</span>
              </p>
              <p class="text-medium-gary fs-2 mb-2">
                投注內容<span class="text-bet-order"
                  >{{ item.playTypeRadioName }}_{{ item.palyNum }}</span
                >
                <span class="text-bet-odds">@{{ item.odds }}</span>
              </p>
              <p class="text-medium-gary fs-2">
                投注时间<span class="text-black">{{
                  formatOrderHistoryDate(item.noteTime)
                }}</span>
              </p>
            </div>
            <div class="text-right d-flex align-content-between flex-wrap-wrap">
              <div class="w-100">
                <p class="fs-8 fs-7-sm mb-1 text-bet-win">
                  {{ item.prizeMoneyText }}
                </p>
                <p class="fs-3 text-bet-lose">
                  {{ item.noteMoneyText }}
                </p>
              </div>
              <div class="w-100 d-flex justify-content-end">
                <div
                  class="d-flex align-items-center text-white fs-2 rounded-full p-1-5 pr-3"
                  :class="getOrderStatusClassName(item.status)"
                >
                  <AssetImage
                    class="mr-2 ic-win-tag"
                    :src="getOrderStatusImgUrl(item.status)"
                    alt=""
                  />
                  {{ item.statusText }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        class="position-absolute top-50 start-50 translate-middle"
        v-if="!isLoading && !orderHistoryList.length"
      >
        <div class="position-relative ic_default"></div>
        <p class="w-100 fs-2 text-center text-black text-medium-gary mt-3">
          暂无数据
        </p>
      </div>
    </div>
    <div
      class="d-flex justify-content-between pt-5 pb-5 pl-7 pr-7 pr-5-sm pl-5-sm border-top border-gray"
    >
      <div class="text-black text-center w-33">
        <p class="mb-1 fs-4">投注</p>
        <p class="text-black fw-bold fs-3">
          <span class="text-black fw-bold fs-5 mr-2">{{ totalBetCount }}</span
          >注
        </p>
      </div>
      <div class="text-black text-center w-33">
        <p class="mb-1 fs-4">中奖</p>
        <p class="fs-5 text-bet-win fw-bold">{{ formattedTotalPrizeMoney }}</p>
      </div>
      <div class="text-black text-center w-33">
        <p class="mb-1 fs-4">盈利</p>
        <p class="fs-5 text-bet-win fw-bold" :class="totalWinMoneyClassName">
          {{ formattedTotalWinMoney }}
        </p>
      </div>
    </div>
    <div
      v-if="orderStatusOptionsVisible"
      class="popup-cover"
      style="position: absolute; right: 0; bottom: 0; top: 0; left: 0"
      @click.self="hideOrderStatusOptions"
    >
      <div class="position-fixed w-100 rounded-main bottom-0 bg-pop">
        <div class="position-relative">
          <div
            class="d-flex justify-content-center align-items-center text-black fw-bold list_title_text"
          >
            选择交易状态
          </div>
          <div
            class="position-absolute list_closebtn"
            @click="hideOrderStatusOptions"
          >
            <div class="cusror-pointer"></div>
          </div>
        </div>
        <div class="list_content">
          <div class="d-flex flex-wrap-wrap">
            <div
              class="w-33 pl-2 pr-2"
              v-for="(option, index) in orderStatusOptions"
              @click="changeCurrentOrderStatusOption(option.value)"
            >
              <div
                v-if="index < 3"
                class="d-flex justify-content-center align-items-center pt-6 pb-6 mb-4 mt-4 pl-1 pr-1 fw-bold cusror-pointer list-btn"
                :class="{ active: isCurrentOrderStatusOption(option.value) }"
              >
                {{ option.text }}
              </div>
              <div
                v-else
                class="d-flex justify-content-center align-items-center pt-6 pb-6 pl-1 pr-1 fw-bold cusror-pointer list-btn"
                :class="{ active: isCurrentOrderStatusOption(option.value) }"
              >
                {{ option.text }}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div
      v-if="datePickerVisible"
      class="popup-cover"
      style="position: absolute; right: 0; bottom: 0; top: 0; left: 0"
      @click.self="hideDatePicker"
    >
      <div class="position-fixed w-100 rounded-main bottom-0 bg-pop">
        <div class="position-relative d-flex justify-content-between">
          <div
            class="d-flex justify-content-center align-items-center pl-7 pt-7 pb-7 text-medium-gary fs-5 cusror-pointer"
            @click="hideDatePicker"
          >
            取消
          </div>
          <div
            class="d-flex justify-content-center align-items-center pt-7 pb-7 text-black fs-6 fw-bold"
          >
            选择日期
          </div>
          <div
            class="d-flex justify-content-center align-items-center pr-7 pt-7 pb-7 text-medium-gary fs-5 cusror-pointer"
            @click="confirmDate"
          >
            确认
          </div>
        </div>
        <div class="pt-4 pb-10 pl-6 pr-6 pl-4-sm pr-4-sm">
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
    </div>
  </div>
</template>

<script lang="ts">
import dayjs from "dayjs";
import { defineComponent } from "vue";
import { VueScrollPicker } from "vue-scroll-picker";
import { MutationType } from "@/store";
import { VirtualScroll, MqEvent } from "@/mixins";
import { OrderHistory, event as eventModel } from "@/models";
import { OrderStatus } from "@/enums";
import api from "@/api";
import AssetImage from "@/components/shared/AssetImage.vue";

const queryDateFormat = "YYYY-MM-DD HH:mm:ss";

// todo:彩種清單
export default defineComponent({
  components: { VueScrollPicker, AssetImage },
  mixins: [VirtualScroll, MqEvent],
  data() {
    return {
      nextCursor: "",
      hasNextPage: true,
      totalBetCount: 0,
      totalPrizeMoney: 0,
      totalWinMoney: 0,
      orderStatusOptionsVisible: false,
      currentOrderStatusOption: "",
      datePickerVisible: false,
      currentYearOption: 0,
      currentMonthOption: 0,
      currentDateOption: 0,
      confirmedDate: "",
    };
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.loadAsync();
    },
    async loadAsync() {
      if (this.isLoading || !this.hasNextPage) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);

        let count = 30;
        let result = await api.getOrderHistoryAsync(
          this.lotteryInfo.lotteryId,
          this.currentOrderStatusOption,
          this.confirmedDate,
          this.nextCursor,
          count
        );

        this.list = this.list.concat(result.dataDetail);
        this.nextCursor = result.nextCursor;
        this.hasNextPage = !!result.nextCursor;
        this.totalBetCount = result.totalBetCount;
        this.totalPrizeMoney = result.totalPrizeMoney;
        this.totalWinMoney = result.totalWinMoney;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    showOrderStatusOptions() {
      this.hideDatePicker();
      this.orderStatusOptionsVisible = true;
    },
    hideOrderStatusOptions() {
      this.orderStatusOptionsVisible = false;
    },
    isCurrentOrderStatusOption(value: string) {
      return this.currentOrderStatusOption === value;
    },
    changeCurrentOrderStatusOption(value: string) {
      this.currentOrderStatusOption = value;
      this.hideOrderStatusOptions();
      this.resetPagination();
      this.loadAsync();
    },
    resetPagination() {
      this.list = [];
      this.virtualScroll.list = [];
      this.hasNextPage = true;
      this.nextCursor = "";
      this.totalBetCount = 0;
      this.totalPrizeMoney = 0;
      this.totalWinMoney = 0;

      let container = this.$refs.scrollContainer as HTMLDivElement;

      if (container) container.scrollTop = 0;

      this.calculateVirtualScroll();
    },
    showDatePicker() {
      let confirmedDate = dayjs(this.confirmedDate);
      this.currentYearOption = confirmedDate.year();
      this.currentMonthOption = confirmedDate.month() + 1;
      this.currentDateOption = confirmedDate.date();

      this.hideOrderStatusOptions();
      this.datePickerVisible = true;
    },
    hideDatePicker() {
      this.datePickerVisible = false;
    },
    confirmDate() {
      let year = this.currentYearOption;
      let month = this.currentMonthOption - 1;
      let date = this.currentDateOption;
      this.confirmedDate = dayjs(new Date(year, month, date)).format(
        queryDateFormat
      );

      this.hideDatePicker();
      this.resetPagination();
      this.loadAsync();
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
    $_onScrollToBottom() {
      this.loadAsync();
    },
    formatOrderHistoryDate(date: string) {
      return (date && dayjs(date).format(queryDateFormat)) || "";
    },
    getOrderStatusClassName(orderStatus: OrderStatus) {
      let mappings = {
        [OrderStatus.Unawarded]: "bg-tag-wait",
        [OrderStatus.Won]: "bg-tag-win",
        [OrderStatus.Lost]: "bg-tag-lose",
        [OrderStatus.SystemCancel]: "bg-tag-lose",
        [OrderStatus.SystemRefund]: "bg-tag-lose",
      };

      return mappings[orderStatus];
    },
    getOrderStatusImgUrl(orderStatus: OrderStatus) {
      let mappings = {
        [OrderStatus.Unawarded]: "ic_bet_waiting",
        [OrderStatus.Won]: "ic_bet_win",
        [OrderStatus.Lost]: "ic_bet_lose",
        [OrderStatus.SystemCancel]: "ic_bet_lose",
        [OrderStatus.SystemRefund]: "ic_bet_system_close",
      };

      let image = mappings[orderStatus];
      return "@/assets/images/" + image + ".svg";
    },
  },
  created() {
    this.confirmedDate = dayjs().format(queryDateFormat);
  },
  mounted() {
    this.loadAsync();
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    orderStatusOptions() {
      return [
        { text: "全部状态", value: "" },
        { text: "待开奖", value: OrderStatus.Unawarded },
        { text: "已中奖", value: OrderStatus.Won },
        { text: "未中奖", value: OrderStatus.Lost },
        // { text: '和局', value: OrderStatus.SystemCancel },
        { text: '系统撤单', value: OrderStatus.SystemRefund }
      ];
    },
    currentOrderStatusOptionText(): string {
      let option = this.orderStatusOptions.find((x) =>
        this.isCurrentOrderStatusOption(x.value)
      );

      return option?.text || "";
    },
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
    isLoading() {
      return this.$store.state.isLoading;
    },
    totalWinMoneyClassName() {
      if (!this.totalWinMoney) return "";

      if (this.totalWinMoney > 0) return "text-bet-win";

      return "text-bet-lose";
    },
    orderHistoryList() {
      return this.virtualScroll.list as OrderHistory[];
    },
    formattedTotalPrizeMoney() {
      return this.totalPrizeMoney.toFixed(4);
    },
    formattedTotalWinMoney() {
      return this.totalWinMoney.toFixed(4);
    },
    $_virtualScrollItemElemHeight() {
      return 85;
    },
  },
});
</script>

<style src="vue-scroll-picker/lib/style.css"></style>
