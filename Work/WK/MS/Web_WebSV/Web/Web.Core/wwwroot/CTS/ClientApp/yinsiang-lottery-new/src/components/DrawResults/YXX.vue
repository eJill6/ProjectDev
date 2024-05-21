<template>
  <div class="game_middle mb height">
    <div class="yusiasieh_wrapper height">
      <div
        class="yusiasieh_dice sm" v-for="(n, index) in drawNumber">
        <AssetImage :src="`@/assets/images/game/ic_dice_${getAliasName(n)}.png`" />
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { IssueNo } from "@/models";
import AssetImage from "@/components/shared/AssetImage.vue";
import { MqEvent, WinNumbers, YXX } from "@/mixins";

export default defineComponent({
  mixins: [MqEvent, YXX, WinNumbers.Recently, WinNumbers.Layout],
  components: { AssetImage },
  data() {
    return {
      drawNumber: [] as string[],
    };
  },
  methods: {
    gameResult() {
      if (this.lastDrawNumber.length > 0) {
        this.drawNumber = this.lastDrawNumber;
      }
    },
  },
  watch: {
    issueNo: {
      handler(value) {
        this.gameResult();
      },
      deep: true,
    },
  },
  created() {
    this.gameResult();
  },
  computed: {
    issueNo(): IssueNo {
      return this.$store.state.issueNo;
    },
    lastDrawNumber() {
      return (this.issueNo.lastDrawNumber && this.issueNo.lastDrawNumber.split(",")) || [];
    },
  },
});
</script>
