<template>
  <div :class="clockClass.contentClassName">
    <div :class="clockClass.tendigitsClassName">
      <div :class="[clockClass.textClassName, { warning: isWaring }]">
        {{ countdownTime.secondsTenDigits }}
      </div>
    </div>
    <div :class="clockClass.digitsClassName">
      <div :class="[clockClass.textClassName, { warning: isWaring }]">
        {{ countdownTime.secondsDigits }}
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, watch } from "vue";
import { DateTimeModel, ClockViewModel } from "@/models";
import { TimeRules } from "@/enums";
import { useStore } from "vuex";

const props = defineProps({
  clockClass: {
    type: Object as () => ClockViewModel,
    required: true,
  },
  hasWaring: Boolean,
});

const clockClass = reactive(props.clockClass);
const store = useStore();
let countdownTime: DateTimeModel = reactive({
  timeRule: TimeRules.unknown,
  secondsTotal: 0,
  secondsTenDigits: "0",
  secondsDigits: "0",
});

const isWaring = computed(
  () =>
    countdownTime.secondsTotal < 10 &&
    countdownTime.timeRule === TimeRules.issueNoCountdown &&
    props.hasWaring
);

watch(
  () => store.getters.countdownTime,
  (newValue: DateTimeModel) => {
    countdownTime.timeRule = newValue.timeRule;
    countdownTime.secondsTotal = newValue.secondsTotal;
    countdownTime.secondsTenDigits = newValue.secondsTenDigits;
    countdownTime.secondsDigits = newValue.secondsDigits;
  }
);
</script>
