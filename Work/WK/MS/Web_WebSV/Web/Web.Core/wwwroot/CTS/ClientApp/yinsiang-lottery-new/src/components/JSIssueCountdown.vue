<template>
  <div class="confirm_up">
    <div class="confirm_infos between">
      <div class="confirm_gameimg" :class="logoClass"><AssetImage :src="logoUrl" /></div>
      <div class="confirm_text">
        {{ closuring ? "封盤中" : showIssueNo ? "本期截止" : "暂无下期信息" }}
      </div>
    </div>
    <CountdownView></CountdownView>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { TimeRules } from "@/enums";
import AssetImage from "@/components/shared/AssetImage.vue";
import { CountdownView } from "@/components";

export default defineComponent({
  mixins: [],
  components: { AssetImage, CountdownView },
  computed: {
    showIssueNo(): boolean {
      return (
        this.$store.getters.showTimeRuleStatus === TimeRules.issueNoCountdown
      );
    },
    closuring(): boolean {
      return (
        this.$store.getters.showTimeRuleStatus === TimeRules.closureCountdown
      );
    },
    logoClass() {
      let typeName = this.$store.state.lotteryInfo.gameTypeName as string;    
      if (typeName){
        switch(typeName.toLocaleLowerCase()){
          case "lp":
            return "roulette";                       
          case "yxx":
            return "yusiasieh";            
        }    
        return "";
      }
    },
    logoUrl() {
      let typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      let iconImg = "";
      if (typeName){
        iconImg = typeName.toLocaleLowerCase();
        switch(iconImg){
          case "lp":
            iconImg = "roulette";
            break;           
          case "yxx":
            iconImg = "yusiasieh";
            break;      
        }    
        iconImg = `@/assets/images/modal/logo_${iconImg}_h.png`;
      }
      return iconImg;
    },
  },
});
</script>
