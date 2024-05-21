import { createApp, Component } from "vue";
import { store } from "./store";
import router from "./router";
import { DialogSettingModel } from "@/models";
let array = [] as Component []
const createDialog = (
  dialog: Component,
  model?: DialogSettingModel,
  callback?: (object: DialogSettingModel) => void
) => {
  if (array.indexOf(dialog) !== -1)
    return;
  array.push(dialog);
  let dialogBody = document.createElement("div") as HTMLDivElement;
  dialogBody.className = "confirm_main";

  document.body.appendChild(dialogBody);

  createApp(dialog, {
    dialogSetting: model,
    onClose() {
      dialogBody.parentNode?.removeChild(dialogBody);
      array.splice(array.indexOf(dialog));
    },
    onCallback(callbackObject: DialogSettingModel) {
      dialogBody.parentNode?.removeChild(dialogBody);
      array.splice(array.indexOf(dialog));
      if (callback) {
        callback(callbackObject);
      }
    },
  })
    .use(store)
    .use(router)
    .mount(".confirm_main");

  // let popupCoverDiv = document.createElement("div");
  // popupCoverDiv.className = "popup_cover";
  // popupCoverDiv.style.cssText =
  //   "position:fixed;z-index: 0;width:100%;height:100%;";
  // popupCoverDiv.onclick = () => {
  //   dialogBody.parentNode?.removeChild(dialogBody);
  // };
  // dialogBody.appendChild(popupCoverDiv);
  // dialogBody.onclick = () => {
  //   dialogBody.parentNode?.removeChild(dialogBody);
  // };
};

export default createDialog;
