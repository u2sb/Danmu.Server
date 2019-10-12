# 其他方案

## HLS直播流

对直播延时没有要求的可以使用，如果互动性较强（要求延时控制在几秒之内），最好不要使用这种方案（据说又拍云可以优化在3s以内，但是具体效果怎么样没有尝试过，因为又拍云平台相对于其他同类型的直播服务价格稍微高一点）

```
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css">
    <script src="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js"></script>

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
                url: 'http://live.xwhite.studio/hls/streamname.m3u8',
                type: 'hls'
            }
        });

        dp.on('fullscreen', function () {
            if (/Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                screen.orientation.lock('landscape');
            }
        });
    </script>
</body>
```

## RTMP直播流

需要使用`swf.js`，并且需要运行flash，不推荐使用，感兴趣的可以自行百度