<template>
  <div id="login" class="big-div">
    <el-container>
      <el-header>
        <div class="header-box">
          <div style="float: right;">
            <p>
              <router-link :to="'/'" class="header-link">主页</router-link>
              <a href="https://dandoc.u2sb.top" target="_blank" class="header-link">文档</a>
            </p>
          </div>
        </div>
      </el-header>
      <el-main>
        <el-col :span="12">
          <div>
            <p>Danmaku，一个开源弹幕服务器</p>
          </div>
        </el-col>
        <el-col :span="12">
          <div style="float: right;">
            <div class="p-box">
              <el-form ref="form" :model="form" :rules="rules" label-width="80px">
                <el-form-item label="用户名" prop="name">
                  <el-input v-model="form.name" placeholder="请输入用户名"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="password" style="margin-top: 30px;">
                  <el-input v-model="form.password" placeholder="请输入密码" show-password></el-input>
                </el-form-item>
                <el-form-item style="float: right; margin-top: 30px;">
                  <el-button type="primary" @click="onSubmit('form')" style="width: 100px">登录</el-button>
                  <el-button @click="resetForm('form')" style="width: 100px">重置</el-button>
                </el-form-item>
              </el-form>
            </div>
          </div>
        </el-col>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import crypto from 'crypto'
import {Notification} from 'element-ui'

export default {
  name: 'login',
  data() {
    return {
      form: {
        name: '',
        password: ''
      },
      rules: {
        name: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
        password: [{ required: true, message: '请输入密码', trigger: 'blur' }]
      }
    }
  },
  methods: {
    onSubmit(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          let md5 = crypto.createHash('md5')
          let formData = {
            name: this.form.name,
            password: md5.update(this.form.password).digest('hex'),
            url: this.$route.query.url || '/'
          }
          window.console.log(formData)
          this.$http.post('/api/login', formData).then(res => {
            let dataObj = eval(res.data)
            if (dataObj.code === 0) {
              this.$router.push({ path: dataObj.url })
            } else {
              Notification.error({
                title: '错误',
                message: '用户名或密码错误',
                position: 'bottom-right'
              })
            }
          })
        } else {
          return false
        }
      })
    },
    resetForm(formName) {
      this.$refs[formName].resetFields()
    }
  }
}
</script>

<style scoped>
.big-div {
  background: url(/img/banner.png),
    linear-gradient(103deg, rgba(32, 157, 230, 1), rgba(55, 222, 242, 1));
  background-repeat: no-repeat, no-repeat;
  background-position: center bottom;
  min-height: 800px;
}

.header-box {
  height: 60px;
  width: 100%;
  border-bottom: 1px solid rgba(255, 255, 255, 0.3);
}

.header-link {
  margin-right: 30px;
  color: white;
  text-decoration: none;
}

.p-box {
  background: white;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
  border-radius: 5px;
  margin-right: 50px;
  margin-top: 100px;
  height: 250px;
  width: 350px;
  padding: 50px 20px 0px 10px;
}
</style>