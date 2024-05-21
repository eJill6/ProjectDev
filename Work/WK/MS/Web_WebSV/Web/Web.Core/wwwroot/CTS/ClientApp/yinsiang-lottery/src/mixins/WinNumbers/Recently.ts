import { defineComponent } from 'vue';

export default defineComponent({
    computed: {
        currentDrawNumbers(): string[] {
            let issueNo = this.$store.state.issueNo;

            return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
        },
    }
});