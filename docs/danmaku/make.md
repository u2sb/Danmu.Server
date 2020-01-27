---
title: 编译
---

Linux环境已有预编译版本发布，无需自己编译，可以直接看[安装](install.md)部分，其他操作系统请自行编译。

Windows 环境下推荐使用 Visual Studio ， Linux 环境下可以按照下面的教程编译。

以下所有安装过程以 Debian10 为例，其他系统请自行判断是否需要其他步骤。


## 开发环境

参考 [安装 .NET Core SDK](https://docs.microsoft.com/zh-cn/dotnet/core/install/linux-package-manager-debian10#install-the-net-core-sdk)

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
sudo apt-get install dotnet-sdk-3.1
```

Windows 环境请参考[相关文档](https://docs.microsoft.com/zh-cn/dotnet/core/install/sdk?pivots=os-windows)

## 源码

```
git clone https://github.com/MonoLogueChi/Dplayer.Danmaku -b master --depth 1
cd Dplayer.Danmaku
```

## 编译

> 因为项目开启 R2R，所以不能交叉编译，想要交叉编译需要关闭 R2R，具体方法为，在 Danmaku/Danmaku.csproj 中找到 <PublishReadyToRun>true</PublishReadyToRun>，删掉这一行，或者改为 false。

### Linux下编译

```bash
dotnet publish -c Release-Linux64 -r linux-x64 --self-contained false --output publish

dotnet publish -c Release-Linux32 -r linux-x86 --self-contained false --output publish
```

### Windows下编译

```bash
dotnet publish -c Release-Win -r win-x64 --self-contained false --output publish

dotnet publish -c Release-Win -r win-x86 --self-contained false --output publish
```

编译后会在 `publish` 目录下生成二进制文件，具体参数请参考[相关文档](https://docs.microsoft.com/zh-cn/dotnet/core/deploying/deploy-with-cli)，看不懂没关系，微软的文档只有看的多了才能看得懂。
