<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">{{ shopName }}</div>
        <div class="header_btn align_left"></div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height bg_second">
        <div class="chat_attention">
          如果商家没有及时回复您，请留下您的联系方式，商家会在第一时间联系您哦。
        </div>
        <div
          class="overflow no_scrollbar"
          ref="scrollContainer"
          @scroll="handleScroll"
        >
          <!-- 你的切版放這裡 -->
          <div class="chat_container" v-for="(message, index) in messageList">
            <div
              class="date_wrapper"
              v-if="
                (message.publishUserID.toString() === roomId ||
                  message.publishUserID === userId) &&
                (index == 0 || showTime(message, index))
              "
            >
              <div class="date">{{ message.publishDateTimeText }}</div>
            </div>

            <!--左邊頭像顯示-->
            <ul
              class="from_member"
              v-if="message.publishUserID.toString() === roomId"
            >
              <li>
                <div class="dialog_avatar">
                  <AssetImage :item="setImageItem(shopAvatar)" />
                </div>
                <div class="dialog_chatset">
                  <template v-if="message.messageType === 2">
                    <div
                      class="dialog_frame dialog_frame_photo"
                      @click="enlargeEvent(message)"
                    >
                      <AssetImage :item="setImageItem(message.message)" />
                    </div>
                  </template>
                  <template v-else>
                    <div class="dialog_chat_arrow">
                      <CdnImage
                        src="@/assets/images/modal/ic_arrow_chatmember.svg"
                        alt=""
                      />
                    </div>
                    <div class="dialog_frame">
                      <div class="dialog_chattext">
                        {{ message.message }}
                      </div>
                    </div>
                  </template>
                </div>
              </li>
            </ul>

            <!--右邊頭像顯示-->
            <ul class="from_self" v-if="message.publishUserID === userId">
              <li>
                <div class="dialog_chatset">
                  <template v-if="message.messageType === 2">
                    <div
                      class="dialog_frame dialog_frame_photo"
                      @click="enlargeEvent(message)"
                    >
                      <AssetImage :item="setImageItem(message.message)" />
                    </div>
                  </template>
                  <template v-else>
                    <div class="dialog_chat_arrow">
                      <CdnImage
                        src="@/assets/images/modal/ic_arrow_chatself.svg"
                        alt=""
                      />
                    </div>
                    <div class="dialog_frame dialog_frame_padding">
                      <div class="dialog_chattext">{{ message.message }}</div>
                    </div>
                  </template>
                </div>
                <div class="dialog_avatar">
                  <AssetImage :item="setImageItem(avatar)" />
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>

      <div class="dialog_height">
        <div class="dialog_send">
          <div class="member_send">
            <div class="chat_input">
              <textarea
                placeholder=""
                ref="refSendMessage"
                v-model="inputMessage"
                @keyup.enter="sendMessage"
                :disabled="isBlockSending"
              ></textarea>
            </div>
          </div>
          <div class="btn_feature" @click="updateImageAction">
            <CdnImage src="@/assets/images/modal/ic_chat_photo.svg" alt="" />
            <input
              id="upload_btn"
              ref="updateImageBtn"
              type="file"
              accept="image/*"
              @change="onUpdateImageChanged($event)"
            />
          </div>
          <div class="btn_feature" @click="sendMessage">
            <CdnImage src="@/assets/images/modal/ic_chat_send.svg" alt="" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { CdnImage, AssetImage } from "@/components";
import api from "@/api";
import {
  Tools,
  NavigateRule,
  MqEvent,
  PlayGame,
  ImageCacheManager,
  MediaCenter,
  ScrollManager,
} from "@/mixins";
import {
  event as eventModel,
  OfficialDMMessageListModel,
  ChatMessageViewModel,
  OfficialDMSendMessageModel,
  ImageItemModel,
  MediaModel,
} from "@/models";
import { defineComponent } from "vue";
import { MutationType } from "@/store/mutations";
import toast from "@/toast";
import { MediaType, MqChatNotificationType, SourceType } from "@/enums";

