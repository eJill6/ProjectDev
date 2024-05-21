<template>
  <div class="lotteryrecord_content">
    <div class="lotteryrecord_item" v-for="item in list">
      <div class="lotteryrecord_row">
        <div class="kuaisan_item column_1">
          <div class="kuaisan_text">{{ getShortNumber(item.issueNo) }}</div>
        </div>
        <div class="kuaisan_item">
          <div class="kuaisan_dice jsyxx" v-for="n in item.drawNumbers">
            <AssetImage
              :src="`@/assets/images/game/ic_dice_md_${getAliasName(n)}.png`"
            />
          </div>
        </div>
        <div class="kuaisan_item column_4">
          <div class="kuaisan_text win_type">
            {{ getSum(item.drawNumbers) }}
          </div>
          <div class="fullrange_type" v-if="isBaozi(item.drawNumbers)">
            <AssetImage src="@/assets/images/game/img_fullrange_active.png" />
          </div>
          <template v-else>
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
          </template>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { YXX } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [YXX],
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
  },
});
</script>
