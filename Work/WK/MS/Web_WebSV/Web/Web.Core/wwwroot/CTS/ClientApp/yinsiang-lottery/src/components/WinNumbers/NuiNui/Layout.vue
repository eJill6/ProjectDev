<template>
    <Recently v-if="hasDrawNumbers"></Recently>
    <Scroll v-else></Scroll>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { WinNumbers } from '@/mixins';
import Recently from './Recently.vue';
import Scroll from './Scroll.vue';

export default defineComponent({
    components: { Recently, Scroll },
    mixins: [WinNumbers.Layout]
})
</script>
