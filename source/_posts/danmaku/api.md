# API

## Dplayer弹幕接口

- `/api/dplayer/` ，例如 `https://danmaku.xwhite.studio/api/dplayer/`

## BiliBili弹幕接口

- `/api/dplayer/v3/bilibili/?cid=[cid]` ，例如 `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868`
- `/api/dplayer/v3/bilibili?aid=[aid]` ，例如 `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=41940944`
- `/api/dplayer/v3/bilibili?aid=[aid]&p=[p]` ，例如 `https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=41940944&p=1`

## BiliBili历史弹幕接口

- `/api/dplayer/v3/bilibili/?cid=[cid]&date=[date]` ，例如 ` https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26`
- `/api/dplayer/v3/bilibili/?cid=[cid]&date=[date0]&date=[date1]` ，例如 ` https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868&date=2019-07-26&date=2019-04-01`
- 使用 `aid` 方法同理。

## 直播弹幕接口

直播弹幕接口见 [网页](/live/html.html)