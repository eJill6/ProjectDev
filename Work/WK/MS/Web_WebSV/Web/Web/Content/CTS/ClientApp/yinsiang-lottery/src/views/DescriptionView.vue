<template>
    <div class="h-100 second-content">
        <div class="position-relative">
            <div class="bg-orange text-white fw-bold rounded-main history_page_title">玩法说明</div>
            <div class="position-absolute backbtn" @click="navigateToHome">
            </div>
        </div>
        <div class="h-with-title overflow-scroll-y no-scrollbar" v-if="playTypeRadioComponentName">
            <component :is="playTypeRadioComponentName"></component>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { playTypeRadioComponents, getComponentName } from './Description/index';
// todo:改不同彩種不同內容
export default defineComponent({
    components: { ...playTypeRadioComponents },
    computed: {        
        playTypeRadioComponentName(): string {
            if (!this.$store.getters.currentPlayTypeRadio)
                return '';

            let gameTypeName = this.$store.state.lotteryInfo.gameTypeName as string;            
            return `${getComponentName(gameTypeName, 'description')}`;
        }
    },
    methods: {
        navigateToHome() {
            this.$router.push({ name: 'Home' });
        }
    }
});
</script>