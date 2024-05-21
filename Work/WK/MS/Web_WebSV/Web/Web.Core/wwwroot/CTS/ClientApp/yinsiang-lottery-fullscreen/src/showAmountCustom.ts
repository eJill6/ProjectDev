import AmountCustom from "@/components/Dialogs/AmountCustom.vue";
import { createApp } from "vue";
import { store } from "./store";
import router from "./router";

const showAmountCustom = (callback?: (object: number) => void) => {
  let dialogBody = document.createElement("div") as HTMLDivElement;
  dialogBody.className = "popup_main";
  document.body.appendChild(dialogBody);
  createApp(AmountCustom, {
    onClose() {
      dialogBody.parentNode?.removeChild(dialogBody);
    },
    onCallback(callbackObject: number) {
      dialogBody.parentNode?.removeChild(dialogBody);
      if (callback) {
        callback(callbackObject);
      }
    },
  })
    .use(store)
    .use(router)
    .mount(".popup_main");
};

export default showAmountCustom;
