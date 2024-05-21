import { DialogSettingModel } from "@/models";
import { defineComponent } from "vue";

export default defineComponent({
  props: {
    dialogSetting: {
      type: Object as () => DialogSettingModel,
      required: false,
    },
  },
  emits: ["close", "callback"],
  methods: {
    closeEvent() {
      this.$emit("close");
    },
    callbackEvent(item?: DialogSettingModel) {
      this.$emit("callback", item);
    },
  },
});
