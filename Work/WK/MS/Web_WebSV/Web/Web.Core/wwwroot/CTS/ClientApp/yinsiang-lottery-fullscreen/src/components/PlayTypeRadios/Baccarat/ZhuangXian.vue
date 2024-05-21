<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <div class="play_inner baccarat">
      <div
        class="play_betting xxl"
        :class="getClickClass(0)"
        @click="toggleSelectNumber(0, 0)"
      >
        <div class="play_item" :class="getClass(0)">
          <div
            class="placebet animate_bet"
            v-if="showTotalAmount(playTypeRadio[0][0])"
          >
            <div class="placebet_icon">
              <AssetImage src="@/assets/images/game/ic_placebet.png" />
            </div>
            <div class="bg_coin">
              <div
                class="coin_text"
                :data-text="showTotalAmount(playTypeRadio[0][0])"
              >
                {{ showTotalAmount(playTypeRadio[0][0]) }}
              </div>
            </div>
          </div>
          <div
            class="bet_option"
            :class="getSizeClass(0)"
            :data-text="playTypeRadio[0][0]"
          >
            {{ playTypeRadio[0][0] }}
          </div>
          <div class="bet_num">{{ getNumberOdds(playTypeRadio[0][0]) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>

      <div class="play_middle">
        <div
          class="play_betting row"
          :class="getClickClass(1)"
          @click="toggleSelectNumber(0, 1)"
        >
          <div class="play_item" :class="getClass(1)">
            <div
              class="placebet animate_bet"
              v-if="showTotalAmount(playTypeRadio[0][1])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(playTypeRadio[0][1])"
                >
                  {{ showTotalAmount(playTypeRadio[0][1]) }}
                </div>
              </div>
            </div>
            <div
              class="bet_option"
              :class="getSizeClass(1)"
              :data-text="playTypeRadio[0][1]"
            >
              {{ playTypeRadio[0][1] }}
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[0][1]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>

        <div
          class="play_betting row"
          :class="getClickClass(2)"
          @click="toggleSelectNumber(0, 2)"
        >
          <div class="play_item" :class="getClass(2)">
            <div
              class="placebet animate_bet"
              v-if="showTotalAmount(playTypeRadio[0][2])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(playTypeRadio[0][2])"
                >
                  {{ showTotalAmount(playTypeRadio[0][2]) }}
                </div>
              </div>
            </div>
            <div
              class="bet_option"
              :class="getSizeClass(2)"
              :data-text="playTypeRadio[0][2]"
            >
              {{ playTypeRadio[0][2] }}
            </div>
            <div class="bet_num">
              最高{{ getNumberOdds(playTypeRadio[0][2]) }}
            </div>
          </div>
          <div class="shadow_bet mb_0"></div>
        </div>
      </div>
      <div
        class="play_betting xxl"
        :class="getClickClass(3)"
        @click="toggleSelectNumber(0, 3)"
      >
        <div class="play_item" :class="getClass(3)">
          <div
            class="placebet animate_bet"
            v-if="showTotalAmount(playTypeRadio[0][3])"
          >
            <div class="placebet_icon">
              <AssetImage src="@/assets/images/game/ic_placebet.png" />
            </div>
            <div class="bg_coin">
              <div
                class="coin_text"
                :data-text="showTotalAmount(playTypeRadio[0][3])"
              >
                {{ showTotalAmount(playTypeRadio[0][3]) }}
              </div>
            </div>
          </div>
          <div
            class="bet_option"
            :class="getSizeClass(3)"
            :data-text="playTypeRadio[0][3]"
          >
            {{ playTypeRadio[0][3] }}
          </div>
          <div class="bet_num">{{ getNumberOdds(playTypeRadio[0][3]) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>
    </div>
    <div class="play_block"></div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  methods: {
    getSizeClass(numberIndex: number) {
      return [this.getmiddleArray.includes(numberIndex) ? "sm" : "xxl"];
    },
    getClass(numberIndex: number) {
      const colorMap: { [key: number]: string } = {
        0: "blue",
        1: "green",
        2: "black",
        3: "red",
      };
      return [colorMap[numberIndex]];
    },
    getClickClass(numberIndex: number) {
      let isActive = this.isNumberSelected(0, numberIndex);
      return isActive ? "active" : "";
    },
  },
  computed: {
    getmiddleArray() {
      return [1, 2];
    },
  },
});
</script>
