<template>
  <div class="dplayer">
    <button class="btn-show-player" v-show="!shouldShowPlayer" @click="showPlayer">点击加载播放器</button>
    <div v-if="shouldShowPlayer" ref="container" />
  </div>
</template>

<script>
export default {
  props: {
    src: {
      type: String,
      default: "",
      required: true
    },
    screenshot: {
      type: Boolean,
      default: false
    },
    subtitle: {
      type: String,
      default: ""
    },
    danmuId: {
      type: String,
      default: ""
    },
    danmuApi: {
      type: String,
      default: "https://danmu.u2sb.top/api/dplayer/"
    },
    danmuAddition: {
      type: String,
      default: ""
    },
    danmuAddition1: {
      type: String,
      default: null
    },
    danmuAddition2: {
      type: String,
      default: null
    },
    autoplay: {
      type: Boolean,
      default: false
    },
    theme: {
      type: String,
      default: "#b7daff"
    },
    loop: {
      type: Boolean,
      default: false
    },
    hotkey: {
      type: Boolean,
      default: true
    },
    preload: {
      type: String,
      default: "auto"
    },
    logo: {
      type: String,
      default: ""
    },
    mutex: {
      type: Boolean,
      default: true
    },
    crossOrigin: {
      type: Boolean,
      default: false
    },
    proxy: {
      type: String,
      default: "https://cors-anywhere.herokuapp.com"
    }
  },
  data() {
    return {
      shouldShowPlayer: false
    };
  },
  methods: {
    showPlayer() {
      this.shouldShowPlayer = true;
      Promise.all([
        import(/* webpackChunkName: "dplayer" */ "dplayer/dist/DPlayer.min.js"),
        import(/* webpackChunkName: "dplayer" */ "dplayer/dist/DPlayer.min.css")
      ]).then(([{ default: DPlayer }]) => {
        // eslint-disable-next-line
        let dp = new DPlayer({
          container: this.$refs.container,
          autoplay: this.autoplay,
          theme: this.theme,
          loop: this.loop,
          screenshot: this.screenshot,
          hotkey: this.hotkey,
          preload: this.preload,
          logo: this.logo,
          video: {
            url: this.crossOrigin ? `${this.proxy}/${this.src}` : this.src
          },
          subtitle: this.subtitle
            ? {
                url: this.crossOrigin
                  ? `${this.proxy}/${this.subtitle}`
                  : this.subtitle,
                color: "#000000",
                fontSize: "25px",
                bottom: "2%"
              }
            : null,
          danmu: {
            id: this.danmuId,
            api: this.danmuApi,
            addition: [
              this.danmuAddition,
              this.danmuAddition1,
              this.danmuAddition2
            ].filter(function(s) {
              return s && s.trim();
            }),
            maximum: 10000,
            bottom: "15%"
          },
          mutex: this.mutex
        });
        dp.on("fullscreen", function() {
          if (
            /Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(
              navigator.userAgent
            )
          ) {
            screen.orientation.lock("landscape");
          }
        });
      });
    }
  }
};
</script>

<style lang="stylus" scoped>
button.btn-show-player {
  margin-top: 0.75rem;
  border: 2px solid #4dba87;
  background-color: #fff;
  cursor: pointer;
  outline: none;
  border-radius: 33px;
  padding: 5px 16px;
  font-size: 13px;
  font-weight: 700;
  transition: background-color 0.3s ease;
  color: #4dba87;
}
</style>