<template>
    <div class="d-flex">
        <div class="bet_win_game_num racing" >
            <li v-for="n in drawNumbers" :class="getDiceClassName(n)">{{parseInt(n)}}</li>
        </div>
    </div>
    <div class="d-flex justify-content-end align-items-center">
        <!-- <div class="bet_videobtn">
            <AssetImage src="@/assets/images/ic_bet_video.svg" />动画
        </div> -->
        <div class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black win-type">
            {{ sum }}
        </div>
        <div class="d-flex justify-content-center align-items-center rounded-1 text-white ml-2 win-type" :class="daXiaoClassName">
            {{ daXiaoText }}
        </div>
        <div class="d-flex justify-content-center align-items-center rounded-1 text-white ml-2 win-type" :class="danShuangClassName">
            {{ danShuang }}
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { PK10 } from '@/mixins';
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
    mixins: [PK10],
    components: { AssetImage },
    props: {
        drawNumbers: {
            type: Object as () => string[],
            required: true
        }
    },
    methods: {
        getDiceClassName(number: string) {            
            return `no_${parseInt(number)}_color`;
        }
    },
    computed: {
        sum() {
            return this.getSum(this.drawNumbers);
        },
        daXiaoText() {
            return this.getDaXiaoText(this.drawNumbers);
        },
        danShuang() {
            return this.getDanShuang(this.drawNumbers);
        },
        daXiaoClassName() {
            return this.getDaXiaoBackgroundClassName(this.drawNumbers);
        },
        danShuangClassName() {
            return this.getDanShuangBackgroundClassName(this.drawNumbers);
        }
    }
});

</script>