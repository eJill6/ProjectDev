import { WinnerDialog } from "@/components";
import { createApp } from "vue";
import { store } from "./store";
import router from "./router";

const winnerDialog = (message: string) => {
  let explosionDiv = document.createElement("div") as HTMLDivElement;
  explosionDiv.className = "explosion";
  document.body.appendChild(explosionDiv);
  explosionDiv.style.zIndex = "1";
  let popupDiv = document.createElement("div") as HTMLDivElement;
  popupDiv.className = "toast_main top_spacing";
  document.body.appendChild(popupDiv);

  createApp(WinnerDialog, {
    amount: message,
  })
    .use(store)
    .use(router)
    .mount(".toast_main");
  setTimeout(() => {
    popupDiv.parentNode?.removeChild(popupDiv);
    explosionDiv.parentNode?.removeChild(explosionDiv);
  }, 2000);
};

export default winnerDialog;
