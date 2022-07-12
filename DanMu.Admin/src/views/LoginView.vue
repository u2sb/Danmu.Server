<template>
  <div class="container">
    <div class="login-wrapper">
      <div class="header">Login</div>
      <div class="form-wrapper">
        <input
          type="text"
          name="username"
          placeholder="username"
          class="input-item"
          v-model="username"
        />
        <input
          type="password"
          name="password"
          placeholder="password"
          class="input-item"
          v-model="password"
        />
        <button class="btn" v-on:click="login">Login</button>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import api from "@/api/api.json";

export default defineComponent({
  data() {
    return {
      username: "",
      password: "",
      url: this.$route.query.url || this.$route.meta.url || "/",
    };
  },
  methods: {
    login() {
      fetch(api.login, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          username: this.username,
          password: this.password,
          url: this.url,
        }),
      })
        .then((res) => res.json())
        .then((res) => {
          if (res.code === 0 && res.data) {
            return res.data;
          }
        })
        .then((data) => {
          this.$router.push(data.url || "/");
        });
    },
  },
});
</script>


<style scoped>
* {
  margin: 0;
  padding: 0;
}
html {
  height: 100%;
}
body {
  height: 100%;
}
.container {
  height: 100%;
}
.login-wrapper {
  background-color: #fff;
  width: 358px;
  height: 588px;
  border-radius: 15px;
  padding: 0 50px;
  position: relative;
}
.header {
  font-size: 38px;
  font-weight: bold;
  text-align: center;
  line-height: 200px;
}
.input-item {
  display: block;
  width: 100%;
  margin-bottom: 20px;
  border: 0;
  padding: 10px;
  border-bottom: 1px solid rgb(128, 125, 125);
  font-size: 15px;
  outline: none;
}
.input-item:placeholder {
  text-transform: uppercase;
}
.btn {
  text-align: center;
  padding: 10px;
  width: 100%;
  margin-top: 40px;
  background-image: linear-gradient(to right, #a6c1ee, #fbc2eb);
  color: #fff;
  border-style: none;
}
a {
  text-decoration-line: none;
  color: #abc1ee;
}
</style>