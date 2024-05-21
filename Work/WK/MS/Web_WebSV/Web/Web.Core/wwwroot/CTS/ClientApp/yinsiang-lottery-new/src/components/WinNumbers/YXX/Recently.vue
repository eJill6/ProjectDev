<template>
    <ResultTemplate :drawNumbers="currentDrawNumbers"></ResultTemplate>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers } from '@/mixins';
import ResultTemplate from './ResultTemplate.vue';

export default defineComponent({
    components: { ResultTemplate },
    mixins: [WinNumbers.Recently]
});
</script>

