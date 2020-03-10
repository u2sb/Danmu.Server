<template id="danmu-edit">
  <div>
    <el-dialog title="编辑弹幕" :visible.sync="dialogFormVisible" width="850px">
      <el-form :model="form">
        <el-row>
          <el-col :span="12">
            <el-form-item label="Id" :label-width="formLabelWidth">
              <el-input v-model="form.id" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="Vid" :label-width="formLabelWidth">
              <el-input v-model="form.vid" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24">
            <el-form-item label="Referer" :label-width="formLabelWidth">
              <el-input v-model="form.referer" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="Author" :label-width="formLabelWidth">
              <el-input v-model="form.data.author" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="IP" :label-width="formLabelWidth">
              <el-input v-model="form.ip" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item label="UpdateTime" :label-width="formLabelWidth">
              <el-input v-model="form.updateTime" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="CreateTime" :label-width="formLabelWidth">
              <el-input v-model="form.createTime" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="9">
            <el-form-item label="弹幕时间" :label-width="formLabelWidth">
              <el-input v-model="form.data.time"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="9">
            <el-form-item label="弹幕类型" :label-width="formLabelWidth">
              <el-select v-model="form.data.mode">
                <el-option
                  v-for="item in danmuModes"
                  :key="item.value"
                  :label="item.label"
                  :value="item.value"
                >
                  <span style="float: left">{{ item.value }}</span>
                  <span style="float: right; color: #8492a6; font-size: 13px">{{ item.label }}</span>
                </el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="颜色" :label-width="formLabelWidth">
              <el-color-picker v-model="form.data.color" :predefine="predefineColors"></el-color-picker>
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="弹幕内容" :label-width="formLabelWidth">
          <el-input type="textarea" v-model="form.data.text"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="OnClickCancel()">取 消</el-button>
        <el-button type="primary" @click="OnClickEnter()">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import dayjs from 'dayjs'
import utc from 'dayjs/plugin/utc'

dayjs.extend(utc)

export default {
  name: 'danmuEdit',
  props: {
    id: {
      type: String,
      default: '',
      required: true
    },
    dialogFormVisible: {
      type: Boolean,
      default: false,
      required: true
    }
  },
  data() {
    return {
      danmuData: {
        data: {}
      },
      form: {
        data: {}
      },
      formLabelWidth: '100px',
      predefineColors: [
        '#000000',
        '#FFFFFF',
        '#E54256',
        '#FFE133',
        '#64DD17',
        '#39CCFF',
        '#D500F9'
      ],
      danmuModes: [
        {
          value: 0,
          label: ''
        },
        {
          value: 1,
          label: '滚动弹幕'
        },
        {
          value: 2,
          label: ''
        },
        {
          value: 3,
          label: ''
        },
        {
          value: 4,
          label: '底部弹幕'
        },
        {
          value: 5,
          label: '顶部弹幕'
        },
        {
          value: 6,
          label: ''
        },
        {
          value: 7,
          label: ''
        },
        {
          value: 8,
          label: '高级弹幕'
        },
        {
          value: 9,
          label: '特殊弹幕'
        }
      ]
    }
  },
  methods: {
    OnClickCancel() {
      this.$emit('close')
    },
    OnClickEnter() {
      this.danmuData = this.Form2Data(this.form)
      this.$http.post('/api/admin/danmuedit/edit', this.danmuData).then(res => {
        let dataObj = eval(res.data)
        if (dataObj.code === 0) {
          this.$notify({
            title: '提示',
            message: '修改成功',
            position: 'bottom-right'
          })
          this.$emit('close', true)
        } else {
          this.$notify.error({
            title: '错误',
            message: '修改失败',
            position: 'bottom-right'
          })
        }
      })
    },
    GetData(id) {
      this.$http
        .get('/api/admin/danmuedit/get', {
          params: {
            id: id
          }
        })
        .then(res => {
          let dataObj = eval(res.data)
          if (dataObj.code === 0) {
            this.danmuData = dataObj.data
            this.form = this.Data2Form(dataObj.data)
          } else if (dataObj.code === 401) {
            this.$router.push({ path: '/login', query: { url: this.$router.fullPath } })
          }
        })
    },
    Data2Form(data) {
      let f = JSON.parse(JSON.stringify(data))
      let v = f.video.referer
      f.data.color = `#${f.data.color.toString(16)}`
      f.referer = new URL(`${v.protocol}://${v.host}:${v.port}${v.path}${v.query}`).toString()
      //f.referer = `${v.protocol}://${v.host}:${v.port}${v.path}${v.query}`
      f.createTime = dayjs
        .utc(f.createTime)
        .local()
        .format('YYYY-MM-DD HH:mm:ss')
      f.updateTime = dayjs
        .utc(f.updateTime)
        .local()
        .format('YYYY-MM-DD HH:mm:ss')
      return f
    },
    Form2Data(form) {
      let f = JSON.parse(JSON.stringify(form))
      f.data.color = parseInt(f.data.color.substring(1), 16)
      f.createTime = dayjs.utc(f.createTime)
      f.updateTime = dayjs.utc()
      return f
    }
  },
  watch: {
    dialogFormVisible(val) {
      if (!val) {
        this.$emit('close')
      } else {
        this.GetData(this.id)
      }
    }
  }
}
</script>