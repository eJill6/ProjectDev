import api from "@/api";
import { MediaType, SourceType } from "@/enums";
import { MediaModel, MediaResultModel } from "@/models";
import { MutationType } from "@/store";
import toast from "@/toast";
import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {
      imageZoomItem: {} as MediaModel,
    };
  },
  methods: {
    onFileChanged(event: any, source: SourceType, media: MediaType) {
      const maxSize = 1024 * 1024 * 5; // 5MB
      const files = event.target.files;
      const file = files[0];
      var reader = new FileReader();
      reader.readAsDataURL(file);

      reader.onload = async (event: ProgressEvent<FileReader>) => {
        const target = event.target as FileReader;
        let imageData = target.result as string;

        const size = file.size;

        if (size > maxSize) {
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
        };
        this.imageSelect.push(image);
        this.$store.commit(MutationType.SetImageSelect, this.imageSelect);
      };
    },
    convertBase64ToBlob(base64Image: string) {
      // Split into two parts
      const parts = base64Image.split(";base64,");
      // Hold the content type
      const imageType = parts[0].split(":")[1];
      // Decode Base64 string
      const decodedData = window.atob(parts[1]);
      // Create UNIT8ARRAY of size same as row data length
      const uInt8Array = new Uint8Array(decodedData.length);
      // Insert all character code into uInt8Array
      for (let i = 0; i < decodedData.length; ++i) {
        uInt8Array[i] = decodedData.charCodeAt(i);
      }
      // Return BLOB image after conversion
      return new Blob([uInt8Array], { type: imageType });
    },
    base64ToArrayBuffer(base64: string) {
      var binaryString = atob(base64);
      var bytes = new Uint8Array(binaryString.length);
      for (var i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
      }

      return bytes.buffer as ArrayBuffer;
    },
    cleanImagesState() {
      this.$store.commit(MutationType.SetImageSelect, []);
      this.$store.commit(MutationType.SetCloudImage, {});
    },
    async uploadImages() {
      const cloudImage = this.imageSelect.filter((item) => item.isCloud);

      let results = cloudImage.map(
        (item) =>
          <MediaResultModel>{
            id: item.fileName,
            fullMediaUrl: item.bytes,
          }
      );

      const newImageList = this.imageSelect.filter((item) => !item.isCloud);

      const delayPromise = async (
        apiData: MediaModel
      ): Promise<MediaResultModel> => {
        const parts = apiData.bytes.split(";base64,");
        const updateModel = { ...apiData };
        updateModel.bytes = parts[1];
        try {
          const result = await api.createMedia(updateModel);
          return await Promise.resolve(result);
        } catch (e) {
          return await Promise.reject();
        }
      };
      try {
        await newImageList.reduce(
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
    deleteImage(image: MediaModel) {
      const index = this.imageSelect
        .map((item) => item.bytes)
        .indexOf(image.bytes);
      if (index > -1) this.imageSelect.splice(index, 1);
      this.imageZoomItem = {} as MediaModel;
    },
    setImageList(
      photoList: { [name: string]: string },
      sourceType: SourceType
    ) {
      this.$store.commit(MutationType.SetCloudImage, photoList);
      const keys = Object.keys(photoList);
      const imageList = keys.map(
        (name) =>
          <MediaModel>{
            bytes: photoList[name],
            fileName: name,
            sourceType: sourceType,
            mediaType: MediaType.Image,
            isCloud: true,
          }
      );
      this.$store.commit(MutationType.SetImageSelect, imageList);
    },
  },
  computed: {
    imageSelect() {
      return this.$store.state.imageSelectList;
    },
  },
});
