<template>
  <div class="history_list history_height overflow no-scrollbar">
      <div class="history_row spacing" v-for="item in list">
          <div class="kuaisan_item column_1">
              <div class="kuaisan_text">{{ item.issueNo }}</div>
          </div>
          <div class="kuaisan_item column_fit">
              <div class="omlhc_num" :class="getSeBoBackgroundClassName(n)" v-for="n in drawNumbers(item.drawNumbers)">
                <div class="omlhc_text" :class="getSeBoBackgroundClassName(n)">{{n}}</div>
              </div>

              <div class="omlhc_plus"><AssetImage src="@/assets/images/game/omlsc_sm_plus.png"/></div>
              <div class="omlhc_num" :class="getColorBackgroundClassName(specialDrawNumber(item.drawNumbers))"><div class="omlhc_text">
                {{ specialDrawNumber(item.drawNumbers) }}
              </div></div>
          </div>
          <div class="kuaisan_item column_fit">
              <div class="kuaisan_text win_type">
                {{ specialDrawNumber(item.drawNumbers) }}
              </div>
              <div class="kuaisan_text win_type" :class="getDaXiaoBackgroundClassName(specialDrawNumber(item.drawNumbers))">
                {{ getDaXiaoText(specialDrawNumber(item.drawNumbers)) }}
              </div>
              <div class="kuaisan_text win_type" :class="getDanShuangBackgroundClassName(specialDrawNumber(item.drawNumbers))">
                {{ getDanShuang(specialDrawNumber(item.drawNumbers)) }}
              </div>
              <div class="kuaisan_text win_type" :class="colorBackgroundClassName(item.drawNumbers)">
                {{ getSeBoText(specialDrawNumber(item.drawNumbers)) }}
              </div>
              <div class="kuaisan_text win_type">
                {{ getShengXiao(specialDrawNumber(item.drawNumbers)) }}
              </div>
          </div>
      </div>
  </div>
  
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { LHC } from "@/mixins";
import { IssueHistory } from "@/models";
import AssetImage from "../shared/AssetImage.vue";

export default defineComponent({
    mixins: [LHC],
    components: { AssetImage },
    props: {
        list: {
            type: Object as () => IssueHistory[],
            required: true,
        },
    },
    methods: {
        drawNumbers(numbers: string[]) {
            return numbers.slice(0, 6);
        },
        specialDrawNumber(numbers: string[]) {
            return numbers.slice(-1)[0];
        },
        getSpecialDiceClassName(numbers: string[]) {
            const number = this.specialDrawNumber(numbers);
            return this.getSeBoBackgroundClassName(number);
        },
        colorBackgroundClassName(numbers: string[]) {
            const number = this.specialDrawNumber(numbers);
            return this.getColorBackgroundClassName(number);
        },
    },
});
</script>
