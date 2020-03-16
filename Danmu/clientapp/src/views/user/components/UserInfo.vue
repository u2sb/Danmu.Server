<template>
    <el-form
            ref="form"
            v-loading="loading"
            :model="form"
            :rules="rules"
            size="small"
            label-position="right"
            label-width="100px"
            status-icon
            style="max-width: 600px"
    >
        <el-form-item label="用户名：" prop="name">
            <el-input v-model="form.name" :maxlength="100"/>
        </el-form-item>
        <el-form-item label="邮箱：" prop="email">
            <el-input v-model="form.email" :maxlength="100"/>
        </el-form-item>
        <el-form-item label="手机：" prop="phoneNumber">
            <el-input v-model="form.phoneNumber" :maxlength="20"/>
        </el-form-item>
        <el-form-item label="用户角色：">
            <el-input :value="form.role" readonly/>
        </el-form-item>
        <el-form-item label="创建时间：">
            <el-input :value="form.createTime" readonly/>
        </el-form-item>
        <el-form-item label="修改时间：">
            <el-input :value="form.updateTime" readonly/>
        </el-form-item>
        <el-form-item>
            <el-button :loading="operating" type="primary" size="small" @click="submit">提 交</el-button>
        </el-form-item>
    </el-form>
</template>

<script>
    import { mapState } from 'vuex'
    import { getUserInfo, changeInfo } from '@/api/admin/account'
    import { mergeObj } from '@/utils'
    import { getLocalTime } from '@/utils/date'
    import { elSuccess } from '@/utils/message'

    export default {
        name: 'UserInfo',
        data() {
            return {
                loading: false,
                operating: false,
                form: {
                    id: null,
                    name: null,
                    email: null,
                    phoneNumber: null,
                    role: null,
                    createTime: null,
                    updateTime: null
                },
                rules: {
                    name: [{ required: true, message: '用户名不能为空', trigger: 'change' }],
                    email: [{ type: 'email', message: '邮箱格式有误', trigger: 'change' }]
                }
            }
        },
        computed: {
            ...mapState('user', {
                id: state => state.id
            })
        },
        methods: {
            init() {
                this.loading = true
                getUserInfo(this.id)
                    .then(data => {
                        data.createTime = getLocalTime(data.createTime)
                        data.updateTime = getLocalTime(data.updateTime)
                        mergeObj(this.form, data)
                    })
                    .finally(() => this.loading = false)
            },
            submit() {
                if (this.loading || this.operating) return
                this.$refs.form.validate(v => {
                    if (!v) return
                    this.operating = true
                    const { id, name, email, phoneNumber } = this.form
                    changeInfo({ id, name, email, phoneNumber })
                        .then(() => {
                            elSuccess('提交成功')
                            this.$store.commit('user/setName', name)
                            this.init()
                        })
                        .finally(() => this.operating = false)
                })
            }
        },
        mounted() {
            this.init()
        }
    }
</script>
