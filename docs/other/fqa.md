---
title: FQA
---

## 为什么是PostgreSQL？

最初是使用MySQL的，但是后面发现pgsql更适合这个项目。

## 为什么不兼容其他数据库？

首先是SQLite，这也是我一直想要兼容的数据库，但是重构以后插入数据时涉及到了json查询，SQLite不支持json，所以这里会比较麻烦，以后会想办法解决的。

其次是MySQL，兼容MySQL的成本不高，只需要几句代码就可以做到兼容，但是比较大的问题是测试，我没有充足的时间去测试MySQL是否能正常运行，如果有人愿意帮我测试的话，欢迎联系我。

接下来是SQL Server，兼容成本也是相当的低，而且SQL Server的性能也是非常不错的，以后有时间的话会支持的。

<ClientOnly>
  <Vssue title="FQA-Other | 弹幕服务器文档" />
</ClientOnly>