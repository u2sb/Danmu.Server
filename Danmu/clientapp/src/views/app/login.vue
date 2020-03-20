<template>
    <div class="login-page">
        <div class="login-container">
            <div class="top">
                <div class="title">
                    <img src="/favicon.png">
                    <span>Danmu.Server</span>
                </div>
                <div class="intro">
                    一个开源的弹幕服务器
                </div>
            </div>
            <el-form ref="form" :model="form" :rules="rules" label-position="left">
                <el-form-item prop="name">
                    <el-input
                            ref="name"
                            v-model="form.name"
                            :maxlength="16"
                            placeholder="请输入用户名"
                            type="text"
                    >
                        <i slot="prefix" class="el-input__icon el-icon-user"/>
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input
                            ref="password"
                            v-model="form.password"
                            type="password"
                            :maxlength="100"
                            placeholder="请输入密码"
                            @keyup.enter.native="login"
                    >
                        <i slot="prefix" class="el-input__icon el-icon-lock"/>
                    </el-input>
                </el-form-item>
                <el-button :loading="loading" style="width: 100%" type="primary" @click="login">登录</el-button>
            </el-form>
        </div>
    </div>
</template>

<script>
    import { routerMode } from '@/config'
    import { isEmpty } from '@/utils'
    import md5 from 'js-md5'
    import { elSuccess } from '@/utils/message'

    export default {
        name: 'login',
        data() {
            return {
                form: {
                    name: '',
                    password: ''
                },
                rules: {
                    name: [{ required: true, message: '请输入用户名', trigger: 'change' }],
                    password: [
                        { required: true, message: '请输入密码', trigger: 'change' },
                        { min: 6, message: '密码长度不能低于6位', trigger: 'change' }
                    ]
                },
                loading: false
            }
        },
        methods: {
            login() {
                if (this.loading) return
                this.$refs.form.validate(valid => {
                    if (!valid) return
                    this.loading = true
                    this.$store.dispatch('user/login', { name: this.form.name, password: md5(this.form.password) })
                        .then(() => this.success())
                        .catch(() => this.loading = false)
                })
            },
            success() {
                elSuccess('登录成功')
                let redirect = this.$route.query.redirect || '/'
                window.location.href = routerMode === 'history' ? redirect : '/#' + redirect
            }
        },
        mounted() {
            if (isEmpty(this.form.name)) this.$refs.name.focus()
            else this.$refs.password.focus()
        },
        beforeDestroy() {
            this.$message.closeAll()
        }
    }
</script>

<style lang="scss" scoped>
    @import "~@/assets/styles/login";
</style>
