<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <div class="play_inner bothsides">
      <template v-for="item in getData()">
        <div
          class="play_betting"
          :class="getClass(item.index)"
          @click="toggleSelectNumber(0, item.index)"
        >
          <div class="play_item" :class="getClass(item.index)">
            <div class="placebet animate_bet" v-if="showTotalAmount(item.text)">
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div class="coin_text" :data-text="showTotalAmount(item.text)">
                  {{ showTotalAmount(item.text) }}
                </div>
              </div>
            </div>
            <template v-for="str in arrChange(item.text)">
              <div
                class="bet_option"
                :class="getSizeClass(item.index)"
                :data-text="str"
              >
                {{ str }}
              </div>
            </template>
            <div class="bet_num">{{ getNumberOdds(item.text) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>
      </template>
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
      const colorMap: { [key: number]: string } = {
        0: "",
        1: "",
        2: "sm",
        3: "sm",
      };
      return [colorMap[numberIndex]];
    },
    getData() {
      let list = this.playTypeRadio[0];
      //順序寫死:閑對、任意對子、完美對子、庄對
      return [1, 2, 3, 0].map((index) => ({
        index: index,
        text: list[index],
      }));
    },
    getClass(numberIndex: number) {
      const colorMap: { [key: number]: string } = {
        0: "red",
        1: "blue",
        2: "spacing orange",
        3: "spacing orange",
      };
      let isActive = this.isNumberSelected(0, numberIndex);
      return [isActive ? "active" : "", colorMap[numberIndex]];
    },
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
    arrChange(str: string) {
      const newArr = [];
      let arr = str.split("");
      while (arr.length > 0) {
        newArr.push(arr.splice(0, 2).join(""));
      }
      return newArr;
    },
  },
});
</script>
