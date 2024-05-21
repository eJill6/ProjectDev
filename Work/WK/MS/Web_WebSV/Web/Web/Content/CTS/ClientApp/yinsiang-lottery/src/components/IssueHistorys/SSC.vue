<template>
  <table class="w-100 fs-3 bet-h-table">
    <tbody>
      <tr v-for="item in list">
        <td>{{ item.issueNo }}</td>
        <td>
          <div class="d-flex justify-content-center">
            <div
              class="lottery_ball_history"              
              v-for="n in item.drawNumbers"
            >
              {{ n }}
            </div>
          </div>
        </td>
        <td>
          <div class="d-flex justify-content-center pt-3-5 pb-3-5">
            <div
              class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 win-type-history"
            >
              {{ getSum(item.drawNumbers) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type-history"
              :class="getDaXiaoBackgroundClassName(item.drawNumbers)"
            >
              {{ getDaXiaoText(item.drawNumbers) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type-history"
              :class="getDanShuangBackgroundClassName(item.drawNumbers)"
            >
              {{ getDanShuang(item.drawNumbers) }}
            </div>
            <div
              class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 ml-2 win-type-history"              
            >
              {{ getLongHuHuo(item.drawNumbers) }}
            </div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SSC } from "@/mixins";
import { IssueHistory } from "@/models";

export default defineComponent({
  mixins: [SSC],
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
});
</script>
