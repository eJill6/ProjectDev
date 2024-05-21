<template>
  <div class="lotteryrecord_content">
    <div class="lotteryrecord_item" v-for="item in list">
      <div class="lotteryrecord_row">
        <div class="kuaisan_item column_1">
          <div class="kuaisan_text">{{ getShortNumber(item.issueNo) }}</div>
        </div>
        <div class="kuaisan_item">
          <div class="pk10_num" v-for="n in item.drawNumbers">
            <AssetImage
              :src="`@/assets/images/result/pk10_num_md_${parseInt(n)}.png`"
              alt=""
            />
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
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PK10 } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [PK10],
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
