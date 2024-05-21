<template>
  <!-- header second start -->
  <header class="header_second_height header_second_bg">
    <div class="header_middle">
      <div class="header_second_title">
        <p>投注记录</p>
      </div>
    </div>
  </header>
  <!-- header second  end -->
  <!-- filter start-->
  <div class="record_filter">
    <div class="type_filter" @click="showLotteryOptions">
      <AssetImage src="@/assets/images/record/ic_gametype.png" />
      <p>{{ currentLotteryOptionText }}</p>
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
  <!-- filter end-->

  <!-- 滑動區塊 start-->
  <div class="flex_second_height">
    <div class="overflow no-scrollbar" @scroll="onScroll" ref="scrollContainer">
      <div
        class="second_adding_basic"
        :style="{
          'padding-top': virtualScroll.paddingTop + 'px',
          'padding-bottom': virtualScroll.paddingBottom + 'px',
        }"
      >
        <div class="record_content" v-if="orderHistoryList.length">
          <div
            class="record_item"
            :class="getrecord_itemClassName(item.status)"
            v-for="item in orderHistoryList"
          >
            <div class="game_title">{{ item.lotteryType }}</div>
            <div class="game_info_group">
              <div class="game_info">
                <AssetImage src="@/assets/images/record/ic_arrow_gary.png" />
                <div class="item">期号</div>
                <div class="white_text">{{ item.issueNo }}</div>
              </div>
              <div class="game_info">
                <AssetImage src="@/assets/images/record/ic_arrow_gary.png" />
                <div class="item">投注内容</div>
                <div class="yellow_text">
                  {{ item.playTypeRadioName }}_{{ item.palyNum }}
                </div>
                <div class="blue_text">@{{ item.odds }}</div>
              </div>
              <div class="game_info">
                <AssetImage src="@/assets/images/record/ic_arrow_gary.png" />
                <div class="item">投注时间</div>
                <div class="white_text">
                  {{ formatOrderHistoryDate(item.noteTime) }}
                </div>
                <div class="white_text">
                  {{ formatOrderHistoryTime(item.noteTime) }}
                </div>
              </div>
            </div>
            <div class="game_Win">{{ item.prizeMoneyText }}</div>
            <div class="game_bet">{{ item.noteMoneyText }}</div>
            <div
              class="record_status"
              :class="getOrderStatusClassName(item.status)"
            >
              <div class="status_content">
                <AssetImage :src="getOrderStatusImgUrl(item.status)" />
                <p :text="item.statusText">{{ item.statusText }}</p>
              </div>
            </div>
          </div>
        </div>
        <div class="record_empty" v-else>
          <AssetImage src="@/assets/images/game/img_emptydata.png" />
          <p>暂无数据</p>
        </div>
      </div>
      <div class="record_total">
        <div class="total_content">
          <div class="bet_total">
            <!-- <AssetImage src="@/assets/images/record/ic_bet.png" /> -->
            <div class="item">投注</div>
            <div class="content bet_text">
              <p>{{ totalBetCount }}</p>
              <p>注</p>
            </div>
          </div>

          <div class="bet_total">
            <!-- <AssetImage src="@/assets/images/record/ic_win_L.png" /> -->
            <div class="item">中奖</div>
            <div class="content win_text">
              <p>{{ formattedTotalPrizeMoney }}</p>
            </div>
          </div>

          <div class="bet_total">
            <!-- <AssetImage src="@/assets/images/record/ic_profit.png" /> -->
            <div class="item">盈利</div>
            <div class="content profit_text">
              <p>{{ formattedTotalWinMoney }}</p>
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
import { VueScrollPicker } from "vue-scroll-picker";
import { MutationType } from "@/store";
import { VirtualScroll, MqEvent } from "@/mixins";
import {
  DialogSettingModel,
  OrderHistory,
  event as eventModel,
} from "@/models";
import { OrderStatus } from "@/enums";
import api from "@/api";
import { AssetImage } from "@/components/shared";
import { defaultLotteryInfo, isAndroid, orderStatusTypeList } from "@/gameConfig";
import createDialog from "@/createDialog";
import { Dialogs } from "@/components";

