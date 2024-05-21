<template>
  <video
    v-if="item.isCloud"
    ref="videoPlayer"
    class="video-js vjs-default-skin"
    preload="auto"
    playsinline
    :poster="imageString"
    data-setup="{}"
    :class="item.class"
  >
    <source :src="item.src" type="application/x-mpegURL" v-if="isM3U8" />
    <source :src="item.src" v-else />
    <p class="vjs-no-js">
      To view this video please enable JavaScript, and consider upgrading to a
      web browser that
      <!-- <a href="https://videojs.com/html5-video-support/" target="_blank">
        supports HTML5 video
      </a> -->
    </p>
  </video>
  <video
    v-else
    :src="item.src"
    :type="videoType"
    ref="videoPlayer"
    class="video-js vjs-default-skin"
    :class="item.class"
    preload="auto"
    playsinline
    data-setup="{}"
  ></video>
</template>

<script lang="ts">
import api from "@/api";
import { defineComponent } from "vue";
import { VideoItemModel } from "@/models";
import videojs from "video.js";
import { hlsKey } from "@/defaultConfig";

export default defineComponent({
  props: {
    item: {
      type: Object as () => VideoItemModel,
      required: true,
    },
    showMediaControl: Boolean,
    bottomHight: Number,
  },
  data() {
    return {
      videoOptions: {
        controls: false,
        controlBar: {
          fullscreenToggle: false,
          pictureInPictureToggle: false,
          progressControl: false,
          remainingTimeDisplay: false,
          // playToggle: false,
          // captionsButton: false,
          // chaptersButton: false,
          // subtitlesButton: false,
          // playbackRateMenuButton: false,
        },
        sources: [] as { [name: string]: string }[],
      },
      imageSrc:
        require("@/assets/images/element/defaultCoverImage.png") as string,
    };
  },
  emits: ["clickEvent"],
  methods: {
    setM3U8Video() {
      const path: { [name: string]: string } = {
        src: this.item.src,
        type: this.videoType,
      };
      this.videoOptions.sources.push(path);
      this.videoOptions.controls = this.showMediaControl;
      videojs(this.$refs.videoPlayer, this.videoOptions, () => {});

      if (this.item.isCloud) {
        videojs.Vhs.xhr.beforeRequest = function (options: {
          headers: { [x: string]: string };
        }) {
          options.headers = options.headers || {};
          options.headers["x-token"] = hlsKey;
          return options;
        };
      }
    },
  },
  mounted() {
    if (this.showMediaControl) {
      // setTimeout(() => {
      this.setM3U8Video();
      // });
    }
  },
  computed: {
    videoType() {
      return this.isM3U8 ? "application/x-mpegURL" : "video/mp4";
    },
    isM3U8() {
      return /(.m3u8)/i.test(this.item.src) && this.item.isCloud;
    },
    imageString() {
      if (!this.item.coverUrl)
        return api.getImageUrl("@/assets/images/element/defaultCoverImage.png");
      const container = this.$store.state.imageCache.get(this.item.id);
      if (container) {
        var cacheImgSrc = container.get(this.item.coverUrl);
        if (cacheImgSrc) {
          return cacheImgSrc;
        } else {
          return api.getImageUrl(this.item.coverUrl);
        }
      }
      if (this.item.coverUrl && this.item.coverUrl.indexOf("http") !== 0) {
        return this.item.coverUrl;
      }
      return api.getImageUrl("@/assets/images/element/defaultCoverImage.png");
    },
  },
});
</script>
<style src="video.js/dist/video-js.css"></style>

<style>
.video-js .vjs-control-bar {
  display: none !important;
}
.vjs-poster {
  display: none !important;
}
</style>
