import { defineComponent } from "vue";

export default defineComponent({
  methods: {
    callbackEvent(item?: Object) {
      this.$emit("callback", item);
    },
    closeEvent() {
      this.$emit("close");
    },
    closeCallbackEvent(item?: Object){
      this.$emit("cancelCallback", item);
    }
  },
});
