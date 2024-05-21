import "@/assets/css/reset.css";
import "@/assets/css/element.css";
import "@/assets/css/game.css";
import "@/assets/css/modal.css";
import "@/assets/css/public.css";
import "@/assets/css/animation.css";
import "@/assets/css/record.css";
import "@/assets/css/plan.css";
import "@/assets/css/result.css";
import "@/assets/css/rule.css";
import "@/assets/css/customize.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { store } from "./store";
import "./message";

let rootContainer = document.getElementById(
  "client-app-lottery"
) as HTMLElement;
rootContainer.className = "main_container bg_main";

declare global {
  interface Array<T> {
    flatMap<E>(callback: (t: T) => Array<E>): Array<E>;
  }
}

if (!Array.prototype.flatMap) {
  Object.defineProperty(Array.prototype, "flatMap", {
    value: function (f: Function) {
      return this.reduce((ys: any, x: any) => {
        return ys.concat(f.call(this, x));
      }, []);
    },
    enumerable: false,
  });
}

createApp(App).use(store).use(router).mount(rootContainer);
