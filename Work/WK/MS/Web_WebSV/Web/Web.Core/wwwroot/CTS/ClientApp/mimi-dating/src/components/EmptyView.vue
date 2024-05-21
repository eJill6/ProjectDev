<template>
  <div class=" flex_height bg_personal">
                     <div class="overflow no_scrollbar">
                         <div class="empty_state_content">
                             <div class="empty_state">
                                 <div class="icon"><img src="@/assets/images/public/pic_none.png" alt=""></div>
                                 <div class="nodata">{{messageString}}</div>
                             </div>
                         </div>
                     </div>
                 </div>
 </template>
 <script lang="ts">
 import CdnImage from "./CdnImage.vue";
 import { defineComponent } from "vue";
 
 export default defineComponent({
     components:{ CdnImage },
     props:{
         message:String
     },
     computed:{
         messageString(){
             return this.message || "";
         }
     }
 })
 </script>