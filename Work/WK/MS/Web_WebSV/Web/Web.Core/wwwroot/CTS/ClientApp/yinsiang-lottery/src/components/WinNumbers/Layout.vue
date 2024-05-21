<template>
    <component :is="winNumbersComponentName"></component>
</template>

<script lang="ts">
import { MqEvent } from '@/mixins';

const context = require.context('', true, /Layout.vue$/);

const components = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string) => `win_${gameTypeName}`;

context.keys().forEach(fileName => {
    let config = context(fileName);
    let fileNameParts = fileName.split('/');
    let gameTypeName = fileNameParts[1];    
    let componentName = getComponentName(gameTypeName);
    let options = config.default;

    components[componentName] = options;
});

import { defineComponent } from "vue";

export default defineComponent({
    components,
    mixins:[MqEvent],
    computed: {
        winNumbersComponentName(): string {
            return getComponentName(this.$store.state.lotteryInfo.gameTypeName);
        }
    },
});
</script>