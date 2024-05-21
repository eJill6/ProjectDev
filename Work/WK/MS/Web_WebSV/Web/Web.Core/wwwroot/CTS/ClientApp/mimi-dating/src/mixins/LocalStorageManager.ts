import { MutationType } from "@/store";
import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {
      noReminderPreventFraud: true,
      reminderUnixTime: 0,
      noReminderPreventFraudKey: "noReminderPreventFraudKey",
      reminderUnixTimeKey: "reminderUnixTimeKey",
    };
  },
  methods: {
    setNoReminderPreventFraudValue(value: boolean) {
      this.$store.commit(MutationType.SetPreventFraudDialog, value);
      localStorage.setItem(
        this.noReminderPreventFraudKey,
        JSON.stringify(value)
      );
    },
    setReminderUnixTime(value: number) {
      this.$store.commit(MutationType.SetReminderUnixTime, value);
      localStorage.setItem(this.reminderUnixTimeKey, JSON.stringify(value));
    },
  },
  computed: {
    getNoReminderPreventFraudValue(): boolean {
      const value = localStorage.getItem(this.noReminderPreventFraudKey);
      return value ? (JSON.parse(value) as boolean) : false;
    },
    getReminderUnixTime(): number {
      const value = localStorage.getItem(this.reminderUnixTimeKey);
      return value ? (JSON.parse(value) as number) : 0;
    },
  },
});
