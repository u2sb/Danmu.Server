# 部署

部署以Linux环境为例介绍，Windows环境类似。

## 运行环境

- 系统要求： Linux服务器(推荐)或Windows服务器
- .net core runtime 3.0

参考资料：[https://dotnet.microsoft.com/download/dotnet-core/3.0/runtime](https://dotnet.microsoft.com/download/dotnet-core/3.0/runtime)

## 数据库

数据库支持情况：

- [x] PostgreSQL (10或更高)
- [x] MySQL (8.0或更高)
- [x] SQLite
- [ ] SQLServer (计划中)

具体数据库安装方法可以参考 [数据库安装](sql.html)。

推荐使用PostgreSQL，这样可以获得最好的性能。

## 配置文件

64位Linux系统可下载编译好的[二进制文件](https://github.com/MonoLogueChi/Dplayer.Danmaku/releases)，Windows系统或其他Linux系统需要自行[编译](make.html)。

下载解压后修改配置文件`appsetting.json`

### Logging

无需修改

### Urls

- 类型：`string`
- 解释：url，需要多个时使用`;`分割，预编译条件为`Linux`时不生效 [^Urls](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.0#endpoint-configuration)

### AllowedHosts

- 类型：`string`
- 解释：主机筛选，正常情况下无需修改，多个Hosts之间用`;`分割 [^AllowedHosts](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.0#host-filtering)

### WithOrigins

- 类型：`string[]`
- 解释：允许跨域的域名，视频弹幕服务器可以使用`*`匹配。

### LiveWithOrigins

- 类型：`string[]`
- 解释：允许跨域的域名，直播弹幕服务器不能使用`*`匹配

### DanmakuSQL

- 类型：`class`
- 解释：数据库相关配置

具体解释：  
- Sql：`int`或`string`，表示数据库类型，对应关系：
  - 0 - "PostgreSQL"
  - 1 - "MySQL"
  - 2 - "SQLite"
  - 3 - "SQLServer"
- Host：`string`，数据库连接地址
- Port：`int`，数据库连接端口，使用默认端口时可填写0
- UserName：`string`，数据库连接用户
- PassWord：`string`，数据库连接密码
- DataBase：`string`，数据库名称
- VerSion：`string`，数据库版本号，仅使用MySQL时生效

### Admin

- 类型：`class`
- 解释：后台管理员相关配置

具体解释：  
- User：`string`，登录用户名
- Password：`string`，登录密码

### BCookie

- 类型：`string`
- 解释：BiliBili的Cookie，用于获取历史弹幕，登录过期时需要重新填写

获取Cookie方法：

- 首先登录BiliBili，建议先退出再重新登录，这样可以延长有效期;
- 访问任意弹幕相关的网站，例如 [https://api.bilibili.com/x/v2/dm/history?type=1&oid=1176840&date=2019-07-26](https://api.bilibili.com/x/v2/dm/history?type=1&oid=1176840&date=2019-07-26);
- `F12` `F5`，在`Network`选项卡中找到相应的请求，复制Cookie出来;
- 当Cookie过期以后，再重复上面的步骤，填写新的Cookie进去。

![获取Cookie](/images/docs/cookie.png)

> PS: 当你登录账号的电脑上退出登录，或者登录过期的时候，你就需要重新填写Cookie进去了，填写完新的Cookie别忘了重启一下程序。


## 运行测试

```
./Danmaku
or
dotnet Danmaku.dll
```

检查是否能够正常运行，数据库是否正常创建。

## 进程守护

进程守护工具有很多，仅以pm2为例介绍

### 配置文件

```yaml danmaku.yml
apps:
  - script: Danmaku
    name: danmaku
    cwd: /www/dotnet/danmaku  #解压的根目录
    autorestart: true
    error_file: /www/dotnet/log/error.log  #错误日志
    out_file: /www/dotnet/log/out.log  #输出日志
    merge_logs: true
```

### 使用

安装nodejs和pm2

node安装方法可以看[https://nodejs.org/en/download/package-manager/](https://nodejs.org/en/download/package-manager/)

安装pm2
```
sudo npm install pm2 -g
```

启动  
```
pm2 start danmaku.yml
```

保存  
```
pm2 save
```

开机自启
```
sudo pm2 startup
```

## 反向代理

配置反向代理服务器，这里以Caddy和Nginx为例

示例配置文件仅仅是关键配置，详细配置请自行补充

### Caddy

```
https://example.com {
    proxy / unix:/tmp/dplayer.danmaku.sock {
        websocket
        transparent
    }
}
```

### Nginx

```
location /
{
    proxy_pass unix:/tmp/dplayer.danmaku.sock;
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