export default defineComponent({
  components: { CdnImage, AssetImage },
  mixins: [
    Tools,
    NavigateRule,
    MqEvent,
    PlayGame,
    ImageCacheManager,
    MediaCenter,
    ScrollManager,
  ],
  data() {
    return {
      inputMessage: "",
      isBlockSending: false,
      searchModel: {} as OfficialDMMessageListModel,
      sendMessageModel: {} as OfficialDMSendMessageModel,
      SourceType,
      MediaType,
      imageMaxCount: 1,
    };
  },
  methods: {
    async enlargeEvent(item: ChatMessageViewModel) {
      item.message = item.message.replace("225-225-", "");
      await this.messageDownload(this.roomId, [item]);
      const imageItem = this.setImageItem(item.message);
      this.$store.commit(MutationType.SetImageItem, imageItem);
      this.$router.push({
        name: "ImageDetail",
      });
    },
    async loadAsync() {
      if (this.isLoading) return;
      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        this.searchModel.searchDirectionTypeValue = 2;
        let result = await api.getRoomMessages(this.searchModel);
        if (result.length > 0) {
          const imageList = result.filter((item) => item.messageType === 2);
          await this.messageDownload(this.roomId, imageList);
          this.searchModel.lastMessageID = result[0].messageIDText;
          this.scrollStatus.list = result.concat(this.messageList);
        }
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    async onReceiveMsg(arg: eventModel.ReceiveMsgArg) {
      if (arg.ChatNotificationType === MqChatNotificationType.NewMessage) {
        setTimeout(async () => {
          this.searchModel.searchDirectionTypeValue = 1;
          if (this.messageList && this.messageList.length) {
            this.searchModel.lastMessageID =
              this.messageList[this.messageList.length - 1].messageIDText;
          }
          let result = await api.getRoomMessages(this.searchModel);

          const imageList = result.filter((item) => item.messageType === 2);
          await this.messageDownload(this.roomId, imageList);

          this.scrollStatus.list = this.scrollStatus.list.concat(result);
          this.searchModel.lastMessageID = this.messageList[0].messageIDText;
          await this.scrollToBottom();
        }, 1000);
      }
    },
    async onDeleteRoom(arg: eventModel.DeleteRoomArg) {
      if (
        arg.ChatNotificationType === MqChatNotificationType.DeleteChat &&
        this.roomId === arg.RoomID
      ) {
        this.scrollStatus.list = [];
        toast("订单结束已清除对话");
        setTimeout(() => {
          this.navigateToPrevious();
        }, 1000);
      }
    },
    async updateImageAction() {
      (this.$refs.updateImageBtn as HTMLElement).click();
    },
    async onUpdateImageChanged(event: any) {
      if (!this.isBlockSending) {
        this.isBlockSending = true;
        this.$store.commit(MutationType.SetIsLoading, true);
        const maxSize = 1024 * 1024 * 5; // 5MB
        const files = event.target.files;
        if (files.length !== this.imageMaxCount) {
          toast(`最多只能选择${this.imageMaxCount}个档案`);
          this.$store.commit(MutationType.SetIsLoading, false);
          this.isBlockSending = false;
          return;
        }
        for (let i = 0; i < files.length; i++) {
          const file = files[i];

          const isImage = await this.checkImageFormat(file);

          if (!isImage) {
            toast(`照片格式错误`);
            this.$store.commit(MutationType.SetIsLoading, false);
            this.isBlockSending = false;
            return;
          }
          var reader = new FileReader();

          reader.readAsDataURL(file);

          reader.onload = async (event: ProgressEvent<FileReader>) => {
            const target = event.target as FileReader;
            let imageData = target.result as string;
            const size = file.size;
            if (size > maxSize) {
              toast("图档过大，需限制5M以下，请重新上传");
              this.$store.commit(MutationType.SetIsLoading, false);
              this.isBlockSending = false;
              return;
            }
            const image: MediaModel = {
              bytes: imageData,
              fileName: file.name,
              sourceType: SourceType.PrivateMessage,
              mediaType: MediaType.Image,
              id: "",
              subId: "",
            };
            const updateImages = await this.uploadFile([], [image]);
            const updateImage = updateImages[0];

            try {
              this.sendMessageModel.roomID = this.roomId;
              this.sendMessageModel.Message = updateImage.id;
              this.sendMessageModel.MessageTypeValue = 2; //圖片信息

              if (this.sendMessageModel.Message) {
                const result = await api.sendMessage(this.sendMessageModel);

                if (result.messageIDText) {
                  let model: ChatMessageViewModel = {
                    message: updateImage.fullMediaUrl,
                    messageType: 2,
                    messageID: result.messageIDText,
                    messageIDText: result.messageIDText,
                    publishUserID: this.userId,
                    publishTimestamp: result.publishTimestamp,
                    publishDateTimeText: result.publishDateTimeText,
                  };
                  await this.messageDownload(this.roomId, [model]);
                  this.scrollStatus.list.push(model);
                  await this.scrollToBottom(true);
                }
              }
            } catch (error) {
              toast("讯息发送失败");
            } finally {
              this.inputMessage = "";
              setTimeout(() => {
                this.$store.commit(MutationType.SetIsLoading, false);
                this.isBlockSending = false;
                this.$nextTick(() => {
                  const messageInput = this.$refs
                    .refSendMessage as HTMLInputElement;
                  messageInput.focus();
                });
              }, 1000);
            }
          };
        }
      }
    },
    async sendMessage() {
      if (this.inputMessage && !this.isBlockSending) {
        this.isBlockSending = true;
        try {
          if (this.inputMessage.trim().length === 0) {
            return;
          }
          this.inputMessage = this.inputMessage.replace("\n", "");
          (this.sendMessageModel.roomID = this.roomId),
            (this.sendMessageModel.Message = this.inputMessage),
            (this.sendMessageModel.MessageTypeValue = 1); //文字信息
          if (this.sendMessageModel.Message) {
            const result = await api.sendMessage(this.sendMessageModel);
            if (result.messageIDText) {
              let model: ChatMessageViewModel = {
                message: this.inputMessage,
                messageType: 1,
                messageID: result.messageIDText,
                messageIDText: result.messageIDText,
                publishUserID: this.userId,
                publishTimestamp: result.publishTimestamp,
                publishDateTimeText: result.publishDateTimeText,
              };
              this.scrollStatus.list.push(model);
              await this.scrollToBottom(true);
            }
          }
        } catch (error) {
          toast("讯息发送失败");
        } finally {
          this.inputMessage = "";
          setTimeout(() => {
            this.isBlockSending = false;
            this.$nextTick(() => {
              const messageInput = this.$refs
                .refSendMessage as HTMLInputElement;
              messageInput.focus();
            });
          }, 1000);
        }
      }
    },
    setImageItem(src: string) {
      let item: ImageItemModel = {
        id: this.roomId,
        subId: src,
        class: "",
        src: src,
        alt: "",
      };
      return item;
    },
    async scrollToBottom(isSender: boolean = false) {
      this.$nextTick(() => {
        const container = this.$refs.scrollContainer as HTMLDivElement; // 获取对象
        container.scrollTop = container.scrollHeight; // 滚动高度
      });
      if (this.messageList.length > 0 && !isSender) {
        await api.clearUnreadMessage(this.roomId);
      }
    },
    async handleScroll() {
      const { scrollTop, clientHeight, scrollHeight } = this.$refs
        .scrollContainer as HTMLDivElement;
      if (scrollTop <= 0 && !this.isLoading) {
        await this.loadAsync();
      }
    },
    async onDeleteMsgs(arg: eventModel.DeleteRoomArg) {
      if (this.roomId === arg.RoomID) {
        this.navigateToPrivate();
      }
    },
    downLoadImages() {
      this.singleDownload(this.avatar);
      this.singleDownload(this.shopAvatar);
    },
    showTime(model: ChatMessageViewModel, index: number) {
      if (index == 0) {
        return false;
      }
      const lastChatMessage = this.messageList[index - 1];
      const lastTime = new Date(
        lastChatMessage?.publishDateTimeText || ""
      ).getTime();
      const currentTime = new Date(model.publishDateTimeText || "").getTime();
      if (currentTime - lastTime >= 300000) {
        return true;
      }
      return false;
    },
  },
  async created() {
    this.scrollStatus.list = [];
    this.searchModel.roomID = this.roomId;
    await this.setUserInfo();
    this.downLoadImages();
    await this.loadAsync();
    await this.scrollToBottom();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 20;
    },
    userId() {
      return this.$store.state.centerInfo.userId;
    },
    roomId() {
      return this.$route.query.roomId as string;
    },
    shopName() {
      return this.$store.state.privateMessageShopName as string;
    },
    shopAvatar() {
      return this.$store.state.privateMessageAvatar as string;
    },
    messageList() {
      return (this.scrollStatus.list as ChatMessageViewModel[]) || [];
    },
    avatar() {
      return this.$store.state.centerInfo.avatar;
    },
  },
});
</script>
