<template>
  <table class="w-100 fs-3 bet-pk-table">
    <tbody>
      <tr v-for="item in list">
        <td class="number_top">{{ item.issueNo }}</td>
        <td>
          <div class="list_group racing pt-4 pb-3">
            <li :class="getDiceClassName(n)" v-for="n in item.drawNumbers">
              {{ parseInt(n) }}
            </li>
          </div>

          <div class="d-flex list_group pb-4">
            <div class="d-flex justify-content-center align-items-center rounded-1 bg-white text-black fs-2 win-type">
              {{ getSum(item.drawNumbers) }}
            </div>
            <div class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type" 
              :class="getDaXiaoBackgroundClassName(item.drawNumbers)">
              {{ getDaXiaoText(item.drawNumbers) }}
            </div>
            <div class="d-flex justify-content-center align-items-center rounded-1 text-white fs-2 ml-2 win-type"
              :class="getDanShuangBackgroundClassName(item.drawNumbers)">
              {{ getDanShuang(item.drawNumbers) }}
            </div>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PK10 } from "@/mixins";
import { IssueHistory } from "@/models";

export default defineComponent({
  mixins: [PK10],
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
  methods: {
    getDiceClassName(number: string) {
      return `no_${parseInt(number)}_color`;
    },
  },
});
</script>