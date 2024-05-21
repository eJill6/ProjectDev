<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 圍骰 start -->
    <div class="play_inner jsyxx">
      <div
        class="play_betting jsyxx fullrange"
        :class="getClickClass(2, 0)"
        @click="toggleSelectNumber(2, 0)"
      >
        <div class="play_item black">
          <div
            class="placebet animate_bet"
            v-if="showTotalAmount(playTypeRadio[2][0])"
          >
            <div class="placebet_icon">
              <AssetImage src="@/assets/images/game/ic_placebet.png" />
            </div>
            <div class="bg_coin">
              <div
                class="coin_text"
                :data-text="showTotalAmount(playTypeRadio[2][0])"
              >
                {{ showTotalAmount(playTypeRadio[2][0]) }}
              </div>
            </div>
          </div>
          <div class="bet_option xxl" :data-text="playTypeRadio[2][0]">
            {{ playTypeRadio[2][0] }}
          </div>
          <div class="bet_num">{{ getNumberOdds(playTypeRadio[2][0]) }}</div>
        </div>
        <div class="shadow_bet"></div>
      </div>

      <div class="play_middle jsyxx">
        <!-- 魚 -->
        <div
          class="play_betting jsyxx md"
          :class="getClickClass(0, 0)"
          @click="toggleSelectNumber(0, 0)"
        >
          <div class="play_item red">
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
            <div class="bet_img_option fullrange">
              <AssetImage src="@/assets/images/game/img_fullrange_fish.png" />
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[0][0]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>

        <!-- 銅錢 -->
        <div
          class="play_betting jsyxx md"
          :class="getClickClass(1, 0)"
          @click="toggleSelectNumber(1, 0)"
        >
          <div class="play_item blue">
            <div
              class="placebet animate_bet"
              v-if="showTotalAmount(playTypeRadio[1][0])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(playTypeRadio[1][0])"
                >
                  {{ showTotalAmount(playTypeRadio[1][0]) }}
                </div>
              </div>
            </div>
            <div class="bet_img_option fullrange">
              <AssetImage
                src="@/assets/images/game/img_fullrange_coppercoins.png"
              />
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[1][0]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>
      </div>
      <div class="play_middle jsyxx">
        <!-- 蝦 -->
        <div
          class="play_betting jsyxx md"
          :class="getClickClass(0, 1)"
          @click="toggleSelectNumber(0, 1)"
        >
          <div class="play_item green">
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
            <div class="bet_img_option fullrange">
              <AssetImage src="@/assets/images/game/img_fullrange_shrimp.png" />
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[0][1]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>

        <!-- 螃蟹 -->
        <div
          class="play_betting jsyxx md"
          :class="getClickClass(1, 1)"
          @click="toggleSelectNumber(1, 1)"
        >
          <div class="play_item green">
            <div
              class="placebet animate_bet"
              v-if="showTotalAmount(playTypeRadio[1][1])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(playTypeRadio[1][1])"
                >
                  {{ showTotalAmount(playTypeRadio[1][1]) }}
                </div>
              </div>
            </div>
            <div class="bet_img_option fullrange">
              <AssetImage src="@/assets/images/game/img_fullrange_crab.png" />
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[1][1]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>
      </div>
      <div class="play_middle jsyxx">
        <!-- 葫蘆 -->
        <div
          class="play_betting jsyxx md"
          :class="getClickClass(0, 2)"
          @click="toggleSelectNumber(0, 2)"
        >
          <div class="play_item blue">
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
            <div class="bet_img_option fullrange">
              <AssetImage src="@/assets/images/game/img_fullrange_gourd.png" />
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[0][2]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>

        <!-- 雞 -->
        <div
          class="play_betting jsyxx md"
          :class="getClickClass(1, 2)"
          @click="toggleSelectNumber(1, 2)"
        >
          <div class="play_item red">
            <div
              class="placebet animate_bet"
              v-if="showTotalAmount(playTypeRadio[1][2])"
            >
              <div class="placebet_icon">
                <AssetImage src="@/assets/images/game/ic_placebet.png" />
              </div>
              <div class="bg_coin">
                <div
                  class="coin_text"
                  :data-text="showTotalAmount(playTypeRadio[1][2])"
                >
                  {{ showTotalAmount(playTypeRadio[1][2]) }}
                </div>
              </div>
            </div>
            <div class="bet_img_option fullrange">
              <AssetImage
                src="@/assets/images/game/img_fullrange_chicken.png"
              />
            </div>
            <div class="bet_num">{{ getNumberOdds(playTypeRadio[1][2]) }}</div>
          </div>
          <div class="shadow_bet"></div>
        </div>
      </div>
    </div>
    <div class="play_block"></div>
    <!-- 圍骰 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  methods: {
    getClickClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
});
</script>
