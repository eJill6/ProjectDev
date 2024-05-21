<template>
    <div class="h-history-list overflow-scroll-y no-scrollbar  position-relative">
        <table class="w-100 fs-3 bet-h-table" v-if="!isLoading && list.length">
            <tbody>
                <tr v-for="item in list">
                    <td>{{ item.issueNo }}期</td>
                    <td>{{ item.lotteryType }}</td>
                    <td><span class="text-bet-lose-darkbg">{{ item.noteMoneyText }}</span></td>
                    <td><span class="text-bet-win-darkbg">{{ getPrizeMoneyText(item) }}</span></td>
                    <td>
                        <div class="d-flex justify-content-center align-items-center cusror-pointer p-3-5" @click="betAsync(item)">
                            <p class="text-yellow mr-1">续投</p>
                            <AssetImage src="@/assets/images/ic_arrow_go_bet.svg" alt=""/>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="position-absolute top-50 start-50 translate-middle" v-if="!isLoading && !list.length">
            <div class="position-relative ic_default size-s">
                
            </div>
            <p class="w-100 fs-2 text-center text-white mt-2">暂无数据</p>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from '@/store';
import { OrderHistory, event as eventModel} from '@/models';
import { OrderStatus } from "@/enums";
import { PlayMode, RebatePro,MqEvent } from '@/mixins';
import api from "@/api";
import AssetImage from '@/components/shared/AssetImage.vue';

export default defineComponent({
    components: {AssetImage},
    mixins: [PlayMode, RebatePro, MqEvent],
    data() {
        return {
            list: [] as OrderHistory[],
        };
    },
    methods: {
        onLotteryDraw(arg: eventModel.LotteryDrawArg) {                
            this.getListAsync();
        },
        async getListAsync() {
            if (this.isLoading) return;

            try {
                this.$store.commit(MutationType.SetIsLoading, true);
                let result = await api.getOrderHistoryAsync(this.lotteryInfo.lotteryId, '', '', '', 8);                
                this.list = result.dataDetail;
            } catch (error) {
                console.error(error);
            }
            finally {
                this.$store.commit(MutationType.SetIsLoading, false);
            }
        },
        getPrizeMoneyText(orderHistroy: OrderHistory) {
            return orderHistroy.status === OrderStatus.Unawarded ? '待开奖' : orderHistroy.prizeMoneyText;
        },
        async betAsync(orderHistroy: OrderHistory) {
            let playConfig = this.$store.state.playConfigs.find(x => x.playModeId === +orderHistroy.playModeId);
            if (!playConfig) return;

            let playType = playConfig.playTypeInfos.find(x => x.info.playTypeID === orderHistroy.playTypeId);
            if (!playType) return;

            let playTypeRadio = Object.values(playType.playTypeRadioInfos).flatMap(x => x).find(x => x.info.playTypeRadioID === orderHistroy.playTypeRadioId);
            if (!playTypeRadio) return;

            await this.changePlayTypeRadioAsync(playTypeRadio);
            const playNums = orderHistroy.palyNum.split(' ');
            const gameType = playNums[0];
            const selectedBetNumber = playNums[1];
            let betInfo = {
                id: '1',
                playTypeRadioName: gameType,
                selectedBetNumber: selectedBetNumber,
                odds: this.getNumberOddsByPlayName(gameType, selectedBetNumber)
            };

            this.$store.commit(MutationType.SetCurrentBetInfo, [betInfo]);
            this.$router.push({ name: 'ConfirmBet' });
        }
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
        }
    }
});
</script>