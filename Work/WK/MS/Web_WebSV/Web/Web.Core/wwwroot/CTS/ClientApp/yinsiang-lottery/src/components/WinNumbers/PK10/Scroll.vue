<template>
    <ResultTemplate :drawNumbers="randomDrawNumbers"></ResultTemplate>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from '@/mixins';
import ResultTemplate from './ResultTemplate.vue';

export default defineComponent({
    components: { ResultTemplate },
    mixins: [WinNumbers.Scroll],
    computed: {
        $_gameTypeDrawNumbers(): string[] {
            return ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10'];
        },
        $_gameTypeDrawNumberCount(): number {
            return 10;
        }
    }
});
</script>

