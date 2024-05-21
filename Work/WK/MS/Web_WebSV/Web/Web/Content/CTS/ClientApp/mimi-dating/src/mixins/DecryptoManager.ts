import { defineComponent } from "vue";
import axios from "axios";
import CryptoJS from "crypto-js";
import { decryptoKey } from "@/defaultConfig";
import { ImageItemModel } from "@/models";

export default defineComponent({
  data() {
    return {};
  },
  methods: {
    //一次下載全部檔案
    async fetchAllDownload(images: string[]) {
      const filePromises = images.map((file) => {
        return this.getBase64Image(file);
      });

      // Profit
      return (await Promise.all(filePromises)) as string[];
    },
    //單一下載檔案
    async fetchSingleDownload(imageUrl: string) {
      const result = (await this.getBase64Image(imageUrl)) as string;
      return result;
    },
    getBase64Image(imageUrl: string) {
      return new Promise(async (resolve, reject) => {
        const response = await fetch(imageUrl);
        const imageBlob = await response.blob();

        const hasDecrypto = this.doNeedDecrypto(imageUrl);

        const reader = new FileReader();
        reader.onload = async (event) => {
          try {
            const target = event.target as FileReader;
            const base64String = target.result as string;
            const result = hasDecrypto
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
    //單一下載檔案
    async fetchImageFileDownload(imageItem: ImageItemModel) {
      const self = this;
      function getImageItem() {
        return new Promise(async (resolve, reject) => {
          const response = await fetch(imageItem.src);
          const imageBlob = await response.blob();

          const hasDecrypto = self.doNeedDecrypto(imageItem.src);

          const reader = new FileReader();
          reader.onload = async (event) => {
            try {
              const target = event.target as FileReader;
              const base64String = target.result as string;
              const result = hasDecrypto
                ? self.decryptoFile(base64String)
                : base64String;
              imageItem.src = result;
              // Resolve the promise with the response value
              resolve(imageItem);
            } catch (err) {
              reject(err);
            }
          };
          reader.onerror = (error) => {
            reject(error);
          };
          reader.readAsDataURL(imageBlob);
        });
      }
      const resultIamge = (await getImageItem()) as ImageItemModel;
      return resultIamge;
    },
    ///透過fetch下載，使用callback方式回傳
    async fetchDownload(
      imageUrl: string,
      callback?: (decryptoImage: string) => void
    ) {
      const response = await fetch(imageUrl);
      const imageBlob = await response.blob();
      const hasDecrypto = this.doNeedDecrypto(imageUrl);
      this.readFile(imageBlob, hasDecrypto, callback);
    },
    ///透過axios下載，使用callback方式回傳
    async axiosDonload(
      imageUrl: string,
      callback?: (decryptoImage: string) => void
    ) {
      const result = await axios.get(imageUrl, {
        responseType: "blob",
      });
      const imageBlob = result.data as Blob;
      const hasDecrypto = this.doNeedDecrypto(imageUrl);
      this.readFile(imageBlob, hasDecrypto, callback);
    },
    //讀加密檔案
    readFile(
      imageBlob: Blob,
      hasDecrypto: boolean = false,
      callback?: (decryptoImage: string) => void
    ) {
      const reader = new FileReader();
      reader.readAsDataURL(imageBlob);
      reader.onload = async (event: ProgressEvent<FileReader>) => {
        const target = event.target as FileReader;
        const base64String = target.result as string;
        const result = hasDecrypto
          ? this.decryptoFile(base64String)
          : base64String;
        if (callback) callback(result);
      };
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
  },
});
