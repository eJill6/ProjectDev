import { defineComponent } from 'vue';

export default defineComponent({
    computed: {
        currentDrawNumbers(): string[] {
            let issueNo = this.$store.state.issueNo;

            return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
        },
        lastDrawNumbers(): string[] {
            let issueNo = this.$store.state.lastIssueNo;

            return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
        }
    }
});