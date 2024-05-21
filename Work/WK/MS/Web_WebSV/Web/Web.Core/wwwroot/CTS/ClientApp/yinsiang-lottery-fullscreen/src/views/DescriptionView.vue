<template>
  <!-- header second start -->
  <header class="header_second_height header_second_bg">
    <div class="header_middle">
      <div class="header_second_title"><p>玩法说明</p></div>
    </div>
  </header>
  <!-- header second  end -->
  <!-- 滑動區塊 start-->
  <div class="flex_second_height" :class="backgroundClass">
    <div class="overflow no-scrollbar">
      <div class="second_adding_basic">
        <div class="header_gamerule">
          <LotteryIcons className="header_gamerule_img"></LotteryIcons>
          <div class="header_gamerule_item">
            <div class="header_gamerule_title">
              {{ lotteryInfo.lotteryTypeName }}
            </div>
            <!-- <div class="header_gamerule_arrow">
              <AssetImage src="@/assets/images/game/ic_game_arrow.png" />
            </div> -->
          </div>
        </div>
        <div class="gamerule_outter" v-if="playTypeRadioComponentName">
          <component :is="playTypeRadioComponentName"></component>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { playTypeRadioComponents, getComponentName } from "./Description/index";
import { AssetImage } from "@/components/shared";
import LotteryIcons from "../components/LotteryIcons";
import { GameType } from "@/enums";
// todo:改不同彩種不同內容
export default defineComponent({
  components: { ...playTypeRadioComponents, AssetImage, LotteryIcons },
  computed: {
    playTypeRadioComponentName(): string {
      if (!this.$store.getters.currentPlayTypeRadio) return "";

      let gameTypeName = this.lotteryInfo.gameTypeName as string;
      return `${getComponentName(gameTypeName, "description")}`;
    },
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    backgroundClass() {
      let gameTypeId = this.lotteryInfo.gameTypeId;
      let lotteryCode = this.lotteryInfo.lotteryCode as string;
      let gameTypeName = this.lotteryInfo.gameTypeName as string;
      if (gameTypeId === GameType.K3) {
        return " bg_gamerule_kuaisan";
      } else if (gameTypeName.toLocaleLowerCase() === "ssc") {
        return " bg_gamerule_omssc";
      } else if (gameTypeName.toLocaleLowerCase() === "lhc") {
        return " bg_gamerule_omlhc";
      } else if (gameTypeName.toLocaleLowerCase() === "sg") {
        return " bg_gamerule_jssg";
      }
      return ` bg_gamerule_${lotteryCode.toLocaleLowerCase()}`;
    },
  },
  methods: {
    navigateToHome() {
      this.$router.push({ name: "Home" });
    },
  },
});
</script>
