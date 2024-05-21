<template>
  <div class="game_middle">
    <div class="player">
      <div class="card_ani_group" v-if="showAnimation">
        <!--發牌動畫 開始-->
        <template v-if="stages === CardStages.deal" v-for="n in [1, 2]">
          <div :class="`card_vertical_left_0${n}`">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
        </template>
        <div
          class="card_horizontal_left"
          v-if="
            playerArea.cards[2].originalNumber !== `0` &&
            stages === CardStages.leftDraw
          "
        >
          <AssetImage src="@/assets/images/poker_card/poker_back.png" />
        </div>
        <!--發牌動畫 結束-->

        <!--翻牌動畫 開始-->
        <template v-if="stages >= CardStages.flop">
          <div class="vertical_left_result_01">
            <AssetImage
              :src="`@/assets/images/poker_card/${getDiceClassName(
                playerArea.cards[1]
              )}.png`"
            />
          </div>
          <div class="vertical_left_result_01_back">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
          <div class="vertical_left_result_02">
            <AssetImage
              :src="`@/assets/images/poker_card/${getDiceClassName(
                playerArea.cards[0]
              )}.png`"
            />
          </div>
          <div class="vertical_left_result_02_back">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
        </template>

        <template
          v-if="
            playerArea.cards[2].originalNumber !== `0` &&
            stages >= CardStages.rightDraw
          "
        >
          <div class="horizontal_result_left">
            <AssetImage
              :src="`@/assets/images/poker_card/${getDiceClassName(
                playerArea.cards[2]
              )}.png`"
            />
          </div>
          <div class="horizontal_result_left_back">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
        </template>
      </div>
      <div class="inner">
        <div class="result" v-if="stages === CardStages.show">
          <div
            :class="[playerResult ? 'win' : 'lose']"
            :data-text="`${playerArea.point}点`"
          >
            {{ `${playerArea.point}点` }}
          </div>
          <div
            class="win"
            :data-text="playerVictoryString"
            v-if="playerVictoryString"
          >
            {{ playerVictoryString }}
          </div>
        </div>
        <div class="title left" data-text="闲">闲</div>
        <div class="poker left">
          <div
            class="vertical"
            v-for="n in 3"
            :class="{ turnleft: n === 1 }"
            v-if="showAnimation"
          >
            <AssetImage src="@/assets/images/game/poker_default_v.png" />
          </div>
          <template v-else>
            <div class="vertical turnleft">
              <template v-if="playerArea.cards[2].originalNumber === `0`">
                <AssetImage src="@/assets/images/game/poker_default_v.png" />
              </template>
              <template
                v-if="
                  playerArea.cards[2].originalNumber !== `0` &&
                  stages === CardStages.leftDraw
                "
              >
                <AssetImage src="@/assets/images/poker_card/poker_back.png" />
              </template>
              <template
                v-if="
                  playerArea.cards[2].originalNumber !== `0` &&
                  stages >= CardStages.rightDraw
                "
              >
                <AssetImage
                  :src="`@/assets/images/poker_card/${getDiceClassName(
                    playerArea.cards[2]
                  )}.png`"
                />
              </template>
            </div>
            <div class="vertical">
              <template v-if="stages === CardStages.deal">
                <AssetImage src="@/assets/images/poker_card/poker_back.png" />
              </template>
              <template v-else>
                <AssetImage
                  :src="`@/assets/images/poker_card/${getDiceClassName(
                    playerArea.cards[1]
                  )}.png`"
                />
              </template>
            </div>
            <div class="vertical">
              <template v-if="stages === CardStages.deal">
                <AssetImage src="@/assets/images/poker_card/poker_back.png" />
              </template>
              <template v-else>
                <AssetImage
                  :src="`@/assets/images/poker_card/${getDiceClassName(
                    playerArea.cards[0]
                  )}.png`"
                />
              </template>
            </div>
          </template>
        </div>
      </div>
    </div>
    <div class="logo">
      <AssetImage :src="logoUrl" />
    </div>
    <div class="board">
      <AssetImage src="@/assets/images/game/logo_board.png" />
    </div>
    <IssueNo></IssueNo>
    <div class="bank">
      <div class="card_ani_group" v-if="showAnimation">
        <!--發牌動畫 開始-->
        <template v-if="stages === CardStages.deal" v-for="n in [1, 2]">
          <div :class="`card_vertical_right_0${n}`">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
        </template>
        <div
          class="card_horizontal_right"
          v-if="
            bankerArea.cards[2].originalNumber !== `0` &&
            stages === CardStages.rightDraw
          "
        >
          <AssetImage src="@/assets/images/poker_card/poker_back.png" />
        </div>
        <!--發牌動畫 結束-->

        <!--翻牌動畫 開始-->
        <template v-if="stages >= CardStages.flop" v-for="n in [0, 1]">
          <div :class="`vertical_right_result_0${n + 1}`">
            <AssetImage
              :src="`@/assets/images/poker_card/${getDiceClassName(
                bankerArea.cards[n]
              )}.png`"
            />
          </div>
          <div :class="`vertical_right_result_0${n + 1}_back`">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
        </template>

        <template
          v-if="
            bankerArea.cards[2].originalNumber !== `0` &&
            stages >= CardStages.rightFlopDraw
          "
        >
          <div class="horizontal_result_right">
            <AssetImage
              :src="`@/assets/images/poker_card/${getDiceClassName(
                bankerArea.cards[2]
              )}.png`"
            />
          </div>
          <div class="horizontal_result_right_back">
            <AssetImage src="@/assets/images/poker_card/poker_back.png" />
          </div>
        </template>

        <!--翻牌動畫 結束-->
      </div>

      <div class="inner">
        <div class="result" v-if="stages === CardStages.show">
          <div
            :class="[bankerResult ? 'win' : 'lose']"
            :data-text="`${bankerArea.point}点`"
          >
            {{ `${bankerArea.point}点` }}
          </div>
          <div
            class="win"
            :data-text="bankerVictoryString"
            v-if="bankerVictoryString"
          >
            {{ bankerVictoryString }}
          </div>
        </div>
        <div class="title right" data-text="庄">庄</div>
        <div class="poker right">
          <div
            class="vertical"
            v-for="n in 3"
            :class="{ turnright: n === 3 }"
            v-if="showAnimation"
          >
            <AssetImage src="@/assets/images/game/poker_default_v.png" />
          </div>

          <template v-else>
            <div class="vertical">
              <template v-if="stages === CardStages.deal">
                <AssetImage src="@/assets/images/poker_card/poker_back.png" />
              </template>
              <template v-else>
                <AssetImage
                  :src="`@/assets/images/poker_card/${getDiceClassName(
                    bankerArea.cards[0]
                  )}.png`"
                />
              </template>
            </div>
            <div class="vertical">
              <template v-if="stages === CardStages.deal">
                <AssetImage src="@/assets/images/poker_card/poker_back.png" />
              </template>
              <template v-else>
                <AssetImage
                  :src="`@/assets/images/poker_card/${getDiceClassName(
                    bankerArea.cards[1]
                  )}.png`"
                />
              </template>
            </div>
            <div class="vertical turnright">
              <template v-if="bankerArea.cards[2].originalNumber === `0`">
                <AssetImage src="@/assets/images/game/poker_default_v.png" />
              </template>
              <template
                v-if="
                  bankerArea.cards[2].originalNumber !== `0` &&
                  stages === CardStages.leftDraw
                "
              >
                <AssetImage src="@/assets/images/poker_card/poker_back.png" />
              </template>
              <template
                v-if="
                  bankerArea.cards[2].originalNumber !== `0` &&
                  stages >= CardStages.rightDraw
                "
              >
                <AssetImage
                  :src="`@/assets/images/poker_card/${getDiceClassName(
                    bankerArea.cards[2]
                  )}.png`"
                />
              </template>
            </div>
          </template>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Baccarat, CardStages } from "@/GameRules/JSBaccaratRule";
import { CardSuit, PokerCard } from "@/GameRules/BasePokerRule";
import AssetImage from "@/components/shared/AssetImage.vue";
import { computed, defineComponent, onMounted, reactive, ref } from "vue";
import IssueNo from "../../IssueNo.vue";
import { useStore } from "vuex";
import { TimeRules } from "@/enums";

export default defineComponent({
  components: { AssetImage, IssueNo },
  props: {
    drawNumbers: {
      type: Object as () => string[],
      required: true,
    },
  },
  setup(props) {
    let stages = ref(CardStages.deal);
    const store = useStore();
    let drawNumbers = reactive(props.drawNumbers);
    drawNumbers = drawNumbers.slice(0, 6);
    const gameData = new Baccarat();
    const result = gameData.confirmResult(drawNumbers);
    const playerArea = reactive(result[0]);
    const bankerArea = reactive(result[1]);
    const getDiceClassName = (item: PokerCard) => {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `${CardSuit[item.suit]}_${item.type.toUpperCase()}`;
    };
    const delay = (ms: number) => {
      return new Promise((resolve) => setTimeout(resolve, ms));
    };
    //發牌
    const dealPoker = async () => {
      stages.value = CardStages.deal;
    };
    //翻牌
    const flopCard = async () => {
      stages.value = CardStages.flop;
      await delay(600);
      if (
        playerArea.cards[2].originalNumber === "0" &&
        bankerArea.cards[2].originalNumber === "0"
      ) {
        flopDrawCard();
      } else {
        drawCard();
      }
    };
    //補牌
    const drawCard = async () => {
      stages.value = CardStages.leftDraw;
      await delay(300);
      stages.value = CardStages.leftFlopDraw;
      await delay(300);
      stages.value = CardStages.rightDraw;
      await delay(300);
      stages.value = CardStages.rightFlopDraw;
      await delay(300);
      flopDrawCard();
    };
    //翻補牌
    const flopDrawCard = async () => {
      stages.value = CardStages.show;
    };
    const logoUrl = computed(() => {
      const typeName = store.state.lotteryInfo.gameTypeName as string;
      return !!typeName
        ? `@/assets/images/game/logo_${typeName.toLocaleLowerCase()}.png`
        : "";
    });
    const playerResult = computed(() => {
      return playerArea.isWin || (!playerArea.isWin && !bankerArea.isWin);
    });
    const bankerResult = computed(() => {
      return bankerArea.isWin || (!playerArea.isWin && !bankerArea.isWin);
    });
    const playerVictoryString = computed(() => {
      if (playerArea.isWin) {
        return "闲赢";
      } else if (!playerArea.isWin && !bankerArea.isWin) {
        return "和";
      }
      return "";
    });
    const bankerVictoryString = computed(() => {
      if (bankerArea.isWin) {
        return "庄赢";
      } else if (!playerArea.isWin && !bankerArea.isWin) {
        return "和";
      }
      return "";
    });
    const isDealEvent = drawNumbers.every((value) => value === "0");
    const isFlopEvent = drawNumbers.some((value) => value !== "0");

    const chromeVersion = navigator.userAgent.match(/Chrome\/(\S+)/);
    const version = chromeVersion ? chromeVersion[1] : "";
    const ver = parseInt(version) | 0;
    const showAnimation = ver === 0 || ver > 77;
    onMounted(() => {
      const isShowIssueNo =
        store.getters.showTimeRuleStatus === TimeRules.issueNoCountdown;

      if (isShowIssueNo) {
        flopDrawCard();
      } else if (isDealEvent) {
        dealPoker();
      } else if (isFlopEvent) {
        flopCard();
      }
    });
    return {
      logoUrl,
      getDiceClassName,
      playerArea,
      bankerArea,
      stages,
      CardStages,
      playerResult,
      bankerResult,
      playerVictoryString,
      bankerVictoryString,
      version,
      showAnimation,
    };
  },
});
</script>
