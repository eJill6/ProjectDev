<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="navigateToHome">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">私信</div>
        <div class="header_btn align_left"></div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height bg_second">
        <!-- 圖片放大 -->
        <div class="overflow no_scrollbar">
          <!-- 滑動區域 -->
          <div
            class="message_wrapper"
            v-for="room in roomList"
            @click="goToPrivateDetail(room.roomID)"
          >
            <div class="photo">
              <CdnImage
                :src="room.avatarUrl"
                v-if="room.avatarUrl && room.avatarUrl.indexOf('aes') < 0"
              />
              <AssetImage :item="setImageItem(room)" v-else />
            </div>
            <div class="outer">
              <div class="inner">
                <div class="inner_up">
                  <div class="title">{{ room.roomName }}</div>
                  <div class="date">{{ room.publishDateTimeText }}</div>
                </div>
                <div class="inner_down">
                  <div class="text">{{ contentText(room) }}</div>
                  <div v-if="room.unreadCount > 0" class="dot"></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { CdnImage, AssetImage } from "@/components";
import api from "@/api";
import { defineComponent } from "vue";
import {
  Tools,
  NavigateRule,
  MqEvent,
  PlayGame,
  ScrollManager,
  VirtualScroll,
  ImageCacheManager,
} from "@/mixins";
import {
  LastMessageViewModel,
  event as eventModel,
  OfficialDMRoomListModel,
  BaseInfoModel,
  ImageItemModel,
} from "@/models";
import { MutationType } from "@/store/mutations";
import { MqChatNotificationType, PostType } from "@/enums";
import toast from "@/toast";

export default defineComponent({
  components: { CdnImage, AssetImage },
  mixins: [
    Tools,
    NavigateRule,
    MqEvent,
    PlayGame,
    ScrollManager,
    VirtualScroll,
    ImageCacheManager,
  ],
  data() {
    return {
      searchModel: {} as OfficialDMRoomListModel,
    };
  },
  async created() {
    this.scrollStatus.list = [];
    await this.loadAsync();
  },
  methods: {
    contentText(room: LastMessageViewModel) {
      return room.messageType === 1 ? room.message : `图片已传送`;
    },
    async loadAsync() {
      if (this.isLoading) return;
      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        let result = await api.getLastMessageInfos(this.searchModel);
        if (result.length) {
          const medias = result.map(
            (model) =>
              <BaseInfoModel>{
                postId: model.messageIDText,
                postType: PostType.None,
                coverUrl: model.avatarUrl,
              }
          );
          this.baseImageInfoDownload(medias);
          this.searchModel.lastMessageID =
            result[result.length - 1].messageIDText;
          this.scrollStatus.list = this.scrollStatus.list.concat(result);
        }
      } catch (error) {
        toast("网络错误，请刷新后重试");
        //console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
        this.calculateVirtualScroll();
      }
    },
    async getLastestMsg(roomId: string) {
      let isSuccess = true;
      this.searchModel.lastMessageID = undefined;
      this.searchModel.roomId = roomId;
      var lastMessageViewModel = this.roomList.find((x) => x.roomID == roomId);
      const messageInfo = await api.getLastMessageInfos(this.searchModel);
      if (!messageInfo || messageInfo.length == 0) {
        isSuccess = false;
      } else {
        if (lastMessageViewModel != undefined) {
          const index = this.roomList.indexOf(lastMessageViewModel);
          lastMessageViewModel.unreadCount = 1;
          lastMessageViewModel.message = messageInfo[0].message;
          lastMessageViewModel.publishDateTimeText =
            messageInfo[0].publishDateTimeText;
          this.scrollStatus.list.unshift(
            this.scrollStatus.list.splice(index, 1)[0]
          );
        } else {
          this.scrollStatus.list = messageInfo.concat(this.roomList);
        }
      }
      return isSuccess;
    },
    async onReceiveMsg(arg: eventModel.ReceiveMsgArg) {
      try {
        if (arg.ChatNotificationType === MqChatNotificationType.NewMessage) {
          const isSuccess = await this.getLastestMsg(arg.RoomID);
          if (!isSuccess) {
            setTimeout(async () => {
              await this.getLastestMsg(arg.RoomID);
            }, 1000);
          }
        }
      } catch (error) {
        console.error(error);
      } finally {
        this.searchModel.roomId = undefined;
        if (this.roomList.length > 0) {
          this.searchModel.lastMessageID =
            this.roomList[this.roomList.length - 1].messageIDText;
        }
      }
    },
    async onDeleteRoom(arg: eventModel.DeleteRoomArg) {
      var lastMessageViewModel = this.roomList.find(
        (x) => x.roomID == arg.RoomID
      );
      if (
        lastMessageViewModel &&
        arg.ChatNotificationType === MqChatNotificationType.DeleteChat
      ) {
        const index = this.roomList.indexOf(lastMessageViewModel);
        this.scrollStatus.list.splice(index, 1);
      }
    },
    async goToPrivateDetail(roomId: string) {
      const room = this.roomList.find((x) => x.roomID === roomId);
      this.navigateToPrivateDetail(roomId, room?.roomName, room?.avatarUrl);
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
    setImageItem(model: LastMessageViewModel) {
      let item: ImageItemModel = {
        id: model.messageIDText,
        subId: model.avatarUrl,
        class: "",
        src: model.avatarUrl,
        alt: "",
      };
      return item;
    },
  },
  computed: {
    logonMode() {
      return this.$store.state.logonMode;
    },
    roomList() {
      return this.scrollStatus.list as LastMessageViewModel[];
    },
  },
});
</script>
