<template>
    <div class="popup_main_content bottom bg2 action_sheet">
      <div class="head">
        <div class="btn align_left" @click="closeEvent"><div>取消</div></div>
        <div class="title">{{popupInfo.title}}</div>
        <div class="btn align_right highlight" @click="confirmEveit">
          <div>完成</div>
        </div>
      </div>
      <div class="scroll_content overflow no_scrollbar">
        <div class="d-flex">
          <VueScrollPicker
            :options="getBeginTimeOptions"
            v-model="currentSelectBeginTime"
          />
          <VueScrollPicker
            :options="getEndTimeOptions"
            v-model="currentSelectEndTime"
          />
        </div>
      </div>
    </div>
  </template>
  <script lang="ts">
  import {PopupModel } from "@/models";
import toast from "@/toast";
  import { defineComponent } from "vue";
  import { VueScrollPicker } from "vue-scroll-picker";
  import BaseView from "./BaseDialog";
  

  export default defineComponent({
    extends: BaseView,
    props: {
      propObject: {
        type: Object as () => PopupModel,
        required: true,
      },
    },
    components: { VueScrollPicker },
    data() {
      return {
        selectedValue: [] as string[],
        currentSelectBeginTime: "",
        currentSelectEndTime:"",
        timeFormat: { hour: 'numeric', minute: '2-digit', hour12: false } as Intl.DateTimeFormatOptions,
      };
    },
    methods: {
      confirmEveit() {

        if(this.currentSelectBeginTime=="" || this.currentSelectEndTime=="")
        {
          toast("请选择营业时间");
          return;
        }
        let timeStr=this.currentSelectBeginTime+'至'+this.currentSelectEndTime;
        this.callbackEvent(timeStr);
      }
    
    },
    created(){
      if(this.propObject.content[0].value)
      {
        let times=this.propObject.content[0].value.split("至");
        if(times.length>0)
        {
          if(this.dataSource.indexOf(times[0])>=0 && this.dataSource.indexOf(times[1])>=0)
          {
            this.currentSelectBeginTime=times[0];
            this.currentSelectEndTime=times[1];
          }
        }
      }
    },
    computed: {  
      dataSource():string[]{
        
         return ['周日', '周一', '周二', '周三', '周四', '周五', '周六'];
      },
      getBeginTimeOptions(): string[] {
       
        return this.dataSource;
      },
      getEndTimeOptions(): string[] {
        return this.dataSource;
      },
      popupInfo() {
        return this.propObject;
      },
    },
  });
  </script>
  <style src="vue-scroll-picker/lib/style.css"></style>
  <style scoped>
  .d-flex {
    display: flex;
  }
  </style>
  