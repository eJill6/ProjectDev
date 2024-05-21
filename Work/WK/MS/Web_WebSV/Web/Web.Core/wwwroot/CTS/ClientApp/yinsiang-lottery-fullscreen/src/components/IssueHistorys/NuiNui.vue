<template>
  <div class="lotteryrecord_content">
    <template v-for="item in gameData">
      <div class="nuinui_row">
        <!-- 藍方 -->
        <div class="blue md">
          <div class="blue_inner">
            <div class="item">
              <div
                class="title"
                :data-text="`蓝方${item.drawNumbers[0].isWin ? '胜' : ''}`"
              >
                {{ `蓝方${item.drawNumbers[0].isWin ? "胜" : ""}` }}
              </div>
              <div class="bg_result md">
                <div class="result md">
                  <AssetImage
                    :src="`@/assets/images/game/img_${
                      item.drawNumbers[0].isWin ? 'win' : 'lose'
                    }_nui_${item.drawNumbers[0].imageType}.png`"
                  />
                </div>
              </div>
            </div>
            <div class="poker">
              <div class="piece md" v-for="blueArea in item.drawNumbers[0].cards">
                <AssetImage :src="getPokerImageSrc(blueArea)" />
              </div>
            </div>
          </div>
        </div>
        <div class="issue md">{{ item.issueNo }}</div>
        <div class="versus spacing">
          <AssetImage src="@/assets/images/game/img_versus.png" />
        </div>
        <!-- 紅方 -->
        <div class="red md">
          <div class="red_inner">
            <div class="poker">
              <div class="piece md" v-for="redArea in item.drawNumbers[1].cards">
                <AssetImage :src="getPokerImageSrc(redArea)" />
              </div>
            </div>
            <div class="item">
              <div
                class="title"
                :data-text="`红方${item.drawNumbers[1].isWin ? '胜' : ''}`"
              >
                {{ `红方${item.drawNumbers[1].isWin ? "胜" : ""}` }}
              </div>
              <div class="bg_result md">
                <div class="result md">
                  <AssetImage
                    :src="`@/assets/images/game/img_${
                      item.drawNumbers[1].isWin ? 'win' : 'lose'
                    }_nui_${item.drawNumbers[1].imageType}.png`"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="history_line spacing"></div>
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SSC } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";
import { CardSuit, NuiNui, NuiNuiPoker, PokerCard } from "@/GameRules/NuiNuiRule";

export interface IssueNuiNuiHistory {
  issueNo: string;
  drawNumbers: NuiNuiPoker[];
}

export default defineComponent({
  mixins: [SSC],
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
    getPokerImageSrc(item: PokerCard) {
      return `@/assets/images/poker/${CardSuit[item.suit]}_${item.type.toLocaleUpperCase()}.png`
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
