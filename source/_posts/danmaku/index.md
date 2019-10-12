# 弹幕后端服务

[Dplayer](http://dplayer.js.org)是一个带有弹幕功能的视频播放器，弹幕功能需要后端配合才能使用，作者提供的那个免费使用的后端已经挂掉许久了（写文档的时候反正是没恢复）。粗略翻了一下GitHub，包括Dplayer作者开源的那个后端。大大小小有七八个后端吧，看到这些，我决定再造一个轮子。

Dplaeyr的弹幕[接口](http://dplayer.js.org/zh/guide.html#弹幕)和相关[API](http://dplayer.js.org/zh/guide.html#api)可以看[文档](http://dplayer.js.org/zh/guide.html)

{% raw %}
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css">
<div id="dplayer"></div>
<script async>
    loadScript('https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js',function(){
        const dp = new DPlayer({
            container: document.getElementById('dplayer'),
            volume: 0.7,
            video: {
                quality:[{
                    name: '1080P',
                    url: '/videos/s_1080.mp4'
                }, {
                    name: '720P',
                    url: '/videos/s_720.mp4'
                }, {
                    name: '540P',
                    url: '/videos/s_540.mp4'
                }],
                defaultQuality: 2,
                pic: '/videos/s.jpg',
                thumbnails: '/videos/thumbnails.jpg'
            },
            danmaku: {
                id: 'C6CC6218F1FB8770',
                api: 'https://danmaku.xwhite.studio/api/dplayer/',
                addition: ['https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?cid=73636868','https://danmaku.xwhite.studio/api/dplayer/v3/bilibili?cid=73636868&date=2019-07-01&date=2019-04-01&date=2019-02-01']
            },
            highlight: [{
                time: 67,
                text: '洗脑'
            }]
        });

        dp.on('fullscreen', function () {
            if (/Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                screen.orientation.lock('landscape');
            }
        });
    });
</script>
{% endraw %}

> 感谢[Ximenbao](https://space.bilibili.com/20806597/)提供的演示视频


```
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css">
<script src="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js"></script>
<div id="dplayer"></div>
<script>
    const dp = new DPlayer({
        container: document.getElementById('dplayer'),
        video: {
            url: 'demo.mp4'
        },
        subtitle: {
                url: 'demo.vtt'
        },
        danmaku: {
                id: '视频的ID，建议使用视频Hash值，例如CRC64',
                api: 'https://danmaku.xwhite.studio/api/dplayer/',  //api
                addition: ['https://danmaku.xwhite.studio/api/dplayer/v3/bilibili/?cid=73636868']   //解析BiliBili弹幕
        }
    });

    //修复手机横屏问题
    dp.on('fullscreen', function () {
        if (/Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            screen.orientation.lock('landscape');
        }
    });
</script>
```
