import "@/assets/css/reset.css";
import "@/assets/css/element.css";
import "@/assets/css/public.css";
import "@/assets/css/modal.css";
import "@/assets/css/card.css";
import "@/assets/css/sheet.css";
import "@/assets/css/index.css";
import "@/assets/css/post.css";
import "@/assets/css/list_view.css";
import "@/assets/css/me.css";
import "@/assets/css/vip.css";
import "@/assets/css/wallet.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { store } from "./store";
import "./message";
import VueGtag from "vue-gtag";
import "amfe-flexible";
let rootContainer = document.getElementById(
  "client-app-lottery"
) as HTMLElement;
rootContainer.className = "root";

const app = createApp(App);
const googleStatisticsCode = (<any>window).mm.googleStatisticsCode as string;
app
  .use(store)
  .use(router)
  .use(
    VueGtag,
    {
      config: { id: googleStatisticsCode },
    },
    router
  )
  .mount(rootContainer);
