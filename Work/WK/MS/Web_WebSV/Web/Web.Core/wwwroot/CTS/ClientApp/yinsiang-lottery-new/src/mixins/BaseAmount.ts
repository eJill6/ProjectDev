import { defineComponent } from "vue";

export default defineComponent({
  computed: {
    allowedMinBaseAmount() {
      return 2;
    },
    allowedMaxBaseAmount() {
      return 10000;
    },
    baseAmount() {
      return this.$store.state.baseAmount;
    },
    baseAmountUnits() {
      return [2, 5, 10, 50, 100, 200, 500];
    },
    baseAmountUnitColors(): { [key: number]: string }{
      return {
        2: "blue",
        5: "yellow",
        10: "pink",
        50: "red",
        100: "bluepurple",
        200: "green",
        500: "orange",
      };
    },
    baseAmountUnitDictionary(): { [key: number]: string } {
      return {
        2: "two",
        5: "five",
        10: "ten",
        50: "fifty",
        100: "hundred",
        200: "two_hundred",
        500: "five_hundred",
      };
    },
    baseAmountIconClassName(): string {
      const suffix = this.baseAmountUnitDictionary[this.baseAmount];

      return !suffix ? "default" : suffix;
    },
  },
});
