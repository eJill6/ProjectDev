import { defineComponent } from "vue";

export default defineComponent({
    methods: {
        getNumberOdds(number: string) {
            const playName = this.currentPlayGame.playName as string;
            return this.currentRebatePro && this.currentRebatePro[playName][number] || '';
        },
        getNumberOddsByPlayName(playName:string, number: string) {
            return this.currentRebatePro && this.currentRebatePro[playName][number] || '';
        }
    },
    computed: {
        currentRebatePro() {
            const info = this.$store.getters.currentRebatePro
            if(!info) 
                return null;
            let numberOddType : {[key:string]: {[key:string]:string}} = {};
            for(const numberOdd  in info?.numberOdds){
                const splitNumberOdd = numberOdd.split(' ');
                const gameKind = splitNumberOdd[0];
                const gameType = splitNumberOdd[1];
                const value = info?.numberOdds[numberOdd];
                if(!numberOddType[gameKind])
                {
                    numberOddType[gameKind] = {} as {[key:string]:string}
                }
                numberOddType[gameKind][gameType] = value.toString()
            }
            return numberOddType;
        },
        currentPlayGame(){
            return this.$store.getters.playTypeSelected
        }
    }
});