const queryDateFormat = "YYYY-MM-DD";
const queryTimeFormat = "HH:mm:ss";
// todo:彩種清單
export default defineComponent({
  components: { VueScrollPicker, AssetImage },
  mixins: [VirtualScroll, MqEvent],
  data() {
    return {
      nextCursor: "",
      lotteryId: 0,
      hasNextPage: true,
      totalBetCount: 0,
      totalPrizeMoney: 0,
      totalWinMoney: 0,
      currentOrderStatusOption: "",
      confirmedDate: "",
    };
  },
  methods: {
    onLoadData(arg: eventModel.LotteryDrawArg) {
      this.loadAsync();
    },
    async loadAsync() {
      if (this.isLoading || !this.hasNextPage) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);

        let count = 30;
        const id = !this.lotteryId ? null : this.lotteryId;
        let result = await api.getOrderHistoryAsync(
          id,
          this.currentOrderStatusOption,
          this.confirmedDate,
          this.nextCursor,
          count,
          "0"
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
    showLotteryOptions() {
      const model: DialogSettingModel = {
        selectedIndex: this.lotteryId,
      };
      createDialog(Dialogs.LotteryPicker, model, (model) => {
        this.changeCurrentLotteryOption(model.selectedIndex || 0);
      });
    },
    showOrderStatusOptions() {
      const model: DialogSettingModel = {
        confirmString: this.currentOrderStatusOption,
      };
      createDialog(Dialogs.OrderStatusPicker, model, (model) => {
        this.changeCurrentOrderStatusOption(model.confirmString || "");
      });
    },
    isCurrentOrderStatusOption(value: string) {
      return this.currentOrderStatusOption === value;
    },
    changeCurrentLotteryOption(value: number) {
      this.lotteryId = value;
      this.resetPagination();
      this.loadAsync();
    },
    changeCurrentOrderStatusOption(value: string) {
      this.currentOrderStatusOption = value;
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
      const model: DialogSettingModel = {
        confirmString: this.confirmedDate,
      };
      createDialog(Dialogs.DatePicker, model, (model) => {
        this.confirmedDate = model.confirmString || "";
        this.confirmDate();
      });
    },
    confirmDate() {
      this.resetPagination();
      this.loadAsync();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    formatOrderHistoryDate(date: string) {
      return (date && dayjs(date).format(queryDateFormat)) || "";
    },
    formatOrderHistoryTime(date: string) {
      return (date && dayjs(date).format(queryTimeFormat)) || "";
    },
    getrecord_itemClassName(orderStatus: OrderStatus) {
      let mappings = {
        [OrderStatus.Unawarded]: "waiting_bg",
        [OrderStatus.Won]: "win_bg",
        [OrderStatus.Lost]: "lose_bg",
        [OrderStatus.SystemCancel]: "lose_bg",
        [OrderStatus.SystemRefund]: "lose_bg",
      };
      return mappings[orderStatus];
    },
    getOrderStatusClassName(orderStatus: OrderStatus) {
      let mappings = {
        [OrderStatus.Unawarded]: "wait_status_bg",
        [OrderStatus.Won]: "win_status_bg",
        [OrderStatus.Lost]: "lose_status_bg",
        [OrderStatus.SystemCancel]: "lose_status_bg",
        [OrderStatus.SystemRefund]: "back_status_bg",
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
    setAndroidStyle() {
      return isAndroid;
    },
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    orderStatusOptions() {
      return orderStatusTypeList;
    },
    currentOrderStatusOptionText(): string {
      let option = this.orderStatusOptions.find((x) =>
        this.isCurrentOrderStatusOption(x.value)
      );

      return option?.text || "";
    },
    currentLotteryOptionText(): string {
      const list = this.$store.state.lotteryMenuInfo.filter(
        (x) => x.lotteryID === this.lotteryId
      );
      const info = list.length > 0 ? list[0] : defaultLotteryInfo;
      return info.lotteryType || "";
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
      return this.totalPrizeMoney.toFixed(2);
    },
    formattedTotalWinMoney() {
      return this.totalWinMoney.toFixed(2);
    },
    $_virtualScrollItemElemHeight() {
      return 107;
    },
  },
});
</script>

<!-- <style src="vue-scroll-picker/lib/style.css"></style> -->
