<template>
  <BaseView :lastPostType="lastPostType"></BaseView>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { PostType } from "@/enums";
import BaseView from "./BaseView.vue";
import { DialogControl, NavigateRule, LocalStorageManager } from "@/mixins";

export default defineComponent({
  data() {
    return {
      lastPostType: 0
    }
  },
  components: { BaseView },
  mixins: [DialogControl, NavigateRule, LocalStorageManager],
  methods: {
    showPrevent() {
      const reminderDate = new Date(this.reminderUnixTimeValue);
      const today = new Date();
      if (reminderDate.getDay() !== today.getDay()) {
        this.$store.commit(MutationType.SetPreventFraudDialog, false);
      }
      if (!this.reminderFraud) {
        this.$store.commit(MutationType.SetPreventFraudDialog, true);
        this.showPreventDialog(() => {
          this.navigateToPrevent();
        });
      }
    },
  },
  async created() {
    let filter = this.$store.state.filter;
    filter.statusType = 0;
    this.$store.commit(MutationType.SetFilter, filter);
    this.lastPostType = this.$store.state.postType;
    this.$store.commit(MutationType.SetPostType, PostType.Square);
    this.showPrevent();
  },
  computed: {
    reminderFraud() {
      const result = this.getNoReminderPreventFraudValue;
      return this.$store.state.noReminderPreventFraud || result;
    },
    reminderUnixTimeValue() {
      this.getReminderUnixTime;
      return this.$store.state.reminderUnixTime;
    },
  },
});
</script>
