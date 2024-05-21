import { defineComponent } from "vue";
import { event } from "vue-gtag";

export default defineComponent({
  methods: {
    setGaEventName(name: string) {
      event(name);
    },
  },
});
