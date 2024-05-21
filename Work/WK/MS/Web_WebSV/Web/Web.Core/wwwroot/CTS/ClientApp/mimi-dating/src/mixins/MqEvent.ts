import { defineComponent } from "vue";
import { event as eventModel } from "@/models";
import event from "@/event";

export default defineComponent({
  methods: {
    onReceiveMsg(arg: eventModel.ReceiveMsgArg) {},
    onDeleteRoom(arg: eventModel.DeleteRoomArg) {},
  },
  created() {
    event.on("ReceiveMsg", this.onReceiveMsg);
    event.on("DeleteRoom", this.onDeleteRoom);
  },
  beforeUnmount() {
    event.off("ReceiveMsg", this.onReceiveMsg);
    event.off("DeleteRoom", this.onDeleteRoom);
  },
});
