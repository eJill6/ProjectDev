import { defineComponent } from 'vue';
import { BetInfo, PlayTypeRadioInfo, SelectedModel } from '@/models';
import { MutationType } from '@/store';
import RebatePro from './RebatePro';

export default defineComponent({
    mixins: [RebatePro],
    data() {
        return {

        };
    },
    watch: {
        selectedNumbers: {
            handler() {
                this.convertSelectedNumbers();
            },
            deep: true
        }
    },
    methods: {
        isNumberSelected(fieldIndex: number, numberIndex: number) {
            const playName = this.gameSelected.playName as string;
            return !!this.selectedNumbers[playName][fieldIndex][numberIndex];
        },
        addSelectedNumber(fieldIndex: number, numberIndex: number) {
            const playName = this.gameSelected.playName as string;
            let number = this.currentPlayConfig[playName][fieldIndex][numberIndex];
            this.selectedNumbers[playName][fieldIndex].splice(numberIndex, 1, number);
        },
        removeSelectedNumber(fieldIndex: number, numberIndex: number) {
            const playName = this.gameSelected.playName as string;
            this.selectedNumbers[playName][fieldIndex].splice(numberIndex, 1, '');
        },
        toggleSelectNumber(fieldIndex: number, numberIndex: number) {
            if (this.isNumberSelected(fieldIndex, numberIndex))
                this.removeSelectedNumber(fieldIndex, numberIndex);
            else
                this.addSelectedNumber(fieldIndex, numberIndex);
        },
        convertSelectedNumbers() {
            const results = [] as BetInfo[];
            let numberId:number = 0;
            for(let gameType in this.currentPlayConfig){
                let selectedNumbers = this.selectedNumbers[gameType].map(numbers => numbers.filter(n => !!n)); //filter:去掉null empty '' undefined; 
                let betInfos:BetInfo[] = selectedNumbers.flatMap(x => x).map((x, i) => ({
                    id: (numberId++).toString(),
                    playTypeRadioName: gameType,
                    selectedBetNumber: x,
                    odds: this.getNumberOddsByPlayName(gameType,x)
                }));
                results.push(...betInfos);
            }
            this.$store.commit(MutationType.SetCurrentBetInfo, results);
        },
    },
    computed: {
        currentPlayTypeRadio(): PlayTypeRadioInfo {
            return this.$store.getters.currentPlayTypeRadio as PlayTypeRadioInfo;
        },
        playTypeRadio():Array<Array<string>>{
            const playConfig = this.$store.getters.playConfig;
            const playTypeSelected = this.$store.getters.playTypeSelected;
            const gameType = playTypeSelected.playName as string;
            return playConfig[gameType]
        },
        currentPlayConfig(){
            return this.$store.getters.playConfig;
        },
        gameSelected(){
            return this.$store.getters.playTypeSelected;
        },
        selectedNumbers(){
            return this.$store.getters.selectedNumbers
        }
    }
});
