import { CommentModel, ImageItemModel } from "@/models";
import { defineComponent } from "vue";

export default defineComponent({  
  props: {
    info: {
      type: Object as () => CommentModel,
      required: true,
    },
  },
  data() {
    return {
      rowMaxCount: 3,
    };
  },
  methods: {
    outterImageList(iamges: string[]): string[][] {
      const outterArray: string[][] = [];

      const foreachCount = Math.ceil(iamges.length / this.rowMaxCount);
      for (let i = 0; i < foreachCount; i++) {
        const startIndex = i * this.rowMaxCount;
        const newArray = iamges.slice(
          startIndex,
          startIndex + this.rowMaxCount
        );
        outterArray.push(newArray);
      }
      return outterArray;
    },
    setImageItem(coverUrl: string) {            
      let item: ImageItemModel = {
        id: "",
        subId: "",
        class: "box_image",
        src: coverUrl,
        alt: "",
      };
      return item;
    },
    showImageZoom(imageUrl: string) {
      this.$emit("show", imageUrl);
    },
  },
});
