<template>
  <div id="admin">
    <el-container direction="vertical">
      <el-header>
        <div class="header-box">
          <el-row type="flex" justify="end">
            <el-col :xs="0" :sm="2" :md="5" :lg="7" :xl="9">
              <div>
                <p>
                  <router-link :to="'/'" style="text-decoration: none">
                    <el-link>Danmu</el-link>
                  </router-link>
                </p>
              </div>
            </el-col>
            <el-col :span="8">
              <div v-show="showVidSelect" style="float: right; margin-top: 8px">
                <el-select v-model="vid" filterable allow-create placeholder="请输入vid">
                  <el-option v-for="item in vids" :key="item" :label="item" :value="item"></el-option>
                </el-select>
              </div>
              <p></p>
            </el-col>
            <el-col :xs="20" :sm="14" :md="11" :lg="9" :xl="7">
              <div style="float: right;">
                <el-menu
                  :default-active="activeIndex"
                  class="el-menu-demo"
                  mode="horizontal"
                  @select="handleSelect"
                >
                  <el-menu-item index="1">所有弹幕</el-menu-item>
                  <el-submenu index="2">
                    <template slot="title">弹幕检索</template>
                    <el-menu-item index="2-1">视频检索</el-menu-item>
                    <el-menu-item index="2-2" disabled>时间检索</el-menu-item>
                    <el-menu-item index="2-3" disabled>复杂检索</el-menu-item>
                  </el-submenu>
                  <el-menu-item index="3" disabled>用户中心</el-menu-item>
                  <el-menu-item index="4">
                    <el-link href="/api/admin/logout" :underline="false">退出登录</el-link>
                  </el-menu-item>
                </el-menu>
              </div>
            </el-col>
          </el-row>
        </div>
      </el-header>
      <el-main>
        <router-view></router-view>
      </el-main>
      <el-footer>
        <div class="footer-box">
          <div style="float: right;">
            <p>
              <el-link
                href="https://github.com/MonoLogueChi/Dplayer.danmu"
                target="_blank"
                :underline="false"
              >问题反馈</el-link>
            </p>
          </div>
        </div>
      </el-footer>
    </el-container>
  </div>
</template>


<script>
export default {
  name: 'admin',
  data() {
    return {
      activeIndex: '1',
      menuRoutes: {
        '1': { name: 'danmulist' },
        '2-1': { name: 'danmulistbyvid', params: { vid: '' } },
        '2-2': { name: 'danmulistbyvid', params: { vid: '' } },
        '2-3': { name: 'danmulistbyvid', params: { vid: '' } }
      }
    }
  },
  computed: {
    showVidSelect: {
      get() {
        return this.$store.state.admin.showVidSelect
      },
      set(value) {
        this.$store.commit('changeShowVidSelect', value)
      }
    },
    vid: {
      get() {
        return this.$store.state.admin.vid
      },
      set(value) {
        this.$store.commit('changeVid', value)
      }
    },
    vids: {
      get() {
        return this.$store.state.admin.vids
      },
      set(value) {
        this.$store.commit('changeVids', value)
      }
    }
  },
  methods: {
    handleSelect(key) {
      let route = this.menuRoutes[key]
      if (route && this.$route.name != route.name) {
        this.$router.push(route)
      }
    },
    QueryByVid() {}
  }
}
</script>

<style lang="scss">
#admin {
  .el-header {
    background-color: #fff;
    color: #333;
  }

  .header-box {
    @extend .el-header;
    height: 60px;
    border-bottom: 1px solid #dcdfe6;
  }

  .footer-box {
    border-top: 1px solid #dcdfe6;
  }

  .el-aside {
    background-color: #d3dce6;
    color: #333;
    text-align: center;
    line-height: 200px;
  }

  .el-main {
    color: #333;
    text-align: center;
    min-height: 80vh;
  }

  body > .el-container {
    margin-bottom: 40px;
  }

  .el-container:nth-child(5) .el-container:nth-child(6) {
    .el-aside {
      line-height: 260px;
    }
  }

  .el-container:nth-child(7) {
    .el-aside {
      line-height: 320px;
    }
  }

  .el-table__body {
    td {
      padding: 6px 0px;
    }
  }
}
</style>