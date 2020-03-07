---
title: API
---

## Dplayer

Api 非常简单，通信使用了`signalR`，封装成都比较高，具体可以参考 [livedanmaku.js](/js/livedanmaku.js)。

实际使用过程中无需了解 api，是要看 [简单使用](/live/install.md#简单使用) 就可以了。

## 其他播放器

通信使用了 `signaIR`，引入相关库后，只需要记住几个特定的方法就可以了，

```html
<script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr/dist/browser/signalr.min.js"></script>
```

```js
//弹幕服务器地址请自行搭建，我的公共服务器不允许跨域请求
var url = "https://danmu.u2sb.top/api/live/danmu";

//房间号，每个视频必须统一，而且房间号需要是唯一的，这是识别弹幕来自哪个视频唯一依据
var group = "cd30ae05-4ad5-5135-bd6d-337ac0de102e";

//弹幕发送用户，拓展使用，如果没有用户系统无需配置
var user = "user";

var connection = new signalR.HubConnectionBuilder().withUrl(url).build(); //初始化

//连接房间，初始化时完成
connection
  .start()
  .then(function() {
    connection.invoke("Connection", group).catch(err => console.error(err));
  })
  .catch(err => console.error(err));

//收到弹幕消息时触发
connection.on("ReceiveMessage", function(user, message) {
  onMessage(JSON.parse(message));
});

//向服务端发送弹幕，需要在发送弹幕时调用
function Send(mess) {
  connection
    .invoke("SendMessage", group, user, JSON.stringify(mess))
    .catch(err => console.error(err));
}

//收到消息时触发的事件，一般是向播放器发送弹幕
function onMessage(mess) {
  //下面是ArtPlayer的示例
  mess.time = art.currentTime;
  art.plugins.artplayerPluginDanmuku.emit(mess);
}

//下面是ArtPlayer发送弹幕的简单示例
function onClick() {
  var value = document.getElementById("dan").value;
  var mmm = {
    text: value,
    time: art.currentTime,
    color: "#fff",
    size: 25,
    border: false,
    mode: 0
  };
  Send(mmm);
  //需要注意，服务端只会发送其他播放器的弹幕，并不会返回当前播放器发送的弹幕
  //需要手动发送弹幕到播放器
  art.plugins.artplayerPluginDanmuku.emit(mmm);
}
```

<ClientOnly>
  <Vssue title="Api-Live | 弹幕服务器文档" />
</ClientOnly>
