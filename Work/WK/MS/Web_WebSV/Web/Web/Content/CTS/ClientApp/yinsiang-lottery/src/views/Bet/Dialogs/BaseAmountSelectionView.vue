<template>
  <div class="popup-cover" style="position: absolute; right: 0; bottom: 0; top: 0; left: 0">
    <div class="position-fixed w-100 rounded-main bottom-0 bg-pop">
      <div class="position-relative">
        <div
          class="d-flex justify-content-center align-items-center text-black fw-bold betlist_title_text"
        >
          设置筹码
        </div>
        <div class="position-absolute list_closebtn" @click="navigateToBet">
          <div class="cusror-pointer" ></div>
        </div>
      </div>
      <div class="chips-rwd-p">
        <div
          class="d-flex justify-content-between"
          v-for="(unitGroup, groupIndex) in groupedBaseAmountUnits"
        >
          <div
            class="chips-btn"
            :class="{ active: isSelectedBaseAmount(unit) }"
            v-for="unit in unitGroup"
            @click="changeSelectedBaseAmount(unit)"
          >
            <div
              class="d-flex justify-content-center align-items-center text-white fs-8 chips chips-2 cusror-pointer"
              :class="[
                getClassName(unit),
                { active: isSelectedBaseAmount(unit) },
              ]"
            >
              {{ unit }}
            </div>
          </div>
          <div
            class="chips-btn"
            v-if="groupIndex"
            @click="navigateToBaseAmountCustom"
          >
            <div
              class="d-flex justify-content-center align-items-center text-white fs-4 chips chips-custom cusror-pointer"
            >
              自定义
            </div>
          </div>
        </div>
      </div>
      <div class="d-flex justify-content-center pt-6 pb-9">
        <button
          class="second-gradient rounded-full text-white fs-6 fs-6-sm pt-4-5 pb-4-5 list_confirm_btn"
          @click="confirmBaseAmount"
        >
          确定
        </button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaseAmount } from "@/mixins";
import { MutationType } from "@/store";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [BaseAmount],
  data() {
    return {
      selectedBaseAmount: 0,
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    navigateToBaseAmountCustom() {
      this.$router.push({ name: "Bet_BaseAmountCustom" });
    },
    getClassName(amount: number) {
      return `chips-${amount}`;
    },
    isSelectedBaseAmount(amount: number) {
      return this.selectedBaseAmount === amount;
    },
    changeSelectedBaseAmount(amount: number) {
      this.selectedBaseAmount = amount;
    },
    confirmBaseAmount() {
      this.$store.commit(MutationType.SetBaseAmount, this.selectedBaseAmount);
      this.navigateToBet();
    },
  },
  created() {
    this.selectedBaseAmount = this.baseAmount;
  },
  computed: {
    groupedBaseAmountUnits() {
      let countPerGroup = 4;
      let groupCount = Math.ceil(this.baseAmountUnits.length / countPerGroup);
      let result = [] as number[][];

      for (let i = 0; i < groupCount; i++) {
        let offest = i * countPerGroup;
        result[i] = this.baseAmountUnits.slice(offest, offest + countPerGroup);
      }

      return result;
    },
  },
});
</script>