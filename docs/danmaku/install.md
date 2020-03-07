---
title: 安装
---

## 环境要求

- Linux (推荐) 或 Windows 服务器
- asp.net core runtime 3.1
- 数据库 (PostgreSQL，MySQL 或 SQLite)
- Web 服务器 (Nginx, Apache, Caddy 等)
- 进程守护工具 (PM2, Supervisor 等)

## 安装 .net Core

:::tip 提示
使用独立部署方式无需安装 `asp .net core runtime`，相关信息见[安装程序](#安装程序)
:::

以下所有安装过程以 Debian10 为例，其他系统请自行判断是否需要其他步骤。

参考 [安装 ASP.NET Core 运行时](https://docs.microsoft.com/zh-cn/dotnet/core/install/linux-package-manager-debian10#install-the-aspnet-core-runtime)

```bash
wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg
sudo mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/
wget -q https://packages.microsoft.com/config/debian/10/prod.list
sudo mv prod.list /etc/apt/sources.list.d/microsoft-prod.list
sudo chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg
sudo chown root:root /etc/apt/sources.list.d/microsoft-prod.list

sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install aspnetcore-runtime-3.1
```

## 数据库

目前数据库支持情况：

- [x] PostgreSQL (10 或更高)
- [x] ~~MySQL (8.0 或更高)~~
- [ ] SQLite
- [ ] ~~SQLServer (计划中，无限期)~~

数据库相关设置，可以参考[其他文档](/other/)。

**推荐使用 PostgreSQL**，程序开发就是以 PostgreSQL 为主，SQLite 做兼容性测试，MySQL 不能保证功能 100% 可用。

## 安装程序

下载编译好的[二进制文件](https://github.com/MonoLogueChi/Danmu.Server/releases)，Windows 系统或其他 Linux 系统需要自行[编译](make.md)。

编译文件说明：

- `arm` : arm 平台
- `r2r` : ReadyToRun 编译选项，预先 AOT 编译，启动更快，但是二进制文件更大，推荐使用
- `scd` : 独立部署，无需额外安装 `asp .net core runtime`，开箱即用

下载后解压，或编译后拷贝到合适的位置，修改配置文件 appsetting.json

```bash
tar -xvf linux64.tar.xz
cd Danmaku
vim appsetting.json
```

### 配置文件解释

```json appsetting.json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "KestrelSettings": {
    "Port": [5000],
    "UnixSocketPath": ["/tmp/danmu.sock"]
  },
  "WithOrigins": ["*"],
  "LiveWithOrigins": [
    "http://localhost",
    "http://localhost:4000",
    "https://doc.video.xwhite.studio"
  ],
  "AdminWithOrigins": ["http://localhost:5000"],
  "DanmuSql": {
    "Sql": 0,
    "Host": "127.0.0.1",
    "Port": 0,
    "UserName": "danmaku",
    "PassWord": "danmaku",
    "DataBase": "danmaku",
    "PoolSize": 16
  },
  "Admin": {
    "User": "MonoLogueChi",
    "Password": "000000",
    "MaxAge": 60
  },
  "BCookie": ""
}
```

具体解释一下

- Logging: 无需更改
- KestrelSettings:
  - Port: `string[]` 程序运行端口
  - UnixSocketPath: `string[]` Unix 域套接字地址
- WithOrigins: `string[]` 允许跨域地址，可以使用通配符匹配
- LiveWithOrigins: `string[]` 直播弹幕服务允许跨域地址，不可以使用通配符匹配
- AdminWithOrigins: `string[]` 管理地址跨域，不做二次开发不建议配置，不可以使用通配符匹配
- DanmakuSQL:
  - Sql: `enum` 使用数据库类型，填写编号(`int`)或名称(`string`)，0 - PostgreSQL、~~1 -SQLite~~
  - Host: `string` 数据库连接地址
  - Port: `int` 数据库连接端口，0 为默认端口
  - UserName: `string` 数据库连接用户名，SQLite 填写无效
  - PassWord: `string` 数据库连接密码，SQLite 填写无效
  - DataBase: `string` 连接数据库
  - PoolSize: `int` 连接池
- Admin:
  - User: `string` 管理后台用户名
  - Password: `string` 管理后台密码
  - MaxAge: `int` 登录有效时间，分钟
- BCookie: `string` BiliBili Cookie，仅用于历史弹幕获取，不需要历史弹幕不需要填写

### 获取 BCookie 的方法

- 首先登录 BiliBili，建议先退出再重新登录，这样可以延长有效期；
- 访问任意弹幕相关的网站，例如 [https://api.bilibili.com/x/v2/dm/history?type=1&oid=1176840&date=2019-07-26](https://api.bilibili.com/x/v2/dm/history?type=1&oid=1176840&date=2019-07-26)
- F12 F5，在 Network 选项卡中找到相应的请求，复制 Cookie 出来；
- 当 Cookie 过期以后，再重复上面的步骤，填写新的 Cookie 进去。

![](@img/danmaku/cookie.png)

> PS: 当你登录账号的电脑上退出登录，或者登录过期的时候，你就需要重新填写 Cookie 进去了，填写完新的 Cookie 别忘了重启一下程序。

## 运行测试

```bash
./Danmaku
or
dotnet Danmaku.dll
```

检查是否能够正常运行，数据库是否正常创建。

## 其他配置

推荐使用 [Supervisor](http://www.supervisord.org/) + [Caddy(v1)](https://caddyserver.com/v1/)

### Caddy

下载 caddy，需要插件

- http.supervisor
- supervisor

下载以后放在一个自己认为合适的位置，创建配置文件

```bash
mkdir config
touch config/xxxxx.caddyfile
```

修改配置文件

```
https://danmu.u2sb.top {
    gzip
    tls youmail@xxx.com
    supervisor {
        command ./Danmaku
        dir /www/dotnet/Danmaku/
        redirect_stdout stdout
        redirect_stderr stderr
    }
    proxy / unix:/tmp/danmu.sock {
        websocket
        transparent
    }
}
```

根据你自己的情况修改配置文件。

### Supervisor

#### 安装

```bash
sudo apt install supervisor
```

#### 添加配置文件

```bash
sudo touch /etc/supervisor/conf.d/caddy.conf
sudo vim /etc/supervisor/conf.d/caddy.conf
```

```ini caddy.conf
[program:caddy]
command = /www/caddy/caddy -conf /www/caddy/conf/*.caddyfile -agree -quic
directory = /www/caddy/
environment = DNSPOD_API_KEY=""
user = www
stopsignal = INT
autostart = true
autorestart = true
startsecs = 5
stderr_logfile = /www/caddy/log/error.log
stdout_logfile = /www/caddy/log/out.log
```

根据自己的情况创建配置文件，具体关于 Supervisor 的使用请自行百度。

### 使用Nginx

Nginx配置较caddy稍麻烦，下面只是一个示例

```nginx
location /
{
    proxy_pass unix:/tmp/danmu.sock;
    proxy_set_header Host $host;
    proxy_set_header X-Real-IP $remote_addr;
    proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
    proxy_set_header REMOTE-HOST $remote_addr;
    
    proxy_connect_timeout 15s;
    proxy_read_timeout 1800s;
    proxy_send_timeout 20s;
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection "upgrade";

    add_header X-Cache $upstream_cache_status;
}
```

## Dplayer 的简单应用

```html
<link
  rel="stylesheet"
  href="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.css"
/>
<script src="https://cdn.jsdelivr.net/npm/dplayer/dist/DPlayer.min.js"></script>
<div id="dplayer"></div>
<script>
  const dp = new DPlayer({
    container: document.getElementById("dplayer"),
    video: {
      url: "demo.mp4"
    },
    subtitle: {
      url: "demo.vtt"
    },
    danmaku: {
      id: "视频的ID，建议使用视频Hash值，例如CRC64",
      api: "https://danmu.u2sb.top/api/danmu/dplayer/", //你自己的api
      addition: [
        "https://danmu.u2sb.top/api/danmu/dplayer/v3/bilibili/?cid=73636868"
      ] //解析BiliBili弹幕
    }
  });

  //修复手机横屏问题
  dp.on("fullscreen", function() {
    if (
      /Android|webOS|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)
    ) {
      screen.orientation.lock("landscape");
    }
  });
</script>
```

<ClientOnly>
  <Vssue title="安装-Danmaku | 弹幕服务器文档" />
</ClientOnly>
