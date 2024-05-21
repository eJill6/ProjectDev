import { defineComponent, createApp } from "vue";
import SuccessfulView from "@/components/Dialogs/SuccessfulView.vue";
import router from "../router";
import { store } from "../store";
export default defineComponent({
  methods: {
    showSuccessfulView() {
      let popupDiv = document.createElement("div") as HTMLDivElement;
      popupDiv.className = "toast_main";
      document.body.appendChild(popupDiv);
      createApp(SuccessfulView).use(store).use(router).mount(".toast_main");
      setTimeout(() => {
        popupDiv.parentNode?.removeChild(popupDiv);
      }, 1000);
    },
  },
});
