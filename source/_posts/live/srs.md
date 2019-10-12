# Simple-RTMP-Server

> SRS定位是运营级的互联网直播服务器集群，追求更好的概念完整性和最简单实现的代码。

SRS是一位重量级的选手，商业级的直播解决方案，但是使用起来也不是很麻烦。

项目地址：[https://github.com/ossrs/srs.git](https://github.com/ossrs/srs.git)


## 编译安装

```
git clone https://github.com/ossrs/srs.git -b master --depth 1
```

> 文件比较大，速度也会比较慢，请耐心等待

```
cd srs/trunk

./configure
make
```

## 运行

```
./objs/srs -c conf/srs.conf
```

如果确认运行没啥问题，那就测试一下推流吧，和前面Nginx打测试方法基本上一样