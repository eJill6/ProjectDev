<template>
  <div class="longdragon_content">
    <div class="longdragon_digits">
      <div :class="['digits_text']">
        {{ longCountdownTime.secondsTenDigits }}
      </div>
    </div>
    <div class="longdragon_digits">
      <div :class="['digits_text']">
        {{ longCountdownTime.secondsDigits }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref, watch, defineEmits } from "vue";
import { ChangLongDateTimeModel, DateTimeModel } from "@/models";
import { TimeRules } from "@/enums";
import { useStore } from "vuex";

const props = defineProps({
  lotteryId: Number,
  hasWaring: Boolean,
});

const emit = defineEmits<{
  (e: "returnToZero"): void;
}>();

const store = useStore();
const lotteryId = computed(() => props.lotteryId);
// const lotteryId = ref(props.lotteryId);

let longCountdownTime: DateTimeModel = reactive({
  timeRule: TimeRules.unknown,
  secondsTotal: 0,
  secondsTenDigits: "0",
  secondsDigits: "0",
});

currentLotteryInfo(store.state.changLongLotteryTimeInfo);

function currentLotteryInfo(models: ChangLongDateTimeModel[]) {
  const result = models.find((x) => {
    return x.lotteryId === lotteryId.value;
  }) as ChangLongDateTimeModel;
  if (result) {
    countdownIssueNo(result);
  }
}

function countdownIssueNo(model: ChangLongDateTimeModel) {
  longCountdownTime.secondsTenDigits = `${Math.floor(model.secondsTotal / 10)}`;
  longCountdownTime.secondsDigits = `${model.secondsTotal % 10}`;
  if (!model.secondsTotal) {
    returnToZero();
  }
}

function returnToZero() {
  emit("returnToZero");
}

watch(
  () => [store.getters.countdownTime, store.state.changLongLotteryTimeInfo],
  () => {
    currentLotteryInfo(store.state.changLongLotteryTimeInfo);
  }
);
</script>
