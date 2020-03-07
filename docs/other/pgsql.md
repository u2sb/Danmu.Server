---
title: PostgreSQL安装
---

> 参考文档：[https://www.postgresql.org/download/](https://www.postgresql.org/download/)

以 Debian10 为例，其他系统请看官方文档，与 Debian10 类似，其他 Linux 系统请看官方文档。

## 添加 apt 源

```bash
sudo touch /etc/apt/sources.list.d/pgdg.list
sudo vim /etc/apt/sources.list.d/pgdg.list
```

写入内容

`/etc/apt/sources.list.d/pgdg.list`

```
deb http://apt.postgresql.org/pub/repos/apt/ buster-pgdg main
```

安装证书

```bash
wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
```

## 安装

```bash
sudo apt-get update
sudo apt-get install postgresql-12
```

## 设置 PostgreSQL 密码

```bash
sudo -u postgres psql
```

```sql
ALTER USER postgres WITH PASSWORD 'password';
```

## 创建新用户和数据库

```sql
CREATE ROLE "danmu" LOGIN PASSWORD 'danmu';

CREATE DATABASE "danmu"
WITH
  OWNER = "danmu"
  ENCODING = 'UTF8'
;
```

<ClientOnly>
  <Vssue title="PostgreSQL-Other | 弹幕服务器文档" />
</ClientOnly>
