<template>
  <div class="confirm_main">
    <!-- 確認投注 start -->
    <div class="confirm_wrapper">
      <div class="confirm_outter bg_consecutive">
        <div class="confirm_close" @click="navigateToBet"><AssetImage src="@/assets/images/modal/ic_confirm_bet_close.png" alt="" /></div>
        <div class="setting_wrapper">
          <div class="setting_item" v-for="(n, index) in countOptions" :class="isSelected(n) ? 'active' : ''" @click="changeSelectedCount(n)">
            <div class="consecutive_title">连开期数<div class="consecutive_text">{{n}}</div>期</div>
            <div class="consecutive_arrow" ><AssetImage src="@/assets/images/game/img_twoarrow_up.png" alt="" /></div>
          </div>
        </div>
        <div class="menu_block" v-if="hasIOS"></div>
      </div>
    </div>
    <!-- 確認投注 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { AssetImage } from "@/components/shared";
import BaseDialog from "./BaseDialog";
import { isIOS } from "@/gameConfig";

export default defineComponent({
  extends: BaseDialog,
  components: { AssetImage },  
  data() {
    return {
      countOptions: [2, 4, 7, 9],
      selectedCount: 0,
    };
  },
  methods: {
    navigateToBet() {
      this.closeEvent();
    },
    isSelected(n: number) {      
      return this.selectedCount === n;
    },
    changeSelectedCount(n: number) {      
      this.$store.commit(MutationType.SetFilterChangLongCount, n);
      this.navigateToBet();
    },
  },
  created() {    
    this.selectedCount = this.$store.state.filterChangLongCount;
  },
  computed:{
    hasIOS(){
      return isIOS;
    }
  }
});
</script>