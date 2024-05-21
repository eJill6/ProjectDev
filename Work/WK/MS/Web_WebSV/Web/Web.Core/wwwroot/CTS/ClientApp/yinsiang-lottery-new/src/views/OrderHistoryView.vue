<template>
  <div class="confirm_main">
    <!-- 投注记录 start -->
    <div class="confirm_wrapper">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToBet">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="投注记录">投注记录</div>
        </div>
        <div class="record_filter">
          <div class="type_filter">
            <AssetImage src="@/assets/images/record/ic_gametype.png" />
            <p>{{lotteryInfo.lotteryTypeName}}</p>
          </div>
          <div class="type_filter" @click="showOrderStatusOptions">
            <AssetImage src="@/assets/images/record/ic_state.png" />
            <p>{{ currentOrderStatusOptionText }}</p>
          </div>
          <div class="type_filter" @click="showDatePicker">
            <AssetImage src="@/assets/images/record/ic_calendar.png" />
            <p>{{ formattedConfirmedDateText }}</p>
          </div>
        </div>

        <div class="flex_second_height">
          <div
            class="overflow no-scrollbar"
            @scroll="onScroll"
            ref="scrollContainer"
          >
            <div
              class="second_adding_basic"
              :style="{
                'padding-top': virtualScroll.paddingTop + 'px',
                'padding-bottom': virtualScroll.paddingBottom + 'px',
              }"
            >
              <div class="record_content" v-if="orderHistoryList.length">
                <div
                  v-for="item in orderHistoryList"
                  class="record_item"
                  :class="
                    item.status === OrderStatus.Won ? 'gold_bg' : 'gray_bg'
                  "
                >
                  <div
                    class="game_title"
                    :class="{ gray: item.status !== OrderStatus.Won }"
                  >
                    {{ item.lotteryType }}
                  </div>
                  <div class="game_info_group">
                    <div class="game_info">
                      <AssetImage
                        src="@/assets/images/record/ic_arrow_gray.png"
                      />
                      <div class="text">期号</div>
                      <div class="text gray">{{ item.issueNo }}</div>
                    </div>
                    <div class="game_info">
                      <AssetImage
                        src="@/assets/images/record/ic_arrow_gray.png"
                      />
                      <div class="text">投注内容</div>
                      <div class="text yellow">
                        {{ item.playTypeRadioName }}_{{ item.palyNum }}
                      </div>
                      <div class="text blue">@{{ item.odds }}</div>
                    </div>
                    <div class="game_info">
                      <AssetImage
                        src="@/assets/images/record/ic_arrow_gray.png"
                      />
                      <div class="text">投注时间</div>
                      <div class="text gray">
                        {{ formatOrderHistoryDate(item.noteTime) }}
                      </div>
                    </div>
                  </div>
                  <div class="game_Win">{{ item.prizeMoneyText }}</div>
                  <div class="game_bet">{{ item.noteMoneyText }}</div>
                  <div
                    class="record_status"
                    :class="
                      item.status === OrderStatus.Won
                        ? 'gold_status_bg'
                        : 'gray_status_bg'
                    "
                  >
                    <div
                      class="status_content"
                      :class="getOrderStatusClassName(item.status)"
                    >
                      <AssetImage :src="getOrderStatusImgUrl(item.status)" />
                      <p :text="item.statusText">{{ item.statusText }}</p>
                    </div>
                  </div>
                </div>
              </div>
              <div
                class="record_empty"
                v-if="!isLoading && !orderHistoryList.length"
              >
                <AssetImage src="@/assets/images/record/img_emptydata.png" />
                <p>暂无数据</p>
              </div>
            </div>
          </div>
        </div>
        <div class="record_total">
          <div class="total_content">
            <div class="bet_total">
              <div class="item">投注</div>
              <div class="content bet_text">
                <p>{{ totalBetCount }}</p>
                <span>注</span>
              </div>
            </div>

            <div class="bet_total">
              <div class="item">中奖</div>
              <div class="content win_text">
                <p>{{ formattedTotalPrizeMoney }}</p>
              </div>
            </div>

            <div class="bet_total">
              <div class="item">盈利</div>
              <div class="content profit_text">
                <p>{{ formattedTotalWinMoney }}</p>
              </div>
              <!-- <div class="content deficit_text">
                            <p>-253.00</p>
                        </div> -->
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 投注记录 end -->
  </div>

  <div class="confirm_main" v-if="orderStatusOptionsVisible">
    <!-- 選擇交易狀態 start -->
    <div class="confirm_wrapper auto_height">
      <div class="confirm_outter">
        <div class="confirm_close" @click="hideOrderStatusOptions">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="选择交易状态">
            选择交易状态
          </div>
        </div>
        <div class="setting_wrapper">
          <div class="deal_btns">
            <div class="inner">
              <div
                class="default"
                :class="{ active: isCurrentOrderStatusOption(option.value) }"
                @click="changeCurrentOrderStatusOption(option.value)"
                v-for="(option, index) in orderStatusOptions"
              >
                <div class="text" :data-text="option.text">
                  {{ option.text }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 選擇交易狀態 end -->
  </div>

  <div class="confirm_main" v-if="datePickerVisible">
    <!-- 選擇日期 start -->
    <div class="confirm_wrapper auto_height">
      <div class="confirm_outter">
        <div class="confirm_close" @click="hideDatePicker">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="选择日期">选择日期</div>
        </div>
        <div
          class="setting_wrapper pt-4 pb-10 pl-6 pr-6 pl-4-sm pr-4-sm"
          style="display: flex"
        >
          <div
            class="consecutive_line"
            :style="{
              top: `calc(45% - 0.427rem)`,
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
              top: `calc(60% - 0.427rem)`,
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
        <div class="setting_btns pd" @click="confirmDate">
          <div class="btn_default basis_40 confirm">确定</div>
        </div>
      </div>
    </div>
    <!-- 選擇日期 end -->
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
import { orderStatusTypeList } from "@/gameConfig";

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
      OrderStatus,
    };
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;
      this.rouletteHandle(arg);
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
        [OrderStatus.Unawarded]: "ic_waiting",
        [OrderStatus.Won]: "ic_win",
        [OrderStatus.Lost]: "ic_lose",
        [OrderStatus.SystemCancel]: "ic_tie",
        [OrderStatus.SystemRefund]: "ic_back",
      };
      let image = mappings[orderStatus];
      return `@/assets/images/record/${image}.png`;
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
    currentOrderStatusOptionText(): string {
      let option = orderStatusTypeList.find((x) =>
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
      return 102;
    },
    orderStatusOptions() {
      return orderStatusTypeList;
    },
  },
});
</script>

<style src="vue-scroll-picker/lib/style.css"></style>
