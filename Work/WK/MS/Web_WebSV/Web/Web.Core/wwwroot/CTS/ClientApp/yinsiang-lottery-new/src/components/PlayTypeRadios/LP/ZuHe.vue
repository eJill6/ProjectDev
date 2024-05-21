<template>
  <div class="betting_table">
    <div class="table">
      <AssetImage src="@/assets/images/game/table_border_three.png" />
    </div>

    <!-- 1~12 -->
    <div
      class="britishgreen block left"
      :class="getClass(fieldIndex, 0)"
      @click="toggleSelectNumber(fieldIndex, 0)"
    >
      <div class="focus lg"></div>
      <div class="text white" :data-text="newPlayTypeRadio[0]">
        {{ newPlayTypeRadio[0] }}
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[0]) }}</div>
    </div>

    <!-- 13~24 -->
    <div
      class="britishgreen block center"
      :class="getClass(fieldIndex, 1)"
      @click="toggleSelectNumber(fieldIndex, 1)"
    >
      <div class="focus lg"></div>
      <div class="text white" :data-text="newPlayTypeRadio[1]">
        {{ newPlayTypeRadio[1] }}
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[1]) }}</div>
    </div>

    <!-- 25~36 -->
    <div
      class="britishgreen block right"
      :class="getClass(fieldIndex, 2)"
      @click="toggleSelectNumber(fieldIndex, 2)"
    >
      <div class="focus lg"></div>
      <div class="text white" :data-text="newPlayTypeRadio[2]">
        {{ newPlayTypeRadio[2] }}
      </div>
      <div class="num">{{ getNumberOdds(newPlayTypeRadio[2]) }}</div>
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
