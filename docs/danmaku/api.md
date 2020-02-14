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

- `/api/dplayer/v3/bilibili/?cid=[cid]&date=[date]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26`
- `/api/dplayer/v3/bilibili/?cid=[cid]&date=[date0]&date=[date1]` ，例如  
  `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26&date=2019-04-01`
- 使用 aid 方法同理。

## ArtPlayer 弹幕接口

- `/api/dplayer/v1/` ，例如 `https://danmaku.xwhite.studio/api/artplayer/v1/?id=C6CC6218F1FB8770`

### BiliBili 弹幕接口

- `/api/artplayer/v1/bilibili?cid=[cid]` ，例如  
  `https://danmaku.xwhite.studio/api/artplayer/v1/bilibili/?cid=1176840`
- `/api/artplayer/v1/bilibili?aid=[aid]&p=[p]` ，例如  
  `https://danmaku.xwhite.studio/api/artplayer/v1/bilibili?aid=810872&p=1`

<ClientOnly>
  <Vssue title="API-Danmaku | 弹幕服务器文档" />
</ClientOnly>
