<template>
  <div id="danmakulist">
    <el-table
      v-loading="loading"
      :data="tableData"
      border
      height="72vh"
      style="width: 100%; font-size:80%;"
    >
      <el-table-column fixed prop="num" label width="40"></el-table-column>
      <el-table-column fixed prop="vid" label="Vid" width="250" show-overflow-tooltip></el-table-column>
      <el-table-column prop="text" label="数据" width="400"></el-table-column>
      <el-table-column prop="date" label="时间" width="220" show-overflow-tooltip></el-table-column>
      <el-table-column prop="ip" label="IP" width="150" show-overflow-tooltip></el-table-column>
      <el-table-column prop="referer" label="网址" width="800" show-overflow-tooltip></el-table-column>
      <el-table-column fixed="right" prop="isDelete" label="删除" width="50"></el-table-column>
      <el-table-column fixed="right" label="操作" width="120">
        <template slot-scope="scope">
          <el-button-group>
            <el-button
              @click.native.prevent="handleEdit(tableData[scope.$index])"
              type="primary"
              icon="el-icon-edit"
              size="mini"
            ></el-button>
            <el-button
              @click="handleDelete(tableData[scope.$index])"
              type="primary"
              icon="el-icon-delete"
              size="mini"
            ></el-button>
          </el-button-group>
        </template>
      </el-table-column>
    </el-table>
    <div>
      <div class="block">
        <el-pagination
          @size-change="handleSizeChange"
          @current-change="handleCurrentChange"
          :current-page="currentPage"
          :page-sizes="pageSizes"
          :page-size="pageSize"
          layout="total, sizes, prev, pager, next, jumper"
          :total="count"
        ></el-pagination>
      </div>
    </div>
    <danmaku-edit
      v-bind:id="danmakuEditId"
      v-bind:dialogFormVisible="dialogFormVisible"
      @close="DialogClose"
    ></danmaku-edit>
  </div>
</template>


<script>
import Enumerable from 'linq'
import DanmakuEdit from './Edit/DanmakuEdit'
export default {
  name: 'danmakulist',
  data() {
    return {
      count: 0,
      tableData: [],
      pageSizes: [10, 20, 30, 50, 100],
      pageSize: 30,
      currentPage: 1,
      dialogFormVisible: false,
      danmakuEditId: '',
      loading: true
    }
  },
  mounted: function() {
    this.GetData(this.pageSize, this.currentPage)
    this.GetCount()
  },
  methods: {
    handleSizeChange(val) {
      this.pageSize = val
      this.GetData(this.pageSize, this.currentPage)
    },
    handleCurrentChange(val) {
      this.currentPage = val
      this.GetData(this.pageSize, this.currentPage)
    },
    handleEdit(row) {
      this.danmakuEditId = row.id
      this.dialogFormVisible = true
    },
    handleDelete(row) {
      this.$confirm('是否删除？', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          this.$http
            .get('/api/admin/danmakuedit/delete', {
              params: {
                id: row.id
              }
            })
            .then(res => {
              let dataObj = eval(res.data)
              if (dataObj.code === 0) {
                this.GetData(this.pageSize, this.currentPage)
                this.$notify({
                  title: '提示',
                  message: '删除成功',
                  position: 'bottom-right'
                })
              } else {
                this.$notify.error({
                  title: '错误',
                  message: '删除失败',
                  position: 'bottom-right'
                })
              }
            })
        })
        .catch(() => {
          this.$notify({
            title: '提示',
            message: '取消删除',
            position: 'bottom-right'
          })
        })
    },
    GetCount() {
      this.$http.get('/api/admin/danmakulist/count').then(res => {
        let dataObj = eval(res.data)
        if (dataObj.code === 0) {
          this.count = dataObj.data.count
        } else {
          this.$router.push({ path: '/login', query: { ReturnUrl: this.$router.fullPath } })
        }
      })
    },
    GetData(size, page) {
      this.loading = true
      this.$http
        .get('/api/admin/danmakulist', {
          params: {
            size: size,
            page: page
          }
        })
        .then(res => {
          let dataObj = eval(res.data)
          if (dataObj.code === 0) {
            this.tableData = Enumerable.from(dataObj.data)
              .select((s, i) => ({
                num: i,
                id: s.id,
                vid: s.vid,
                text: s.danmakuData.text,
                ip: s.ip,
                date: s.date,
                referer: s.referer,
                isDelete: s.isDelete ? '是' : '否'
              }))
              .toArray()
            this.loading = false
          } else if (dataObj.code === 401) {
            this.$router.push({ path: '/login', query: { ReturnUrl: this.$router.fullPath } })
          }
        })
    },
    DialogClose(isSuccess) {
      if (isSuccess === true) {
        this.GetData(this.pageSize, this.currentPage)
      }
      this.dialogFormVisible = false
    }
  },
  components: {
    'danmaku-edit': DanmakuEdit
  }
}
</script>