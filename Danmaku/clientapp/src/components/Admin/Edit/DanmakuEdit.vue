<template id="danmaku-edit">
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
          <el-col :span="12">
            <el-form-item label="Referer" :label-width="formLabelWidth">
              <el-input v-model="form.referer" disabled></el-input>
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
            <el-form-item label="Author" :label-width="formLabelWidth">
              <el-input v-model="form.danmakuData.author" disabled></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="Date" :label-width="formLabelWidth">
              <el-input v-model="form.date" disabled></el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="9">
            <el-form-item label="弹幕时间" :label-width="formLabelWidth">
              <el-input v-model="form.danmakuData.time"></el-input>
            </el-form-item>
          </el-col>
          <el-col :span="9">
            <el-form-item label="弹幕类型" :label-width="formLabelWidth">
              <el-select v-model="form.danmakuData.type">
                <el-option label="滚动" value="滚动"></el-option>
                <el-option label="顶部" value="顶部"></el-option>
                <el-option label="底部" value="底部"></el-option>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="颜色" :label-width="formLabelWidth">
              <el-color-picker v-model="form.danmakuData.color" :predefine="predefineColors"></el-color-picker>
            </el-form-item>
          </el-col>
        </el-row>

        <el-form-item label="弹幕内容" :label-width="formLabelWidth">
          <el-input type="textarea" v-model="form.danmakuData.text"></el-input>
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
export default {
  name: 'danmakuEdit',
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
      danmakuData: {
        danmakuData: {}
      },
      form: {
        danmakuData: {}
      },
      formLabelWidth: '100px',
      predefineColors: ['#000000', '#FFFFFF', '#E54256', '#FFE133', '#64DD17', '#39CCFF', '#D500F9']
    }
  },
  methods: {
    OnClickCancel() {
      this.$emit('close')
    },
    OnClickEnter() {
      this.danmakuData = this.Form2Data(this.form)
      this.$http.post('/api/admin/danmakuedit/edit', this.danmakuData).then(res => {
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
        .get('/api/admin/danmakuedit/get', {
          params: {
            id: id
          }
        })
        .then(res => {
          let dataObj = eval(res.data)
          if (dataObj.code === 0) {
            this.danmakuData = dataObj.data
            this.form = this.Data2Form(dataObj.data)
          } else if (dataObj.code === 401) {
            this.$router.push({ path: '/login', query: { ReturnUrl: this.$router.fullPath } })
          }
        })
    },
    Data2Form(data) {
      let f = JSON.parse(JSON.stringify(data))
      f.danmakuData.color = `#${f.danmakuData.color.toString(16)}`
      f.danmakuData.type = { 0: '滚动', 1: '顶部', 2: '底部' }[f.danmakuData.type]
      return f
    },
    Form2Data(form) {
      let f = JSON.parse(JSON.stringify(form))
      f.danmakuData.type = { 滚动: 0, 顶部: 1, 底部: 2 }[f.danmakuData.type]
      f.danmakuData.color = parseInt(f.danmakuData.color.substring(1), 16)
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