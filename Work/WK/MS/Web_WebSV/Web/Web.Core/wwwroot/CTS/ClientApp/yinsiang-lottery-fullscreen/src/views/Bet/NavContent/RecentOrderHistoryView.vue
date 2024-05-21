<template>
  <div
    class="history_row"
    v-for="item in list"
    v-if="!isLoading && list.length"
  >
    <div class="kuaisan_item">
      <div class="kuaisan_text cornsilk">{{ item.issueNo }}</div>
    </div>
    <div class="kuaisan_item">
      <div class="kuaisan_text cornsilk">{{ item.lotteryType }}</div>
    </div>
    <div class="kuaisan_item">
      <div class="kuaisan_text kiwigreen">{{ item.noteMoneyText }}</div>
    </div>
    <div class="kuaisan_item">
      <div class="kuaisan_text goldenyellow">
        {{ getPrizeMoneyText(item) }}
      </div>
    </div>
    <div class="kuaisan_item gesture" @click="betAsync(item)">
      <div class="kuaisan_continue">续投</div>
      <div class="btn_continue">
        <AssetImage src="@/assets/images/game/img_twoarrow_right.png" alt="" />
      </div>
    </div>
  </div>
  <div class="flex_empty" v-else>
    <div class="emptydata">
      <div class="emptydata_img">
        <AssetImage src="@/assets/images/game/img_emptydata.png" alt="" />
      </div>
      <div class="emptydata_text">暂无数据</div>
    </div>
  </div>
  <!--TODO-->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import {
  ChangLongSelectedItem,
  OrderHistory,
  event as eventModel,
} from "@/models";
import { OrderStatus, GameType } from "@/enums";
import { PlayTypeRadio, PlayMode, RebatePro, MqEvent } from "@/mixins";
import api from "@/api";
import { AssetImage } from "@/components/shared";
import createDialog from "@/createDialog";
import { Dialogs } from "@/components";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio, PlayMode, RebatePro, MqEvent],
  data() {
    return {
      list: [] as OrderHistory[],
      delayOverload: 0,
    };
  },
  methods: {
    onLoadData(arg: eventModel.LotteryDrawArg) {
      clearTimeout(this.delayOverload);
      this.delayOverload = setTimeout(() => {
        this.getListAsync();
      }, 1000);
    },
    async getListAsync() {
      if (this.isLoading) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        let result = await api.getOrderHistoryAsync(null, "", "", "", 10, "0");
        this.list = result.dataDetail;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
        this.$store.commit(MutationType.ReloadBetHistory, false);
      }
    },
    getPrizeMoneyText(orderHistroy: OrderHistory) {
      return orderHistroy.status === OrderStatus.Unawarded
        ? "待开奖"
        : orderHistroy.prizeMoneyText;
    },
    async betAsync(orderHistroy: OrderHistory) {
      let lotteryInfo = this.$store.state.lotteryMenuInfo.find(
        (x) => x.lotteryID === orderHistroy.lotteryId
      );
      if (!lotteryInfo) return;

      if (this.$store.getters.allRebatePros) {
        let radioOdds = await api.getRebateProsAsync();
        this.$store.commit(MutationType.SetAllRebatePros, radioOdds);
      }

      let rebate = this.$store.getters.allRebatePros[
        orderHistroy.lotteryId
      ].find((x) => x.lotteryId === orderHistroy.lotteryId);
      if (!rebate) return;

      // 清除投注項
      this.$store.commit(MutationType.SetNumbers, null);
      this.$store.commit(
        MutationType.SetChangLongNumbers,
        {} as ChangLongSelectedItem
      );

      const playNums = orderHistroy.palyNum.split(" ");
      const gameType = playNums[0];
      const selectedBetNumber = playNums[1];

      let betInfo = {
        id: "1",
        gameTypeId: rebate.gameTypeId,
        gameTypeName: GameType[rebate.gameTypeId].toString(),
        lotteryTypeName: orderHistroy.lotteryType,
        lotteryId: orderHistroy.lotteryId,
        betAmount: orderHistroy.noteMoneyText
          ? Math.abs(Number(orderHistroy.noteMoneyText))
          : 0,
        playTypeRadioName: gameType,
        selectedBetNumber: selectedBetNumber,
        odds: this.getBetOdds(
          lotteryInfo.lotteryID,
          gameType,
          selectedBetNumber
        ).toString(),
      };

      this.$store.commit(MutationType.SetCurrentBetInfo, [betInfo]);

      createDialog(Dialogs.ConfirmBet);
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
  },
  watch: {
    "$store.state.reloadBetHistory": {
      handler(newValue) {
        if (newValue) {
          this.getListAsync();
        }
      },
      deep: true,
    },
  },
});
</script>
