import messageOpenUrl from "@/message";
import { PromptDialog } from "@/components";
import { createApp } from "vue";
import { store } from "./store";
import router from "./router";

const promptDialog = (message: string, url: string, logonMode: number) => {
  let popupDiv = document.createElement("div") as HTMLDivElement;
  popupDiv.className = "popup_main";
  document.body.appendChild(popupDiv);

  createApp(PromptDialog, {
    message: message,
    onClose() {
      popupDiv.parentNode?.removeChild(popupDiv);
    },
    onConfirm() {
      messageOpenUrl.openUrl(logonMode, url, null);
    },
  })
    .use(store)
    .use(router)
    .mount(".popup_main");
};

export default promptDialog;
