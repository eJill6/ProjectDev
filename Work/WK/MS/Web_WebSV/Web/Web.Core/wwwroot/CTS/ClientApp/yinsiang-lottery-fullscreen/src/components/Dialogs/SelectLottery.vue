<template>
  <div class="confirm_wrapper">
    <div class="confirm_outter bg_consecutive">
      <div class="confirm_close" @click="navigateToBet">
        <AssetImage
          src="@/assets/images/modal/ic_confirm_bet_close.png"
          alt=""
        />
      </div>
      <div class="confirm_header">
        <div class="confirm_header_title">选择彩种</div>
      </div>
      <div class="flex_height overflow no-scrollbar setting_wrapper max_h">
        <div class="switch_inner" v-for="items in LotteryMenu">
          <div
            class="switch_item"
            :class="[
              item.typeURL,
              {
                focus:
                  item.lotteryID === lotteryInfo.lotteryId &&
                  !item.isMaintaining,
              },
              { disable: item.isMaintaining },
            ]"
            v-for="item in items"
            @click="changeGameMode(item)"
          ></div>
        </div>
      </div>
      <div class="menu_block" v-if="hasIOS"></div>
    </div>
  </div>
</template>

<script lang="ts">
import { PlayMode } from "@/mixins";
import { LotteryInfo, LotteryMenuInfo } from "@/models";
import { MutationType } from "@/store";
import { defineComponent } from "vue";
import { AssetImage } from "../shared";
import BaseDialog from "./BaseDialog";
import { isIOS } from "@/gameConfig";

export default defineComponent({
  extends: BaseDialog,
  mixins: [PlayMode],
  components: { AssetImage },
  data() {
    return {};
  },
  methods: {
    navigateToBet() {
      this.closeEvent();
    },
    changeGameMode(info: LotteryMenuInfo) {
      if (this.lotteryInfo.lotteryId === info.lotteryID || info.isMaintaining) {
        return;
      }
      this.$store.commit(MutationType.SetGameType, info.typeURL);
      this.navigateToBet();
    },
  },
  async created() {
    await this.getAllLotteryInfo();
  },
  computed: {
    LotteryMenu(): Array<Array<LotteryMenuInfo>> {
      let list = this.$store.state.lotteryMenuInfo.filter(
        (x) => x.lotteryID < 999000
      );
      let matrix = [] as Array<Array<LotteryMenuInfo>>,
        i,
        k;
      const elementsPerSubArray = 2;
      for (i = 0, k = -1; i < list.length; i++) {
        if (i % elementsPerSubArray === 0) {
          k++;
          matrix[k] = [];
        }
        const newItem = list[i];
        matrix[k].push(newItem);
      }

      return matrix;
    },
    lotteryInfo(): LotteryInfo {
      return this.$store.state.lotteryInfo;
    },
    hasIOS() {
      return isIOS;
    },
  },
});
</script>
