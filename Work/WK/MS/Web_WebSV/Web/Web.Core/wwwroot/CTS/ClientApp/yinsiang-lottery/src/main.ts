import "@/assets/css/reset.css";
import "@/assets/css/variables.css";
import "@/assets/css/theme.css";
import "@/assets/css/utilities.css";
import "@/assets/css/customize.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { store } from "./store";
import "./message";

let rootContainer = document.getElementById(
  "client-app-lottery"
) as HTMLElement;
rootContainer.className = "h-100 main-content rounded-main overflow-scroll-y";

declare global {
  interface Array<T> {
    flatMap<E>(callback: (t: T) => Array<E>): Array<E>
  }
}

if(!Array.prototype.flatMap){
  Object.defineProperty(Array.prototype, 'flatMap', {
    value: function(f: Function) {
        return this.reduce((ys: any, x: any) => {
            return ys.concat(f.call(this, x))
        }, [])
    },
    enumerable: false,
})
}

createApp(App).use(store).use(router).mount(rootContainer);
