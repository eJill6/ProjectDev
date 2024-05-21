<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 輪盤 紅黑 start -->
    <div class="play_inner jslp">
      <div
        class="play_betting jslp"
        :class="getClickClass(fieldIndex, 0)"
        @click="toggleSelectNumber(fieldIndex, 0)"
      >
        <div class="play_item red">
          <div
            class="placebet animate_bet"
            v-if="showTotalAmount(newPlayTypeRadio[0])"
          >
            <div class="placebet_icon">
              <AssetImage src="@/assets/images/game/ic_placebet.png" />
            </div>
            <div class="bg_coin">
              <div
                class="coin_text"
                :data-text="showTotalAmount(newPlayTypeRadio[0])"
              >
                {{ showTotalAmount(newPlayTypeRadio[0]) }}
              </div>
            </div>
          </div>
          <div class="bet_option xxl" :data-text="newPlayTypeRadio[0]">
            {{ newPlayTypeRadio[0] }}
          </div>
          <div class="bet_num">{{ getNumberOdds(newPlayTypeRadio[0]) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>

      <div class="play_middle">
        <div
          class="play_betting jslp"
          :class="getClickClass(fieldIndex, 4)"
          @click="toggleSelectNumber(fieldIndex, 4)"
        >
          <div class="play_item blue">
            <div
              class="placebet sm animate_bet"
              v-if="showTotalAmount(newPlayTypeRadio[4])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(newPlayTypeRadio[4])"
                >
                  {{ showTotalAmount(newPlayTypeRadio[4]) }}
                </div>
              </div>
            </div>
            <div class="bet_option sm" :data-text="newPlayTypeRadio[4]">
              {{ newPlayTypeRadio[4] }}
            </div>
            <div class="bet_num">{{ getNumberOdds(newPlayTypeRadio[4]) }}</div>
          </div>
          <div class="shadow_bet sm"></div>
        </div>

        <div
          class="play_betting jslp"
          :class="getClickClass(fieldIndex, 2)"
          @click="toggleSelectNumber(fieldIndex, 2)"
        >
          <div class="play_item blue mb_0">
            <div
              class="placebet sm animate_bet"
              v-if="showTotalAmount(newPlayTypeRadio[2])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(newPlayTypeRadio[2])"
                >
                  {{ showTotalAmount(newPlayTypeRadio[2]) }}
                </div>
              </div>
            </div>
            <div class="bet_option sm" :data-text="newPlayTypeRadio[2]">
              {{ newPlayTypeRadio[2] }}
            </div>
            <div class="bet_num">{{ getNumberOdds(newPlayTypeRadio[2]) }}</div>
          </div>
          <div class="shadow_bet sm mb_0"></div>
        </div>
      </div>
      <div class="play_middle">
        <div
          class="play_betting jslp"
          :class="getClickClass(fieldIndex, 5)"
          @click="toggleSelectNumber(fieldIndex, 5)"
        >
          <div class="play_item blue">
            <div
              class="placebet sm animate_bet"
              v-if="showTotalAmount(newPlayTypeRadio[5])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(newPlayTypeRadio[5])"
                >
                  {{ showTotalAmount(newPlayTypeRadio[5]) }}
                </div>
              </div>
            </div>
            <div class="bet_option sm" :data-text="newPlayTypeRadio[5]">
              {{ newPlayTypeRadio[5] }}
            </div>
            <div class="bet_num">{{ getNumberOdds(newPlayTypeRadio[5]) }}</div>
          </div>
          <div class="shadow_bet sm"></div>
        </div>
        <div
          class="play_betting jslp"
          :class="getClickClass(fieldIndex, 3)"
          @click="toggleSelectNumber(fieldIndex, 3)"
        >
          <div class="play_item blue mb_0">
            <div
              class="placebet sm animate_bet"
              v-if="showTotalAmount(newPlayTypeRadio[3])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(newPlayTypeRadio[3])"
                >
                  {{ showTotalAmount(newPlayTypeRadio[3]) }}
                </div>
              </div>
            </div>
            <div class="bet_option sm" :data-text="newPlayTypeRadio[3]">
              {{ newPlayTypeRadio[3] }}
            </div>
            <div class="bet_num">{{ getNumberOdds(newPlayTypeRadio[3]) }}</div>
          </div>
          <div class="shadow_bet sm mb_0"></div>
        </div>
      </div>
      <div
        class="play_betting jslp"
        :class="getClickClass(fieldIndex, 1)"
        @click="toggleSelectNumber(fieldIndex, 1)"
      >
        <div class="play_item black">
          <div class="placebet animate_bet" v-if="showTotalAmount(newPlayTypeRadio[1])">
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div class="coin_text" data-text="100">100</div>
              </div>
            </div>
          <div class="bet_option xxl" :data-text="newPlayTypeRadio[1]">
            {{ newPlayTypeRadio[1] }}
          </div>
          <div class="bet_num">{{ getNumberOdds(newPlayTypeRadio[1]) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>
    </div>
    <div class="play_block"></div>
    <!-- 輪盤 紅黑 end -->
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  data() {
    return {
      fieldIndex: 0,
    };
  },
  methods: {
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
  computed: {
    newPlayTypeRadio() {
      const radio = this.playTypeRadio.flatMap((x: any) => x);
      return radio;
    },
  },
});
</script>
