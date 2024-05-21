<template>
  <!-- 圍骰 start -->
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_seven.png" />
    </div>

    <div class="chineseblack block corner" :class="getClass(2, 0)" @click="toggleSelectNumber(2, 0)">
      <div class="focus sm"></div>
      <div class="text yellow spacing" :data-text="playTypeRadio[2][0]">{{playTypeRadio[2][0]}}</div>
      <div class="num fixed1">{{ getNumberOdds(playTypeRadio[2][0]) }}</div>
    </div>
    <div class="middle">
      <!-- 魚 -->
      <div class="winered block corner_none" :class="getClass(0, 0)" @click="toggleSelectNumber(0, 0)">
        <div class="focus sm"></div>
        <div class="icon">
          <AssetImage src="@/assets/images/game/img_fullrange_fish.png" />
        </div>
        <div class="num sm">{{ getNumberOdds(playTypeRadio[0][0]) }}</div>
      </div>

      <!-- 銅錢 -->
      <div class="ceruleanblue corner_none block" :class="getClass(1, 0)" @click="toggleSelectNumber(1, 0)">
        <div class="focus sm"></div>
        <div class="icon">
          <AssetImage src="@/assets/images/game/img_fullrange_coppercoins.png" />
        </div>
        <div class="num sm">{{ getNumberOdds(playTypeRadio[1][0]) }}</div>
      </div>
    </div>
    <div class="middle">
      <!-- 蝦 -->
      <div class="versegreen corner_none block" :class="getClass(0, 1)" @click="toggleSelectNumber(0, 1)">
        <div class="focus sm"></div>
        <div class="icon">
          <AssetImage src="@/assets/images/game/img_fullrange_shrimp.png" />
        </div>
        <div class="num sm">{{ getNumberOdds(playTypeRadio[0][1]) }}</div>
      </div>

      <!-- 螃蟹 -->
      <div class="versegreen corner_none block" :class="getClass(1, 1)" @click="toggleSelectNumber(1, 1)">
        <div class="focus sm"></div>
        <div class="icon">
          <AssetImage src="@/assets/images/game/img_fullrange_crab.png" />
        </div>
        <div class="num sm">{{ getNumberOdds(playTypeRadio[1][1]) }}</div>
      </div>
    </div>
    <div class="middle">
      <!-- 葫蘆 -->
      <div class="ceruleanblue block corner_tr" :class="getClass(0, 2)" @click="toggleSelectNumber(0, 2)">
        <div class="focus sm"></div>
        <div class="icon">
          <AssetImage src="@/assets/images/game/img_fullrange_gourd.png" />
        </div>
        <div class="num sm">{{ getNumberOdds(playTypeRadio[0][2]) }}</div>
      </div>

      <!-- 雞 -->
      <div class="winered block corner_br" :class="getClass(1, 2)" @click="toggleSelectNumber(1, 2)">
        <div class="focus sm"></div>
        <div class="icon">
          <AssetImage src="@/assets/images/game/img_fullrange_chicken.png" />
        </div>
        <div class="num sm">{{ getNumberOdds(playTypeRadio[1][2]) }}</div>
      </div>
    </div>
  </div>
  <!-- 圍骰 end -->
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
      let isActive = this.isNumberSelected(fieldIndex, numberIndex);
      return isActive ? "active" : "";
    },
  },
});
</script>
