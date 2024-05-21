<template>
  <table class="w-100 fs-3 bet-nuinui-table">
    <tbody>
      <template v-for="item in gameData">
        <tr>
          <td class="nuinui_number_top d-flex">
            <div>第</div>
            <div>{{ item.issueNo }}</div>
            <div>期</div>
          </td>
        </tr>
        <tr>
          <td>
            <div class="pt-3 nuinui_result">
              <div class="result_blue">
                <div class="poker_group d-flex justify-content-between">
                  <div
                    class="poker_card"
                    :class="getDiceClassName(blueArea)"
                    v-for="blueArea in item.drawNumbers[0].cards"
                  ></div>
                </div>
                <div
                  v-if="item.drawNumbers[0].imageType"
                  :class="
                    item.drawNumbers[0].victoryConditions.weight !==
                    NuiNuiWeight.noNui
                      ? `blue_result`
                      : `gary_result`
                  "
                >
                  <AssetImage
                    :src="`@/assets/images/nuinui_sesult/game_result_${item.drawNumbers[0].imageType}.png`"
                  />
                </div>
                <div class="blue_win_tag" v-if="item.drawNumbers[0].isWin">
                  <AssetImage
                    src="@/assets/images/nuinui_sesult/win_tag_left.png"
                  />
                </div>
              </div>
              <div class="result_red">
                <div class="poker_group d-flex justify-content-between">
                  <div
                    class="poker_card"
                    :class="getDiceClassName(redArea)"
                    v-for="redArea in item.drawNumbers[1].cards"
                  ></div>
                </div>
                <div v-if="item.drawNumbers[1].imageType" :class="
                    item.drawNumbers[1].victoryConditions.weight !==
                    NuiNuiWeight.noNui
                      ? `red_result`
                      : `gary_result`
                  ">
                  <AssetImage
                    :src="`@/assets/images/nuinui_sesult/game_result_${item.drawNumbers[1].imageType}.png`"
                  />
                </div>
                <div class="red_win_tag" v-if="item.drawNumbers[1].isWin">
                  <AssetImage
                    src="@/assets/images/nuinui_sesult/win_tag_right.png"
                  />
                </div>
              </div>
            </div>
          </td>
        </tr>
      </template>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistory } from "@/models";
import {
  NuiNui,
  PokerResult,
  CardSuit,
  PokerCard,
  NuiNuiWeight,
} from "@/GameRules/NuiNuiRule";
import AssetImage from "@/components/shared/AssetImage.vue";

export interface IssueNuiNuiHistory {
  issueNo: string;
  drawNumbers: PokerResult[];
}

export default defineComponent({
  components: { AssetImage },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
  data() {
    return {
      NuiNuiWeight: NuiNuiWeight,
    };
  },
  methods: {
    getDiceClassName(item: PokerCard) {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `poker_${CardSuit[item.suit]}_${item.type}`;
    },
  },
  computed: {
    gameData() {
      let newList: IssueNuiNuiHistory[] = [];
      const gameData = new NuiNui();
      this.list.forEach((item) => {
        let nuiNuiHistory: IssueNuiNuiHistory = {
          issueNo: item.issueNo,
          drawNumbers: gameData.confirmResult(item.drawNumbers),
        };
        newList.push(nuiNuiHistory);
      });
      return newList;
    },
  },
});
</script>
