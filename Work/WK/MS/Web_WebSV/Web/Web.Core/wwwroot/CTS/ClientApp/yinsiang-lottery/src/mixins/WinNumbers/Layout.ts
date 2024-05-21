import { defineComponent } from 'vue';

export default defineComponent({
    computed: {
        hasDrawNumbers(): boolean {
            return !!this.$store.state.issueNo.lastDrawNumber;
        }
    }
});