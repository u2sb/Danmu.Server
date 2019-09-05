# Dplayer弹幕服务器C#版

## 文档

[https://doc.video.xwhite.studio/danmaku/cs.html](https://doc.video.xwhite.studio/danmaku/cs.html)


## 0.3版本

##关于0.3版本

0.3 版本目前仍在测试阶段，不建议使用。  
0.3 版本是完全的重构，所以并不兼容以前版本的数据库，所以不能直接从以前版本升级到0.3版本，等0.3版本正式发布之后会写一个数据库迁移工具。  
因为 0.3 版本使用了 .net core 3.0，所以需要 .net core 3.0 发布正式版以后再发布（很快就要发布了）。

### 使用

此版本仅供有能力使用的人进行测试使用，如果认为自己能力不足，请等待正式版本发布。

以Linux为例，64位系统可以下载已经编译好的文件直接使用，32位系统需要自己编译，因为开启了 R2R ，所以不能交叉编译，如需交叉编译请关闭 R2R，方法请自行搜索。

```
dotnet publish -c Release-Linux64 linux-x64 --self-contained false

dotnet publish -c Release-Linux32 linux-x86 --self-contained false
```

安装并配置好 PostgreSQL ，配置文件中填写的用户需要有创建数据库的权限，如果没有创建数据库的权限，需要自己手动创建好指定的数据库。

安装好 .net core 环境，需要 .net core 3.0 ，安装方法自己看官网，写的很详细。下载编译好的二进制文件，解压，运行  

```
./Danmaku
```

程序会自动创建数据库，并且配置好数据表的字段。

Linux使用了Unix套接字作为连接，所以反向代理需要代理 `unix:/tmp/dplayer.danmaku.sock`，目前此选项无法配置，后续会提供可配置的连接方式。  
Windows使用5000端口连接。



## 这是什么？

这是[Dplayer播放器](https://github.com/MoePlayer/DPlayer)的弹幕服务器

## 为什么要做这个东西？

因为作者免费提供那个弹幕服务器貌似是挂掉了，看了一下开源的[源码](https://github.com/MoePlayer/DPlayer-node)，还需要`docker`，我看到这东西就烦，然后转手就自己撸了一个出来。

## 这货有哪些功能？

原版有的功能这东西都有，顺便增强了一下BiliBili弹幕解析能力

- [x] 独立弹幕（和原版一样）
- [x] 解析BiliBili弹幕（支持分P和历史弹幕）

## 这货性能怎么样？

这个问题不好回答，具体性能，并发能力我也没测试过，如果哪位老哥是搞测试的，可以帮忙测试一下那就更好了。

为什么选择PostgreSQL作为数据库？这个纯粹是一个练手的项目，最开始打算是用Redis，但是对这东西一点也不了解，就先pass了。后来用了MySQL，等后面我就开始思考，能不能利用MySQL的NOSQL特性提高一下性能呢？但是我的测试机器只有1G内存，要跑MySQL5.7，甚至MySQL8.0，不想说什么了。再后来受到其他项目的启发，选择了PostgreSQL，试了一下编译安装，过程非常简单，没遇到什么坑，再试一下数据库操作，也非常简单，中间也没有有道什么坑，就这样确定了最终使用的数据库。

## 怎么用？

后面会写一个关于编译和部署的教程的，如果需要的人多的话，Docker版也在计划之内

## 会不会提供免费的弹幕服务器？

拿国外的小JJ搞了一个，性能有限，不提供任何保障，随时有可能挂掉，随时可能删库跑路，仅作演示使用，请勿用于生产环境

弹幕接口 `https://danmaku.xwhite.studio/api/dplayer/`

## 示例？

简单给一个吧   
视频：[https://doc.video.xwhite.studio/danmaku/index.html](https://doc.video.xwhite.studio/danmaku/index.html)   
普通弹幕接口 https://danmaku.xwhite.studio/api/dplayer/v3/?id=C6CC6218F1FB8770 （视频设定的id，最大长度32，推荐使用视频的CRC等唯一值作为id）  
BiliBili弹幕接口：  
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=28019559
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=17150441&p=1
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=1176840
- https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?aid=810872&p=1

Dplayer弹幕接口：

```js
    danmaku: {
        id: 'C6CC6218F1FB8770',     //建议使用视频 CRC64 或者其他唯一值
        api: 'https://danmaku.xwhite.studio/api/dplayer/',
        addition: ['https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?cid=cid']    //可使用 cid 或者aid + p作为参数，p 默认为1
    }
```


## TODO

- [ ] 管理面板
- [x] 直播弹幕
- [x] BiliBili历史弹幕叠加
