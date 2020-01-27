---
title: PostgreSQL
---

> 参考文档：[https://www.postgresql.org/download/](https://www.postgresql.org/download/)

以 Debian10 为例，其他系统请看官方文档，与 Debian10 类似，其他 Linux 系统请看官方文档。

## 添加apt源

```bash
touch /etc/apt/sources.list.d/pgdg.list
vim /etc/apt/sources.list.d/pgdg.list
```

写入内容

```apt /etc/apt/sources.list.d/pgdg.list
deb http://apt.postgresql.org/pub/repos/apt/ buster-pgdg main
```

安装证书

```bash
wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
```

## 安装

```bash
sudo apt-get update
apt-get install postgresql-11
```

## 设置 PostgreSQL 密码

```bash
sudo -u postgres psql

ALTER USER postgres WITH PASSWORD 'password';
```

剩下就是创建新的用户和数据库，到这一步自行百度就可以了。