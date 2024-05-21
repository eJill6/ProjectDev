import { defineComponent } from "vue";
import { ActionType } from '@/store';

export default defineComponent({
    methods: {
        refreshBalanceAsync() {
            this.$store.dispatch(ActionType.RefreshBalanceAsync);            
        }
    },
    created() {
        this.refreshBalanceAsync();
    },
    computed: {
        balance() {
            return this.$store.state.balance;
        },
        formattedBalance() {
            return this.$store.state.formattedBalance;
        }
    }
});