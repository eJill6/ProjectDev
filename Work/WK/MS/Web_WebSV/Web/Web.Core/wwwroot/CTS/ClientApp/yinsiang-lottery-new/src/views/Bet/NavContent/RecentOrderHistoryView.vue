<template>
  <div class="h-history-list overflow-scroll-y no-scrollbar position-relative">
    <div
      class="history_list overflow no-scrollbar"
      v-if="!isLoading && list.length"
    >
      <div class="history_row spacing" v-for="item in list" :key="item.issueNo">
        <div class="record_item">
          <div class="text cornsilk">{{ item.issueNo }}期</div>
        </div>
        <div class="record_item" :class="lotteryTypeColumnClass">
          <div class="text cornsilk">{{ item.lotteryType }}</div>
        </div>
        <div class="record_item">
          <div class="text kiwigreen">{{ item.noteMoneyText }}</div>
        </div>
        <div class="record_item">
          <div class="text goldenyellow">{{ getPrizeMoneyText(item) }}</div>
        </div>
        <div class="record_item gesture" @click="betAsync(item)">
          <div class="continue">续投</div>
          <div class="arrow_right">
            <img src="@/assets/images/record/img_twoarrow_right.png" />
          </div>
        </div>
      </div>
    </div>
    <div
      class="history_list overflow no-scrollbar"
      v-if="!isLoading && !list.length"
    >
      <div class="flex_empty">
        <div class="emptydata">
          <div class="emptydata_img">
            <img src="@/assets/images/record/img_emptydata.png" />
          </div>
          <div class="emptydata_text" data-text="暂无数据">暂无数据</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { OrderHistory, event as eventModel } from "@/models";
import { OrderStatus } from "@/enums";
import { PlayMode, RebatePro, MqEvent } from "@/mixins";
import api from "@/api";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayMode, RebatePro, MqEvent],
  data() {
    return {
      list: [] as OrderHistory[],
    };
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;
      this.rouletteHandle(arg);
      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.getListAsync();
    },
    async getListAsync() {
      if (this.isLoading) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        let result = await api.getOrderHistoryAsync(
          this.lotteryInfo.lotteryId,
          "",
          "",
          "",
          8
        );
        this.list = result.dataDetail;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    getPrizeMoneyText(orderHistroy: OrderHistory) {
      return orderHistroy.status === OrderStatus.Unawarded
        ? "待开奖"
        : orderHistroy.prizeMoneyText;
    },
    async betAsync(orderHistroy: OrderHistory) {
      let playConfig = this.$store.state.playConfigs.find(
        (x) => x.playModeId === +orderHistroy.playModeId
      );
      if (!playConfig) return;

      let playType = playConfig.playTypeInfos.find(
        (x) => x.info.playTypeID === orderHistroy.playTypeId
      );
      if (!playType) return;

      let playTypeRadio = Object.values(playType.playTypeRadioInfos)
        .flatMap((x) => x)
        .find((x) => x.info.playTypeRadioID === orderHistroy.playTypeRadioId);
      if (!playTypeRadio) return;

      await this.changePlayTypeRadioAsync(playTypeRadio);
      const playNums = orderHistroy.palyNum.split(" ");
      const gameType = playNums[0];
      const selectedBetNumber = playNums[1];
      let betInfo = {
        id: "1",
        playTypeRadioName: gameType,
        selectedBetNumber: selectedBetNumber,
        odds: this.getNumberOddsByPlayName(gameType, selectedBetNumber),
      };

      this.$store.commit(MutationType.SetCurrentBetInfo, [betInfo]);
      this.$router.push({ name: "ConfirmBet" });
    },
  },
  async created() {
    await this.getListAsync();
  },
  computed: {
    isLoading() {
      return this.$store.state.isLoading;
    },
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    isYXX() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return typeName.toLocaleLowerCase() === "yxx";
    },
    lotteryTypeColumnClass(){
      return this.isYXX ? "style1" : ""
    }
  },
});
</script>
