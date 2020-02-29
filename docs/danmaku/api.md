---
title: API
---

## Dplayer 弹幕接口

- `/api/dplayer/` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/`

### BiliBili 弹幕接口

- `/api/dplayer/v3/bilibili/?cid=[cid]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868`
- `/api/dplayer/v3/bilibili?aid=[aid]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=41940944`
- `/api/dplayer/v3/bilibili?aid=[aid]&p=[p]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=41940944&p=1`

### BiliBili 历史弹幕接口

> 需配合 [BCookie](/danmaku/install.html#配置文件解释) 使用

- `/api/dplayer/v3/bilibili/?cid=[cid]&date=[date]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26`
- `/api/dplayer/v3/bilibili/?cid=[cid]&date=[date0]&date=[date1]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26&date=2019-04-01`
- 使用 aid 方法同理。

## ArtPlayer 弹幕接口

### 请求弹幕

- `/api/artplayer/v2?id=[id]` ，例如  
  `https://danmaku.xwhite.studio/api/artplayer/v2?id=C6CC6218F1FB8770`
- `/api/artplayer/v2/[id]`，例如  
  `https://danmaku.xwhite.studio/api/artplayer/v2/C6CC6218F1FB8770`
- `/api/artplayer/v2/[id].[format]`，例如  
  `https://danmaku.xwhite.studio/api/artplayer/v2/C6CC6218F1FB8770.xml`
  `https://danmaku.xwhite.studio/api/artplayer/v2/C6CC6218F1FB8770.json`(json 格式的弹幕)

### 发送弹幕

发送弹幕使用了类似 Dplayer 的接口

> V2 接口目前还未确定形式，后期可能会更改，V1 接口已经确定，后面基本不会变动了

- 接口 `/api/artplayer/v1/`
- 方法 `POST`
- 类型 `application/json`
- 参数

```json
{
  "id": "C6CC6218F1FB8770", //视频ID
  "text": "弹幕啊", //弹幕文本
  "time": 23.359518, //时间
  "color": 16777215, //颜色 需16进制转10进制
  "type": 0, //弹幕类型 0-滚动 1-固定
  "author": "DIYgod" //用户名
}
```

### BiliBili 弹幕接口

- `/api/artplayer/bilibili?cid=[cid]`  
  `/api/artplayer/bilibili.xml?cid=[cid]`  
  `/api/artplayer/bilibili.json?cid=[cid]`，例如  
  `https://danmaku.xwhite.studio/api/artplayer/bilibili.xml?cid=1176840`
- `/api/artplayer/bilibili?aid=[aid]&p=[p]`  
  `/api/artplayer/bilibili.xml?aid=[aid]&p=[p]`  
  `/api/artplayer/bilibili.xml?aid=[aid]&p=[p]`，当 p=1 时可以省略参数 p，例如
  `https://danmaku.xwhite.studio/api/artplayer/bilibili.xml?aid=810872&p=1`

### BiliBili 历史弹幕接口

> 需配合 [BCookie](/danmaku/install.html#配置文件解释) 使用

- `/api/artplayer/bilibili?cid=[cid]&date=[date0]&date=[date1]`  
  `/api/artplayer/bilibili.xml?cid=[cid]date=[date0]&date=[date1]`  
  `/api/artplayer/bilibili.json?cid=[cid]date=[date0]&date=[date1]`
- `/api/artplayer/bilibili?aid=[aid]&p=[p]&date=[date0]&date=[date1]`  
  `/api/artplayer/bilibili.xml?aid=[aid]&p=[p]&date=[date0]&date=[date1]`  
  `/api/artplayer/bilibili.xml?aid=[aid]&p=[p]&date=[date0]&date=[date1]`


### ArtPlayer 示例

> 推荐使用 XML 格式的弹幕，JSON 格式的弹幕需要在服务端做额外的转换

```js
var art = new Artplayer({
  container: ".artplayer-app",
  url: "/1.mp4",
  autoSize: true,
  setting: true,
  playbackRate: true,
  fullscreenWeb: true,
  plugins: [
    artplayerPluginDanmuku({
      danmuku:
        "https://danmaku.xwhite.studio/api/artplayer/bilibili.xml?aid=810872&p=1",
      speed: 5,
      maxlength: 50,
      margin: [10, 100],
      opacity: 1,
      fontSize: 25,
      synchronousPlayback: false
    })
  ]
});
```

```js
var art = new Artplayer({
  container: ".artplayer-app",
  url: "/1.mp4",
  autoSize: true,
  setting: true,
  playbackRate: true,
  fullscreenWeb: true,
  plugins: [
    artplayerPluginDanmuku({
      danmuku: () =>
        fetch(
          "https://danmaku.xwhite.studio/api/artplayer/bilibili.json?aid=810872&p=1"
        )
          .then(res => res.json())
          .then(res => res.data),
      speed: 5,
      maxlength: 50,
      margin: [10, 100],
      opacity: 1,
      fontSize: 25,
      synchronousPlayback: false
    })
  ]
});
```

<ClientOnly>
  <Vssue title="API-Danmaku | 弹幕服务器文档" />
</ClientOnly>
