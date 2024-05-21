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
            return [ '1', '2', '3', '4', '5', '6', '7', '8', '9', '10',
            '11', '12', '13', '14', '15', '16', '17', '18', '19', '20',
            '21', '22', '23', '24', '25', '26', '27', '28', '29', '30',
            '31', '32', '33', '34', '35', '36', '37', '38', '39', '40',
            '41', '42', '43', '44', '45', '46', '47', '48', '49'];
        },
        $_gameTypeDrawNumberCount(): number {
            return 7;
        }
    }
});
</script>

