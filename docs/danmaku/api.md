---
title: API
---

## 通用弹幕接口

### 请求弹幕

- `/api/danmu/v1?id=[id]` ，例如  
  `https://danmu.u2sb.top/api/danmu/v1/?id=C6CC6218F1FB8770`
- `/api/danmu/v1/[id]`，例如  
  `https://danmu.u2sb.top/api/danmu/v1/C6CC6218F1FB8770`
- `/api/danmu/v1/[id].[format]`，例如  
  `https://danmu.u2sb.top/api/danmu/v1/C6CC6218F1FB8770.xml`
  `https://danmu.u2sb.top/api/danmu/v1/C6CC6218F1FB8770.json`(json 格式的弹幕)

### 发送弹幕

- 接口 `/api/danmu/v1/`
- 方法 `POST`
- 类型 `application/json`
- 参数

```json
{
"id": "C6CC6218F1FB8770", //视频ID
"time": 28.48447, //时间
"mode": 1,  //模式 1-滚动 4-底部 5-顶部
"size": 25, //字号
"color": 16777215,  //颜色，需转10进制
"author": "DIYgod", //作者
"authorId": 0,  //作者id，填写时可配合用户系统，待完善
"text": "弹幕", //弹幕内容
"referer" : ""  //来源，不填为当前网址，如果是hash导航，需手动填写
},
```

### BiliBili弹幕

- `/api/danmu/v1/bilibili/?cid=[cid]`  
  `/api/danmu/v1/bilibili/danmu.xml?cid=[cid]`  
  `/api/danmu/v1/bilibili/danmu.json?cid=[cid]`，例如  
  `https://danmu.u2sb.top/api/danmu/v1/bilibili/danmu.xml?cid=1176840`
- `/api/danmu/v1/bilibili/?aid=[aid]&p=[p]`  
  `/api/danmu/v1/bilibili/danmu.xml?aid=[aid]&p=[p]`  
  `/api/danmu/v1/bilibili/danmu.json?aid=[aid]&p=[p]`，当 p=1 时可以省略参数 p，例如
  `https://danmu.u2sb.top//api/danmu/v1/bilibili/danmu.xml?aid=810872&p=1`

## Dplayer 弹幕接口

- `/api/danmu/dplayer/` ，例如  
  `https://danmu.u2sb.top/api/danmu/dplayer/`

### BiliBili 弹幕接口

- `/api/danmu/dplayer/v3/bilibili/?cid=[cid]` ，例如  
  `https://danmu.u2sb.top/api/danmu/dplayer/v3/bilibili/?cid=73636868`
- `/api/danmu/dplayer/v3/bilibili/?aid=[aid]` ，例如  
  `https://danmu.u2sb.top/api/danmu/dplayer/v3/bilibili/?aid=41940944`
- `/api/danmu/dplayer/v3/bilibili/?aid=[aid]&p=[p]` ，例如  
  `https://danmu.u2sb.top/api/danmu/dplayer/v3/bilibili/?aid=41940944&p=1`

### BiliBili 历史弹幕接口

> 需配合 [BCookie](/danmaku/install.html#配置文件解释) 使用

- `/api/danmu/dplayer/v3/bilibili/?cid=[cid]&date=[date]` ，例如  
  `https://danmu.u2sb.top/api/danmu/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26`
- `/api/danmu/dplayer/v3/bilibili/?cid=[cid]&date=[date0]&date=[date1]` ，例如  
  `https://danmu.u2sb.top/api/danmu/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26&date=2019-04-01`
- 使用 aid 方法同理。

## ArtPlayer 弹幕接口

### 请求弹幕

- `/api/danmu/artplayer/v1?id=[id]` ，例如  
  `https://danmu.u2sb.top/api/danmu/artplayer/v1/?id=C6CC6218F1FB8770`
- `/api/danmu/artplayer/v1/[id]`，例如  
  `https://danmu.u2sb.top/api/danmu/artplayer/v1/C6CC6218F1FB8770`
- `/api/danmu/artplayer/v1/[id].[format]`，例如  
  `https://danmu.u2sb.top/api/danmu/artplayer/v1/C6CC6218F1FB8770.xml`
  `https://danmu.u2sb.top/api/danmu/artplayer/v1/C6CC6218F1FB8770.json`(json 格式的弹幕)

### 发送弹幕

- 接口 `/api/danmu/artplayer/v1/`
- 方法 `POST`
- 类型 `application/json`
- 参数

```json
{
  "id": "C6CC6218F1FB8770", //视频ID
  "text": "弹幕啊", //弹幕文本
  "time": 23.359518, //时间
  "color": "#fff", //颜色 
  "mode": 0, //弹幕类型 0-滚动 1-固定
}
```

### BiliBili 弹幕接口

- `/api/danmu/artplayer/v1/bilibili/?cid=[cid]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.xml?cid=[cid]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.json?cid=[cid]`，例如  
  `https://danmu.u2sb.top/api/danmu/artplayer/v1/bilibili/danmu.xml?cid=1176840`
- `/api/danmu/artplayer/v1/bilibili/?aid=[aid]&p=[p]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.xml?aid=[aid]&p=[p]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.json?aid=[aid]&p=[p]`，当 p=1 时可以省略参数 p，例如
  `https://danmu.u2sb.top//api/danmu/artplayer/v1/bilibili/danmu.xml?aid=810872&p=1`

### BiliBili 历史弹幕接口

> 需配合 [BCookie](/danmaku/install.html#配置文件解释) 使用

- `/api/danmu/artplayer/v1/bilibili/?cid=[cid]&date=[date0]&date=[date1]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.xml?cid=[cid]date=[date0]&date=[date1]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.json?cid=[cid]date=[date0]&date=[date1]`
- `/api/danmu/artplayer/v1/bilibili/?aid=[aid]&p=[p]&date=[date0]&date=[date1]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.xml?aid=[aid]&p=[p]&date=[date0]&date=[date1]`  
  `/api/danmu/artplayer/v1/bilibili/danmu.json?aid=[aid]&p=[p]&date=[date0]&date=[date1]`


### ArtPlayer 示例

> 推荐使用 XML 格式的弹幕，JSON 格式的弹幕需要在服务端做额外的转换

```js xml格式
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
        "https://danmu.u2sb.top/api/danmu/artplayer/v1/bilibili.xml?aid=810872&p=1",
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

```js json格式
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
          "https://danmu.u2sb.top/api/danmu/artplayer/v1/bilibili.json?aid=810872&p=1"
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
