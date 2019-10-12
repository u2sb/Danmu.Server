# 服务端搭建

服务端使用 `nginx` + [`nginx-http-flv-module`](https://github.com/winshining/nginx-http-flv-module) + `ffmpeg` 组合，以前也录制过一个视频，可以看一下作为参考
[https://www.bilibili.com/video/av54940290](https://www.bilibili.com/video/av54940290)

## 编译安装nginx

相信大多数人的服务器上都已经安装了nginx，不管是你是直接编译安装的，还是用lnmp或者宝塔之类的东西安装的，只要是编译安装的都算。如果没有安装，只要clone源码就可以了，后面的步骤基本上是一样的。


clone `nginx-http-flv-module` 源码

```
git clone https://github.com/winshining/nginx-http-flv-module.git --depth 1
```

查看nginx编译信息

```
nginx -V
```

后面的复制下来，也就是从第一个`--`开始一直到结束，等下可以直接粘贴上去。

定位到niginx的源码目录

```
./configure 刚才复制的那一长串 --add-module=/path/to/nginx-http-flv-module
```

如果是第一次编译安装（以前没有安装过，或者是以前用的默认参数）

```
./configure --add-module=/path/to/nginx-http-flv-module
```

编译&安装

```
make
make install
```

然后耐心等待编译完成就可以了，具体时间视服务器配置而定。

最后不放心的话可以再检查一遍

```
nginx -V
```

## 配置文件

首先要对ngixn的配置文件有一定的了解，才能看得懂下面的这些内容，否则就是看天书。

首先先在nginx配置文件添加，不要写错位置，是在http的外层添加，因为rtmp和http属于同一级别的

```
...
events
    {
        ...
    }

http
    {
        ...
    }
include /www/server/panel/vhost/nginx/server/*.conf;
...
```

然后就是在`/www/server/panel/vhost/nginx/server`创建一个配置文件，比如`rtmp.conf`，写入：

```
rtmp {
    server {
        listen 1935;
        server_name live.xwhite.studio

        application live {
            live on;
            hls on;
            hls_path /www/wwwroot/hls;
        }
    }
}
```

更多配置项请看 [https://github.com/winshining/nginx-http-flv-module#example-configuration](https://github.com/winshining/nginx-http-flv-module#example-configuration)


网站（http模块中）写入相关配置

```
location /live {
    flv_live on; #open flv live streaming (subscribe)
    chunked_transfer_encoding  on; #open 'Transfer-Encoding: chunked' response

    add_header 'Access-Control-Allow-Origin' '*'; #add additional HTTP header
    add_header 'Access-Control-Allow-Credentials' 'true'; #add additional HTTP header
}

location /hls {
    types {
        application/vnd.apple.mpegurl m3u8;
        video/mp2t ts;
    }

    root /www/wwwroot;
    add_header 'Cache-Control' 'no-cache';
}
```

## 推流地址

```
rtmp://live.xwhite.studio/live/streamname
```

> `1935`端口可省略，其他端口请加上端口号，streamname自己定义，后面拉流的适合要使用相同的流名称，在obs中可以将此部分分开填写。

> 小知识，OBS中的推流地址其实是拼接起来的，比如

![OBS](/images/docs/obs.png)

实际推流地址是`rtmp://172.18.107.50:1935/hls/123456`

## 拉流地址

### RTMP流
```
rtmp://live.xwhite.studio/live/streamname
```

### HTTP-FLV流
```
http://live.xwhite.studio/live?port=1935&app=live&stream=streamname
```

> 注意，域名为你http网站配置的域名，当推流端口为1935时，可省略port参数，app参数为rtmp模块中设置的参数，示例中为live。

### HLS流

```
http://live.xwhite.studio/hls/streamname.m3u8
```

> 注意，域名为你http网站配置的域名。



## 转码

转码和流转发都使用ffmpeg，以Debian为例简单结束

### 安装ffmpeg

```
apt-get install ffmpeg
```

### 流转发

```
rtmp {
    server {
        listen 1935;
        application live {
            live on;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v copy -f flv rtmp://js.live-send.acg.tv/live-js/?streamname=xxxxxxxxx&key=xxxxxxxxxxx;
        }
    }
}
```

这一段代码是把接收到的流转发到B站直播间或者是其他直播间，后面的推流地址就是你的直播间推流地址，如果需要转发到多个直播间，就再多复制几次就可以了，比如：

```
rtmp {
    server {
        listen 1935;
        application live {
            live on;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v copy -f flv rtmp://js.live-send.acg.tv/live-js/?streamname=直播间0&key=xxxxxxxxxxx;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v copy -f flv rtmp://js.live-send.acg.tv/live-js/?streamname=直播间1&key=xxxxxxxxxxx;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v copy -f flv rtmp://js.live-send.acg.tv/live-js/?streamname=直播间2&key=xxxxxxxxxxx;
        }
    }
}
```

> 如果是国内想在国外直播，可能需要一台国外的服务器去转发一下你的直播流；
> 如果是国外想在国内直播，可能需要一台国内的服务器去转发一下你的直播流；
> 如果仅仅是一个直播间的话，不需要使用ffmpeg，直接用stream模块转发就可以了，如果需要转发到多个直播地址，就需要使用ffmpeg了。


### 转码

如果需要多清晰度的话，就需要服务器转码了

```
rtmp {
    server {
        listen 1935;
        application live {
            live on;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v libx264 -preset fast -profile high -level 51 -b:v 4M -maxrate 6M -s 1920x1080 -f flv rtmp://127.0.0.1/high/$name;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v libx264 -preset fast -profile high -level 51 -b:v 2M -maxrate 5M -s 1280x720 -f flv rtmp://127.0.0.1/medium/$name;
            exec ffmpeg -re -i rtmp://127.0.0.1/live/$name -c:a copy -c:v libx264 -preset fast -profile high -level 51 -b:v 1M -maxrate 3M -s 960x540 -f flv rtmp://127.0.0.1/low/$name;
        }
        application high {
            live on;
        }
        application medium {
            live on;
        }
        application low {
            live on;
        }
    }
}
```

这样推到live上的流就会被转码成三个分辨率，分别推到`high` `medium` `low`三个app上，而streamname参数使用`$name`变量传递，没有发生改变，拉流规则和上面介绍的相同，只需要更换`app`参数就可以拉取到不同分辨率的流。

### 转码+流转发

就是把上面两个结合起来，把转码的配置中的推流地址改成其他直播间的推流地址就可以了。


> 因为某些特殊的原因，我这里就不提供直播服务器演示了（我是真的怂了），如果有人需要定制服务，可以通过邮箱联系我 [xxwhite@foxmail.com](mailto:xxwhite@foxmail.com)，算是商业服务，服务费用稍微有点高，仅仅是想玩玩的话就不用联系我了。

