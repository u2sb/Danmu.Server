---
title: 编译
---

正常情况下[下载](https://github.com/MonoLogueChi/Danmu.Server/releases)编译完成的版本即可，无需自己编译，可以直接看[安装](install.md)部分，如有特殊需要，请参考以下文本。

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

下载最新的 [tag](https://github.com/MonoLogueChi/Danmu.Server/releases/latest)，然后解压源码

:::warning 警告
不要直接从 GitHub 上 Clone 源码，未进入 tag 的源码都是正在开发中的。
:::

## 编译

先生成静态文件

```bash
cd /path/to/clientapp/
npm install
npm run build
```

### 编译 Linux 版本

```bash
dotnet publish -c Release-Linux64 -r linux-x64 --self-contained false --output publish
```

### 编译 Windows 版本

```bash
dotnet publish -c Release-Win -r win-x64 --self-contained false --output publish

dotnet publish -c Release-Win -r win-x86 --self-contained false --output publish
```

编译后会在 `publish` 目录下生成二进制文件，具体参数请参考[相关文档](https://docs.microsoft.com/zh-cn/dotnet/core/deploying/deploy-with-cli)，看不懂没关系，微软的文档只有看的多了才能看得懂。

<ClientOnly>
  <Vssue title="编译-Danmaku | 弹幕服务器文档" />
</ClientOnly>
