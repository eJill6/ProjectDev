import api from "@/api";
import { MediaType, SourceType } from "@/enums";
import {
  MediaModel,
  MediaResultModel,
  MergeUploadModel,
  VideoUrlModel,
} from "@/models";
import { MutationType } from "@/store";
import toast from "@/toast";
import { defineComponent } from "vue";
import ImageCacheManager from "./ImageCacheManager";
import CryptoJS from "crypto-js";

export default defineComponent({
  data() {
    return {
      imageZoomItem: {} as MediaModel,
      mbSize: 1024 * 1024 * 1,
    };
  },
  mixins: [ImageCacheManager],
  methods: {
    async onFileChanged(
      event: any,
      source: SourceType,
      media: MediaType,
      totle: number
    ) {
      const maxSize = 1024 * 1024 * (media === MediaType.Image ? 5 : 50); // 5MB
      const files = event.target.files;
      const count =
        (media === MediaType.Image
          ? this.imageSelect.filter((imgItem)=>imgItem.sourceType===source).length
          : this.videoSelect.length) + files.length;
      if (count > totle) {
        toast(`最多只能选择${totle}个档案`);
        return;
      }
      for (let i = 0; i < files.length; i++) {
        const file = files[i];

        const isImage = await this.checkImageFormat(file);

        if (!isImage && media === MediaType.Image) {
          toast(`照片格式错误`);
          return;
        }
        if (isImage && media === MediaType.Video) {
          toast(`视频格式错误`);
          return;
        }

        var reader = new FileReader();

        reader.readAsDataURL(file);

        reader.onload = async (event: ProgressEvent<FileReader>) => {
          const target = event.target as FileReader;
          let imageData = target.result as string;
          const size = file.size;
          if (size >= maxSize && media == MediaType.Video) {
            toast(`上传失败`);
            return;
          }

          if (size > maxSize && media === MediaType.Image) {
            const decimal = 1; //無條件進位小數點第一數

            const multiple =
              Math.ceil((maxSize / size) * Math.pow(10, decimal)) /
              Math.pow(10, decimal);

            let img = new Image();
            img.src = imageData;
            img.onload = () => {
              let canvas = document.createElement("canvas");
              // 畫布大小為圖片的 multiple 倍
              canvas.width = img.width * multiple;
              canvas.height = img.height * multiple;
              let ctx = canvas.getContext("2d");
              ctx?.drawImage(img, 0, 0, canvas.width, canvas.height); // 把圖片畫在畫布上(0,0)作標到(canvas.width,canvas.height)

              imageData = canvas.toDataURL(file.type);
            };
          }
          const image: MediaModel = {
            bytes: imageData,
            fileName: file.name,
            sourceType: source,
            mediaType: media,
            id: "",   
            subId: "",
          };
          this.mediaSelect.push(image);
          this.$store.commit(MutationType.SetMediaSelect, this.mediaSelect);
        };
      }
      if(source!==SourceType.BossApply && source!==SourceType.BusinessPhoto){

        const selector = document.getElementById(
          "upload_btn"
        ) as HTMLInputElement;
        
        if(selector!=null){
          selector.value = "";
        }

      }else
      {
          let selector=document.getElementById("uploadCoverPhoto_upload_btn") as HTMLInputElement;
          if(selector!=null){
            selector.value = "";
          }else
          {
              selector=document.getElementById("uploadShopPhoto_upload_btn") as HTMLInputElement;
              selector.value = "";
          }
      }
    },
    checkImageFormat(file: any) {
      return new Promise(async (resolve, reject) => {
        const _self = this;
        var reader = new FileReader();
        reader.onloadend = (event: ProgressEvent<FileReader>) => {
          const target = event.target as FileReader;
          let imageData = target.result as ArrayBuffer;
          let result = new Uint8Array(imageData);
          let header = "";
          for (var i = 0; i < result.length; i++) {
            header += result[i].toString(16);
          }

          const isImage = _self.isImageFormat(header); //真正的format
          resolve(isImage);
        }; //end: onloaded

        reader.readAsArrayBuffer(file.slice(0, 4));
      });
    },
    isImageFormat(val: string) {
      let type = "";
      switch (val) {
        case "89504e47":
          type = "image/png";
          break;
        case "ffd8ffdb":
        case "ffd8ffe0":
        case "ffd8ffee":
        case "ffd8ffe1":
          type = "image/jpeg";
          break;
        default:
          type = ""; // Or you can use the blob.type as fallback
          break;
      }
      return type !== "";
    },
    isVideoFormat(val: string) {
      let type = "";
      switch (val) {
        case "41564920":
          type = "video/avi";
          break;
        case "00018":
        case "0001c":
        case "00020":
          type = "video/mp4";
          break;
        case "00014":
          type = "video/mov";
          break;
        default:
          type = ""; // Or you can use the blob.type as fallback
          break;
      }
      return type !== "";
    },
    getVideoType(val: string) {
      if (val.toLowerCase().indexOf("mp4") > -1) {
        return "video/mp4";
      } else {
        return "";
      }
    },
    convertBase64ToBlob(model: MediaModel) {
      // // Split into two parts
      // const parts = base64Image.split(";base64,");

      // // Hold the content type
      // const imageType = parts[0].split(":")[1];
      // const imageString = parts[1];
      const imageString = model.bytes;
      const mediaType = this.getVideoType(model.fileName);
      // Decode Base64 string
      const decodedData = window.atob(imageString);
      // Create UNIT8ARRAY of size same as row data length
      const uInt8Array = new Uint8Array(decodedData.length);
      // Insert all character code into uInt8Array
      for (let i = 0; i < decodedData.length; ++i) {
        uInt8Array[i] = decodedData.charCodeAt(i);
      }
      // Return BLOB image after conversion
      return new Blob([uInt8Array], { type: mediaType });
    },
    base64ToArrayBuffer(base64Image: string) {
      // Split into two parts
      const parts = base64Image.split(";base64,");
      // Hold the content type
      const binaryString = window.atob(parts[1]);
      const bytes = new Uint8Array(binaryString.length);
      for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
      }
      return bytes.buffer as ArrayBuffer;
    },
    cleanMediaState() {
      this.imageZoomItem = {} as MediaModel;
      this.$store.commit(MutationType.SetMediaSelect, []);
    },
    async uploadImages() {
      const cloudImage = this.mediaSelect.filter(
        (item) => item.isCloud && item.mediaType === MediaType.Image
      );

      const newImageList = this.mediaSelect.filter(
        (item) => !item.isCloud && item.mediaType === MediaType.Image
      );
      
      return await this.uploadFile(cloudImage, newImageList);
    },
    async uploadImagesBySourType(sourceType:SourceType) {
      const cloudImage = this.mediaSelect.filter(
        (item) => item.isCloud && item.mediaType === MediaType.Image && item.sourceType===sourceType
      );

      const newImageList = this.mediaSelect.filter(
        (item) => !item.isCloud && item.mediaType === MediaType.Image && item.sourceType===sourceType
      );
      
      return await this.uploadFile(cloudImage, newImageList);
    },
    async uploadVideos() {
      const cloudVideo = this.mediaSelect.filter(
        (item) => item.isCloud && item.mediaType === MediaType.Video
      );

      const newVideoList = this.mediaSelect.filter(
        (item) => !item.isCloud && item.mediaType === MediaType.Video
      );
      return await this.uploadFile(cloudVideo, newVideoList);
    },
    async uploadFile(cloudFile: MediaModel[], newFileListe: MediaModel[]) {
     
      let results = cloudFile.map(
        (item) =>
          <MediaResultModel>{
            id: item.fileName,
            fullMediaUrl: item.bytes,
          }
      );
      const delayPromise = async (
        apiData: MediaModel
      ): Promise<MediaResultModel> => {
        const parts = apiData.bytes.split(";base64,");
        const updateModel = { ...apiData };
        updateModel.bytes = parts[1];
        try {
         
          let result = {} as MediaResultModel;
          if (apiData.mediaType === MediaType.Video) {
            result = await this.splitUpload(updateModel);
          } else {
            result = await api.createMedia(updateModel);
          }
          return await Promise.resolve(result);
        } catch (e) {
          return await Promise.reject();
        }
      };
      try {
        await newFileListe.reduce(
          async (
            AccumulatorPromise: Promise<MediaModel[]>,
            current: MediaModel
          ) => {
            let acc = await AccumulatorPromise;
            const processed = await delayPromise(current);
            results.push(processed);
            return acc;
          },
          Promise.resolve([] as MediaModel[])
        );
      } catch (e) {
        console.error(e);
        toast(e);
        results = [];
      } finally {
        return results;
      }
    },
    async splitUpload(model: MediaModel) {
      let paths: string[] = [];
      let blobFile = this.convertBase64ToBlob(model);
      const uploadAuthModel = await api.getUploadVideoUrl();
      let start = 0;
      while (start < blobFile.size) {
        const chunk = this.slice(blobFile, start, this.mbSize);
        const response = await this.uploadChunk(chunk, uploadAuthModel);
        const jsonData = await response.json();
        if (
          jsonData["data"] !== undefined &&
          Array.isArray(jsonData["data"]) &&
          jsonData["data"].length > 0
        ) {
          paths.push(jsonData["data"][0]);
        } else {
          console.error(`UploadChunk Fail ${JSON.stringify(jsonData)}`);
          throw "UploadChunk Fail";
        }

        start += this.mbSize;
      }

      const files = model.fileName.split(".");
      const fileExtension = (files.pop() || "").toLocaleLowerCase();

      const mergeModel: MergeUploadModel = {
        mediaType: model.mediaType,
        sourceType: model.sourceType,
        paths: paths,
        suffix: fileExtension,
      };
      return await this.mergeUpload(mergeModel);
    },
    slice(blob: Blob, start: number, chunkSize: number) {
      const chunkEnd = Math.min(start + chunkSize, blob.size);
      return blob.slice(start, chunkEnd);
    },
    async uploadChunk(blobChunk: Blob, uploadAuthModel: VideoUrlModel) {
      const ts = uploadAuthModel.ts;

      const md5Chunk = (await this.genMD5Chunk(blobChunk)) as string;

      const formData = new FormData();
      formData.append(md5Chunk, blobChunk, md5Chunk);

      try {
        const requestHeaders: HeadersInit = new Headers();
        requestHeaders.set("ts", `${ts}`);
        requestHeaders.set("sign", uploadAuthModel.sign);
        const response = await fetch(uploadAuthModel.url, {
          method: "POST",
          body: formData,
          headers: requestHeaders,
        });
        return response;
      } catch (e) {
        console.error(`Upload Fail ${JSON.stringify(e)}`);
        throw e;
      }
    },
    async genMD5Chunk(blobChunk: Blob) {
      return new Promise((resolve, reject) => {
        let reader = new FileReader();
        reader.onloadend = (event: ProgressEvent<FileReader>) => {
          const target = event.target as FileReader;
          const data = target.result as any;
          const md5Chunk = CryptoJS.MD5(
            CryptoJS.lib.WordArray.create(data)
          ).toString();
          resolve(md5Chunk);
        };

        reader.onerror = reject;
        // reader.addEventListener("loadend", (e) => {
        //   const md5Chunk = CryptoJS.MD5(
        //     CryptoJS.lib.WordArray.create(e.target.result)
        //   ).toString();
        //   resolve(md5Chunk);
        // });
        // reader.addEventListener("error", reject);
        reader.readAsArrayBuffer(blobChunk);
      });
    },

    async mergeUpload(model: MergeUploadModel) {
      return await api.mergeUpload(model);
    },
    deleteImage(image: MediaModel) {
      let index = this.mediaSelect
        .map((item) => item.bytes)
        .indexOf(image.bytes);
      if (index < 0) {
        this.mediaSelect.map((item) => item.fileName).indexOf(image.fileName);
      }
      if (index > -1) {
        this.mediaSelect.splice(index, 1);
      }
      this.imageZoomItem = {} as MediaModel;
    },
    async setImageList(
      postId: string,
      photoList: { [name: string]: string },
      sourceType: SourceType
    ) {
      const keys = Object.keys(photoList);
      const mediaList = keys.map(
        (name) =>
          <MediaModel>{
            bytes: photoList[name],
            fileName: name,
            sourceType: sourceType,
            mediaType: MediaType.Image,
            isCloud: true,
            id: postId,
            subId: photoList[name],
          }
      );
      this.mediaDownload(mediaList);

      let result: MediaModel[] = JSON.parse(JSON.stringify(this.mediaSelect));
      result = result.length > 0 ? result.concat(mediaList) : mediaList;
      this.$store.commit(MutationType.SetMediaSelect, result);
    },
    async setVideoList(
      postId: string,
      videoList: { [name: string]: string },
      sourceType: SourceType
    ) {
      const keys = Object.keys(videoList);
      const mediaList = keys.map(
        (name) =>
          <MediaModel>{
            bytes: videoList[name],
            fileName: name,
            sourceType: sourceType,
            mediaType: MediaType.Video,
            isCloud: true,
            id: postId,
            subId: videoList[name],
          }
      );
      // await this.mediaDownload(mediaList);

      this.$store.commit(MutationType.SetMediaSelect, mediaList);
    },
  },
  computed: {
    mediaSelect() {
      return this.$store.state.mediaSelectList;
    },
    imageSelect() {
      return this.$store.state.mediaSelectList.filter(
        (item) => item.mediaType === MediaType.Image
      );
    },
    videoSelect() {
      return this.$store.state.mediaSelectList.filter(
        (item) => item.mediaType === MediaType.Video
      );
    },
  },
});
