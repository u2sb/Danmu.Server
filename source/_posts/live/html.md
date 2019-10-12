# 网页

> 参考了[Dplayer文档](http://dplayer.js.org/guide.html#live)，详细配置请看文档。


{% raw %}
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css">
<div id="dplayer"></div>
<script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr/dist/browser/signalr.min.js"></script>
<script src="https://cdn.jsdelivr.net/gh/MonoLogueChi/doc.video.xwhite.studio@0.0.2/source/js/livedanmaku.js"></script>
<script src="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js"></script>
<script>
window.onload=function(){
    const dp = new DPlayer({
        container: document.getElementById('dplayer'),
        live: true,
        loop: true,
        autoplay: true,
        volume: 0.6,
        video: {
            url: '/videos/s_720.mp4'
        },
        danmaku: true,
        apiBackend: liveDan('https://danmaku.xwhite.studio/api/dplayer/live', "san", function (dan) {
            dp.danmaku.draw(dan);
        })
    });
    dp.on('fullscreen', function () {
        if (/Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            screen.orientation.lock('landscape');
        }
    });
}
</script>
{% endraw %}





```
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css">
    <script src="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js"></script>
    <script src="https://cdn.jsdelivr.net/gh/MonoLogueChi/doc.video.xwhite.studio@0.0.2/source/js/livedanmaku.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/flv.js/dist/flv.min.js"></script>
</head>
<body>
    <div id="dplayer" oncontextmenu="return false"></div>
    <script>
        const dp = new DPlayer({
            container: document.getElementById('dplayer'),
            screenshot: true,
            autoplay: true,
            muted: true,    //设置后会静音，目的是让移动平台也可以自动播放
            live: true,
            video: {
                url: 'http://live.xwhite.studio/live?port=1935&app=live&stream=streamname',
                type: 'flv'
            },
            danmaku: true,
            apiBackend: liveDan('https://danmaku.xwhite.studio/api/dplayer/live', "san", function (dan) {
                dp.danmaku.draw(dan);
            })
        });

        dp.on('fullscreen', function () {
            if (/Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                screen.orientation.lock('landscape');
            }
        });
    </script>
</body>
```

需要引入我写的js `livedanmaku.js`，然后配置仿照我上面的写就好了，弹幕服务器地址 `https://danmaku.xwhite.studio/api/dplayer/live`，`san` 是你的房间，每个直播间需要唯一，建议使用uuid，如果是你自己搭建的弹幕服务器，路由都是你自己配的。

> 旧版本需要引入 `https://cdn.jsdelivr.net/gh/MonoLogueChi/doc.video.xwhite.studio@0.0.1/source/js/livedanmaku.js`
