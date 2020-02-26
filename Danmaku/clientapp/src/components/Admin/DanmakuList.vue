<template>
  <div id="danmakulist">
    <p>这里是弹幕列表</p>
    <p>
      <a href="https://dandoc.u2sb.top" target="_blank">文档</a>
    </p>
    <p>
      <router-link :to="'/'">管理</router-link>
    </p>
    <p>
      <router-link :to="'/login'">登录</router-link>
    </p>
    <p>
      <a href="/api/logout">退出</a>
    </p>
    <el-button type="primary" @click="onSubmit()" style="width: 100px">登录</el-button>
  </div>
</template>


<script>
export default {
  name: 'danmakulist',
  methods: {
    onSubmit() {
      this.$http.get('/api/danmakulist').then(res => {
        window.console.log(res.data)
      })
    }
  }
}
</script>