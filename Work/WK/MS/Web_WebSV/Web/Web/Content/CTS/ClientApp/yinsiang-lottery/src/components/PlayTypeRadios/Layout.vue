<template>
    <component v-if="playTypeRadioComponentName" :is="playTypeRadioComponentName"></component>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

const context = require.context('', true, /\w+.vue$/);

const components = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string, playTypeRadioName: string) => `${gameTypeName}_${playTypeRadioName}`;

context.keys().forEach(fileName => {
    let componentConfig = context(fileName);
    let fileNameParts = fileName.split('/');

    if (fileNameParts.length !== 3) // 只抓資料夾下的
        return;

    let gameTypeName = fileNameParts[1];
    let playTypeRadioName = fileNameParts[2].replace(/\.\w+$/, '');
    let componentName = getComponentName(gameTypeName, playTypeRadioName);
    let componentOptions = componentConfig.default;
    components[componentName] = componentOptions;
});

export default defineComponent({
    components,
    computed: {
        playTypeRadioComponentName(): string {
            if (!this.$store.getters.currentPlayTypeRadio)
                return '';

            let gameTypeName = this.$store.state.lotteryInfo.gameTypeName;
            let playTypeRadioName = this.$store.getters.playTypeSelected.playEnum;
            return getComponentName(gameTypeName, playTypeRadioName);
        }
    }
})
</script>
