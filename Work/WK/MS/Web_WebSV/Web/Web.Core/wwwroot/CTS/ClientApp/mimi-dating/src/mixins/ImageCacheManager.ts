import { defineComponent } from "vue";
import axios from "axios";
import CryptoJS from "crypto-js";
import { decryptoKey } from "@/defaultConfig";
import {
  BannerModel,
  ProductListModel,
  ImageCacheModel,
  MyPostListModel,
  MediaModel,
  CommentModel,
  OfficialShopModel,
  OfficialListModel,
  ResMyBookingPostModel,
  BaseInfoModel,
  MyOfficialPostListModel,
  BookingManageModel,
  OfficialPostModel,
  MediaResultModel,
  MyFavoritePostModel,
  ReportDetailModel,
  ImageItemModel,
  ChatMessageViewModel,
  MyFavoriteShopModel,NextPagePostCoverModel
} from "@/models";
import { MutationType } from "@/store";
import Tools from "./Tools";

const {
  AbortController,
  abortableFetch,
} = require("abortcontroller-polyfill/dist/cjs-ponyfill");
const _nodeFetch = require("node-fetch");
const { fetch, Request } = abortableFetch({
  fetch: _nodeFetch,
  Request: _nodeFetch.Request,
});
let controller = new AbortController();
const abort = () => controller.abort();
// const signal = controller.signal;

