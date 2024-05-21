<template>
    <component v-if="playTypeRadioComponentName" :is="playTypeRadioComponentName"></component>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

const context = require.context('', true, /\w+.vue$/);

const components = {} as { [k: string]: any };

const getComponentName = (gameTypeName: string, playTypeRadioName: string, isChangelong: boolean) => `${isChangelong ? '' : gameTypeName}${isChangelong ? '' : '_'}${playTypeRadioName}`;

export default defineComponent({
    components,
    created(){
        context.keys().forEach(fileName => {
            let componentConfig = context(fileName);
            let fileNameParts = fileName.split('/');   

            if (fileNameParts.length !== 3 && fileNameParts.indexOf(this.changelongFilename) === -1) // 只抓資料夾下的
                return;
            let isChanglong = fileNameParts.indexOf(this.changelongFilename) > -1;

            let gameTypeName = isChanglong ? '' : fileNameParts[1];

            let playTypeRadioName = isChanglong ? 
                fileNameParts[1].replace(/\.\w+$/, '') : 
                fileNameParts[2].replace(/\.\w+$/, '');

            let componentName = getComponentName(gameTypeName, playTypeRadioName, isChanglong);
            let componentOptions = componentConfig.default;
            components[componentName] = componentOptions;
            
        });
    },
    computed: {
        playTypeRadioComponentName(): string {
            if (!this.$store.getters.currentPlayTypeRadio)
                return '';

            let gameTypeName = this.$store.state.lotteryInfo.gameTypeName;
            let playTypeRadioName = this.$store.getters.playTypeSelected.playEnum;            

            return getComponentName(gameTypeName, playTypeRadioName, playTypeRadioName === this.$store.state.changlongKey);                
        },
        changelongFilename(): string {
            return `${this.$store.state.changlongKey}.vue`;
        }
    }
})
</script>
