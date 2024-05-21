<template>
<ConfirmBetView :isDocumentary="false" v-if="isShow"></ConfirmBetView>
</template>

<script lang="ts">
import {defineComponent} from 'vue'
import ConfirmBetView from './ConfirmBetView.vue';
import { MutationType } from '@/store';
import { BetInfo, MsSetting, OrderHistory } from '@/models';
import api from "@/api";
import { PlayMode, RebatePro } from '@/mixins';

export default defineComponent({
    mixins: [PlayMode, RebatePro],
    name:"ReConfirmBetView",
    components:{ConfirmBetView},
    data(){
        return {
            isShow: false
        };
    },
    async created(){
        await this.orderDetail()
    },
    computed:{        
        lotteryInfo() {
            return this.$store.state.lotteryInfo;
        },
        betInfo() {
            return this.$store.state.currnetBetInfo;
        },
        msSetting():MsSetting {
            return this.$store.state.msSetting;
        }
    },
    methods:{
        async orderDetail(){
            const palyId = this.msSetting.orderNo;
            const lotteryId = this.lotteryInfo.lotteryId;            
            if(palyId) {
                this.$store.commit(MutationType.SetIsLoading, true);
                const result = await api.getFollwBetOrderAsync(palyId, lotteryId);                     
                this.betAsync( result.dataDetail);
                this.$store.commit(MutationType.SetIsLoading, false);
            } else {
                this.isShow = true;  
            }
        },
        async betAsync(orderHistroys: OrderHistory[]) {
            let betInfos: BetInfo[] = [];
            
            for(let index = 0; index < orderHistroys.length; index ++) {
                const orderHistroy = orderHistroys[index];
                let playConfig = this.$store.state.playConfigs.find(x => x.playModeId === +orderHistroy.playModeId);
                if (!playConfig) {
                    this.isShow = true;
                    return
                };

                let playType = playConfig.playTypeInfos.find(x => x.info.playTypeID === orderHistroy.playTypeId);
                if (!playType) {
                    this.isShow = true;
                    return
                };

                let playTypeRadio = Object.values(playType.playTypeRadioInfos).flatMap(x => x).find(x => x.info.playTypeRadioID === orderHistroy.playTypeRadioId);
                if (!playTypeRadio) {
                    this.isShow = true;
                    return
                };

                await this.changePlayTypeRadioAsync(playTypeRadio);
                const playNums = orderHistroy.palyNum.split(' ');
                const gameType = playNums[0];
                const selectedBetNumber = playNums[1];

                let betInfo = {
                    id: index.toString(),
                    playTypeRadioName: gameType,
                    selectedBetNumber: selectedBetNumber,
                    odds: this.getNumberOddsByPlayName(gameType, selectedBetNumber)
                };

                betInfos.push(betInfo);
            }            
            if(orderHistroys.length > 0 && orderHistroys.length === betInfos.length) {
                const betInfo = orderHistroys[0];
                this.$store.commit(MutationType.SetBaseAmount, Number(betInfo.noteMoneyText));
                this.$store.commit(MutationType.SetCurrentBetInfo, betInfos);                
            } 
            this.isShow = true;           
        },
        navigateToBet() {
            this.$router.push({ name: 'Bet' });
        },
    }
});
</script>