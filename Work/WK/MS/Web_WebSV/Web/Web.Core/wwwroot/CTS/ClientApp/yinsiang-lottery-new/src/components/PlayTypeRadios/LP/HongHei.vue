<template>
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_six.png" />
    </div>

    <!-- 紅 -->
    <div
      class="red block f_5"
      :class="getClass(fieldIndex, 0)"
      @click="toggleSelectNumber(fieldIndex, 0)"
    >
      <div class="focus lg"></div>
      <div class="text red" :data-text="newPlayTypeRadio[0]">
        {{ newPlayTypeRadio[0] }}
      </div>
      <div class="diamond">
        <AssetImage src="@/assets/images/game/img_red_diamond.png" />
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[0]) }}</div>
    </div>

    <div class="middle">
      <!-- 大 -->
      <div
        class="sort block"
        :class="getClass(fieldIndex, 4)"
        @click="toggleSelectNumber(fieldIndex, 4)"
      >
        <div class="focus sm"></div>
        <div class="text yellow" :data-text="newPlayTypeRadio[4]">
          {{ newPlayTypeRadio[4] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[4]) }}</div>
      </div>

      <!-- 單 -->
      <div
        class="sort block"
        :class="getClass(fieldIndex, 2)"
        @click="toggleSelectNumber(fieldIndex, 2)"
      >
        <div class="focus sm"></div>
        <div class="text yellow" :data-text="newPlayTypeRadio[2]">
          {{ newPlayTypeRadio[2] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[2]) }}</div>
      </div>
    </div>

    <div class="middle">
      <!-- 小 -->
      <div
        class="sort block"
        :class="getClass(fieldIndex, 5)"
        @click="toggleSelectNumber(fieldIndex, 5)"
      >
        <div class="focus sm"></div>
        <div class="text yellow" :data-text="newPlayTypeRadio[5]">
          {{ newPlayTypeRadio[5] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[5]) }}</div>
      </div>

      <!-- 雙 -->
      <div
        class="sort block"
        :class="getClass(fieldIndex, 3)"
        @click="toggleSelectNumber(fieldIndex, 3)"
      >
        <div class="focus sm"></div>
        <div class="text yellow" :data-text="newPlayTypeRadio[3]">
          {{ newPlayTypeRadio[3] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[3]) }}</div>
      </div>
    </div>

    <!-- 黑 -->
    <div
      class="black block f_5"
      :class="getClass(fieldIndex, 1)"
      @click="toggleSelectNumber(fieldIndex, 1)"
    >
      <div class="focus lg"></div>
      <div class="text black" :data-text="newPlayTypeRadio[1]">
        {{ newPlayTypeRadio[1] }}
      </div>
      <div class="diamond">
        <AssetImage src="@/assets/images/game/img_black_diamond.png" />
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[1]) }}</div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "../../shared/AssetImage.vue";
export default defineComponent({
  components: { AssetImage },
  mixins: [PlayTypeRadio],
  data() {
    return {
      fieldIndex: 0,
    };
  },
  methods: {
    getClass(fieldIndex: number, numberIndex: number) {
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
