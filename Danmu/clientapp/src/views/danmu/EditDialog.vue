<template>
    <dialog-form :loading="loading" title="编辑弹幕" :value="value" @close="cancel" @open="open">
        <el-form
                ref="form"
                :model="form"
                :rules="rules"
                label-position="right"
                label-width="100px"
                status-icon
        >
            <el-row :gutter="20">
                <dialog-form-item label="弹幕ID：">
                    <el-input :value="form.id" readonly/>
                </dialog-form-item>
                <dialog-form-item label="视频ID：">
                    <el-input :value="form.vid" readonly/>
                </dialog-form-item>
                <dialog-form-item label="referer：">
                    <el-input :value="temp.referer" readonly/>
                </dialog-form-item>
                <dialog-form-item label="用 户：">
                    <el-input :value="temp.author" readonly/>
                </dialog-form-item>
                <dialog-form-item label="ip：">
                    <el-input :value="temp.ip" readonly/>
                </dialog-form-item>
                <dialog-form-item label="创建时间：">
                    <el-input :value="temp.createTime" readonly/>
                </dialog-form-item>
                <dialog-form-item label="修改时间：">
                    <el-input :value="temp.updateTime" readonly/>
                </dialog-form-item>
                <dialog-form-item label="弹幕时间：" prop="data.time">
                    <el-input v-model="form.data.time"/>
                </dialog-form-item>
                <dialog-form-item label="弹幕类型：" prop="data.mode">
                    <el-select v-model="form.data.mode" clearable @clear="form.data.mode=null">
                        <el-option v-for="{value,label} in danmuModes" :key="value" :value="value" :label="label"/>
                    </el-select>
                </dialog-form-item>
                <dialog-form-item label="弹幕大小：" prop="data.size">
                    <el-input-number v-model="form.data.size" :min="1" :step="1" step-strictly/>
                </dialog-form-item>
                <dialog-form-item label="弹幕颜色：" prop="data.color">
                    <el-color-picker v-model="form.data.color" :predefine="predefineColors"/>
                </dialog-form-item>
                <dialog-form-item label="是否删除：" prop="isDelete">
                    <el-radio-group v-model="form.isDelete">
                        <el-radio :label="true">是</el-radio>
                        <el-radio :label="false">否</el-radio>
                    </el-radio-group>
                </dialog-form-item>
                <dialog-form-item label="弹幕内容：" prop="data.text" full>
                    <el-input type="textarea" v-model="form.data.text"/>
                </dialog-form-item>
            </el-row>
        </el-form>
        <template v-slot:footer>
            <el-button plain size="small" @click="cancel">取 消</el-button>
            <el-button size="small" type="primary" @click="confirm">确 定</el-button>
        </template>
    </dialog-form>
</template>

<script>
    import DialogForm from '@/components/DialogForm'
    import DialogFormItem from '@/components/DialogForm/DialogFormItem'
    import { danmuModes, predefineColors } from './constant'
    import { isEmpty, mergeObj, resetObj } from '@/utils'
    import { elAlert } from '@/utils/message'
    import { getById } from '@/api/admin/danmu'
    import { getLocalTime } from '@/utils/date'
    import { update } from '@/api/admin/danmu'

    export default {
        name: 'EditDialog',
        components: { DialogForm, DialogFormItem },
        props: {
            value: Boolean,
            type: { type: String, default: 'edit' },
            id: String
        },
        data() {
            return {
                loading: false,
                form: {
                    id: null,
                    vid: null,
                    data: {
                        time: null,
                        mode: null,
                        size: null,
                        color: null,
                        text: null
                    },
                    isDelete: false
                },
                rules: {
                    'data.time': [{ required: true, message: '弹幕时间不能为空', trigger: 'change' }],
                    'data.mode': [{ required: true, message: '弹幕类型不能为空', trigger: 'change' }],
                    'data.size': [{ required: true, message: '弹幕大小不能为空', trigger: 'change' }],
                    'data.color': [{ required: true, message: '弹幕颜色不能为空', trigger: 'change' }],
                    'data.text': [{ required: true, message: '弹幕内容不能为空', trigger: 'change' }]
                },
                temp: {
                    referer: null,
                    author: null,
                    ip: null,
                    createTime: null,
                    updateTime: null
                },
                danmuModes,
                predefineColors
            }
        },
        methods: {
            json2form(json) {
                mergeObj(this.form, json)
                this.form.data.color = `#${json.data.color.toString(16)}`
                this.temp.createTime = getLocalTime(json.createTime)
                this.temp.updateTime = getLocalTime(json.updateTime)
                this.temp.author = json.data.author
                this.temp.ip = json.ip
                const { protocol, host, port, path, query } = json.video.referer
                this.temp.referer = new URL(`${protocol}://${host}:${port}${path}${query}`).toString()
            },
            form2json() {
                return {
                    ...this.form,
                    data: { ...this.form.data, color: parseInt(this.form.data.color.substring(1), 16) }
                }
            },
            open() {
                const id = this.id
                this.loading = true
                if (isEmpty(id)) return elAlert(`获取数据失败，请传入id`, () => this.cancel())
                getById(id)
                    .then(data => this.json2form(data))
                    .catch(e => {
                        console.error(e)
                        return elAlert(`获取数据失败，请重试`, () => this.cancel())
                    })
                    .finally(() => this.loading = false)
            },
            clearForm() {
                resetObj(this.form)
                resetObj(this.temp)
            },
            cancel() {
                this.$emit('input', false)
                this.clearForm()
                this.$refs.form.resetFields()
                this.loading = false
            },
            confirm() {
                if (this.loading) return
                this.$refs.form.validate(v => {
                    if (!v) return
                    this.loading = true
                    update(this.form2json())
                        .then(() => this.$emit('success', '修改成功'))
                        .finally(() => this.loading = false)
                })
            }
        }
    }
</script>
