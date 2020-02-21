<template>
  <div id='home'>
      <p>这里是弹幕服务器主页</p>
      <p><a href="https://dandoc.u2sb.top">文档</a></p>
      <p><router-link :to="'/'">管理</router-link></p>
      <p><router-link :to="'/login'">登录</router-link></p>
  </div>
</template>


<script>
export default {
  name: 'home'
}
</script>