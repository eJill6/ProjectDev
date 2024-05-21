<template>
    <div class="lotteryrecord_content">
        <div class="lotteryrecord_item" v-for="item in list">
            <div class="lotteryrecord_row">
                <div class="kuaisan_item column_1">
                    <div class="kuaisan_text">{{ getShortNumber(item.issueNo) }}</div>
                </div>
                <div class="kuaisan_item">
                    <div class="omlhc_sm_ball" :class="getSeBoBackgroundClassName(n)" v-for="n in drawNumbers(item.drawNumbers)">
                      <div class="num">{{ n }}</div>
                    </div>
                    <div class="omlhc_sm_plus"><AssetImage src="@/assets/images/game/omlsc_sm_plus.png"/></div>
                    <div class="omlhc_sm_ball" :class="getSpecialDiceClassName(item.drawNumbers)">
                      <div class="num">{{specialDrawNumber(item.drawNumbers)}}</div>
                    </div>
                </div>
                <div class="kuaisan_item">
                    <div class="kuaisan_text win_type sm">{{specialDrawNumber(item.drawNumbers)}}</div>
                    <div class="kuaisan_text win_type sm" :class="getDaXiaoBackgroundClassName(specialDrawNumber(item.drawNumbers))">
                      {{ getDaXiaoText(specialDrawNumber(item.drawNumbers)) }}
                    </div>
                    <div class="kuaisan_text win_type sm" :class="getDanShuangBackgroundClassName(specialDrawNumber(item.drawNumbers))">
                      {{ getDanShuang(specialDrawNumber(item.drawNumbers)) }}
                    </div>
                    <div class="kuaisan_text win_type sm"  :class="colorBackgroundClassName(item.drawNumbers)">
                      {{ getSeBoText(specialDrawNumber(item.drawNumbers)) }}
                    </div>
                    <div class="kuaisan_text win_type sm">  
                      {{ getShengXiao(specialDrawNumber(item.drawNumbers)) }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { LHC } from "@/mixins";
import { IssueHistory } from "@/models";
import AssetImage from "../shared/AssetImage.vue";

export default defineComponent({
    mixins: [LHC],
    components: { AssetImage },
    props: {
        list: {
            type: Object as () => IssueHistory[],
            required: true,
        },
    },
    methods: {
        drawNumbers(numbers: string[]) {
            return numbers.slice(0, 6);
        },
        specialDrawNumber(numbers: string[]) {
            return numbers.slice(-1)[0];
        },
        getSpecialDiceClassName(numbers: string[]) {
            const number = this.specialDrawNumber(numbers);
            return this.getSeBoBackgroundClassName(number);
        },
        colorBackgroundClassName(numbers: string[]) {
            const number = this.specialDrawNumber(numbers);
            return this.getColorBackgroundClassName(number);
        },
        getShortNumber(number: string = "") {
          return number.slice(4);
        },
    },
});
</script>
