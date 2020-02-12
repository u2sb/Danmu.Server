# Dplayer弹幕服务器C#版

## 文档

[https://dandoc.u2sb.top](https://dandoc.u2sb.top)

## 这是什么？

这是 [Dplayer 播放器](https://github.com/MoePlayer/DPlayer)的弹幕服务器

## 为什么要做这个东西？

因为作者免费提供那个弹幕服务器貌似是挂掉了，看了一下开源的[源码](https://github.com/MoePlayer/DPlayer-node)，还需要 `docker`，我看到这东西就烦，然后转手就自己撸了一个出来。

## 这货有哪些功能？

原版有的功能这东西都有，顺便增强了一下 Bilibili 弹幕解析能力

- [x] 独立弹幕（和原版一样）
- [x] 解析 Bilibili 弹幕（支持分 P 和历史弹幕）

## 这货性能怎么样？

这个问题不好回答，具体性能，并发能力我也没测试过，如果哪位老哥是搞测试的，可以帮忙测试一下那就更好了。

为什么选择 PostgreSQL 作为数据库？这个纯粹是一个练手的项目，最开始打算是用 Redis，但是对这东西一点也不了解，就先 pass 了。后来用了 MySQL，等后面我就开始思考，能不能利用 MySQL 的 NOSQL 特性提高一下性能呢？但是我的测试机器只有 1G 内存，要跑 MySQL5.7，甚至 MySQL8.0，不想说什么了。再后来受到其他项目的启发，选择了 PostgreSQL，试了一下编译安装，过程非常简单，没遇到什么坑，再试一下数据库操作，也非常简单，中间也没有有道什么坑，就这样确定了最终使用的数据库。

## 怎么用？

后面会写一个关于编译和部署的教程的，如果需要的人多的话，Docker 版也在计划之内。

## 会不会提供免费的弹幕服务器？

拿国外的小 JJ 搞了一个，性能有限，不提供任何保障，随时有可能挂掉，随时可能删库跑路，仅作演示使用，请勿用于生产环境

弹幕接口 `https://danmaku.xwhite.studio/api/dplayer/`

## 示例

视频：[https://dandoc.u2sb.top/danmaku/index.html](https://dandoc.u2sb.top/danmaku/index.html)   
普通弹幕接口： https://danmaku.xwhite.studio/api/dplayer/v3/?id=C6CC6218F1FB8770 （视频设定的 id，最大长度 32，推荐使用视频的 CRC 等唯一值作为 id）  
BiliBili 弹幕接口：  
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=28019559
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=17150441&p=1
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=1176840
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=810872&p=1

Dplayer 弹幕接口：

```js
    danmaku: {
        id: 'C6CC6218F1FB8770',     //建议使用视频 CRC64 或者其他唯一值
        api: 'https://danmaku.xwhite.studio/api/dplayer/',
        addition: ['https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?cid=cid']    //可使用 cid 或者aid + p作为参数，p 默认为1
    }
```

## ArtPlayer弹幕

普通弹幕接口 https://danmaku.xwhite.studio/api/artplayer/v1/?id=C6CC6218F1FB8770  
Bilibili 弹幕代理  
- https://danmaku.xwhite.studio/api/artplayer/v1/bilibili/?cid=1176840
- https://danmaku.xwhite.studio/api/artplayer/v1/bilibili?aid=810872&p=1

## TODO

- [ ] 管理面板
- [x] 直播弹幕
- [x] BiliBili历史弹幕叠加