export default defineComponent({
  mixins: [Tools],
  data() {
    return {
      promiseCount: 0,
    };
  },
  methods: {
    async messageDownload(postId: string, list: ChatMessageViewModel[]) {

      if (!list.length) return;
      for (const item of list) {
        const subItem: ImageCacheModel = {
          id: postId,
          subId: item.message,
          url: item.message,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async commentDownload(postId: string, list: CommentModel[]) {
      if (!list.length) return;
      for (const item of list) {
        for (let urlString of item.photoUrls) {
          const subItem: ImageCacheModel = {
            id: postId,
            subId: urlString,
            url: urlString,
          };
          await this.sortSingleImageData(subItem);
        }
      }
    },
    async orderImageDownload(list: ResMyBookingPostModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        let subId = this.getImageID(item.coverUrl);
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: subId,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async mediaDownload(list: MediaModel[]) {
      if (!list.length) return;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.id,
          subId: item.subId,
          url: item.bytes,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async manageBossBookingDownload(list: BookingManageModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        let subId = this.getImageID(item.avatarUrl);
        const subItem = <ImageCacheModel>{
          id: item.bookingId,
          subId: subId,
          url: item.avatarUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async complaintPostDownload(model: ReportDetailModel) {
      if (!model.photoIds.length) return;
      for (const item of model.photoIds) {
        let subId = this.getImageID(item);
        const subItem = <ImageCacheModel>{
          id: model.reportId,
          subId: subId,
          url: item,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async managerBossOfficialDownload(list: MyOfficialPostListModel[]) {
      if (!list.length) return;

      this.promiseCount = list.length;

      for (const item of list) {
        let subId = this.getImageID(item.coverUrl);
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: subId,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },

    async myFavoriteShopDownload(list: MyFavoriteShopModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;

      for (const item of list) {
        if (item.shopAvatarSource == null) continue;

        const subItem = <ImageCacheModel>{
          id: item.favoriteId,
          subId: item.bossId,
          url: item.shopAvatarSource,
        };
        await this.sortSingleImageData(subItem);
      }
    },

    async myFavoritePostDownload(list: MyFavoritePostModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;

      for (const item of list) {
        if (item.coverUrl == null) continue;

        const subItem = <ImageCacheModel>{
          id: item.favoriteId,
          subId: item.postId,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async myOfficialPostDownload(list: OfficialPostModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        let subId = this.getImageID(item.coverUrl);
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: subId,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async managerDownload(list: MyPostListModel[]) {
      if (!list.length) return;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: item.coverUrl,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async bannerDownload(list: BannerModel[]) {
      if (!list.length) return;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.id,
          subId: item.fullMediaUrl,
          url: item.fullMediaUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async officialDownload(list: OfficialShopModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.applyId,
          subId: item.shopAvatarSource,
          url: item.shopAvatarSource,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async officialShopImage(list: MediaResultModel[]) {
      if (!list.length) return;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.id,
          subId: item.fullMediaUrl,
          url: item.fullMediaUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },

    /* async officialListModelDownload(list: OfficialListModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: item.coverUrl,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },*/
    async officialListDownload(list: OfficialListModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: item.coverUrl,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async postDownload(list: ProductListModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: item.coverUrl,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async singleDownload(src: string) {
      if (!src) return;
      this.promiseCount = 1;
      const subItem = <ImageCacheModel>{
        id: src,
        subId: src,
        url: src,
      };
      await this.sortSingleImageData(subItem);
    },
    async baseImageInfoDownload(list: BaseInfoModel[]) {
      if (!list.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: item.coverUrl,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async nextPagePostCoverDownload(list: NextPagePostCoverModel[]) {
      if (!list?.length) return;
      this.promiseCount = list.length;
      for (const item of list) {
        const subItem = <ImageCacheModel>{
          id: item.postId,
          subId: item.coverUrl,
          url: item.coverUrl,
        };
        await this.sortSingleImageData(subItem);
      }
    },
    async sortImageData(imageList: ImageCacheModel[]) {
      for (const item of imageList) {
        const container = this.imageCache.get(item.id);
        if (container) {
          const subContainer = container.get(item.subId);
          if (!subContainer) {
            await this.downloadImageData(
              item.id,
              item.subId,
              item.url,
              container
            );
          }
        } else {
          await this.downloadImageData(
            item.id,
            item.subId,
            item.url,
            new Map<string, string>()
          );
        }
      }
    },

    async sortSingleImageData(item: ImageCacheModel) {
      const container = this.imageCache.get(item.id);

      if (container) {

      
        const subContainer = container.get(item.subId);
        if (!subContainer) {
          await this.downloadImageData(
            item.id,
            item.subId,
            item.url,
            container
          );
        }
      } else {

        await this.downloadImageData(
          item.id,
          item.subId,
          item.url,
          new Map<string, string>()
        );
      }
      if (this.promiseCount > 0) {
        this.promiseCount = this.promiseCount - 1;
      }
    },
    async downloadImageData(
      postId: string,
      subId: string,
      urlString: string,
      imageCache: Map<string, string>
    ) {
      const result = (await this.getBase64ImageForFatch(urlString)) as string;
      if (result) {
        imageCache.set(subId, result);
        this.$store.commit(MutationType.SetImageCache, {
          key: postId,
          value: imageCache,
        });
      }
    },
    getBase64ImageForFatch(imageUrl: string) {
      return new Promise(async (resolve, reject) => {
        controller = new AbortController();
        const response = await fetch(new Request(imageUrl), {
          signal: controller.signal,
        });
        const imageBlob = await response.blob();

        const reader = new FileReader();
        reader.onload = async (event) => {
          try {
            const target = event.target as FileReader;
            const base64String = target.result as string;
            const result = this.doNeedDecrypto(imageUrl)
              ? this.decryptoFile(base64String)
              : base64String;
            // Resolve the promise with the response value
            resolve(result);
          } catch (err) {
            reject("");
          }
        };
        reader.onerror = (error) => {
          reject("");
        };
        reader.readAsDataURL(imageBlob);
      });
    },
    getBase64ImageForAxios(imageUrl: string) {
      return new Promise(async (resolve, reject) => {
        const result = await axios.get(imageUrl, {
          responseType: "blob",
        });
        const imageBlob = result.data as Blob;

        const reader = new FileReader();
        reader.onload = async (event) => {
          try {
            const target = event.target as FileReader;
            const base64String = target.result as string;
            const result = this.doNeedDecrypto(imageUrl)
              ? this.decryptoFile(base64String)
              : base64String;
            // Resolve the promise with the response value
            resolve(result);
          } catch (err) {
            reject(err);
          }
        };
        reader.onerror = (error) => {
          reject(error);
        };
        reader.readAsDataURL(imageBlob);
      });
    },
    ///解密成一般檔案
    decryptoFile(base64String: string) {
      // split the sha256 hash byte array into key and iv
      let keyPart = CryptoJS.enc.Utf8.parse(decryptoKey);

      const parts = base64String.split(";base64,");
      const arrayData = parts[1];

      const decrypted = CryptoJS.AES.decrypt(arrayData, keyPart, {
        mode: CryptoJS.mode.ECB,
        padding: CryptoJS.pad.NoPadding,
      });

      const resultImage = `data:image/jpeg;base64,${decrypted.toString(
        CryptoJS.enc.Base64
      )}`;
      return resultImage;
    },
    doNeedDecrypto(urlString: string): boolean {
      const parts = urlString.split(".");
      if (parts.length < 2) return false;

      const extension = parts.pop();
      return extension === "aes";
    },
    cancelFetchAction() {
      // try {
      // abort();
      // } catch (e) {
      //   console.error(e);
      // } finally {
      this.promiseCount = 0;
      // }
    },
  },
  computed: {
    imageCache() {
      return (
        this.$store.state.imageCache || new Map<string, Map<string, string>>()
      );
    },
    isPromising() {
      return this.promiseCount > 0;
    },
  },
});
