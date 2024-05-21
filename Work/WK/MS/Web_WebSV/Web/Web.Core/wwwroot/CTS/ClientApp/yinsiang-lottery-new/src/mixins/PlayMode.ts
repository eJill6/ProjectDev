import { defineComponent } from 'vue';
import { PlayConfig, PlayTypeInfo, PlayTypeRadioInfo } from '@/models';
import { MutationType } from '@/store';
import api from '@/api';
import { basePlayTypeId } from '@/gameConfig';

export default defineComponent({
    methods: {
        async changePlayModeAsync(playConfig: PlayConfig) {
            
            if (this.$store.state.currentPlayModeId === playConfig.playModeId)
                return;
            
            let defaultPlayTypeInfos = playConfig.playTypeInfos.filter(x => !!basePlayTypeId.includes(x.basePlayTypeId));

            let defaultPlayTypeInfo = defaultPlayTypeInfos[0];
            await this.changePlayTypeAsync(defaultPlayTypeInfo);
        },
        async changePlayTypeAsync(playTypeInfo: PlayTypeInfo) {
            if (this.$store.state.currentPlayModeId === playTypeInfo.info.playTypeID)
                return;

            let defaultTypeModel = Object.keys(playTypeInfo.playTypeRadioInfos)[0];
            let typeModelPlayTypeRadios = playTypeInfo.playTypeRadioInfos[defaultTypeModel];
            let defaultPlayTypeRadio = typeModelPlayTypeRadios[0];
            await this.changePlayTypeRadioAsync(defaultPlayTypeRadio);
        },
        async changePlayTypeRadioAsync(playTypeRadioInfo: PlayTypeRadioInfo) {
            if (this.$store.state.currentPlayTypeRadioId === playTypeRadioInfo.info.playTypeRadioID)
                return;

            let lotteryId = this.$store.state.lotteryInfo.lotteryId;
            let playModeId = playTypeRadioInfo.info.userType;
            let playTypeId = playTypeRadioInfo.info.playTypeID;
            let playTypeRadioId = playTypeRadioInfo.info.playTypeRadioID;

            this.$store.commit(MutationType.SetIsLoading, true);
            let rebatePros = await api.getRebateProAsync(lotteryId, playTypeId, playTypeRadioId);            
            let defaultRebatePro = rebatePros[0];
            this.$store.commit(MutationType.SetIsLoading, false);

            this.$store.commit(MutationType.SetCurrentPlayMode, playModeId);
            this.$store.commit(MutationType.SetCurrentPlayType, playTypeId);
            this.$store.commit(MutationType.SetCurrentPlayTypeRadio, playTypeRadioId);
            //this.$store.commit(MutationType.ClearConfirmedBetNumbers);
            this.$store.commit(MutationType.SetRebatePros, rebatePros);
            this.$store.commit(MutationType.SetCurrentRebatePro, defaultRebatePro.id);            
        },
        async getAllLotteryInfo() {
            const result = await api.getAllLotteryInfo();    
            this.$store.commit(MutationType.SetAllLotteryInfo, result);             
        }
    }    
});