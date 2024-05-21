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
        baseAmountIconClassName(): string {
            let isCustom = this.baseAmountUnits.indexOf(this.baseAmount) === -1;
            let suffix = isCustom ? 'custom' : this.baseAmount;

            return `chips-small-${suffix}`;
        }
    }
});