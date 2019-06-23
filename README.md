# Dplayer弹幕服务器C#版

## 这是什么？

这是[Dplayer播放器](https://github.com/MoePlayer/DPlayer)的弹幕服务器

## 为什么要做这个东西？

因为作者免费提供那个弹幕服务器貌似是挂掉了，看了一下开源的[源码](https://github.com/MoePlayer/DPlayer-node)，还需要`docker`，我看到这东西就烦，然后转手就自己撸了一个出来。

## 这货有哪些功能？

原版有的功能这东西都有，顺便增强了一下BiliBili弹幕解析能力

- [x] 独立弹幕（和原版一样）
- [x] 解析BiliBili弹幕（比原版多了支持分P）

## 这货性能怎么样？

这个问题不好回答，具体性能，并发能力我也没测试过，MySQL的性能和Redis比，具体差多少，反正也没到那个量级，C#比JS性能强多少也说不准，反正就是你自己用的话，100%是没问题的。解析一个8000+弹幕的bilibili弹幕列表，服务器等待时间能控制在500ms以内（包括网络请求时间）

为什么用MySQL不用Redis？你的业务量根本没必要搞那种骚操作，如果你喜欢，你也可以把MySQL当成内存数据库，也可以把MySQL当成NoSQL用。

## 怎么用？

后面会写一个关于编译和部署的教程的，如果需要的人多的话，Docker版也在计划之内

## 会不会提供免费的弹幕服务器？

拿国外的小JJ搞了一个，性能有限，不提供任何保障，随时有可能挂掉，随时可能删库跑路，仅作演示使用，请勿用于生产环境

## 示例？

简单给一个吧   
视频：https://blog.xxwhite.com/2017/VideoTest.html  
普通弹幕接口 https://danmaku.xwhite.studio/api/dplayer/v3/?id=46190A32F63DFF2CF0A3BB0F3293636C （视频设定的id，最大长度32，推荐使用视频的MD5值作为id）  
BiliBili弹幕接口：  
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=28019559
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=17150441&p=1
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=1176840
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=810872&p=1

Dplayer弹幕接口：

```js
    danmaku: {
        id: '46190A32F63DFF2CF0A3BB0F3293636C',     //建议使用视频MD5或者其他唯一值
        api: 'https://danmaku.xwhite.studio/api/dplayer/',
        addition: ['https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?cid=cid']    //可使用cid或者aid+p作为参数，p默认为1
    }
```


## TODO

- [ ] 管理面板