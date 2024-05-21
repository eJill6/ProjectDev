<template>
  <table class="w-100 fs-3 bet-pk-table">
    <tbody>
      <tr v-for="item in list">
        <td class="number_top">{{ item.issueNo }}</td>
        <td>
          <div class="list_group racing pt-4 pb-3">
            <li
              :class="getSeBoBackgroundClassName(n)"
              v-for="n in drawNumbers(item.drawNumbers)"
            >
              {{ n }}
            </li>
            <AssetImage class="ml-2" src="@/assets/images/ic_bet_plus.svg" />
            <li :class="getSpecialDiceClassName(item.drawNumbers)">
              {{ specialDrawNumber(item.drawNumbers) }}
            </li>
          </div>

          <div class="d-flex list_group pb-3-5">
            <div
              class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 win-type"
            >
              {{ specialDrawNumber(item.drawNumbers) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
              :class="
                getDaXiaoBackgroundClassName(
                  specialDrawNumber(item.drawNumbers)
                )
              "
            >
              {{ getDaXiaoText(specialDrawNumber(item.drawNumbers)) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
              :class="
                getDanShuangBackgroundClassName(
                  specialDrawNumber(item.drawNumbers)
                )
              "
            >
              {{ getDanShuang(specialDrawNumber(item.drawNumbers)) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
              :class="colorBackgroundClassName(item.drawNumbers)"
            >
              {{ getSeBoText(specialDrawNumber(item.drawNumbers)) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 ml-2 win-type"
            >
              {{ getShengXiao(specialDrawNumber(item.drawNumbers)) }}
            </div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
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
