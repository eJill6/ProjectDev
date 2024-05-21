import { defineComponent } from "vue";

export default defineComponent({
  computed: {
    hasDrawNumbers(): boolean {
      return (
        !!this.$store.state.issueNo.lastDrawNumber &&
        this.$store.state.lotteryInfo.lotteryId ===
          this.$store.state.issueNo.lotteryId
      );
    },
    currentDrawNumbers(): string[] {
      let issueNo = this.$store.state.issueNo;

      return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
    },
    lastDrawNumbers(): string[] {
        let issueNo = this.$store.state.lastIssueNo;

        return issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(',') || [];
    }
  },
});