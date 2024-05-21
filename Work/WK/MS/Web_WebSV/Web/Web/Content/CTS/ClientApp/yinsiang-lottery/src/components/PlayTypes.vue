<template>
    <div class="bet-nav-outter" v-if="currentPlayName">
        <button class="bet-nav" :class="[{ 'active': isCurrentPlayType(numberIndex) }]" v-for="(playTypeName, numberIndex) in currentPlayName" @click="changePlayType(numberIndex)">
            {{playTypeName}}
        </button>
    </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayMode } from '@/mixins';
import { MutationType } from "@/store";

export default defineComponent({
    mixins: [PlayMode],
    methods: {
        isCurrentPlayType(index: number) {
            return this.currentPlayTypeSelected === index;
        },
        changePlayType(index: number){
            this.$store.commit(MutationType.SetPlayType,index)
        }
    },
    computed: {
        currentPlayName() { 
            const playConfig = this.$store.getters.playConfig;
            return Object.keys(playConfig);
        },
        currentPlayType() {
            return this.$store.getters.currentPlayType;
        },
        currentPlayTypeSelected(){
            return this.$store.getters.playTypeSelected.selected;
        }
    }
});
</script>