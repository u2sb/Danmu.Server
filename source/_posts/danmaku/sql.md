# 数据库安装

弹幕后端支持多种数据库，请选择最适合自己的安装。

## PostgreSQL

参考文档：[https://www.postgresql.org/download/](https://www.postgresql.org/download/)

以Debian10为例，其他系统请看官方文档，与Debian10类似，~~其他Linux系统可以选择编译安装~~(相关文档已删除)  
```
touch /etc/apt/sources.list.d/pgdg.list
vim /etc/apt/sources.list.d/pgdg.list
```

写入内容  
```
deb http://apt.postgresql.org/pub/repos/apt/ buster-pgdg main
```

安装证书  
```
wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
```

安装  
```
sudo apt-get update
apt-get install postgresql-11
```

设置PostgreSQL密码  
```
sudo -u postgres psql

ALTER USER postgres WITH PASSWORD 'password';
```

## MySQL

参考文档：[https://dev.mysql.com/downloads/](https://dev.mysql.com/downloads/)

以Debian10为例，其他系统请看官方文档，与Debian10类似，~~其他Linux系统可以选择编译安装~~(相关文档已删除)  

下载 [https://dev.mysql.com/downloads/repo/apt/](https://dev.mysql.com/downloads/repo/apt/) 页面上的文件

参考 [https://dev.mysql.com/doc/mysql-apt-repo-quick-guide/en/](https://dev.mysql.com/doc/mysql-apt-repo-quick-guide/en/)

> 需要注意，安装时请选择兼容旧的密码验证策略（其实程序已经支持新的密码验证策略了，这样做是防止其他程序出问题了有人找我背锅）


## SQLite

无需特殊配置，请确保程序所在目录对当前用户有读写权限。


## SQLSwever

暂不支持

> 想要适配也非常简单，只是暂时没有那么多精力去测试了，开发不难但是测试真的烦
