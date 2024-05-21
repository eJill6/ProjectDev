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
        const startTime = this.currentSelectBeginTime;
        const endTime =this.currentSelectEndTime;

        // 将字符串转换为具有小时和分钟的数字
        const startTimeArray = startTime.split(":").map(Number);
        const endTimeArray = endTime.split(":").map(Number);

        // 将时间转换为总分钟数
        const startTotalMinutes = startTimeArray[0] * 60 + startTimeArray[1];
        const endTotalMinutes = endTimeArray[0] * 60 + endTimeArray[1];

        // // 检查结束时间是否大于开始时间
        // if (endTotalMinutes < startTotalMinutes) {
        //   toast("结束时间大于开始时间");
        //   return;
        // } 

        let timeStr=this.currentSelectBeginTime+'-'+this.currentSelectEndTime;
        this.callbackEvent(timeStr);
      }
    },
    created(){

      if(this.propObject.content[0].value)
      {
        let times=this.propObject.content[0].value.split("-");
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
      dataSource():string[]
      {
        // 获取当前时间的小时部分
        const currentHour = new Date().getHours();
        // 生成整数时间的字符串数组（从当前小时开始，例如，当前是 15:30，那么生成 "15:00", "16:00", ..., "23:00", "00:00", "01:00", ..., "14:00"）
        const integerTimes = Array.from({ length: 24 }, (_, index) => {
          const hour = (currentHour + index) % 24;
          return new Date(2000, 0, 1, hour, 0).toLocaleTimeString('en-US', this.timeFormat);
        });

        return integerTimes;
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
  