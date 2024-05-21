<template>
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border.png" />
    </div>

    <!-- 闲 -->
    <div
      class="player block"
      :class="getClass(fieldIndex, 0)"
      @click="toggleSelectNumber(fieldIndex, 0)"
    >
      <div class="focus lg"></div>
      <div class="text blue" :data-text="newPlayTypeRadio[0]">
        {{ newPlayTypeRadio[0] }}
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[0]) }}</div>
    </div>
    <div class="middle">
      <!-- 和 -->
      <div
        class="tie block"
        :class="getClass(fieldIndex, 1)"
        @click="toggleSelectNumber(fieldIndex, 1)"
      >
        <div class="focus sm"></div>
        <div class="text green" :data-text="newPlayTypeRadio[1]">
          {{ newPlayTypeRadio[1] }}
        </div>
        <div class="num">{{ getNumberOdds(newPlayTypeRadio[1]) }}</div>
      </div>

      <!-- 庄幸运6 -->
      <div
        class="lucky block"
        :class="getClass(fieldIndex, 2)"
        @click="toggleSelectNumber(fieldIndex, 2)"
      >
        <div class="focus sm"></div>
        <div class="text yellow" :data-text="newPlayTypeRadio[2]">
          {{ newPlayTypeRadio[2] }}
        </div>
        <div class="num">{{ `最高${getNumberOdds(newPlayTypeRadio[2])}` }}</div>
      </div>
    </div>

    <!-- 庄 -->
    <div
      class="bank block"
      :class="getClass(fieldIndex, 3)"
      @click="toggleSelectNumber(fieldIndex, 3)"
    >
      <div class="focus lg"></div>
      <div class="text carmine" :data-text="newPlayTypeRadio[3]">
        {{ newPlayTypeRadio[3] }}
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[3]) }}</div>
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
