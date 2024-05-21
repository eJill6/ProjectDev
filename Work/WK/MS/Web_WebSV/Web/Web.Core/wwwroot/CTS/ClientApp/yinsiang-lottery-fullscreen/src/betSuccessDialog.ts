import { BetSuccessDialog } from "@/components";
import { createApp } from "vue";
import { store } from "./store";
import router from "./router";

const betSuccessDialog = () => {
  let popupDiv = document.createElement("div") as HTMLDivElement;
  popupDiv.className = "toast_main top_spacing";
  document.body.appendChild(popupDiv);

  createApp(BetSuccessDialog).use(store).use(router).mount(".toast_main");
  setTimeout(() => {
    popupDiv.parentNode?.removeChild(popupDiv);
  }, 2000);
};

export default betSuccessDialog;
