<template>
  <div class="h-100 second-content">
    <div class="position-relative">
      <div class="bg-orange text-white fw-bold rounded-main history_page_title">
        玩法说明
      </div>
      <div class="position-absolute backbtn" @click="navigateToHome"></div>
    </div>
    <div
      class="h-with-title overflow-scroll-y no-scrollbar"
      v-if="playTypeRadioComponentName"
    >
      <component :is="playTypeRadioComponentName"></component>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { playTypeRadioComponents, getComponentName } from "./Description/index";
import { event as eventModel } from "@/models";
import { MqEvent } from "@/mixins";
import { MutationType } from "@/store";
// todo:改不同彩種不同內容
export default defineComponent({
  mixins: [MqEvent],
  components: { ...playTypeRadioComponents },
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
    },
    navigateToHome() {
      this.$router.push({ name: "Home" });
    },
  },
});
</script>
