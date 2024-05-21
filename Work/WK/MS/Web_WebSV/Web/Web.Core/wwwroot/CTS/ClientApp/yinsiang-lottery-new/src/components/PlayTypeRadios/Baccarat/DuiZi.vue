<template>
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_four.png" />
    </div>

    <div class="middle">
      <!-- 闲对 -->
      <div
        class="player block corner"
        :class="getClass(fieldIndex, 1)"
        @click="toggleSelectNumber(fieldIndex, 1)"
      >
        <div class="focus sm"></div>
        <div class="text blue sm" :data-text="newPlayTypeRadio[1]">
          {{ newPlayTypeRadio[1] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[1]) }}</div>
      </div>

      <!-- 任意对子 -->
      <div
        class="lucky block corner_right"
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
      <!-- 庄对 -->
      <div
        class="bank block corner"
        :class="getClass(fieldIndex, 0)"
        @click="toggleSelectNumber(fieldIndex, 0)"
      >
        <div class="focus sm"></div>
        <div class="text carmine sm" :data-text="newPlayTypeRadio[0]">
          {{ newPlayTypeRadio[0] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[0]) }}</div>
      </div>

      <!-- 完美对子 -->
      <div
        class="lucky block corner_left"
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
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PlayTypeRadio } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

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
