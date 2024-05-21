import { defineComponent } from "vue";

export default defineComponent({
  methods: {
    getClassName(amount: number) {
      if (amount <= 2) {
        return `yellow`;
      } else if (amount <= 5) {
        return `blue`;
      } else if (amount <= 10) {
        return `pink`;
      } else if (amount <= 50) {
        return `red`;
      } else if (amount <= 100) {
        return `bluepurple`;
      } else if (amount <= 200) {
        return `green`;
      } else if (amount <= 500) {
        return `orange`;
      }
      return `black`;
    },
  },
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
    amountClass() {
      const amount = this.$store.state.baseAmount;
      if (amount === 2) {
        return `two`;
      } else if (amount === 5) {
        return `five`;
      } else if (amount === 10) {
        return `ten`;
      } else if (amount === 50) {
        return `fifty`;
      } else if (amount === 100) {
        return `hundred`;
      } else if (amount === 200) {
        return `two_hundred`;
      } else if (amount === 500) {
        return `five_hundred`;
      }
      return `default`;
    },
    baseAmountUnits() {
      return [2, 5, 10, 50, 100, 200, 500];
    },
    baseAmountIconClassName(): string {
      let isCustom = this.baseAmountUnits.indexOf(this.baseAmount) === -1;
      let suffix = isCustom ? "custom" : this.baseAmount;

      return `chips-small-${suffix}`;
    },
    baseFilterChangLongCount() {
      return this.$store.state.filterChangLongCount;
    },
  },
});
