---
title: FQA
---

# FQA

## 为什么是 PostgreSQL？

最初是使用 MySQL 的，但是后面发现 pgsql 更适合这个项目。

## 为什么不兼容其他数据库？

首先是 SQLite，这也是我一直想要兼容的数据库，但是重构以后插入数据时涉及到了 json 查询，SQLite 不支持 json，所以这里会比较麻烦，以后会想办法解决的。

其次是 MySQL，兼容 MySQL 的成本不高，只需要几句代码就可以做到兼容，但是比较大的问题是测试，我没有充足的时间去测试 MySQL 是否能正常运行，如果有人愿意帮我测试的话，欢迎联系我。

接下来是 SQL Server，兼容成本也是相当的低，而且 SQL Server 的性能也是非常不错的，以后有时间的话会支持的。

## 使用 Nginx 反代时报错？

关于Nginx反代的问题，不是一句两句就可以说清楚的，欢迎加入QQ群讨论。



## 反馈

[QQ 群](https://shang.qq.com/wpa/qunwpa?idkey=f2a6dba8d97899969101dd29210d972f04febd0ff8cf08ed50dd27790f23c9a9) 159891059

![](@img/danmu/qq.png =300x300)

<ClientOnly>
  <Vssue title="FQA-Other | 弹幕服务器文档" />
</ClientOnly>
