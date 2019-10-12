# 编译

> Linux系统已编译完成，可直接下载使用，其他系统需要自行编译。

## 开发环境

安装 `.net core 3.0 SDK`，详情请见

[https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/install](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/install)

## 源码

clone我的仓库

```
git clone https://github.com/MonoLogueChi/Dplayer.Danmaku -b master --depth 1
```
## 编译

> 因为项目开启`R2R`，所以不能交叉编译，想要交叉编译需要关闭`R2R`，具体方法为，在`Danmaku/Danmaku.csproj`中找到`<PublishReadyToRun>true</PublishReadyToRun>`，删掉这一行，或者改为`false`。

以下命令分别对应64位和32位系统。

### Linux下编译

```
dotnet publish -c Release-Linux64 -r linux-x64 --self-contained false --output publish
dotnet publish -c Release-Linux32 -r linux-x86 --self-contained false --output publish
```

使用Windows预编译配置也可以编译，但是不建议这样做，以后可能会改。如果使用Windows以便于配置编译，后面会使用Tcp连接，而不是Unix套接字。

```
dotnet publish -c Release-Win -r linux-x64 --self-contained false --output publish
dotnet publish -c Release-Win -r linux-x86 --self-contained false --output publish
```

### Windows下编译

```
dotnet publish -c Release-Win -r win-x64 --self-contained false --output publish
dotnet publish -c Release-Win -r win-x86 --self-contained false --output publish
```

### MacOS下编译

> 此部分内容仅仅上是在理论上可行，没有测试过。真的有人用Mac服务器吗？

```
dotnet publish -c Release-Win -r osx-x64 --self-contained false --output publish        #使用Tcp连接
dotnet publish -c Release-Linux64 -r osx-x64 --self-contained false --output publish    #使用Unix套接字连接
```
