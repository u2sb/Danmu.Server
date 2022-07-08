<template>
  <a-layout has-sider>
    <a-layout-sider
      :style="{
        overflow: 'auto',
        height: '100vh',
        position: 'fixed',
        left: 0,
        top: 0,
        bottom: 0,
      }"
    >
      <div class="logo" />
      <a-menu v-model:selectedKeys="selectedKeys" theme="dark" mode="inline">
        <a-menu-item key="1">
          <router-link to="/">
            <home-outlined /> <span class="nav-text">主页</span>
          </router-link>
        </a-menu-item>
        <a-menu-item key="2">
          <router-link to="/list">
            <unordered-list-outlined /> <span class="nav-text">弹幕列表</span>
          </router-link>
        </a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout :style="{ marginLeft: '200px' }">
      <a-layout-header :style="{ background: '#fff', padding: 0 }" />
      <a-layout-content :style="{ margin: '24px 16px 0', overflow: 'initial' }">
        <div
          :style="{
            padding: '24px',
            background: '#fff',
            textAlign: 'center',
            minHeight: '85vh',
          }"
        >
          <router-view />
        </div>
      </a-layout-content>
      <a-layout-footer :style="{ textAlign: 'center' }">
        <a href="https://github.com/u2sb/Danmu.Server" target="_blank">
          Danmu.Server </a
        >@2022
      </a-layout-footer>
    </a-layout>
  </a-layout>
</template>
<script lang="ts">
import { HomeOutlined, UnorderedListOutlined } from "@ant-design/icons-vue";
import { defineComponent, ref } from "vue";
export default defineComponent({
  components: {
    HomeOutlined,
    UnorderedListOutlined,
  },
  setup() {
    return {
      selectedKeys: ref<string[]>(["4"]),
    };
  },
});
</script>
<style>
#components-layout-demo-fixed-sider .logo {
  height: 32px;
  background: rgba(255, 255, 255, 0.2);
  margin: 16px;
}
.site-layout .site-layout-background {
  background: #fff;
}

[data-theme="dark"] .site-layout .site-layout-background {
  background: #141414;
}
</style>