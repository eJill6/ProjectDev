<template>
  <div class="confirm_main">
    <!-- 玩法说明 start -->
    <div class="confirm_wrapper">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToHome">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="玩法说明">玩法说明</div>
        </div>
        <div class="setting_wrapper pd_0 flex_second_height" v-if="playTypeRadioComponentName">
          <component :is="playTypeRadioComponentName"></component>
        </div>
      </div>
    </div>
    <!-- 玩法说明 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { playTypeRadioComponents, getComponentName } from "./Description/index";
import { event as eventModel } from "@/models";
import { MqEvent } from "@/mixins";
import { MutationType } from "@/store";
import AssetImage from "@/components/shared/AssetImage.vue";

// todo:改不同彩種不同內容
export default defineComponent({
  mixins: [MqEvent],
  components: { ...playTypeRadioComponents, AssetImage },
  computed: {
    playTypeRadioComponentName(): string {
      if (!this.$store.getters.currentPlayTypeRadio) return "";

      let gameTypeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return `${getComponentName(gameTypeName, "description")}`;
    },
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.rouletteHandle(arg);
    },
    navigateToHome() {
      this.$router.push({ name: "Home" });
    },
  },
});
</script>
