<template>
  <div class="lotteryrecord_content">
    <div class="lotteryrecord_item" v-for="item in list">
      <div class="lotteryrecord_row">
        <div class="kuaisan_item column_1">
          <div class="kuaisan_text">{{ getShortNumber(item.issueNo) }}</div>
        </div>
        <div class="kuaisan_item">
          <div class="kuaisan_dice" v-for="n in item.drawNumbers">
            <AssetImage :src="getDiceImageName(n)" />
          </div>
        </div>
        <div class="kuaisan_item">
          <div class="kuaisan_text win_type">
            {{ getSum(item.drawNumbers) }}
          </div>
          <div
            class="kuaisan_text win_type"
            :class="getDaXiaoBackgroundClassName(item.drawNumbers)"
          >
            {{ getDaXiaoText(item.drawNumbers) }}
          </div>
          <div
            class="kuaisan_text win_type"
            :class="getDanShuangBackgroundClassName(item.drawNumbers)"
          >
            {{ getDanShuang(item.drawNumbers) }}
          </div>
        </div>
        <div class="kuaisan_item column_2">
          <div class="kuaisan_text">{{ getWinType(item.drawNumbers) }}</div>
        </div>
        <div class="kuaisan_item column_2">
          <div class="kuaisan_text">
            {{ getWinOdds(item.drawNumbers, getSum(item.drawNumbers)) }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { K3 } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [K3],
  components: { AssetImage },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
  methods: {
    getShortNumber(number: string = "") {
      return number.slice(4);
    },
    getDiceClassName(number: string) {
      return `kuaisan-dice-history-${number}`;
    },
    getDiceImageName(number: string) {
      return `@/assets/images/game/img_md_dice${number}.png`;
    },
    getWinType(numbers: string[]) {
      let distincCount = numbers.filter(
        (value, index, array) => array.indexOf(value) === index
      ).length;
      if (distincCount == 1) {
        return `豹子`;
      } else if (distincCount == 2) {
        return `对子`;
      }
      return `-`;
    },
    getWinOdds(numbers: string[], sum: number) {
      let distincCount = numbers.filter(
        (value, index, array) => array.indexOf(value) === index
      ).length;
      if (distincCount == 1) {
        return `169`;
      }
      if (sum == 4 || sum == 17) {
        return `63`;
      } else if (sum == 5 || sum == 16) {
        return `31.5`;
      } else if (sum == 6 || sum == 15) {
        return `19`;
      }
      return `-`;
    },
  },
  
});
</script>
