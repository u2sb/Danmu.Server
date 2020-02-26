<template>
  <div id="home">
    <p>这里是弹幕服务器主页</p>
    <p>
      <a href="https://dandoc.u2sb.top" target="_blank">文档</a>
    </p>
    <p>
      <router-link :to="'/admin'">管理</router-link>
    </p>
    <p>
      <router-link :to="'/login'">登录</router-link>
    </p>
    <p>
      <a href="/api/logout">退出</a>
    </p>
  </div>
</template>


<script>
export default {
  name: 'home'
}
</script>