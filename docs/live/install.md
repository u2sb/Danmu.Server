---
title: 安装
---

## 安装

程序安装可以参考 [安装](/danmaku/install.md)

## 简单使用

```html
<head>
  <meta charset="UTF-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0" />

  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css" />
  <script src="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/flv.js/dist/flv.min.js"></script>
  <script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr/dist/browser/signalr.min.js"></script>
  <script src="https://cdn.jsdelivr.net/gh/MonoLogueChi/Danmu.Server@gh-pages/js/livedanmaku.js"></script>
</head>
<body>
  <div id="dplayer" oncontextmenu="return false"></div>
  <script>
    const dp = new DPlayer({
      container: document.getElementById("dplayer"),
      screenshot: true,
      autoplay: true,
      muted: true, //设置后会静音，目的是让移动平台也可以自动播放
      live: true,
      video: {
        url:
          "http://live.xwhite.studio/live?port=1935&app=live&stream=streamname",
        type: "flv"
      },
      danmaku: true,
      apiBackend: liveDan(
        "https://danmu.u2sb.top/api/live/danmu",
        "cd30ae05-4ad5-5135-bd6d-337ac0de102e",
        function(dan) {
          dp.danmaku.draw(dan);
        }
      )
    });

    dp.on("fullscreen", function() {
      if (/Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        screen.orientation.lock("landscape");
      }
    });
  </script>
</body>
```

需要引入我写的 js `livedanmaku.js`，然后配置仿照我上面的写就好了，弹幕服务器地址 `https://danmu.u2sb.top/api/dplayer/live`，`cd30ae05-4ad5-5135-bd6d-337ac0de102e` 是你的房间，每个直播间需要唯一，建议使用 uuid，如果是你自己搭建的弹幕服务器，路由都是你自己配的。

通信协议是使用WebSocket，所以需要在反向代理那一层做好设置，允许WebSocket连接。

livedanmaku.js:

<<< @/docs/.vuepress/public/js/livedanmaku.js{highlightLines}

<ClientOnly>
  <Vssue title="安装-Live | 弹幕服务器文档" />
</ClientOnly>